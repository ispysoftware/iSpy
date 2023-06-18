using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Cloud;
using iSpyApplication.Server;
using Microsoft.Win32;
using NAudio.Wave;
using iSpyApplication.Controls;
using iSpyApplication.Joystick;
using iSpyApplication.Utilities;
using Encoder = System.Drawing.Imaging.Encoder;

namespace iSpyApplication
{
    public partial class Settings : Form
    {
        public static readonly object[] StartupModes = 
            {
                "Normal","Minimised","Maximised","FullScreen"
            };

        public static readonly object[] PlaybackModes =
        {
            "Website", "iSpy", "Default"
        };
        public static readonly object[] CloudProviders = 
            {
                "Drive","Dropbox","Flickr","OneDrive","Box"
            };

        public static readonly object[] Priorities =
        {
            "Normal","AboveNormal","High","RealTime"
        };
        private const int Rgbmax = 255;
        private JoystickDevice _jst;
        public int InitialTab;
        public bool ReloadResources;
        readonly string _noDevices = LocRm.GetString("NoAudioDevices");
        private RegistryKey _rkApp;
        private string[] _sticks;
        private static readonly object Jslock = new object();
        private bool _loaded;
        public MainForm MainClass;
        public FolderSelectDialog Fsd = new FolderSelectDialog();

        public Settings()
        {
            InitializeComponent();
            RenderResources();            
        }

        private void Button1Click(object sender, EventArgs e)
        {
            string err = "";

            foreach (var s in mediaDirectoryEditor1.Directories)
            {
                if (!Directory.Exists(s.Entry))
                {
                    err += LocRm.GetString("Validate_MediaDirectory") + " ("+s.Entry+")\n";
                    break;
                }
            }
            
            if (err != "")
            {
                MessageBox.Show(err, LocRm.GetString("Error"));
                return;
            }

            if (numJPEGQuality.Value != MainForm.Conf.JPEGQuality)
            {
                MainForm.EncoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (int) numJPEGQuality.Value);
            }
            MainForm.Conf.Enable_Error_Reporting = chkErrorReporting.Checked;
            MainForm.Conf.Enable_Update_Check = chkCheckForUpdates.Checked;
            MainForm.Conf.Enable_Password_Protect = chkPasswordProtect.Checked;

            MainForm.Conf.Is_Silent_Startup_Check = chkIsSilentOnStartup.Checked;

            MainForm.Conf.NoActivityColor = btnNoDetectColor.BackColor.ToRGBString();
            MainForm.Conf.ActivityColor = btnDetectColor.BackColor.ToRGBString();
            MainForm.Conf.TrackingColor = btnColorTracking.BackColor.ToRGBString();
            MainForm.Conf.VolumeLevelColor = btnColorVolume.BackColor.ToRGBString();
            MainForm.Conf.MainColor = btnColorMain.BackColor.ToRGBString();
            MainForm.Conf.AreaColor = btnColorArea.BackColor.ToRGBString();
            MainForm.Conf.BackColor = btnColorBack.BackColor.ToRGBString();
            MainForm.Conf.BorderHighlightColor = btnBorderHighlight.BackColor.ToRGBString();
            MainForm.Conf.BorderDefaultColor = btnBorderDefault.BackColor.ToRGBString();

            MainForm.Conf.Enabled_ShowGettingStarted = chkShowGettingStarted.Checked;
            MainForm.Conf.Opacity = tbOpacity.Value;
            MainForm.Conf.OpenGrabs = chkOpenGrabs.Checked;
            MainForm.Conf.BalloonTips = chkBalloon.Checked;
            MainForm.Conf.TrayIconText = txtTrayIcon.Text;
            MainForm.Conf.IPCameraTimeout = Convert.ToInt32(txtIPCameraTimeout.Value);
            MainForm.Conf.ServerReceiveTimeout = Convert.ToInt32(txtServerReceiveTimeout.Value);
            MainForm.Conf.ServerName = txtServerName.Text;
            MainForm.Conf.AutoSchedule = chkAutoSchedule.Checked;
            MainForm.Conf.CPUMax = Convert.ToInt32(numMaxCPU.Value);
            MainForm.Conf.MaxRecordingThreads = (int)numMaxRecordingThreads.Value;
            MainForm.Conf.CreateAlertWindows = chkAlertWindows.Checked;
            MainForm.Conf.MaxRedrawRate = (int)numRedraw.Value;
            MainForm.Conf.Priority = ddlPriority.SelectedIndex + 1;
            MainForm.Conf.Monitor = chkMonitor.Checked;
            MainForm.Conf.ScreensaverWakeup = chkInterrupt.Checked;
            MainForm.Conf.PlaybackMode = ddlPlayback.SelectedIndex;
            MainForm.Conf.PreviewItems = (int)numMediaPanelItems.Value;
            MainForm.Conf.BigButtons = chkBigButtons.Checked;
            MainForm.Conf.DeleteToRecycleBin = chkRecycle.Checked;
            MainForm.Conf.SpeechRecognition = chkSpeechRecognition.Checked;
            MainForm.Conf.AppendLinkText = txtAppendLinkText.Text;
            MainForm.Conf.StartupForm = ddlStartUpForm.SelectedItem.ToString();
            MainForm.Conf.TrayOnMinimise = chkMinimiseToTray.Checked;
            MainForm.Conf.MJPEGStreamInterval = (int)numMJPEGStreamInterval.Value;
            MainForm.Conf.AlertOnDisconnect = txtAlertOnDisconnect.Text;
            MainForm.Conf.AlertOnReconnect = txtAlertOnReconnect.Text;
            MainForm.Conf.StartupMode = ddlStartupMode.SelectedIndex;
            MainForm.Conf.EnableGZip = chkGZip.Checked;
            MainForm.Conf.DisconnectNotificationDelay = (int)numDisconnectNotification.Value;
            var l = mediaDirectoryEditor1.Directories.ToList();
            MainForm.Conf.MediaDirectories = l.ToArray();
            var l2 = ftpEditor1.Servers.ToList();
            MainForm.Conf.FTPServers = l2.ToArray();
            MainForm.Conf.MailAlertSubject = txtAlertSubject.Text;
            MainForm.Conf.MailAlertBody = txtAlertBody.Text;
            MainForm.Conf.SMSAlert = txtSMSBody.Text;
            MainForm.Conf.VLCFileCache = (int)numFileCache.Value;
            MainForm.Conf.Password_Protect_Startup = chkPasswordProtectOnStart.Checked;
            MainForm.Conf.BrandPath = lblBrand.Text;
            SaveSMTPSettings();

