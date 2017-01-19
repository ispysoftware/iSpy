using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using FFmpeg.AutoGen;
using iSpyApplication;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;
using Microsoft.Win32;

internal static class Program
{
    //public static Mutex Mutex;
    private static string _apppath = "", _appdatapath = "";
    public static string Platform = "x86";
    public static int GridViews = 0;
    private static uint _previousExecutionState;
    public static WinFormsAppIdleHandler AppIdle;
    public static string AppPath
    {
        get
        {
            if (_apppath != "")
                return _apppath;
            _apppath = (Application.StartupPath.ToLower());
            _apppath = _apppath.Replace(@"\bin\debug", @"\").Replace(@"\bin\release", @"\");
            _apppath = _apppath.Replace(@"\bin\x86\debug", @"\").Replace(@"\bin\x86\release", @"\");
            _apppath = _apppath.Replace(@"\bin\x64\debug", @"\").Replace(@"\bin\x64\release", @"\");

            _apppath = _apppath.Replace(@"\\", @"\");

            if (!_apppath.EndsWith(@"\"))
                _apppath += @"\";
            Directory.SetCurrentDirectory(_apppath);
            return _apppath;
        }   
    }
    public static string AppDataPath
    {
        get
        {
            if (_appdatapath != "")
                return _appdatapath;
            _appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\iSpy\";
            return _appdatapath;
        }
    }

    public static string ExecutableDirectory = "";   
    public static Mutex FfmpegMutex;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        //uninstall?
        string[] arguments = Environment.GetCommandLineArgs();

        foreach (string argument in arguments)
        {
            if (argument.Split('=')[0].ToLower() == "/u")
            {
                string guid = argument.Split('=')[1];
                string path = Environment.GetFolderPath(Environment.SpecialFolder.System);
                var si = new ProcessStartInfo(path + "/msiexec.exe", "/x " + guid);
                Process.Start(si);
                Application.Exit();
                return;
            }
        }

        try
        {
            Application.EnableVisualStyles();            
            Application.SetCompatibleTextRenderingDefault(false);


            bool firstInstance = true;

            var me = Process.GetCurrentProcess();
            var arrProcesses = Process.GetProcessesByName(me.ProcessName);

            //only want to do this if not passing in a command

            if (arrProcesses.Length > 1)
            {
                firstInstance = false;
            }
            
            string executableName = Application.ExecutablePath;
            var executableFileInfo = new FileInfo(executableName);
            ExecutableDirectory = executableFileInfo.DirectoryName;

            bool ei = (!Directory.Exists(AppDataPath) || !Directory.Exists(AppDataPath + @"XML\") ||
                       !File.Exists(AppDataPath + @"XML\config.xml"));
            if (ei)
                EnsureInstall(true);
            else
            {
                try
                {
                    var o = Registry.CurrentUser.OpenSubKey(@"Software\ispy",true);
                    if (o?.GetValue("firstrun") != null)
                    {
                        o.DeleteValue("firstrun");
                        //copy over updated static files on first run of new install
                        EnsureInstall(false);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex, "startup");
                }
            }

            bool silentstartup = false;

            string command = "";
            if (args.Length > 0)
            {
                if (args[0].ToLower().Trim() == "-reset" && !ei)
                {
                    if (firstInstance)
                    {
                        if (
                            MessageBox.Show("Reset iSpy? This will overwrite all your settings.", "Confirm",
                                            MessageBoxButtons.OKCancel) == DialogResult.OK)
                            EnsureInstall(true);
                    }
                    else
                    {
                        MessageBox.Show("Please exit iSpy before resetting it.");
                    }
                }
                if (args[0].ToLower().Trim() == "-silent" || args[0].ToLower().Trim('\\') == "s")
                {
                    if (firstInstance)
                    {
                        silentstartup = true;
                    }
                }
                else
                {
                    command = args.Aggregate(command, (current, s) => current + (s + " "));
                }
            }

            if (!firstInstance)
            {
                if (!string.IsNullOrEmpty(command))
                {
                    File.WriteAllText(AppDataPath + "external_command.txt", command);
                    Thread.Sleep(1000);
                }
                else
                {
                    //show form
                    File.WriteAllText(AppDataPath + "external_command.txt", "showform");
                    Thread.Sleep(1000);
                }
                
                Application.Exit();
                return;
            }

            if (IntPtr.Size == 8)
                Platform = "x64";

            File.WriteAllText(AppDataPath + "external_command.txt", "");

            // in case our https certificate ever expires or there is some other issue
            //ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 1000;

            FfmpegMutex = new Mutex();
            
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            ffmpeg.avdevice_register_all();
            ffmpeg.avcodec_register_all();
            ffmpeg.avfilter_register_all();
            ffmpeg.avformat_network_init();
            ffmpeg.av_register_all();
            

            _previousExecutionState = NativeCalls.SetThreadExecutionState(NativeCalls.EsContinuous | NativeCalls.EsSystemRequired);
            
            AppIdle = new WinFormsAppIdleHandler {Enabled = false};
            var mf = new MainForm(silentstartup, command);

            
            Application.Run(mf);
            FfmpegMutex.Close();

            GC.KeepAlive(FfmpegMutex);
            AppIdle.Enabled = false;
            ffmpeg.avformat_network_deinit();


            if (_previousExecutionState != 0)
            {
                NativeCalls.SetThreadExecutionState(_previousExecutionState);
            }
            
        }
        catch (Exception ex)
        {
            try
            {
                Logger.LogExceptionToFile(ex);
            } catch
            {
                
            }
            while (ex.InnerException != null)
            {
                try
                {
                    Logger.LogExceptionToFile(ex);
                }
                catch
                {

                }
            }
        }
    }

    private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
    {
        return true;
    } 


    static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {        
        try
        {
            var ex = (Exception)e.ExceptionObject;          
            Logger.LogExceptionToFile(ex);
        }
        catch (Exception ex2)
        {
            try
            {
                Logger.LogExceptionToFile(ex2);
            }
            catch
            {

            }
        }
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

    public static void EnsureInstall(bool reset)
    {

        if (!Directory.Exists(AppDataPath))
        {
            Directory.CreateDirectory(AppDataPath);
        }
        if (!Directory.Exists(AppDataPath + @"XML"))
        {
            Directory.CreateDirectory(AppDataPath + @"XML");
        }

        var didest = new DirectoryInfo(AppDataPath + @"XML\");
        var disource = new DirectoryInfo(AppPath + @"XML\");

        TryCopy(disource + @"PTZ2.xml", didest + @"PTZ2.xml", true);
        TryCopy(disource + @"Translations.xml", didest + @"Translations.xml", true);
        TryCopy(disource + @"Sources.xml", didest + @"Sources.xml", true);

        if (reset || !File.Exists(didest + @"objects.xml"))
        {
            TryCopy(disource + @"objects.xml", didest + @"objects.xml", reset);
        }

        if (reset || !File.Exists(didest + @"config.xml"))
        {
            TryCopy(disource + @"config.xml", didest + @"config.xml", reset);
        }

        if (!Directory.Exists(AppDataPath + @"WebServerRoot"))
        {
            Directory.CreateDirectory(AppDataPath + @"WebServerRoot");
        }
        didest = new DirectoryInfo(AppDataPath + @"WebServerRoot");
        disource = new DirectoryInfo(AppPath + @"WebServerRoot");
        CopyAll(disource, didest);

        if (!Directory.Exists(AppDataPath + @"WebServerRoot\Media"))
            Directory.CreateDirectory(AppDataPath + @"WebServerRoot\Media");
        if (!Directory.Exists(AppDataPath + @"WebServerRoot\Media\Audio"))
            Directory.CreateDirectory(AppDataPath + @"WebServerRoot\Media\Audio");
        if (!Directory.Exists(AppDataPath + @"WebServerRoot\Media\Video"))
            Directory.CreateDirectory(AppDataPath + @"WebServerRoot\Media\Video");

        Directory.SetCurrentDirectory(AppPath);

        //reset layout position
        if (reset)
            Registry.CurrentUser.DeleteSubKey(@"Software\ispy\startup",false);

    }

    private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        // Check if the target directory exists, if not, create it.
        if (Directory.Exists(target.FullName) == false)
        {
            Directory.CreateDirectory(target.FullName);
        }

        // Copy each file into it’s new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            try {fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);} catch
            {
            }
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

    private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(e?.Exception?.Message) && e.Exception.Message.IndexOf("NoDriver", StringComparison.Ordinal)!=-1)
            {
                //USB audio plugged/ unplugged (typically the cause) - no other way to catch this exception in the volume level control due to limitation in NAudio
            }
            if (e!=null)
                Logger.LogExceptionToFile(e.Exception);
        }
        catch (Exception ex2)
        {
            try
            {
                Logger.LogExceptionToFile(ex2);
            }
            catch
            {
                
            }
        }
    }
}