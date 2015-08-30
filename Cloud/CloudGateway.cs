using System;
using System.Linq;

namespace iSpyApplication.Cloud
{
    public static class CloudGateway
    {
        public static int MaxUploadQueue = 999;

        public static string Upload(int otid, int oid, string srcPath)
        {
            if (!MainForm.Conf.Subscribed)
                return LocRm.GetString("AccessDenied");

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
                    if (srcPath.EndsWith(".jpg"))
                        dstPath = dstPath.Replace("[MEDIATYPE]", "images");
                    else
                        dstPath = dstPath.Replace("[MEDIATYPE]", "video");
                    break;
            }

            dstPath = dstPath.Replace("/", @"\");

            switch (provider)
            {
                case "Google Drive":
                    return Drive.Upload(srcPath, dstPath);
                case "Dropbox":
                    return Dropbox.Upload(srcPath, dstPath);
            }
            return LocRm.GetString("NotConfigured");
        }
    }
}
