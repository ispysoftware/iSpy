using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using iSpyApplication.Utilities;
using Newtonsoft.Json;

namespace iSpyApplication.Cloud
{
    public static class OneDrive
    {
        private const string ApplicationID = "000000004C193719";
        private const string Secret = "YBG9jT4oqrVDKfn5ug5LFXS";
        private static string baseURL = "https://api.onedrive.com/v1.0/";

        private static volatile bool _uploading;

        private static string _accessToken="";

        private static List<UploadEntry> UploadList { get; set; } = new List<UploadEntry>();

        private static DateTime _expires = DateTime.UtcNow;

        public static string AccessToken
        {
            get
            {
                if (_accessToken != "" && _expires < DateTime.UtcNow)
                {
                    return _accessToken;
                }
                if (!string.IsNullOrEmpty(MainForm.Conf.Cloud.OneDrive))
                {
                    dynamic d = JsonConvert.DeserializeObject(MainForm.Conf.Cloud.OneDrive);
                    var rt = d.refresh_token.ToString();
                    int expSec = Convert.ToInt32(d.expires_in);
                    _expires = DateTime.UtcNow.AddSeconds(expSec);

                    //get new refresh token

                    var request =
                    (HttpWebRequest)
                        WebRequest.Create("https://login.live.com/oauth20_token.srf");

                    var postData = "client_id=" + ApplicationID + "&redirect_uri=https://www.ispyconnect.com/responsecode.aspx&client_secret=" +
                                   Secret + "&refresh_token=" + rt + "&grant_type=refresh_token";

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
                    if (s == null)
                        throw new Exception("null response stream");
                    var responseString = new StreamReader(s).ReadToEnd();

                    dynamic d2 = JsonConvert.DeserializeObject(responseString);

                    _accessToken = d2.access_token.ToString();

                    MainForm.Conf.Cloud.OneDrive = responseString;
                    return _accessToken;
                }
                return "";
            }
        }       

        private class UploadEntry
        {
            public string SourceFilename;
            public string DestinationPath;
        }

        public static string Upload(string filename, string path, out bool success)
        {
            success = false;
            //if (!Statics.Subscribed)
            //    return "NotSubscribed";

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
            get { return AccessToken != ""; }
        }

        public static bool Authorise(string code)
        {
            try
            {
                var request =
                    (HttpWebRequest)
                        WebRequest.Create("https://login.live.com/oauth20_token.srf?client_id=" + ApplicationID + "&redirect_uri=https://www.ispyconnect.com/responsecode.aspx&client_secret=" +
                               Secret + "&code=" + code + "&grant_type=authorization_code");
                
                request.Method = "GET";
                request.AllowAutoRedirect = true;
                
                var response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                if (s==null)
                    throw new Exception("null response stream");
                var responseString = new StreamReader(s).ReadToEnd();

                dynamic d = JsonConvert.DeserializeObject(responseString);

                _accessToken = d.access_token.ToString();

                MainForm.Conf.Cloud.OneDrive = responseString;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MainForm.Conf.Cloud.OneDrive = "";
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

                if (AccessToken == "")
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
                catch (Exception ex)
                {
                    //file doesn't exist
                    Logger.LogException(ex);
                    return;
                }

                try { 
                    var path = entry.DestinationPath.Replace(@"\", "/");
        //            var drive = Get("drive");
                    var url = $"{baseURL}drive/root:/{path}/{fi.Name}:/content";
                    //PUT /drive/root:/{parent-path}/{filename}:/content
                    var request =
                            (HttpWebRequest)
                                WebRequest.Create(url);

                    request.Headers.Add("Authorization","bearer "+ AccessToken);
            
                    request.Method = "PUT";
                    request.ContentType = "application/octet-stream";
                    request.ContentLength = byteArray.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(byteArray, 0, byteArray.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();
                    var  s = response.GetResponseStream();
                    if (s == null)
                        throw new Exception("null response stream");
                    var responseString = new StreamReader(s).ReadToEnd();

                    dynamic d = JsonConvert.DeserializeObject(responseString);
                    Logger.LogMessage("File uploaded to onedrive: "+d.name+" ("+d.id+")");
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "OneDrive");
                }

                Upload(null);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dropbox");
            }
        }

        //private static dynamic Get(string path)
        //{
        //    var request =
        //            (HttpWebRequest)
        //                WebRequest.Create(baseURL+path);

        //    request.Headers.Add("Authorization", "bearer " + AccessToken);
        //    request.Method = "GET";

        //    var response = (HttpWebResponse)request.GetResponse();
        //    var s = response.GetResponseStream();
        //    if (s == null)
        //        throw new Exception("null response stream");
        //    var responseString = new StreamReader(s).ReadToEnd();

        //    return JsonConvert.DeserializeObject(responseString);
        //}

        
    }
}