            MainForm.Conf.ArchiveNew = txtArchive.Text.Trim();
            if (!string.IsNullOrEmpty(MainForm.Conf.ArchiveNew))
            {
                if (!MainForm.Conf.ArchiveNew.EndsWith(@"\"))
                    MainForm.Conf.ArchiveNew += @"\";
            }

            MainForm.Iconfont = new Font(FontFamily.GenericSansSerif, MainForm.Conf.BigButtons ? 22 : 15, FontStyle.Bold, GraphicsUnit.Pixel);
            
            MainForm.Conf.TalkMic = "";
            if (ddlTalkMic.Enabled)
            {
                if (ddlTalkMic.SelectedIndex>0)
                    MainForm.Conf.TalkMic = ddlTalkMic.SelectedItem.ToString();
            }

            MainForm.Conf.MinimiseOnClose = chkMinimise.Checked;
            MainForm.Conf.JPEGQuality = (int) numJPEGQuality.Value;
            MainForm.Conf.IPv6Disabled = !chkEnableIPv6.Checked;

            MainForm.SetPriority();

            var ips = rtbAccessList.Text.Trim().Split(',');
            var t = ips.Select(ip => ip.Trim()).Where(ip2 => ip2 != "").Aggregate("", (current, ip2) => current + (ip2 + ","));
            MainForm.Conf.AllowedIPList = t.Trim(',');
            LocalServer.ReloadAllowedIPs();

            var refs = rtbReferrers.Text.Trim().Split(',');
            var t2 = refs.Select(ip => ip.Trim()).Where(ip2 => ip2 != "").Aggregate("", (current, ip2) => current + (ip2 + ","));
            MainForm.Conf.Referers = t2.Trim(',');
            LocalServer.ReloadAllowedReferrers();

            
            MainForm.Conf.ShowOverlayControls = chkOverlay.Checked;

            string lang = ((ListItem) ddlLanguage.SelectedItem).Value[0];
            if (lang != MainForm.Conf.Language)
            {
                ReloadResources = true;
                LocRm.Reset();
            }
            MainForm.Conf.Language = lang;
            if (chkStartup.Checked)
            {
                if (chkIsSilentOnStartup.Checked)
                {
                    try
                    {
                        _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                        _rkApp?.SetValue("iSpy", "\"" + Application.ExecutablePath + "\" -silent", RegistryValueKind.String);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Logger.LogException(ex);
                    }
                }
                else
                {
                    try
                    {
                        _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                        _rkApp?.SetValue("iSpy", "\"" + Application.ExecutablePath + "\" ", RegistryValueKind.String);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Logger.LogException(ex);
                    }
                }
            }
            else
            {
                try
                {
                    _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    _rkApp?.DeleteValue("iSpy", false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Logger.LogException(ex);
                }
            }

            //SetStorageOptions();

            MainForm.ReloadColors();

            if (ddlJoystick.SelectedIndex > 0)
            {
                string nameid = _sticks[ddlJoystick.SelectedIndex - 1];
                MainForm.Conf.Joystick.id = nameid.Split('|')[1];

                MainForm.Conf.Joystick.XAxis = jaxis1.ID;

                MainForm.Conf.Joystick.InvertXAxis = jaxis1.Invert;

                MainForm.Conf.Joystick.YAxis = jaxis2.ID;
                MainForm.Conf.Joystick.InvertYAxis = jaxis2.Invert;

                MainForm.Conf.Joystick.ZAxis = jaxis3.ID;
                MainForm.Conf.Joystick.InvertZAxis = jaxis3.Invert;

                MainForm.Conf.Joystick.Record = jbutton1.ID;
                MainForm.Conf.Joystick.Snapshot = jbutton2.ID;
                MainForm.Conf.Joystick.Talk = jbutton3.ID;
                MainForm.Conf.Joystick.Listen = jbutton4.ID;
                MainForm.Conf.Joystick.Play = jbutton5.ID;
                MainForm.Conf.Joystick.Next = jbutton6.ID;
                MainForm.Conf.Joystick.Previous = jbutton7.ID;
                MainForm.Conf.Joystick.Stop = jbutton8.ID;
                MainForm.Conf.Joystick.MaxMin = jbutton9.ID;
                MainForm.Conf.Joystick.PTSpeedProfile = jbutton10.ID;
            }
            else
                MainForm.Conf.Joystick.id = "";

            MainForm.Conf.Logging.Enabled = chkEnableLogging.Checked;
            MainForm.Conf.Logging.FileSize = (int)numMaxLogSize.Value;
            MainForm.Conf.Logging.KeepDays = (int)numKeepLogs.Value;
          
            DialogResult = DialogResult.OK;
            Close();
        }

        private jbutton _curButton;
        private jaxis _curAxis;

        void JbuttonGetInput(object sender, EventArgs e)
        {
            jbutton1.Reset();
            jbutton2.Reset();
            jbutton3.Reset();
            jbutton4.Reset();
            jbutton5.Reset();
            jbutton6.Reset();
            jbutton7.Reset();
            jbutton8.Reset();
            jbutton9.Reset();
            jbutton10.Reset();

            if (sender!=null)
                _curButton = (jbutton) sender;
            else
            {
                _curButton = null;
            }
        }

        void JaxisGetInput(object sender, EventArgs e)
        {
            jaxis1.Reset();
            jaxis2.Reset();
            jaxis3.Reset();

            if (sender!=null)
                _curAxis = (jaxis)sender;
            else
            {
                _curAxis = null;
            }
        }

        

        private void Button2Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsLoad(object sender, EventArgs e)
        {
            if (!Helper.HasFeature(Enums.Features.Settings))
            {
                using (var cp = new CheckPassword())
                {
                    cp.ShowDialog(this);
                }
            }

            if (!Helper.HasFeature(Enums.Features.Settings))
            {
                MessageBox.Show(this, LocRm.GetString("AccessDenied"));
                Close();
                return;
            }

            UISync.Init(this);
            tcTabs.SelectedIndex = InitialTab;
            chkErrorReporting.Checked = MainForm.Conf.Enable_Error_Reporting;
            chkCheckForUpdates.Checked = MainForm.Conf.Enable_Update_Check;
            
            chkShowGettingStarted.Checked = MainForm.Conf.Enabled_ShowGettingStarted;
            _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);
            chkStartup.Checked = (_rkApp != null && _rkApp.GetValue("iSpy") != null);

            mediaDirectoryEditor1.Init(MainForm.Conf.MediaDirectories);           

            btnDetectColor.BackColor = MainForm.Conf.ActivityColor.ToColor();
            btnNoDetectColor.BackColor = MainForm.Conf.NoActivityColor.ToColor();
            btnColorTracking.BackColor = MainForm.Conf.TrackingColor.ToColor();
            btnColorVolume.BackColor = MainForm.Conf.VolumeLevelColor.ToColor();
            btnColorMain.BackColor = MainForm.Conf.MainColor.ToColor();
            btnColorArea.BackColor = MainForm.Conf.AreaColor.ToColor();
            btnColorBack.BackColor = MainForm.Conf.BackColor.ToColor();
            btnBorderHighlight.BackColor = MainForm.Conf.BorderHighlightColor.ToColor();
            btnBorderDefault.BackColor = MainForm.Conf.BorderDefaultColor.ToColor();
            chkAutoSchedule.Checked = MainForm.Conf.AutoSchedule;
            numMaxCPU.Value = MainForm.Conf.CPUMax;
            numMaxRecordingThreads.Value = MainForm.Conf.MaxRecordingThreads;
            numRedraw.Value = MainForm.Conf.MaxRedrawRate;
            numMediaPanelItems.Value = MainForm.Conf.PreviewItems;
            txtTrayIcon.Text = MainForm.Conf.TrayIconText;
            chkMinimise.Checked = MainForm.Conf.MinimiseOnClose;
            chkSpeechRecognition.Checked = MainForm.Conf.SpeechRecognition;
            chkMinimiseToTray.Checked = MainForm.Conf.TrayOnMinimise;
            lblBrand.Text = MainForm.Conf.BrandPath;

            if (chkMonitor.Checked && !MainForm.Conf.Monitor)
            {
                Process.Start(Program.AppPath + "iSpyMonitor.exe");
            }
            chkMonitor.Checked = MainForm.Conf.Monitor;

            tbOpacity.Value = MainForm.Conf.Opacity;
            SetColors();

            chkBalloon.Checked = MainForm.Conf.BalloonTips;

            txtIPCameraTimeout.Value = MainForm.Conf.IPCameraTimeout;
            txtServerReceiveTimeout.Value = MainForm.Conf.ServerReceiveTimeout;
            txtServerName.Text = MainForm.Conf.ServerName;
            rtbAccessList.Text = MainForm.Conf.AllowedIPList;

            int i = 0, selind = 0;
            foreach (TranslationsTranslationSet set in LocRm.TranslationSets.OrderBy(p => p.Name))
            {
                ddlLanguage.Items.Add(new ListItem(set.Name, new[] {set.CultureCode}));
                if (set.CultureCode == MainForm.Conf.Language)
                    selind = i;
                i++;
            }
            ddlLanguage.SelectedIndex = selind;
            chkAlertWindows.Checked = MainForm.Conf.CreateAlertWindows;
            chkOverlay.Checked = MainForm.Conf.ShowOverlayControls;
            chkInterrupt.Checked = MainForm.Conf.ScreensaverWakeup;
            chkEnableIPv6.Checked = !MainForm.Conf.IPv6Disabled;
            chkRecycle.Checked = MainForm.Conf.DeleteToRecycleBin;
            txtAppendLinkText.Text = MainForm.Conf.AppendLinkText;
            numMJPEGStreamInterval.Value = MainForm.Conf.MJPEGStreamInterval;
            txtAlertOnDisconnect.Text = MainForm.Conf.AlertOnDisconnect;
            txtAlertOnReconnect.Text = MainForm.Conf.AlertOnReconnect;
            txtArchive.Text = MainForm.Conf.ArchiveNew;
            SetSSLText();
            

            txtAlertSubject.Text = MainForm.Conf.MailAlertSubject;
            txtAlertBody.Text = MainForm.Conf.MailAlertBody;
            txtSMSBody.Text = MainForm.Conf.SMSAlert;

            foreach (string s in StartupModes)
            {
                ddlStartupMode.Items.Add(LocRm.GetString(s));
            }

            foreach (string s in Priorities)
            {
                ddlPriority.Items.Add(LocRm.GetString(s));
            }
            ddlStartupMode.SelectedIndex = MainForm.Conf.StartupMode;

            foreach(var grid in MainForm.Conf.GridViews)
            {
                ddlStartUpForm.Items.Add(grid.name);
            }

            ddlPriority.SelectedIndex = MainForm.Conf.Priority - 1;

            ddlStartUpForm.SelectedItem = MainForm.Conf.StartupForm;
            if (ddlStartUpForm.SelectedItem==null)
                ddlStartUpForm.SelectedIndex = 0;

            ddlPlayback.Items.AddRange(PlaybackModes);
            
            if (MainForm.Conf.PlaybackMode < 0)
                MainForm.Conf.PlaybackMode = 0;

            if (MainForm.Conf.PlaybackMode<ddlPlayback.Items.Count)
                ddlPlayback.SelectedIndex = MainForm.Conf.PlaybackMode;
            try
            {
                numJPEGQuality.Value = MainForm.Conf.JPEGQuality;
            }
            catch (Exception)
            {
                
            }
            chkBigButtons.Checked = MainForm.Conf.BigButtons;

            selind = -1;
            i = 1;
            try
            {
                ddlTalkMic.Items.Add(LocRm.GetString("None"));

                for (int n = 0; n < WaveIn.DeviceCount; n++)
                {
                    ddlTalkMic.Items.Add(WaveIn.GetCapabilities(n).ProductName);
                    if (WaveIn.GetCapabilities(n).ProductName == MainForm.Conf.TalkMic)
                        selind = i;
                    i++;

                }
                ddlTalkMic.Enabled = true;
                if (selind > -1)
                    ddlTalkMic.SelectedIndex = selind;
                else
                {
                    if (ddlTalkMic.Items.Count == 1)
                    {
                        ddlTalkMic.Items.Add(_noDevices);
                        ddlTalkMic.Enabled = false;
                        ddlTalkMic.SelectedIndex = 1;
                    }
                    else
                        ddlTalkMic.SelectedIndex = 0;
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogException(ex);
                ddlTalkMic.Items.Add(_noDevices);
                ddlTalkMic.Enabled = false;
            }

            ddlJoystick.Items.Add(LocRm.GetString("None"));

            _jst = new JoystickDevice();
            var ij = 0;
            _sticks = _jst.FindJoysticks();
            i = 1;
            foreach(string js in _sticks)
            {
                var nameid = js.Split('|');
                ddlJoystick.Items.Add(nameid[0]);
                if (nameid[1] == MainForm.Conf.Joystick.id)
                    ij = i;
                i++;
            }

            ddlJoystick.SelectedIndex = ij;


            jaxis1.ID = MainForm.Conf.Joystick.XAxis;
            jaxis1.SupportDPad = true;
            jaxis1.Invert = MainForm.Conf.Joystick.InvertXAxis;

            jaxis2.ID = MainForm.Conf.Joystick.YAxis;
            jaxis2.Invert = MainForm.Conf.Joystick.InvertYAxis;

            jaxis3.ID = MainForm.Conf.Joystick.ZAxis;
            jaxis3.Invert = MainForm.Conf.Joystick.InvertZAxis;

            jbutton1.ID = MainForm.Conf.Joystick.Record;
            jbutton2.ID = MainForm.Conf.Joystick.Snapshot;
            jbutton3.ID = MainForm.Conf.Joystick.Talk;
            jbutton4.ID = MainForm.Conf.Joystick.Listen;
            jbutton5.ID = MainForm.Conf.Joystick.Play;
            jbutton6.ID = MainForm.Conf.Joystick.Next;
            jbutton7.ID = MainForm.Conf.Joystick.Previous;
            jbutton8.ID = MainForm.Conf.Joystick.Stop;
            jbutton9.ID = MainForm.Conf.Joystick.MaxMin;
            jbutton10.ID = MainForm.Conf.Joystick.PTSpeedProfile;

            jbutton1.GetInput += JbuttonGetInput;
            jbutton2.GetInput += JbuttonGetInput;
            jbutton3.GetInput += JbuttonGetInput;
            jbutton4.GetInput += JbuttonGetInput;
            jbutton5.GetInput += JbuttonGetInput;
            jbutton6.GetInput += JbuttonGetInput;
            jbutton7.GetInput += JbuttonGetInput;
            jbutton8.GetInput += JbuttonGetInput;
            jbutton9.GetInput += JbuttonGetInput;
            jbutton10.GetInput += JbuttonGetInput;

            jaxis1.GetInput += JaxisGetInput;
            jaxis2.GetInput += JaxisGetInput;
            jaxis3.GetInput += JaxisGetInput;

            chkGZip.Checked = MainForm.Conf.EnableGZip;
            numDisconnectNotification.Value = MainForm.Conf.DisconnectNotificationDelay;
            mediaDirectoryEditor1.Enabled = Helper.HasFeature(Enums.Features.Storage);
            HideTab(tabPage11, Helper.HasFeature(Enums.Features.Plugins));

            //important leave here:
            chkPasswordProtect.Checked = MainForm.Conf.Enable_Password_Protect;
            if (Helper.HasFeature(Enums.Features.Plugins))
                ListPlugins();

            chkUseiSpy.Checked = !MainForm.Conf.UseSMTP;
            txtSMTPFromAddress.Text = MainForm.Conf.SMTPFromAddress;
            txtSMTPUsername.Text = MainForm.Conf.SMTPUsername;
            txtSMTPPassword.Text = MainForm.Conf.SMTPPassword;
            txtSMTPServer.Text = MainForm.Conf.SMTPServer;
            chkSMTPUseSSL.Checked = MainForm.Conf.SMTPSSL;
            numSMTPPort.Value = MainForm.Conf.SMTPPort;

            ftpEditor1.Init(MainForm.Conf.FTPServers);
            chkOpenGrabs.Checked = MainForm.Conf.OpenGrabs;
            numFileCache.Value = MainForm.Conf.VLCFileCache;
            rtbReferrers.Text = MainForm.Conf.Referers;
            chkPasswordProtectOnStart.Checked = MainForm.Conf.Password_Protect_Startup;

            chkEnableLogging.Checked = MainForm.Conf.Logging.Enabled;
            numMaxLogSize.Value = MainForm.Conf.Logging.FileSize;
            numKeepLogs.Value = MainForm.Conf.Logging.KeepDays;

            chkIsSilentOnStartup.Checked = MainForm.Conf.Is_Silent_Startup_Check;


            _loaded = true;
        }

        private void HideTab(TabPage t, bool show)
        {
            if (!show)
            {
                tcTabs.TabPages.Remove(t);
            }
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("settings");
            btnColorArea.Text = LocRm.GetString("AreaHighlight");
            btnColorBack.Text = LocRm.GetString("ObjectBack");
            btnColorMain.Text = LocRm.GetString("MainPanel");
            btnColorTracking.Text = LocRm.GetString("Tracking");
            btnBorderHighlight.Text = LocRm.GetString("BorderHighlight");
            btnColorVolume.Text = LocRm.GetString("Level");
            btnDetectColor.Text = LocRm.GetString("Activity");
            btnNoDetectColor.Text = LocRm.GetString("NoActivity");
            button1.Text = LocRm.GetString("Ok");
            button2.Text = LocRm.GetString("Cancel");
            chkBalloon.Text = LocRm.GetString("ShowBalloonTips");
            chkCheckForUpdates.Text = LocRm.GetString("AutomaticallyCheckForUpda");
            chkErrorReporting.Text = LocRm.GetString("AnonymousErrorReporting");
            chkPasswordProtect.Text = LocRm.GetString("PasswordProtectWhenMinimi");
            chkShowGettingStarted.Text = LocRm.GetString("ShowGettingStarted");
            chkStartup.Text = LocRm.GetString("RunOnStartupthisUserOnly");
            chkIsSilentOnStartup.Text = LocRm.GetString("IsSILENTOnStartupthisUserOnly");
            chkAutoSchedule.Text = LocRm.GetString("AutoApplySchedule");
            chkPasswordProtectOnStart.Text = LocRm.GetString("PasswordProtectOnStart");
            
            label14.Text = LocRm.GetString("IspyServerName");
            label16.Text = LocRm.GetString("ispyOpacitymayNotW");
            label2.Text = LocRm.GetString("ServerReceiveTimeout");
            label21.Text = LocRm.GetString("TrayIconText");
            label3.Text = LocRm.GetString("MediaDirectory");
            label4.Text = "ms";
            label7.Text = "ms";
            label42.Text = "ms";
            label49.Text = "ms";
            label62.Text = "s";
            label69.Text = VlcHelper.VLCLocation;

            label8.Text = LocRm.GetString("MjpegReceiveTimeout");
            label47.Text = LocRm.GetString("StartupMode");

            label18.Text = LocRm.GetString("MaxRecordingThreads");
            label13.Text = LocRm.GetString("PlaybackMode");
            tabPage1.Text = LocRm.GetString("Colors");
            tabPage2.Text = LocRm.GetString("Storage");
            tabPage4.Text = LocRm.GetString("Timeouts");
            tabPage6.Text = LocRm.GetString("options");
            tabPage7.Text = LocRm.GetString("IPAccess");
            Text = LocRm.GetString("settings");
            chkAlertWindows.Text = LocRm.GetString("CreateAlertWindow");
            chkOverlay.Text = LocRm.GetString("ShowOverlayControls");
            lblPriority.Text = LocRm.GetString("Priority");
            chkInterrupt.Text = LocRm.GetString("InterruptScreensaverOnAlert");
            label23.Text = LocRm.GetString("JPEGQuality");
            llblHelp.Text = LocRm.GetString("help");
            label17.Text = LocRm.GetString("IPAccessExplainer");
            chkMonitor.Text = LocRm.GetString("RestartIfCrashed");
            chkGZip.Text = LocRm.GetString("Enable GZip");
            label40.Text = LocRm.GetString("Permissions");
            label24.Text = LocRm.GetString("MediaPanelItems");
            label11.Text = LocRm.GetString("ArchiveDirectory");
            label48.Text = LocRm.GetString("DisconnectionNotificationDelay");
            label41.Text = LocRm.GetString("MJPEGFrameInterval");
            label20.Text = LocRm.GetString("VLCFileCache");
            label64.Text = LocRm.GetString("HTTPReferrersAllowed");

            LocRm.SetString(lblMicrophone, "Microphone");
            LocRm.SetString(chkBigButtons, "BigButtons");
            LocRm.SetString(chkMinimise, "MinimiseOnClose");
            LocRm.SetString(chkRecycle, "DeleteToRecycle");
            LocRm.SetString(chkEnableIPv6,"EnableIPv6");
            LocRm.SetString(label15, "MaxCPUTarget");
            LocRm.SetString(label22, "MaxRedrawRate");
            LocRm.SetString(btnBorderDefault, "BorderDefault");
            LocRm.SetString(label25,"YouCanUseRegularExpressions");
            LocRm.SetString(tabPage5,"Talk");
            LocRm.SetString(tabPage8, "Joystick");
            LocRm.SetString(label26, "Joystick");
            LocRm.SetString(tabPage9, "Messaging");
            LocRm.SetString(label19, "AppendLinkText");

            LocRm.SetString(label28, "PanAxis");
            LocRm.SetString(label30, "TiltAxis");
            LocRm.SetString(label32, "ZoomAxis");
            LocRm.SetString(btnCenterAxes, "CenterAxes");


            LocRm.SetString(label34, "Record");
            LocRm.SetString(label29, "Snapshot");
            LocRm.SetString(label27, "Talk");
            LocRm.SetString(label31, "Listen");
            LocRm.SetString(label33, "Play");
            LocRm.SetString(label37, "Stop");
            LocRm.SetString(label35, "Next");
            LocRm.SetString(label36, "Previous");
            LocRm.SetString(label38, "JoystickNote");
            LocRm.SetString(label39, "StartupForm");
            LocRm.SetString(chkMinimiseToTray, "MinimiseToTray");

            LocRm.SetString(label56, "EmailNotifications");
            LocRm.SetString(label50, "EmailSubject");
            LocRm.SetString(label51, "EmailBody");
            LocRm.SetString(label19, "AppendLinkText");
            LocRm.SetString(label57, "SMSNotifications");
            LocRm.SetString(label54, "Message");
            LocRm.SetString(linkLabel5, "Reset");

            chkUseiSpy.Text = LocRm.GetString("UseISpyServers");
            label52.Text = LocRm.GetString("FromAddress");
            label58.Text = LocRm.GetString("Username");

            LocRm.SetString(label59, "Password");
            LocRm.SetString(label53, "Server");
            LocRm.SetString(label61, "Port");
            LocRm.SetString(chkSMTPUseSSL, "UseSSL");
            LocRm.SetString(btnTestSMTP, "Test");
            LocRm.SetString(label43, "WhenDisconnectedFromWebServices");
            LocRm.SetString(label45,"Execute");
            LocRm.SetString(label46, "Execute");
            LocRm.SetString(label44, "WhenReconnectedToWebServices");
            LocRm.SetString(label10, "Plugins");
            LocRm.SetString(linkLabel3, "DownloadPlugins");
            LocRm.SetString(linkLabel4, "RefreshList");
            LocRm.SetString(label12, "ArchiveLocation");
            LocRm.SetString(button3, "RunNow");

            LocRm.SetString(label66, "MaxLogSize");
            LocRm.SetString(label65, "KeepLogsFor");
            LocRm.SetString(label67, "Days");

            tabPage10.Text = LocRm.GetString("ConnectionAlerts");
            tabPage11.Text = LocRm.GetString("Plugins");
            tabPage14.Text = LocRm.GetString("Logging");
            label9.Text = LocRm.GetString("MaximseAndRestore");
            labelJButtn10.Text = LocRm.GetString("PTSpeedProfile");
            label60.Text = LocRm.GetString("SSLCertificate");
            //future
            chkSpeechRecognition.Visible = false;
            label63.Text = LocRm.GetString("Servers");
            chkOpenGrabs.Text = LocRm.GetString("OpenImagesAfterSaving");
            chkEnableLogging.Text = LocRm.GetString("Enable");
            numKeepLogs.Text = LocRm.GetString("KeepLogsForDays");
            numMaxLogSize.Text = LocRm.GetString("MaxFileSizeKB");
            llblHelp.Visible = Helper.HasFeature(Enums.Features.View_Ispy_Links);
        }


        private void SetColors()
        {
            btnDetectColor.ForeColor = InverseColor(btnDetectColor.BackColor);
            btnNoDetectColor.ForeColor = InverseColor(btnNoDetectColor.BackColor);
            btnColorTracking.ForeColor = InverseColor(btnColorTracking.BackColor);
            btnColorVolume.ForeColor = InverseColor(btnColorVolume.BackColor);
            btnColorMain.ForeColor = InverseColor(btnColorMain.BackColor);
            btnColorArea.ForeColor = InverseColor(btnColorArea.BackColor);
            btnColorBack.ForeColor = InverseColor(btnColorBack.BackColor);
            btnBorderHighlight.ForeColor = InverseColor(btnBorderHighlight.BackColor);
            btnBorderDefault.ForeColor = InverseColor(btnBorderDefault.BackColor);
        }

        private static Color InverseColor(Color colorIn)
        {
            return Color.FromArgb(Rgbmax - colorIn.R,
                                  Rgbmax - colorIn.G, Rgbmax - colorIn.B);
        }

        private void chkStartup_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void BtnBrowseVideoClick(object sender, EventArgs e)
        {
            
        }

        private void Button3Click(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnNoDetectColor.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnNoDetectColor.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void BtnDetectColorClick(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnDetectColor.BackColor;

            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnDetectColor.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void BtnColorTrackingClick(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnColorTracking.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnColorTracking.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void BtnColorVolumeClick(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnColorVolume.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnColorVolume.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void BtnColorMainClick(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnColorMain.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnColorMain.BackColor = cdColorChooser.Color;
                MainForm.Conf.MainColor = btnColorMain.BackColor.ToRGBString();
                SetColors();
                MainClass.SetBackground();
            }
        }

        private void BtnColorBackClick(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnColorBack.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnColorBack.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void BtnColorAreaClick(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnColorArea.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnColorArea.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void chkPasswordProtect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPasswordProtect.Checked)
            {
                ddlStartupMode.SelectedIndex = 1;
                ddlStartupMode.Enabled = false;
            }
            else
            {
                ddlStartupMode.Enabled = true;
            }
        }

        private void TbOpacityScroll(object sender, EventArgs e)
        {
            MainClass.Opacity = Convert.ToDouble(tbOpacity.Value) / 100;
        }

        private void chkErrorReporting_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkShowGettingStarted_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            _jst?.ReleaseJoystick();
        }

        private void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LinkLabel1LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void ReloadLanguages()
        {           
            ddlLanguage.Items.Clear();
            RenderResources();
            int i = 0, selind = 0;
            foreach (TranslationsTranslationSet set in LocRm.TranslationSets.OrderBy(p => p.Name))
            {
                ddlLanguage.Items.Add(new ListItem(set.Name, new[] { set.CultureCode }));
                if (set.CultureCode == MainForm.Conf.Language)
                    selind = i;
                i++;
            }
            ddlLanguage.SelectedIndex = selind;
            ReloadResources = true;
        }

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try { _sync.BeginInvoke(action, null); }
                catch { }
            }
        }


        #region Nested type: ListItem

        private struct ListItem
        {
            private readonly string _name;
            internal readonly string[] Value;

            public ListItem(string name, string[] value)
            {
                _name = name;
                Value = value;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        #endregion

        private void llblHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/userguide-settings.aspx");
        }


        private void chkMonitor_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnBorderHighlight_Click(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnBorderHighlight.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnBorderHighlight.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cdColorChooser.Color = btnBorderDefault.BackColor;
            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                btnBorderDefault.BackColor = cdColorChooser.Color;
                SetColors();
            }
        }

        private void ddlJoystick_SelectedIndexChanged(object sender, EventArgs e)
        {
            tblJoystick.Enabled = ddlJoystick.SelectedIndex > 0;

            jaxis1.ID = 0;
            jaxis2.ID = 0;
            jaxis3.ID = 0;

            jbutton1.ID = 0;
            jbutton2.ID = 0;
            jbutton3.ID = 0;
            jbutton4.ID = 0;
            jbutton5.ID = 0;
            jbutton6.ID = 0;
            jbutton7.ID = 0;

            _curButton = null;


            if (tblJoystick.Enabled && _jst!=null)
            {
                string nameid = _sticks[ddlJoystick.SelectedIndex - 1];
                Guid g = Guid.Parse(nameid.Split('|')[1]);
                _jst.ReleaseJoystick();

                if (_jst.AcquireJoystick(g))
                {
                    lock (Jslock)
                    {
                        _axisLast = new int[_jst.Axis.Length];
                        _buttonsLast = new bool[_jst.Buttons.Length];
                        _dPadsLast = new int[_jst.Dpads.Length];
                    }

                    jaxis1.ID = MainForm.Conf.Joystick.XAxis;
                    jaxis2.ID = MainForm.Conf.Joystick.YAxis;
                    jaxis3.ID = MainForm.Conf.Joystick.ZAxis;
                    
                    
                    jbutton1.ID = MainForm.Conf.Joystick.Record;
                    jbutton2.ID = MainForm.Conf.Joystick.Snapshot;
                    jbutton3.ID = MainForm.Conf.Joystick.Talk;
                    jbutton4.ID = MainForm.Conf.Joystick.Listen;
                    jbutton5.ID = MainForm.Conf.Joystick.Play;
                    jbutton6.ID = MainForm.Conf.Joystick.Next;
                    jbutton7.ID = MainForm.Conf.Joystick.Previous;
                    jbutton8.ID = MainForm.Conf.Joystick.Stop;


                    CenterAxes();

                }
                else
                {
                    MessageBox.Show(this, LocRm.GetString("NoJoystick"));
                    tblJoystick.Enabled = false;
                }



            }

            
            
        }

        private int[] _axisLast;
        private int[] _dPadsLast;
        private bool[] _buttonsLast;

        private void tmrJSUpdate_Tick(object sender, EventArgs e)
        {
            if (_jst != null && _axisLast!=null)
            {
                lock (Jslock)
                {
                    _jst.UpdateStatus();
                    for (int i = 0; i < _jst.Axis.Length; i++)
                    {
                        if (_jst.Axis[i] != _axisLast[i])
                        {
                            if (_curAxis != null)
                            {
                                _curAxis.ID = (i + 1);
                            }
                        }
                        _axisLast[i] = _jst.Axis[i];

                    }

                    for (int i = 0; i < _jst.Buttons.Length; i++)
                    {
                         
                        if (_jst.Buttons[i] != _buttonsLast[i])
                        {
                            if (_curButton!=null)
                            {
                                _curButton.ID = (i + 1);
                            }
                        }

                        _buttonsLast[i] = _jst.Buttons[i];

                    }

                    for (int i = 0; i < _jst.Dpads.Length; i++)
                    {
                        if (_jst.Dpads[i] != _dPadsLast[i])
                        {
                            if (_curAxis!=null && _curAxis == jaxis1)
                            {
                                //dpads do x-y plane
                                jaxis2.ID = _curAxis.ID = 0 - (i + 1);
                            }
                        }

                        _dPadsLast[i] = _jst.Dpads[i];
                        
                    }
                }

            }
        }

        private void btnCenterAxes_Click(object sender, EventArgs e)
        {
            CenterAxes();
            MessageBox.Show(this, LocRm.GetString("AxesCentered"));
        }

        private void CenterAxes()
        {
            MainForm.Conf.Joystick.CenterXAxis = jaxis1.ID > 0 ? _jst.Axis[jaxis1.ID - 1] : 0;
            MainForm.Conf.Joystick.CenterYAxis = jaxis2.ID > 0 ? _jst.Axis[jaxis2.ID - 1] : 0;
            MainForm.Conf.Joystick.CenterZAxis = jaxis3.ID > 0 ? _jst.Axis[jaxis3.ID - 1] : 0;
        }

        private void jaxis1_Load(object sender, EventArgs e)
        {

        }

        private void btnFeatureSet_Click(object sender, EventArgs e)
        {
            if (MainForm.Group != "Admin")
            {
                var ap = EncDec.DecryptData(MainForm.Conf.Permissions.First(q => q.name == "Admin").password,MainForm.Conf.EncryptCode);

                var p = new Prompt(LocRm.GetString("AdminPassword"), "", true);
                p.ShowDialog(this);
                string pwd = p.Val;
                p.Dispose();
                if (pwd != ap)
                {
                    MessageBox.Show(this, LocRm.GetString("PasswordIncorrect"));
                    return;
                }
                
               
            }
            var f = new Features();
            f.ShowDialog(this);
            f.Dispose();
            MainClass.RenderResources();
        }

        private void chkCheckForUpdates_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ddlTalkMic_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string _lastPath = "";
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "All Files (*.*)|*.*";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {
                        txtAlertOnDisconnect.Text = fileName;
                    }
                }
            }
        }

