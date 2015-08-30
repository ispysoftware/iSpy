using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace iSpyApplication.Cloud
{
    internal static class YouTubeUploader
    {
        private static readonly object Lock = new object();
        private static List<UserState> _upload = new List<UserState>();
        private static List<UserState> UploadList
        {
            get
            {
                return _upload;
            }
            set
            {
                lock (Lock)
                {
                    _upload = value;
                }
            }
        } 

        private static volatile bool _uploading;
        private static readonly List<String> Uploaded = new List<string>();

        public static bool Authorised => Service != null;

        public static string Upload(int objectId, string filename)
        {
            if (UploadList.SingleOrDefault(p => p.Filename == filename) != null)
                return LocRm.GetString("FileInQueue");

            if (UploadList.Count >= CloudGateway.MaxUploadQueue)
                return LocRm.GetString("UploadQueueFull");

            if (Uploaded.FirstOrDefault(p=>p==filename)!=null)
            {
                return LocRm.GetString("AlreadyUploaded");
            }
            
            var us = new UserState(objectId, filename);
            UploadList.Add(us);

            if (!_uploading)
            {
                _uploading = true;
                ThreadPool.QueueUserWorkItem(Upload, null);
            }

            return LocRm.GetString("AddedToQueue");
        }

        private static CancellationTokenSource _tCancel;
        private static YouTubeService _service;
        public static YouTubeService Service
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }
                if (!String.IsNullOrEmpty(MainForm.Conf.YouTubeToken))
                {
                    var token = new TokenResponse { RefreshToken = MainForm.Conf.YouTubeToken };

                    var credential = new UserCredential(new GoogleAuthorizationCodeFlow(
                        new GoogleAuthorizationCodeFlow.Initializer
                        {
                            ClientSecrets = new ClientSecrets
                            {
                                ClientId = "648753488389.apps.googleusercontent.com",
                                ClientSecret = "Guvru7Ug8DrGcOupqEs6fTB1"
                            },
                        }), "user", token);

                    _service = new YouTubeService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "iSpy",
                    });
                    return _service;
                }
                return null;
            }
        }

        public static bool Authorise()
        {
            _service?.Dispose();
            _service = null;

            try
            {
                _tCancel?.Cancel(true);

                _tCancel = new CancellationTokenSource();

                var t = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = "648753488389.apps.googleusercontent.com",
                        ClientSecret = "Guvru7Ug8DrGcOupqEs6fTB1"
                    },
                    new[] {YouTubeService.Scope.YoutubeUpload},
                    "user", _tCancel.Token, new FileDataStore("YouTube.Auth.Store")).Result;

                if (t?.Token?.RefreshToken != null)
                {

                    MainForm.Conf.YouTubeToken = t.Token.RefreshToken;
                    _service = new YouTubeService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = t,
                        ApplicationName = "iSpy",
                    });
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;

            }
            return true;
        }

        private static void Upload(object state)
        {
            if (UploadList.Count == 0)
            {
                _uploading = false;
                return;
            }

            UserState us;

            try
            {
                var l = UploadList.ToList();
                us = l[0];
                l.RemoveAt(0);
                UploadList = l.ToList();
            }
            catch
            {
                _uploading = false;
                return;
            }


            if (Service == null)
            {
                if (!Authorise())
                {
                    _uploading = false;
                    return;
                }
            }
            if (Service != null)
            {
                if (us.CameraData == null)
                    return;
                var video = new Video
                    {
                        Snippet =
                            new VideoSnippet
                            {
                                Title = "iSpy: " + us.CameraData.name,
                                Description =
                                    MainForm.Website + ": free open source surveillance software: " +
                                    us.CameraData.description,
                                Tags = us.CameraData.settings.youtube.tags.Split(','),
                                CategoryId = "22"
                            },
                        Status =
                            new VideoStatus
                            {
                                PrivacyStatus =
                                    us.CameraData.settings.youtube.@public
                                        ? "public"
                                        : "private"
                            }
                    };

                try
                {
                    using (var fileStream = new FileStream(us.Filename, FileMode.Open))
                    {

                        var videosInsertRequest = Service.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                        videosInsertRequest.ProgressChanged += VideosInsertRequestProgressChanged;
                        videosInsertRequest.ResponseReceived += VideosInsertRequestResponseReceived;
                        _uploaded = false;
                        videosInsertRequest.Upload();
                        if (_uploaded)
                            Uploaded.Add(us.Filename);
                    }
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
                Upload(null);
            }
            else
            {
                _uploading = false;
            }

        }

        private static bool _uploaded;

        static void VideosInsertRequestProgressChanged(IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    //Debug.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    MainForm.LogMessageToFile($"Upload to YouTube failed ({progress.Exception})");
                    break;
            }
        }

        static void VideosInsertRequestResponseReceived(Video video)
        {
            string msg = "YouTube video uploaded: <a href=\"http://www.youtube.com/watch?v=" + video.Id + "\">" +
                                video.Id + "</a>";
            msg += " ("+video.Status.PrivacyStatus+")";
            MainForm.LogMessageToFile(msg);
            _uploaded = true;
        }

        #region Nested type: UserState

        internal class UserState
        {
            private readonly int _objectid;
            public string Filename;


            internal UserState(int objectId, string filename)
            {
                _objectid = objectId;
                CurrentPosition = 0;
                RetryCounter = 0;
                Filename = filename;
            }

            internal objectsCamera CameraData
            {
                get { return MainForm.Cameras.SingleOrDefault(p => p.id == _objectid); }
            }

            internal long CurrentPosition { get; set; }


            internal string Error { get; set; }

            internal int RetryCounter { get; set; }


            internal string HttpVerb { get; set; }

            internal Uri ResumeUri { get; set; }
        }

        #endregion
    }
}