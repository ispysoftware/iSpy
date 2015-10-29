using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

namespace WixCA
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CopyCustomisationFiles(Session session)
        {
            session.Log("Begin CopyCustomisationFiles");

            string path = session.CustomActionData["SourceDir"];

            session.Log("source dir is " + path);

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\iSpy\";
            path = path.Trim().Trim('\\') + @"\";
            try
            {
                if (File.Exists(path + "custom.txt"))
                {
                    TryCopy(path + @"custom.txt", appDataPath + @"custom.txt", true);
                    TryCopy(path + @"logo.jpg", appDataPath + @"logo.jpg", true);
                    TryCopy(path + @"logo.png", appDataPath + @"logo.png", true);
                }

            }
            catch
            {

            }


            return ActionResult.Success;
        }

        private static void TryCopy(string source, string target, bool overwrite)
        {
            try
            {
                File.Copy(source, target, overwrite);
            }
            catch
            {

            }
        }
    }
}
