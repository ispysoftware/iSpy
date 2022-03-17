using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

        var version = Environment.OSVersion.Version;
        bool canrun = true;
        switch (version.Major)
        {
            case 5:
                canrun = false;
                break;
            case 6:
                switch (version.Minor)
                {
                    case 0:
                        canrun = false;
                        break;
                    
                }
                break;
        }
        if (!canrun)
        {
            MessageBox.Show("iSpy is not supported on this operating system. Please uninstall and download v6.5.8.0 instead. Your settings will be saved.");
            Process.Start("http://www.ispyconnect.com/download.aspx");
            return;
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
                    Logger.LogException(ex, "startup");
                }
            }

            VlcHelper.FindVLC();

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
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 1000;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            FfmpegMutex = new Mutex();
            
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
           

            _previousExecutionState = NativeCalls.SetThreadExecutionState(NativeCalls.EsContinuous | NativeCalls.EsSystemRequired);

            AppIdle = new WinFormsAppIdleHandler();
            var mf = new MainForm(silentstartup, command);
            GC.KeepAlive(FfmpegMutex);
            
            Application.Run(mf);
            FfmpegMutex.Close();

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
                Logger.LogException(ex);
            } catch
            {
                
            }
            while (ex.InnerException != null)
            {
                try
                {
                    Logger.LogException(ex);
                }
                catch
                {

                }
                ex = ex.InnerException;
            }
        }
        try
        {
            Logger.WriteLogs();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static AvFormatLogCallback _avLogCallback;
    private static IntPtr _delegatePtr;
    private static av_log_set_callback_callback_func _avlog;
    public static unsafe void SetFfmpegLogging()
    {
#if DEBUG
        ffmpeg.av_log_set_level(ffmpeg.AV_LOG_ERROR);
#else
        ffmpeg.av_log_set_level(ffmpeg.AV_LOG_ERROR);
#endif
        _avLogCallback = AvFormatLogFunc;
        _delegatePtr = Marshal.GetFunctionPointerForDelegate(_avLogCallback);
        _avlog = new av_log_set_callback_callback_func
                 {
                     Pointer = _delegatePtr
                 };

        ffmpeg.av_log_set_callback(_avlog);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public unsafe delegate void AvFormatLogCallback(void* ptr, int level, string fmt, byte* vl);
    public static unsafe void AvFormatLogFunc(void* ptr, int level, string fmt, byte* vl)
    {
        if (level > ffmpeg.av_log_get_level()) { return; }
        fmt = fmt.Trim();
        if (string.IsNullOrEmpty(fmt)) { return; }
        byte[] buffer = new byte[1024];
        string s = "";
        fixed (byte* p = buffer)
        {
            int printPrefix = 1;
            ffmpeg.av_log_format_line(ptr, level, fmt, vl, p, buffer.Length, &printPrefix);
            s = Encoding.UTF8.GetString(buffer);

            int pos = s.IndexOf('\0');
            if (pos >= 0)
                s = s.Substring(0, pos);
        }
        if (!string.IsNullOrEmpty(s))
            Debug.WriteLine(s);
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
            Logger.LogException(ex);
        }
        catch (Exception ex2)
        {
            try
            {
                Logger.LogException(ex2);
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
                Logger.LogException(e.Exception);
        }
        catch (Exception ex2)
        {
            try
            {
                Logger.LogException(ex2);
            }
            catch
            {
                
            }
        }
    }

    public static class MutexHelper
    {
        private static bool _enableMutex = false;
        private static Mutex _mutex;

        private static Mutex FfmpegMutex
        {
            get
            {
                if (_mutex != null)
                    return _mutex;
                _mutex = new Mutex();
                return _mutex;
            }
        }
        public static void Wait()
        {
            if (_enableMutex)
                FfmpegMutex.WaitOne();
        }

        public static void Release()
        {
            if (_enableMutex)
                FfmpegMutex.ReleaseMutex();
        }

        public static void Close()
        {
            if (_enableMutex)
                FfmpegMutex.Close();
        }
    }
}