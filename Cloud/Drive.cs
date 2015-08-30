using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v2.Data.File;

namespace iSpyApplication.Cloud
{
    public static class Drive
    {
        private static DriveService _service;
        private static volatile bool _uploading;

        private static string _refreshToken = "";
        private static List<LookupPair> _lookups = new List<LookupPair>();
        private static List<UploadEntry> _upload = new List<UploadEntry>();
        private static readonly object Lock = new object();
        private static List<UploadEntry> UploadList
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


        public static DriveService Service
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }
                _refreshToken = MainForm.Conf.GoogleDriveConfig;
                if (!String.IsNullOrEmpty(_refreshToken))
                {
                    var token = new TokenResponse { RefreshToken = _refreshToken };
                    var credential = new UserCredential(new GoogleAuthorizationCodeFlow(
                        new GoogleAuthorizationCodeFlow.Initializer
                        {
                            ClientSecrets = new ClientSecrets
                            {
                                ClientId = "648753488389.apps.googleusercontent.com",
                                ClientSecret = "Guvru7Ug8DrGcOupqEs6fTB1"
                            },
                        }), "user", token);
                    _service = new DriveService(new BaseClientService.Initializer
                                                {
                                                    HttpClientInitializer = credential,
                                                    ApplicationName = "iSpy",
                                                });
                    return _service;
                }
                return null;
            }
        }

        private static CancellationTokenSource _tCancel;

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
                    new[] { DriveService.Scope.Drive },
                    "user", _tCancel.Token, new FileDataStore("Drive")).Result;

                _service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = t,
                    ApplicationName = "iSpy",
                });
                if (t?.Token?.RefreshToken != null)
                {

                    MainForm.Conf.GoogleDriveConfig =
                        _refreshToken = t.Token.RefreshToken;

                    _service = new DriveService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = t,
                        ApplicationName = "iSpy",
                    });
                }
                _lookups = new List<LookupPair>();
                _upload = new List<UploadEntry>();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }

            return true;
        }

        private static string GetOrCreateFolder(string path)
        {
            var c = _lookups.FirstOrDefault(p => p.Path == path);
            if (c != null)
                return c.ID;

            string id = "root";
            var l = path.Split('\\');

            var req = Service.Files.List();
            req.Q = "mimeType='application/vnd.google-apps.folder' and trashed=false";
            FileList filelist;
            try
            {
                filelist = req.Execute();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return "";
            }
            bool first = true;
            foreach (string f in l)
            {
                if (f != "")
                {
                    bool found = false;
                    foreach (var cr in filelist.Items)
                    {
                        if (cr.Title == f && cr.Parents.Count > 0 && (cr.Parents[0].Id == id || (first && Convert.ToBoolean(cr.Parents[0].IsRoot))))
                        {
                            found = true;
                            id = cr.Id;
                            break;
                        }
                    }
                    if (!found)
                    {
                        var body = new File
                                   {
                                       Title = f,
                                       MimeType = "application/vnd.google-apps.folder",
                                       Description = "iSpy Folder",
                                       Parents = new List<ParentReference> { new ParentReference { Id = id } }
                                   };
                        File newFolder = Service.Files.Insert(body).Execute();
                        id = newFolder.Id;
                    }
                    first = false;
                }
            }
            //add id to list
            _lookups.Add(new LookupPair { ID = id, Path = path });
            return id;

        }

        private class LookupPair
        {
            public string ID;
            public string Path;
        }

        private class UploadEntry
        {
            public string SourceFilename;
            public string DestinationPath;
        }

        public static string Upload(string filename, string path)
        {
            if (UploadList.SingleOrDefault(p => p.SourceFilename == filename) != null)
                return LocRm.GetString("FileInQueue");

            if (UploadList.Count >= CloudGateway.MaxUploadQueue)
                return LocRm.GetString("UploadQueueFull");

            UploadList.Add(new UploadEntry { DestinationPath = "iSpy\\" + path.Replace("/", "\\").Trim('\\'), SourceFilename = filename });
            if (!_uploading)
            {
                _uploading = true;
                ThreadPool.QueueUserWorkItem(Upload, null);
            }
            return LocRm.GetString("AddedToQueue");

        }

        private static void Upload(object state)
        {
            if (UploadList.Count == 0)
            {
                _uploading = false;
                return;
            }

            UploadEntry entry;

            try
            {
                var l = UploadList.ToList();
                entry = l[0];
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

                FileInfo fi;
                byte[] byteArray;
                try
                {
                    fi = new FileInfo(entry.SourceFilename);
                    byteArray = System.IO.File.ReadAllBytes(fi.FullName);
                }
                catch(Exception ex)
                {
                    //file doesn't exist
                    MainForm.LogExceptionToFile(ex);
                    return;
                }
                var mt = MimeTypes.GetMimeType(fi.Extension);

                var body = new File {Title = fi.Name, Description = "iSpy", MimeType = mt};
                string fid = GetOrCreateFolder(entry.DestinationPath);
                
                try
                {
                    using (var stream = new MemoryStream(byteArray))
                    {
                        body.Parents = new List<ParentReference> {new ParentReference {Id = fid}};
                        var request = Service.Files.Insert(body, stream, mt);
                        request.ProgressChanged += RequestProgressChanged;
                        request.ResponseReceived += RequestResponseReceived;
                        try
                        {
                            request.Upload();
                        }
                        catch (Exception ex)
                        {
                            MainForm.LogExceptionToFile(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
                Upload(null);
            }
            else
                _uploading = false;
            

        }

        static void RequestResponseReceived(File obj)
        {
            string msg = "File uploaded to google drive: <a href=\"" + obj.DownloadUrl + "\">" +obj.Title + "</a>";
            MainForm.LogMessageToFile(msg);
        }

        private static void RequestProgressChanged(IUploadProgress obj)
        {
            switch (obj.Status)
            {
                case UploadStatus.Failed:
                    if (obj.Exception!=null)
                        MainForm.LogErrorToFile("Upload to Google Drive failed ("+obj.Exception.Message+")");
                    else
                    {
                        MainForm.LogErrorToFile("Upload to Google Drive failed");
                    }
                    break;
            }

            if (obj.Exception != null)
            {
                MainForm.LogExceptionToFile(obj.Exception);
            }


        }

    }
}
