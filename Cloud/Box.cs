using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using iSpyApplication.Utilities;
using Newtonsoft.Json;

namespace iSpyApplication.Cloud
{
    public static class Box
    {
        private const string ClientID = "0uvr6c6kvl60p7725i62v9ua4k6bclpj";
        private const string Secret = "oTPL3sMltDGe8i0Pt8fCgQ5FN46Gsdkv";

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
                    dynamic d = JsonConvert.DeserializeObject(MainForm.Conf.Cloud.Box);
                    var rt = d.refresh_token.ToString();
                    int expSec = Convert.ToInt32(d.expires_in);
                    _expires = DateTime.UtcNow.AddSeconds(expSec);

                    //get new refresh token

                    var request =
                    (HttpWebRequest)
                        WebRequest.Create("https://api.box.com/oauth2/token");

                    var postData = "client_id=" + ClientID + "&redirect_uri=https://www.ispyconnect.com/responsecode.aspx&client_secret=" +
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
                        WebRequest.Create("https://api.box.com/oauth2/token");

                var postData = "code=" + code + "&client_id=" + ClientID +
                               "&client_secret=" + Secret +
                               "&redirect_uri=https://www.ispyconnect.com/responsecode.aspx&grant_type=authorization_code";
                var dp = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(dp, 0, postData.Length);
                }

                request.Method = "POST";
                request.AllowAutoRedirect = true;

                var response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                if (s==null)
                    throw new Exception("null response stream");
                var responseString = new StreamReader(s).ReadToEnd();

                dynamic d = JsonConvert.DeserializeObject(responseString);

                _accessToken = d.access_token.ToString();

                MainForm.Conf.Cloud.Box = responseString;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MainForm.Conf.Cloud.Box = "";
                return false;
            }
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

            if (AccessToken == "")
            {
                _uploading = false;
                return;
            }


            try
            {
                var fi = new FileInfo(entry.SourceFilename);
                NameValueCollection values = new NameValueCollection();
                NameValueCollection files = new NameValueCollection();
                string fid = GetOrCreateFolder(entry.DestinationPath);
                values.Add("attributes", "{\"name\":\"" + fi.Name + "\", \"parent\":{\"id\":\""+fid+"\"}}");
                files.Add("file", fi.FullName);
                var responseString = SendHttpRequest("https://upload.box.com/api/2.0/files/content", values, files);

                dynamic d = JsonConvert.DeserializeObject(responseString);
                Logger.LogMessage("File uploaded to box: "+d.entries[0].id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Box");
            }

            Upload(null);            

        }
        private class LookupPair
        {
            public string ID;
            public string Path;

            public LookupPair(string Path, string ID)
            {
                this.Path = Path;
                this.ID = ID;
            }
        }
        private static readonly List<LookupPair> Lookups = new List<LookupPair>();

        private static string GetOrCreateFolder(string path)
        {
            var c = Lookups.FirstOrDefault(p => p.Path == path);
            if (c != null)
                return c.ID;

            string id = "0";
            var l = path.Split(Path.DirectorySeparatorChar);
            var partialPath = "";
            foreach (var f in l)
            {
                partialPath+=f + Path.DirectorySeparatorChar;

                c = Lookups.FirstOrDefault(p => p.Path == partialPath);
                if (c == null)
                {
                    var folder = GetData("folders/" + id + "/items");
                    bool found = false;
                    foreach (dynamic d in folder.entries)
                    {
                        if (d.type == "folder")
                        {
                            if (d.name == f)
                            {
                                id = d.id;
                                Lookups.Add(new LookupPair(partialPath,id));
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        var df = POSTData("folders", "{\"name\":\"" + f + "\", \"parent\": {\"id\": \"" + id + "\"}}");
                        id = df.id;
                        Lookups.Add(new LookupPair(partialPath, id));
                    }
                }
                else
                {
                    id = c.ID;
                }
            }
            return id;
            
        }

        private static dynamic POSTData(string path, string postData)
        {
            var request =
                    (HttpWebRequest)
                        WebRequest.Create("https://api.box.com/2.0/"+path);

            request.Headers.Add("Authorization", "Bearer " + AccessToken);
            var dp = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "text/json";
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

            return JsonConvert.DeserializeObject(responseString);
        }
        private static dynamic GetData(string path)
        {
            var request =
                (HttpWebRequest)
                    WebRequest.Create("https://api.box.com/2.0/"+path);

            request.Headers.Add("Authorization", "Bearer " + AccessToken);
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();
            if (s == null)
                throw new Exception("null response stream");
            var responseString = new StreamReader(s).ReadToEnd();

            dynamic d = JsonConvert.DeserializeObject(responseString);
            return d;
        }

        private static string SendHttpRequest(string url, NameValueCollection values, NameValueCollection files = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            // The first boundary
            byte[] boundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            // The last boundary
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            // Create the request and set parameters
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Bearer " + AccessToken);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get request stream
            Stream requestStream = request.GetRequestStream();

            foreach (string key in values.Keys)
            {
                // Write item to stream
                byte[] formItemBytes = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}", key, values[key]));
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            if (files != null)
            {
                foreach (string key in files.Keys)
                {
                    if (File.Exists(files[key]))
                    {
                        byte[] buffer = new byte[2048];
                        byte[] formItemBytes = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", key, files[key]));
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        requestStream.Write(formItemBytes, 0, formItemBytes.Length);

                        using (FileStream fileStream = new FileStream(files[key], FileMode.Open, FileAccess.Read))
                        {
                            int bytesRead;
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                // Write file content to stream, byte by byte
                                requestStream.Write(buffer, 0, bytesRead);
                            }

                            fileStream.Close();
                        }
                    }
                }
            }

            // Write trailer and close stream
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return reader.ReadToEnd();
            };
        }
    }
}
