using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FlickrNet;
using iSpyApplication.Utilities;

namespace iSpyApplication.Cloud
{
    public static class Flickr
    {
        private const string ApiKey = "af6a47a510e820b8424d2986f43215b3";
        private const string Secret = "2ffa873c4f66411c";
        private static FlickrNet.Flickr _service;
        private static volatile bool _uploading;

        private static bool _gottoken;
        private static OAuthRequestToken _requestToken;

        private static List<UploadEntry> UploadList { get; set; } = new List<UploadEntry>();

        public static string GetAuthoriseURL(out string err)
        {
            err = "";
            try
            {
                MainForm.Conf.Cloud.Flickr = "";

                _service = null;
                _requestToken = Service.OAuthGetRequestToken("oob");

                string url = Service.OAuthCalculateAuthorizationUrl(_requestToken.Token, AuthLevel.Write);
                _gottoken = false;
                return url;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Logger.LogException(ex, "Flickr");
                return "";
            }
        }
        public static FlickrNet.Flickr Service
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }
				if (string.IsNullOrEmpty(MainForm.Conf.Cloud.Flickr))
                    _service = new FlickrNet.Flickr(ApiKey, Secret);
                else
                {
					string[] cfg = MainForm.Conf.Cloud.Flickr.Split('|');
                    if (cfg.Length == 2)
                    {
                        _service = new FlickrNet.Flickr(ApiKey, Secret);
                        Service.OAuthAccessToken = cfg[0];
                        Service.OAuthAccessTokenSecret = cfg[1];
                        _gottoken = true;
                    }
                }
                return _service;
            }
        }
        
        public static bool Authorise(string code)
        {

            try
            {
                var accessToken = Service.OAuthGetAccessToken(_requestToken, code);

                Service.OAuthAccessToken = accessToken.Token;
                Service.OAuthAccessTokenSecret = accessToken.TokenSecret;

                MainForm.Conf.Cloud.Flickr = accessToken.Token + "|" + accessToken.TokenSecret;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
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
            get
            {
                if (Service == null)
                    return false;

                if (_gottoken)
                    return true;

                try
                {
                    if (!string.IsNullOrEmpty(MainForm.Conf.Cloud.Flickr))
                    {
                        string[] cfg = MainForm.Conf.Cloud.Flickr.Split('|');
                        Service.OAuthAccessToken = cfg[0];
                        Service.OAuthAccessTokenSecret = cfg[1];
                        _gottoken = true;
                        //var ps = Service.PhotosetsCreate("iSpy", "iSpy Recorded Content", "");

                        return true;
                    }
                    
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                return false;
            }
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
                    entry = l[0];//could have been cleared by Authorise
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
                try
                {
                    fi = new FileInfo(entry.SourceFilename);
                    var r = s.UploadPicture(entry.SourceFilename, fi.Name,"iSpy video");
                    if (r != null)
                        Logger.LogMessage("Uploaded to flickr: " + fi.Name);
                    else
                        Logger.LogMessage("Upload to flickr failed ("+ fi.Name + ")");
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex,"Flickr");
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
