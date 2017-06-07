using System;
using System.Linq;
using iSpyApplication.Utilities;

namespace iSpyApplication.Cloud
{
    public static class CloudGateway
    {
        public static int MaxUploadQueue = 999;

        public static string Upload(int otid, int oid, string srcPath)
        {
            bool b;
            return Upload(otid, oid, srcPath, out b);
        }
        public static string Upload(int otid, int oid, string srcPath, out bool success)
        {
            success = false;

            //if (!Statics.Subscribed)
            //    return "NotSubscribed";

            try
            {
                string dstPath = "";
                string provider = "";
                switch (otid)
                {
                    case 1:
                        break;
                    case 2:
                        var co = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                        if (co == null)
                            return "object not found";
                        provider = co.settings.cloudprovider.provider;
                        dstPath = co.settings.cloudprovider.path;
                        dstPath = dstPath.Replace("[DIR]", co.directory);
                        dstPath = dstPath.Replace("[NAME]", co.name);
                        dstPath = dstPath.Replace("[DATE]", DateTime.Now.ToString("yyyy-MM-dd"));
                        dstPath = dstPath.Replace("[MEDIATYPE]", srcPath.EndsWith(".jpg") ? "images" : "video");
                        break;
                }

                dstPath = dstPath.Replace("/", "\\");

                switch (provider.ToLowerInvariant())
                {
                    case "drive":
                        return Drive.Upload(srcPath, dstPath, out success);
                    case "dropbox":
                        return Dropbox.Upload(srcPath, dstPath, out success);
                    case "flickr":
                        return Flickr.Upload(srcPath, dstPath, out success);
                    case "onedrive":
                        return OneDrive.Upload(srcPath, dstPath, out success);
                    case "box":
                        return Box.Upload(srcPath, dstPath, out success);
                }

                throw new Exception("Cloud provider not configured");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }
    }
}
