using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DropNet;

namespace iSpyApplication.Cloud
{
    public static class Dropbox
    {
        private const bool Sandbox = true;

        private static DropNetClient _service;
        private static volatile bool _uploading;

        private static List<UploadEntry> _upload = new List<UploadEntry>();
        private static readonly object Lock = new object();
        private static bool _gottoken;

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


        public static DropNetClient Service
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }
                if (String.IsNullOrEmpty(MainForm.Conf.DropBoxConfig))
                    _service = new DropNetClient("6k40bpqlz573mqt", "mx5bth2wj95mkd2");
                else
                {
                    string[] cfg = MainForm.Conf.DropBoxConfig.Split('|');
                    if (cfg.Length == 2)
                    {
                        _service = new DropNetClient("6k40bpqlz573mqt", "mx5bth2wj95mkd2", cfg[0], cfg[1])
                                   {
                                       UseSandbox = Sandbox
                                   };
                        _gottoken = true;
                    }
                }
                return _service;
            }
        }

        public static bool Authorise()
        {
            try
            {
                MainForm.Conf.DropBoxConfig = "";
                _service = null;
                Service.GetToken();
                var url = Service.BuildAuthorizeUrl();
                MainForm.OpenUrl(url);
                _gottoken = false;
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
            return true;

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
                    var accessToken = Service.GetAccessToken();
                    MainForm.Conf.DropBoxConfig = accessToken.Token + "|" + accessToken.Secret;
                    _gottoken = true;
                    string[] cfg = MainForm.Conf.DropBoxConfig.Split('|');
                    _service = new DropNetClient("6k40bpqlz573mqt", "mx5bth2wj95mkd2", cfg[0], cfg[1])
                    {
                        UseSandbox = Sandbox
                    };
                    return true;
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
                return false;
            }
        }

        public static string Upload(string filename, string path)
        {
            if (!Authorised)
            {
                MainForm.LogMessageToFile("Authorise dropbox in settings");
                return LocRm.GetString("CloudAddSettings");
            }
            if (UploadList.SingleOrDefault(p => p.SourceFilename == filename) != null)
                return LocRm.GetString("FileInQueue");

            if (UploadList.Count >= CloudGateway.MaxUploadQueue)
                return LocRm.GetString("UploadQueueFull");

            UploadList.Add(new UploadEntry { DestinationPath = path, SourceFilename = filename });
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
                entry = l[0];//could have been cleared by Authorise
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
                    byteArray = File.ReadAllBytes(fi.FullName);
                }
                catch
                {
                    //file doesn't exist
                    Upload(null);
                    return;
                }

                using (var stream = new MemoryStream(byteArray))
                {
                    try
                    {
                        string p = "/" + entry.DestinationPath.Replace("\\", "/").Trim('/') + "/";
                        var r = Service.UploadFile(p, fi.Name, stream);
                        MainForm.LogMessageToFile("Uploaded to dropbox: /iSpy" + r.Path);
                    }
                    catch (Exception ex)
                    {
                        MainForm.LogExceptionToFile(ex);
                    }
                }
                Upload(null);
            }
            else
                _uploading = false;


        }

    }
}
