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
    public static class Dropbox
    {
        private const string ApiKey = "6k40bpqlz573mqt";
        private const string Secret= "mx5bth2wj95mkd2";
        private static string _accessToken="";

        //private static DropNetClient _service;
        private static volatile bool _uploading;

        private static List<UploadEntry> UploadList { get; set; } = new List<UploadEntry>();

        private static readonly DateTime Expires = DateTime.UtcNow;

        public static string AccessToken
        {
            get
            {
                if (_accessToken != "" && Expires < DateTime.UtcNow)
                {
                    return _accessToken;
                }
                if (!string.IsNullOrEmpty(MainForm.Conf.Cloud.DropBox))
                {
                    dynamic d = JsonConvert.DeserializeObject(MainForm.Conf.Cloud.DropBox);
                    //dropbox doesn't seem to use refresh_token
                    _accessToken = d.access_token.ToString();
                    return _accessToken;
                }
                return "";
            }
        }


        public static bool Authorise(string code)
        {

            try
            {
                var request =
                    (HttpWebRequest)
                        WebRequest.Create("https://api.dropboxapi.com/oauth2/token");

                var postData = "client_id=" + ApiKey + "&client_secret=" +
                               Secret + "&code=" + code + "&grant_type=authorization_code&redirect_uri=https://www.ispyconnect.com/responsecode.aspx";

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

                MainForm.Conf.Cloud.DropBox = responseString;
                return true;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Logger.LogError("Dropbox: "+resp);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MainForm.Conf.Cloud.DropBox = "";
                
            }
            return false;
        }

        private class UploadEntry
        {
            public string SourceFilename;
            public string DestinationPath;
        }

        public static bool Authorised
        {
            get { return AccessToken != ""; }
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


            UploadList.Add(new UploadEntry { DestinationPath = path, SourceFilename = filename });
            if (!_uploading)
            {
                _uploading = true;
                ThreadPool.QueueUserWorkItem(Upload, null);
            }
            success = true;
            return "AddedToQueue";

        }

        private static void Upload(object state)
        {
            try
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
                    entry = l[0]; //could have been cleared by Authorise
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

                try
                {
                    var path = entry.DestinationPath.Replace(@"\", "/");
                    var url = $"https://content.dropboxapi.com/2/files/upload";
                    var request =
                        (HttpWebRequest)
                        WebRequest.Create(url);

                    request.Headers.Add("Authorization", "Bearer " + AccessToken);
                    request.Headers.Add("Dropbox-API-Arg",
                        "{\"path\": \"/" + path + fi.Name +
                        "\",\"mode\": \"add\",\"autorename\": false,\"mute\": false}");

                    request.Method = "POST";
                    request.ContentType = "application/octet-stream";
                    request.ContentLength = byteArray.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(byteArray, 0, byteArray.Length);
                    }

                    var response = (HttpWebResponse) request.GetResponse();
                    var s = response.GetResponseStream();
                    if (s == null)
                        throw new Exception("null response stream");
                    var responseString = new StreamReader(s).ReadToEnd();

                    dynamic d = JsonConvert.DeserializeObject(responseString);
                    Logger.LogMessage("File uploaded to dropbox: " + d.path_lower);
                }
                catch (WebException wex)
                {
                    var s = wex.Response.GetResponseStream();
                    if (s != null)
                    {
                        var resp = new StreamReader(s).ReadToEnd();
                        Logger.LogError("Dropbox: " + resp);
                    }

                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Dropbox");
                }

                Upload(null);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dropbox");
            }
        }

    }
}
