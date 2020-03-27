using System;
using System.Threading;
using System.Windows.Forms;

namespace iSpyMonitor
{
    static class Program
    {
        public static string AppPath = "", AppDataPath="", ProgramName="";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppPath = (Application.StartupPath.ToLower());
            AppPath = AppPath.Replace(@"\bin\debug", @"\").Replace(@"\bin\release", @"\");
            AppPath = AppPath.Replace(@"\bin\x86\debug", @"\").Replace(@"\bin\x86\release", @"\");

            AppPath = AppPath.Replace(@"\\", @"\");

            if (!AppPath.EndsWith(@"\"))
                AppPath += @"\";


            if (args.Length > 0)
            {
                ProgramName = args[0];
                AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + ProgramName +
                              @"\";
                bool firstInstance;
                var mutex = new Mutex(false, "iSpyMonitor", out firstInstance);
                if (firstInstance)
                    Application.Run(new Monitor());
                mutex.Close();
                mutex.Dispose();
            }
        }
    }
}
