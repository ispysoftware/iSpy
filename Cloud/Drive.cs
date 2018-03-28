using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using iSpyApplication.Utilities;
using Newtonsoft.Json;
using File = Google.Apis.Drive.v3.Data.File;

namespace iSpyApplication.Cloud
{
    public static class Drive
    {
        private static DriveService _service;
        private static volatile bool _uploading;

        private static readonly List<LookupPair> Lookups = new List<LookupPair>();
        private static List<UploadEntry> UploadList { get; set; } = new List<UploadEntry>();

        private static DateTime _expires = DateTime.UtcNow;


        public static DriveService Service
        {
            get
            {
                if (_service != null && _expires<DateTime.UtcNow)
                {
                    return _service;
                }
                if (!string.IsNullOrEmpty(MainForm.Conf.Cloud.Drive))
                {
                    dynamic d = JsonConvert.DeserializeObject(MainForm.Conf.Cloud.Drive);
                    var rt = d.refresh_token.ToString();
                    int expSec = Convert.ToInt32(d.expires_in);
                    _expires = DateTime.UtcNow.AddSeconds(expSec);

                    var token = new TokenResponse { RefreshToken =  rt};
                    var secrets = new ClientSecrets
                                  {
                                      ClientId = MainForm.GoogleClientId,
                                      ClientSecret = MainForm.GoogleClientSecret
                    };

                    var ini = new GoogleAuthorizationCodeFlow.Initializer{ClientSecrets = secrets};
                    
                    var flow = new GoogleAuthorizationCodeFlow(ini);
                    var credential = new UserCredential(flow, "user", token);
                    var bi = new BaseClientService.Initializer
                             {
                                 HttpClientInitializer = credential,
                                 ApplicationName = "iSpy",
                             };
                    _service = new DriveService(bi);
                    return _service;
                }
                return null;
            }
        }

        //private static CancellationTokenSource _tCancel;

        private static string GetOrCreateFolder(string path)
        {
            var c = Lookups.FirstOrDefault(p => p.Path == path);
            if (c != null)
                return c.ID;

            string id = "root";
			var l = path.Split(Path.DirectorySeparatorChar);

            var req = Service.Files.List();
            req.Q = "mimeType='application/vnd.google-apps.folder' and trashed=false";
            //req.Fields = "parents";
            FileList filelist;
            try
            {
                filelist = req.Execute();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
            foreach (string f in l)
            {
                if (f != "")
                {
                    bool found = false;
                    foreach (var cr in filelist.Files)
                    {
                        if (cr.Name == f)
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
                            Name = f,
                            MimeType = "application/vnd.google-apps.folder",
                            Description = "iSpy Folder",
                            Parents = new List<string> { id}
                        };
                        File newFolder = Service.Files.Create(body).Execute();
                        id = newFolder.Id;
                    }
                }
            }
            //add id to list
            Lookups.Add(new LookupPair { ID = id, Path = path });
            return id;

        }

        

        private class LookupPair
        {
            public string ID;
            public string Path;
        }

        public static string Upload(string filename, string path, out bool success)
        {
            success = false;

            if (!Authorised)
            {
                return "CloudAddSettings";
            }

            if (UploadList.SingleOrDefault(p => p.SourceFilename == filename) != null)
                return "FileInQueue";

            if (UploadList.Count >= CloudGateway.MaxUploadQueue)
                return "UploadQueueFull";


			UploadList.Add(new UploadEntry { DestinationPath = "iSpy" +"\\"+ path.Replace("/", "\\").Trim(Path.DirectorySeparatorChar), SourceFilename = filename });
            if (!_uploading)
            {
                _uploading = true;
                ThreadPool.QueueUserWorkItem(Upload, null);
            }
            success = true;
            return "AddedToQueue";

        }

        public static bool Authorised
        {
            get { return Service != null; }
        }

        public static bool Authorise(string code)
        {
            _service?.Dispose();
            _service = null;

            try
            {
                var request =
                    (HttpWebRequest)
                        WebRequest.Create("https://www.googleapis.com/oauth2/v4/token");
                
                var postData = "code=" + code + "&client_id=" + MainForm.GoogleClientId +
                               "&client_secret=" + MainForm.GoogleClientSecret +
                               "&redirect_uri=urn:ietf:wg:oauth:2.0:oob&grant_type=authorization_code";
                var dp = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(dp, 0, postData.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                if (s==null)
                    throw new Exception("null response stream");
                var responseString = new StreamReader(s).ReadToEnd();

                dynamic d = JsonConvert.DeserializeObject(responseString);

                string at = d.access_token.ToString();//test it is authorised

                MainForm.Conf.Cloud.Drive = responseString;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MainForm.Conf.Cloud.Drive = "";
                return false;
            }
        }

        private static void Upload(object state)
        {
            try { 
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

                var s = Service;

                if (s == null)
                {
                    _uploading = false;
                    return;
                }
            

                FileInfo fi;
                byte[] byteArray;
                try
                {
                    fi = new FileInfo(entry.SourceFilename);
                    byteArray = Helper.ReadBytesWithRetry(fi);
                }
                catch(Exception ex)
                {
                    //file doesn't exist
                    Logger.LogException(ex);
                    _uploading = false;
                    return;
                }

                var mt = MimeTypes.GetMimeType(fi.Extension);

                var body = new File {Name = fi.Name, Description = "iSpy", MimeType = mt};
                string fid = GetOrCreateFolder(entry.DestinationPath);
                
                try
                {
                    using (var stream = new MemoryStream(byteArray))
                    {
                        body.Parents = new List<string> {fid};
                        var request = s.Files.Create(body, stream, mt);
                        request.ProgressChanged += RequestProgressChanged;
                        request.ResponseReceived += RequestResponseReceived;
                        try
                        {
                            request.Upload();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                Upload(null);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dropbox");
            }

        }

        static void RequestResponseReceived(File obj)
        {
            string msg = "File uploaded to drive: " +obj.Name;
            Logger.LogMessage(msg);
        }

        private static void RequestProgressChanged(IUploadProgress obj)
        {
            switch (obj.Status)
            {
                case UploadStatus.Failed:
                    if (obj.Exception!=null)
                        Logger.LogError("Upload to Drive failed ("+obj.Exception.Message+")");
                    else
                    {
                        Logger.LogError("Upload to Drive failed");
                    }
                    break;
            }

            if (obj.Exception != null)
            {
                Logger.LogException(obj.Exception);
            }


        }


        private class UploadEntry
        {
            public string SourceFilename;
            public string DestinationPath;
        }

    }
}
