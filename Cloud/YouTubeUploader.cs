//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Auth.OAuth2.Flows;
//using Google.Apis.Auth.OAuth2.Responses;
//using Google.Apis.Services;
//using Google.Apis.Upload;
//using Google.Apis.YouTube.v3;
//using Google.Apis.YouTube.v3.Data;
//using iSpyApplication.Utilities;
//using Newtonsoft.Json;

//namespace iSpyApplication.Cloud
//{
//    internal static class YouTubeUploader
//    {
//        private static List<UserState> UploadList { get; set; } = new List<UserState>();

//        private static volatile bool _uploading;
//        private static readonly List<string> Uploaded = new List<string>();

//        public static bool Authorised => Service != null;

//        public static string Upload(int objectId, string filename, out bool success)
//        {
//            success = false;
//            //if (!Statics.Subscribed)
//            //    return "NotSubscribed";

//            if (!Authorised)
//            {
//                return "CloudAddSettings";
//            }

//            if (UploadList.SingleOrDefault(p => p.Filename == filename) != null)
//                return "FileInQueue";

//            if (UploadList.Count >= CloudGateway.MaxUploadQueue)
//                return "UploadQueueFull";

//            if (Uploaded.FirstOrDefault(p=>p==filename)!=null)
//            {
//                return "AlreadyUploaded";
//            }
            
//            var us = new UserState(objectId, filename);
//            UploadList.Add(us);

//            if (!_uploading)
//            {
//                _uploading = true;
//                ThreadPool.QueueUserWorkItem(Upload, null);
//            }
//            success = true;
//            return "AddedToQueue";
//        }

//        private static DateTime _expires = DateTime.UtcNow;
//        private static YouTubeService _service;
//        public static YouTubeService Service
//        {
//            get
//            {
//                if (_service != null && _expires < DateTime.UtcNow)
//                {
//                    return _service;
//                }
//				if (!string.IsNullOrEmpty(MainForm.Conf.Cloud.YouTube))
//                {
//                    dynamic d = JsonConvert.DeserializeObject(MainForm.Conf.Cloud.YouTube);
//                    var rt = d.refresh_token.ToString();
//                    int expSec = Convert.ToInt32(d.expires_in);
//                    _expires = DateTime.UtcNow.AddSeconds(expSec);

//                    var token = new TokenResponse { RefreshToken = rt };
//                    var secrets = new ClientSecrets
//                    {
//                        ClientId = MainForm.GoogleClientId,
//                        ClientSecret = MainForm.GoogleClientSecret
//                    };

//                    var ini = new GoogleAuthorizationCodeFlow.Initializer { ClientSecrets = secrets };

//                    var flow = new GoogleAuthorizationCodeFlow(ini);
//                    var credential = new UserCredential(flow, "user", token);
//                    var bi = new BaseClientService.Initializer
//                    {
//                        HttpClientInitializer = credential,
//                        ApplicationName = "iSpy",
//                    };
//                    _service = new YouTubeService(bi);
//                    return _service;
//                }
//                return null;
//            }
//        }

//        public static bool Authorise(string code)
//        {
//            _service?.Dispose();
//            _service = null;

//            try
//            {
//                var request =
//                    (HttpWebRequest)
//                        WebRequest.Create("https://www.googleapis.com/oauth2/v4/token");

//                var postData = "code=" + code + "&client_id=" + MainForm.GoogleClientId +
//                               "&client_secret=" + MainForm.GoogleClientSecret +
//                               "&redirect_uri=urn:ietf:wg:oauth:2.0:oob&grant_type=authorization_code";
//                var dp = Encoding.ASCII.GetBytes(postData);

//                request.Method = "POST";
//                request.ContentType = "application/x-www-form-urlencoded";
//                request.ContentLength = postData.Length;

//                using (var stream = request.GetRequestStream())
//                {
//                    stream.Write(dp, 0, postData.Length);
//                }