        private void btnChooseFile2_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "All Files (*.*)|*.*";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {
                        txtAlertOnReconnect.Text = fileName;
                    }
                }
            }
        }

        private void jbutton4_Load(object sender, EventArgs e)
        {

        }

        private void jbutton1_Load(object sender, EventArgs e)
        {

        }

        private void chkEnableIPv6_CheckedChanged(object sender, EventArgs e)
        {
            if (_loaded && chkEnableIPv6.Checked)
            {
                MessageBox.Show(this, LocRm.GetString("IPv6Issues"), LocRm.GetString("Warning"));
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/plugins.aspx");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.LoadPlugins();
            ListPlugins();
        }

        private void ListPlugins()
        {
            lbPlugins.Items.Clear();
            
            foreach (String plugin in MainForm.Plugins)
            {
                string name = plugin.Substring(plugin.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                name = name.Substring(0, name.LastIndexOf(".", StringComparison.Ordinal));
                lbPlugins.Items.Add(name);
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            string f = GetFolder(MainForm.Conf.Archive);
            if (f != "")
            {
                txtArchive.Text = f;
            }
        }

        private string GetFolder(string initialPath)
        {
            string f = "";
            if (!string.IsNullOrEmpty(initialPath))
            {
                try
                {
                    Fsd.InitialDirectory = initialPath;
                }
                catch
                {

                }
            }


            if (Fsd.ShowDialog(Handle))
            {
                f = Fsd.FileName;
                if (!f.EndsWith(@"\"))
                    f += @"\";
            }
            return f;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtAlertSubject.Text = "[EVENT]: [SERVER] [OBJECTNAME]";
            txtAlertBody.Text = txtSMSBody.Text = "[EVENT] at [DATE] [TIME]: [SERVER] [OBJECTNAME] [RECORDED] [PLUGIN]";

            txtAlertSubject.Text = "[EVENT]: [SERVER] [OBJECTNAME]";
            txtAlertBody.Text = txtSMSBody.Text = "[EVENT] at [DATE] [TIME]: [SERVER] [OBJECTNAME] [RECORDED] [PLUGIN]";            
            txtAppendLinkText.Text = "<br/>ispyconnect.com";
        }

        private void chkUseiSpy_CheckedChanged(object sender, EventArgs e)
        {
            tlpSMTP.Enabled = !chkUseiSpy.Checked;
        }

        private void btnTestSMTP_Click(object sender, EventArgs e)
        {
            SaveSMTPSettings();
             var p = new Prompt(LocRm.GetString("TestMailTo"), MainForm.Conf.SMTPFromAddress);
            if (p.ShowDialog(this) == DialogResult.OK)
            {
                MessageBox.Show(this, Mailer.Send(p.Val, LocRm.GetString("test"),
                    LocRm.GetString("ISpyMessageTest"))
                    ? LocRm.GetString("MessageSent")
                    : LocRm.GetString("FailedCheckLog"));
            }
        }

        private void SaveSMTPSettings()
        {
            MainForm.Conf.UseSMTP = !chkUseiSpy.Checked;
            MainForm.Conf.SMTPFromAddress = txtSMTPFromAddress.Text;
            MainForm.Conf.SMTPUsername = txtSMTPUsername.Text;
            MainForm.Conf.SMTPPassword = txtSMTPPassword.Text;
            MainForm.Conf.SMTPServer = txtSMTPServer.Text;
            MainForm.Conf.SMTPSSL = chkSMTPUseSSL.Checked;
            MainForm.Conf.SMTPPort = (int)numSMTPPort.Value;

            
        }

        private void chkPasswordProtectSettings_CheckedChanged(object sender, EventArgs e)
        {
            

        }

        private void btnCert_Click(object sender, EventArgs e)
        {
            var c = MainForm.Conf.SSLEnabled;
            var ssl = new SSLConfig();
            ssl.ShowDialog(this);
            ssl.Dispose();
            SetSSLText();
            if (MainForm.Conf.SSLEnabled != c)
            {
                MainClass.ConnectServices(false);

            }
            
        }

        private void SetSSLText()
        {
            lblSSLCert.Text = LocRm.GetString("Off");
            if (MainForm.Conf.SSLEnabled)
            {
                lblSSLCert.Text = MainForm.Conf.SSLCertificate;
            }

        }

        private void ddlPlayback_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mediaDirectoryEditor1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainForm.InstanceReference.RunStorageManagement(true);
        }

        private void ddlStartupMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label69_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = MainForm.Conf.VLCLocation;
            var dr = folderBrowserDialog1.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                MainForm.Conf.VLCLocation = folderBrowserDialog1.SelectedPath;
                label69.Text = MainForm.Conf.VLCLocation;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {
                        lblBrand.Text = fileName;
                    }
                }
            }
        }
    }
}