//                var response = (HttpWebResponse)request.GetResponse();
//                Stream s = response.GetResponseStream();
//                if (s == null)
//                    throw new Exception("null response stream");
//                var responseString = new StreamReader(s).ReadToEnd();

//                dynamic d = JsonConvert.DeserializeObject(responseString);

//                string at = d.access_token.ToString();//test it is authorised

//                MainForm.Conf.Cloud.YouTube = responseString;
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Logger.LogException(ex);
//                MainForm.Conf.Cloud.YouTube = "";
//                return false;
//            }
//        }

//        private static void Upload(object state)
//        {
//            try { 
//                if (UploadList.Count == 0)
//                {
//                    _uploading = false;
//                    return;
//                }

//                UserState us;

//                try
//                {
//                    var l = UploadList.ToList();
//                    us = l[0];
//                    l.RemoveAt(0);
//                    UploadList = l.ToList();
//                }
//                catch
//                {
//                    _uploading = false;
//                    return;
//                }

//                var s = Service;

//                if (s == null)
//                {
//                    _uploading = false;
//                    return;
//                }

//                if (us.CameraData == null)
//                    return;

//                var video = new Video
//                    {
//                        Snippet =
//                            new VideoSnippet
//                            {
//                                Title = "iSpy: " + us.CameraData.name,
//                                Description =
//								    "iSpy surveillance software: " +
//                                    us.CameraData.description,
//                                Tags = us.CameraData.settings.youtube.tags.Split(','),
//                                CategoryId = "22"
//                            },
//                        Status =
//                            new VideoStatus
//                            {
//                                PrivacyStatus =
//                                    us.CameraData.settings.youtube.@public
//                                        ? "public"
//                                        : "private"
//                            }
//                    };

            
//                try
//                {
//                    using (var fileStream = new FileStream(us.Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
//                    {

//                        var videosInsertRequest = s.Videos.Insert(video, "snippet,status", fileStream, "video/*");
//                        videosInsertRequest.ProgressChanged += VideosInsertRequestProgressChanged;
//                        videosInsertRequest.ResponseReceived += VideosInsertRequestResponseReceived;
//                        _uploaded = false;
//                        videosInsertRequest.Upload();
//                        if (_uploaded)
//                        {
//                            Uploaded.Add(us.Filename);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Logger.LogException(ex,"YouTube");
//                }
//                Upload(null);
//            }
//            catch (Exception ex)
//            {
//                Logger.LogException(ex, "Dropbox");
//            }

//        }

//        private static bool _uploaded;

//        static void VideosInsertRequestProgressChanged(IUploadProgress progress)
//        {
//            switch (progress.Status)
//            {
//                case UploadStatus.Uploading:
//                    //Debug.WriteLine("{0} bytes sent.", progress.BytesSent);
//                    break;

//                case UploadStatus.Failed:
//                    Logger.LogMessage($"Upload to YouTube failed ({progress.Exception})");
//                    break;
//            }
//        }

//        static void VideosInsertRequestResponseReceived(Video video)
//        {
//            string msg = "YouTube video uploaded: "+video.Id;
//            msg += " ("+video.Status.PrivacyStatus+")";
//            Logger.LogMessage(msg);
//            _uploaded = true;
//        }

//        #region Nested type: UserState

//        internal class UserState
//        {
//            private readonly int _objectid;
//            public string Filename;


//            internal UserState(int objectId, string filename)
//            {
//                _objectid = objectId;
//                CurrentPosition = 0;
//                RetryCounter = 0;
//                Filename = filename;
//            }

//            internal objectsCamera CameraData
//            {
//				get { return MainForm.Cameras.SingleOrDefault(p => p.id == _objectid); }
//            }

//            internal long CurrentPosition { get; set; }


//            internal string Error { get; set; }

//            internal int RetryCounter { get; set; }


//            internal string HttpVerb { get; set; }

//            internal Uri ResumeUri { get; set; }
//        }

//        #endregion
//    }
//}