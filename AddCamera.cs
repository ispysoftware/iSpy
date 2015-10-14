using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Cloud;
using iSpyApplication.Controls;
using iSpyApplication.Kinect;
using iSpyApplication.Pelco;
using iSpyApplication.Sources;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Sources.Video;
using iSpyPRO.DirectShow.Internals;


namespace iSpyApplication
{
    public partial class AddCamera : Form
    {
        private readonly string[] _alertmodes = {"movement", "nomovement", "objectcount"};

        private readonly object[] _detectortypes = { "Two Frames", "Custom Frame", "Background Modeling", "Two Frames (Color)", "Custom Frame (Color)", "Background Modeling (Color)", "None" };

        private readonly object[] _processortypes = {"Grid Processing", "Object Tracking", "Border Highlighting","Area Highlighting", "None"};

        private readonly object[] _actiontypes = {"Alert", "AlertStopped", "Connection Lost", "Reconnect", "ReconnectFailed","RecordingAlertStarted", "RecordingAlertStopped"};

        public CameraWindow CameraControl;
        public bool StartWizard;
        public bool IsNew;
        private HSLFilteringForm _filterForm;
        private bool _loaded;
        private ConfigureTripWires _ctw;
        private PiPConfig _pip;
        public MainForm MainClass;


        public AddCamera()
        {
            InitializeComponent();
            RenderResources();

            AreaControl.BoundsChanged += AsBoundsChanged;
            AreaControl.Invalidate();
        }

        private void AsBoundsChanged(object sender, EventArgs e)
        {
            if (CameraControl.Camera != null && CameraControl.Camera.MotionDetector != null)
            {
                CameraControl.Camera.SetMotionZones(AreaControl.MotionZones);
            }
            CameraControl.Camobject.detector.motionzones = AreaControl.MotionZones;
        }

        private void BtnSelectSourceClick(object sender, EventArgs e)
        {
            StartWizard = false;
            SelectSource();
        }

        private bool SelectSource()
        {
            bool success = false;
            FindCameras.LastConfig.PromptSave = false;
            
            var vs = new VideoSource { CameraControl = CameraControl, StartWizard = StartWizard };
            vs.ShowDialog(this);
            if (vs.DialogResult == DialogResult.OK)
            {
                CameraControl.Camobject.settings.videosourcestring = vs.VideoSourceString;
                CameraControl.Camobject.settings.sourceindex = vs.SourceIndex;
                CameraControl.Camobject.settings.login = vs.CameraLogin;
                CameraControl.Camobject.settings.password = vs.CameraPassword;
                CameraControl.Camobject.name = vs.FriendlyName;

                bool su = CameraControl.Camobject.resolution != vs.CaptureSize.Width + "x" + vs.CaptureSize.Height;
                if (vs.SourceIndex==3)
                {
                    CameraControl.Camobject.resolution = vs.CaptureSize.Width + "x" + vs.CaptureSize.Height;
                    CameraControl.Camobject.settings.framerate = vs.FrameRate;
                    CameraControl.Camobject.settings.crossbarindex = vs.VideoInputIndex;
                }
                
                chkActive.Enabled = true;
                chkActive.Checked = false;
                Thread.Sleep(1000); //allows unmanaged code to complete shutdown
                chkActive.Checked = true;

                CameraControl.NeedSizeUpdate = su;
                if (CameraControl.VolumeControl == null && CameraControl.Camera!=null)
                {
                    //do we need to add a paired volume control?
                    var c = CameraControl.Camera.VideoSource as ISupportsAudio;
                    if (c!=null)
                    {
                        c.HasAudioStream += AddCameraHasAudioStream;
                    }
                    if (FindCameras.LastConfig.PromptSave)
                    {
                        CameraControl.NewFrame -= NewCameraNewFrame;
                        CameraControl.NewFrame += NewCameraNewFrame;
                    }

                }
                LoadAlertTypes();
                success = true;

                
            }
            vs.Dispose();
            return success;
        }

        private delegate void ShareDelegate();
        void NewCameraNewFrame(object sender, NewFrameEventArgs e)
        {
            if (CameraControl == null)
                return;
            
            CameraControl.NewFrame -= NewCameraNewFrame;

            string r = LocRm.CultureCode;
            if (r != "en")
                return;

            if (IsDisposed || !Visible)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new ShareDelegate(DoShareCamera));
                return;
            }
            DoShareCamera();
        }

        void DoShareCamera()
        {
            if (FindCameras.LastConfig.PromptSave)
            {
                var sc = new ShareCamera();
                sc.ShowDialog(this);
                sc.Dispose();
            }
        }
            

        private delegate void EnableDelegate();

        void AddCameraHasAudioStream(object sender, EventArgs eventArgs)
        {
            if (IsDisposed || !Visible)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new EnableDelegate(AddAudioStream));
                return;
            }
            AddAudioStream();
        }

        private void AddAudioStream()
        {
            var m = MainForm.Microphones.SingleOrDefault(p => p.id == CameraControl.Camobject.settings.micpair);
            
            if (m!=null)
            {
                lblMicSource.Text = m.name;
            }
        }

        private bool _forceClose;
        private void AddCameraLoad(object sender, EventArgs e)
        {
            int j;
            if (CameraControl.Camobject.id == -1)
            {
                if (!SelectSource())
                {
                    _forceClose = true;
                    Close();
                    return;
                }
            }
            if (CameraControl.Camobject.id == -1)
            {
                CameraControl.Camobject.id = MainForm.NextCameraId;
                MainForm.Cameras.Add(CameraControl.Camobject);
            }
            _loaded = false;
            CameraControl.NewFrame -= CameraNewFrame;
            CameraControl.NewFrame += CameraNewFrame;
            CameraControl.IsEdit = true;
            if (CameraControl.VolumeControl != null)
                CameraControl.VolumeControl.IsEdit = true;
            ddlTimestamp.Text = CameraControl.Camobject.settings.timestampformatter;

            //chkUploadYouTube.Checked = CameraControl.Camobject.settings.youtube.autoupload;
            chkPublic.Checked = CameraControl.Camobject.settings.youtube.@public;
            txtTags.Text = CameraControl.Camobject.settings.youtube.tags;
            chkMovement.Checked = CameraControl.Camobject.alerts.active;
            
            foreach(string dt in _detectortypes)
            {
                ddlMotionDetector.Items.Add(LocRm.GetString(dt));
            }

            foreach (string dt in _processortypes)
            {
                ddlProcessor.Items.Add(LocRm.GetString(dt));
            }

            for (j = 0; j < _detectortypes.Length; j++)
            {
                if ((string) _detectortypes[j] == CameraControl.Camobject.detector.type)
                {
                    ddlMotionDetector.SelectedIndex = j;
                    break;
                }
            }
            for (j = 0; j < _processortypes.Length; j++)
            {
                if ((string) _processortypes[j] == CameraControl.Camobject.detector.postprocessor)
                {
                    ddlProcessor.SelectedIndex = j;
                    break;
                }
            }

            foreach (string dt in _actiontypes)
            {
                ddlActionType.Items.Add(LocRm.GetString(dt));
            }
            ddlActionType.SelectedIndex = 0;

            LoadAlertTypes();

            ddlProcessFrames.SelectedItem = CameraControl.Camobject.detector.processeveryframe.ToString(CultureInfo.InvariantCulture);
            txtCameraName.Text = CameraControl.Camobject.name;

            ranger1.Maximum = 100;
            ranger1.Minimum = 0.001;
            ranger1.ValueMin = CameraControl.Camobject.detector.minsensitivity;
            ranger1.ValueMax = CameraControl.Camobject.detector.maxsensitivity;
            ranger1.Gain = CameraControl.Camobject.detector.gain;
            ranger1.ValueMinChanged += Ranger1ValueMinChanged;
            ranger1.ValueMaxChanged += Ranger1ValueMaxChanged;
            ranger1.GainChanged += Ranger1GainChanged;
            ranger1.SetText();
            
            rdoRecordDetect.Checked = CameraControl.Camobject.detector.recordondetect;
            rdoRecordAlert.Checked = CameraControl.Camobject.detector.recordonalert;
            rdoNoRecord.Checked = !rdoRecordDetect.Checked && !rdoRecordAlert.Checked;

            chkSchedule.Checked = CameraControl.Camobject.schedule.active;

            var feats = Enum.GetNames(typeof(RotateFlipType));

            int ind = 0;
            j = 0;

            foreach (var f in feats)
            {
                ddlRotateFlip.Items.Add(new ListItem(Regex.Replace(f, "([a-z,0-9])([A-Z])", "$1 $2"),f));
                if (CameraControl.Camobject.rotateMode == f)
                    ind = j;
                j++;
                
            }
            ddlRotateFlip.SelectedIndex = ind;
            
 
            chkColourProcessing.Checked = CameraControl.Camobject.detector.colourprocessingenabled;
            numMaxFR.Value = CameraControl.Camobject.settings.maxframerate;
            numMaxFRRecording.Value = CameraControl.Camobject.settings.maxframeraterecord;
            
            txtDirectory.Text = CameraControl.Camobject.directory;
            
            rdoContinuous.Checked = CameraControl.Camobject.alerts.processmode == "continuous";
            rdoMotion.Checked = CameraControl.Camobject.alerts.processmode == "motion";
            rdoTrigger.Checked = CameraControl.Camobject.alerts.processmode == "trigger";
            tbFTPQuality.Value = CameraControl.Camobject.ftp.quality;
            tbSaveQuality.Value = CameraControl.Camobject.savelocal.quality;
           
            txtLocalFilename.Text = CameraControl.Camobject.savelocal.filename;

            
            txtPTZChannel.Text = CameraControl.Camobject.settings.ptzchannel;
            
            ShowSchedule(-1);

            if (CameraControl.Camera==null)
            {
                chkActive.Checked = false;
                btnAdvanced.Enabled = btnCrossbar.Enabled = false;
            }
            else
            {
                chkActive.Checked = CameraControl.Camobject.settings.active;
            }
            pnlScheduler.Enabled = chkSchedule.Checked;

            AreaControl.MotionZones = CameraControl.Camobject.detector.motionzones;

            chkActive.Enabled = !string.IsNullOrEmpty(CameraControl.Camobject.settings.videosourcestring);
            
            
            Text = LocRm.GetString("EditCamera");
            if (CameraControl.Camobject.id > -1)
                Text += string.Format(" (ID: {0}, DIR: {1})", CameraControl.Camobject.id, CameraControl.Camobject.directory);


            txtTimeLapse.Text = CameraControl.Camobject.recorder.timelapse.ToString(CultureInfo.InvariantCulture);
            pnlMovement.Enabled = chkMovement.Checked;
            chkSuppressNoise.Checked = CameraControl.Camobject.settings.suppressnoise;

            gpbSubscriber2.Enabled = MainForm.Conf.Subscribed;
            linkLabel9.Visible = !(MainForm.Conf.Subscribed);

            txtBuffer.Value = CameraControl.Camobject.recorder.bufferseconds;
            txtCalibrationDelay.Value = CameraControl.Camobject.detector.calibrationdelay;
            txtInactiveRecord.Value = CameraControl.Camobject.recorder.inactiverecord;
            txtMaxRecordTime.Value = CameraControl.Camobject.recorder.maxrecordtime;
            numMinRecordTime.Value = CameraControl.Camobject.recorder.minrecordtime;
            btnBack.Enabled = false;

            ddlHourStart.SelectedIndex =
                ddlHourEnd.SelectedIndex = ddlMinuteStart.SelectedIndex = ddlMinuteEnd.SelectedIndex = 0;

            txtUploadEvery.Text = CameraControl.Camobject.ftp.intervalnew.ToString(CultureInfo.InvariantCulture);
            numSaveInterval.Text = CameraControl.Camobject.savelocal.intervalnew.ToString(CultureInfo.InvariantCulture);
            numFTPMinimumDelay.Text = CameraControl.Camobject.ftp.minimumdelay.ToString(CultureInfo.InvariantCulture);
            numSaveDelay.Text = CameraControl.Camobject.savelocal.minimumdelay.ToString(CultureInfo.InvariantCulture);

            txtFTPFilename.Text = CameraControl.Camobject.ftp.filename;
            chkFTP.Checked = gbFTP.Enabled = CameraControl.Camobject.ftp.enabled;
            chkLocalSaving.Checked = gbLocal.Enabled = CameraControl.Camobject.savelocal.enabled;
            txtTimeLapseFrames.Text = CameraControl.Camobject.recorder.timelapseframes.ToString(CultureInfo.InvariantCulture);

            chkTimelapse.Checked = CameraControl.Camobject.recorder.timelapseenabled;
            if (!chkTimelapse.Checked)
                groupBox1.Enabled = false;

            
            txtMaskImage.Text = CameraControl.Camobject.settings.maskimage;

            chkPTZFlipX.Checked = CameraControl.Camobject.settings.ptzflipx;
            chkPTZFlipY.Checked = CameraControl.Camobject.settings.ptzflipy;
            chkPTZRotate90.Checked = CameraControl.Camobject.settings.ptzrotate90;

            txtFTPText.Text = CameraControl.Camobject.ftp.text;
            txtSaveOverlay.Text = CameraControl.Camobject.savelocal.text;


            rdoFTPMotion.Checked = CameraControl.Camobject.ftp.mode == 0;
            rdoFTPAlerts.Checked = CameraControl.Camobject.ftp.mode == 1;
            rdoFTPInterval.Checked = CameraControl.Camobject.ftp.mode == 2;

            rdoSaveMotion.Checked = CameraControl.Camobject.savelocal.mode == 0;
            rdoSaveAlerts.Checked = CameraControl.Camobject.savelocal.mode == 1;
            rdoSaveInterval.Checked = CameraControl.Camobject.savelocal.mode == 2;

            txtUploadEvery.Enabled = rdoFTPInterval.Checked;
            numSaveInterval.Enabled = rdoSaveInterval.Checked;

            
            
            LoadPTZs();
            txtPTZURL.Text = CameraControl.Camobject.settings.ptzurlbase;

            txtAccessGroups.Text = CameraControl.Camobject.settings.accessgroups;
            

            


            ddlCopyFrom.Items.Clear();
            ddlCopyFrom.Items.Add(new ListItem(LocRm.GetString("CopyFrom"), "-1"));
            foreach(objectsCamera c in MainForm.Cameras)
            {
                if (c.id != CameraControl.Camobject.id)
                    ddlCopyFrom.Items.Add(new ListItem(c.name,c.id.ToString(CultureInfo.InvariantCulture)));
            }
            ddlCopyFrom.SelectedIndex = 0;


            txtPTZUsername.Text = CameraControl.Camobject.settings.ptzusername;
            txtPTZPassword.Text = CameraControl.Camobject.settings.ptzpassword;
            tbQuality.Value = CameraControl.Camobject.recorder.quality;

            numTimelapseSave.Value = CameraControl.Camobject.recorder.timelapsesave;
            numFramerate.Value = CameraControl.Camobject.recorder.timelapseframerate;

            try
            {
                ddlProfile.SelectedIndex = CameraControl.Camobject.recorder.profile;
            }
            catch
            {
                ddlProfile.SelectedIndex = 0;
            }
            

            var m = MainForm.Microphones.SingleOrDefault(p => p.id == CameraControl.Camobject.settings.micpair);
            lblMicSource.Text = m != null ? m.name : LocRm.GetString("None");

            PopulateTalkDevices();
            numTalkPort.Value = CameraControl.Camobject.settings.audioport > -1 ? CameraControl.Camobject.settings.audioport : 80;
            txtAudioOutIP.Text = CameraControl.Camobject.settings.audioip;
            txtTalkUsername.Text = CameraControl.Camobject.settings.audiousername;
            txtTalkPassword.Text = CameraControl.Camobject.settings.audiopassword;

            string t2 = CameraControl.Camobject.recorder.trigger ?? "";

            ddlTriggerRecording.Items.Add(new ListItem("None", ""));

            foreach (var c in MainForm.Cameras.Where(p=>p.id!=CameraControl.Camobject.id))
            {
                ddlTriggerRecording.Items.Add(new ListItem(c.name, "2," + c.id));                
            }
            foreach (var c in MainForm.Microphones.Where(p => p.id != CameraControl.Camobject.settings.micpair))
            {
                ddlTriggerRecording.Items.Add(new ListItem(c.name, "1," + c.id));
            }
            foreach (ListItem li in ddlTriggerRecording.Items)
            {
                if (li.Value == t2)
                    ddlTriggerRecording.SelectedItem = li;
            }

            if (ddlTriggerRecording.SelectedIndex == -1)
                ddlTriggerRecording.SelectedIndex = 0;


            numMaxCounter.Value = CameraControl.Camobject.ftp.countermax;
            numSaveCounter.Value = CameraControl.Camobject.savelocal.countermax;

            chkIgnoreAudio.Checked = CameraControl.Camobject.settings.ignoreaudio;

            tblStorage.Enabled = chkStorageManagement.Checked = CameraControl.Camobject.settings.storagemanagement.enabled;
            numMaxAge.Value = CameraControl.Camobject.settings.storagemanagement.maxage;
            numMaxFolderSize.Value = CameraControl.Camobject.settings.storagemanagement.maxsize;

            actionEditor1.LoginRequested += ActionEditor1LoginRequested;

            //chkNotifyDisconnect.Checked = CameraControl.Camobject.settings.notifyondisconnect;

            numAutoOff.Value = CameraControl.Camobject.detector.autooff;
            chkArchive.Checked = CameraControl.Camobject.settings.storagemanagement.archive;
            chkUploadGrabs.Checked = CameraControl.Camobject.settings.cloudprovider.images;
            chkUploadRecordings.Checked = CameraControl.Camobject.settings.cloudprovider.recordings;
            txtCloudPath.Text = CameraControl.Camobject.settings.cloudprovider.path;
            chkMessaging.Checked = CameraControl.Camobject.settings.messaging;

            LoadMediaDirectories();
            PopFTPServers();
            ddlCloudProviders.Items.Add(LocRm.GetString("PleaseSelect"));
            ddlCloudProviders.Items.AddRange(Settings.CloudProviders);
            ddlCloudProviders.SelectedIndex = 0;
            foreach (var o in ddlCloudProviders.Items)
            {
                if (o.ToString() == CameraControl.Camobject.settings.cloudprovider.provider)
                {
                    ddlCloudProviders.SelectedItem = o;
                    break;
                }
            }
            intervalConfig1.Init(CameraControl);
            _loaded = true;
        }

        private void LoadMediaDirectories()
        {
            ddlMediaDirectory.Items.Clear();
            foreach (var s in MainForm.Conf.MediaDirectories)
            {
                ddlMediaDirectory.Items.Add(new ListItem(s.Entry, s.ID.ToString(CultureInfo.InvariantCulture)));
                if (s.ID == CameraControl.Camobject.settings.directoryIndex)
                    ddlMediaDirectory.SelectedItem = ddlMediaDirectory.Items[ddlMediaDirectory.Items.Count - 1];
            }
            if (ddlMediaDirectory.SelectedIndex == -1)
                ddlMediaDirectory.SelectedIndex = 0;
        }

        void ActionEditor1LoginRequested(object sender, EventArgs e)
        {
            Login();
        }

        private void LoadAlertTypes()
        {
            ddlAlertMode.Items.Clear();
            int iMode = 0;

            var items = new List<string>();
            if (Helper.HasFeature(Enums.Features.Motion_Detection))
            {
                foreach (string s in _alertmodes)
                {
                    ddlAlertMode.Items.Add(LocRm.GetString(s));
                    items.Add(s);
                }
            }

            //provider specific alert options
            switch (CameraControl.Camobject.settings.sourceindex)
            {
                case 7:
                    ddlAlertMode.Items.Add("Virtual Trip Wires");
                    items.Add("Virtual Trip Wires");
                    break;
            }

            foreach (String plugin in MainForm.Plugins)
            {
                string name = plugin.Substring(plugin.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                name = name.Substring(0, name.LastIndexOf(".", StringComparison.Ordinal));
                ddlAlertMode.Items.Add(name);
                items.Add(name);
            }


            int iCount = 0;
            if (CameraControl.Camobject.alerts.mode != null)
            {
                foreach (string name in items)
                {
                    if (name.ToLower() == CameraControl.Camobject.alerts.mode.ToLower())
                    {
                        iMode = iCount;
                        break;
                    }
                    iCount++;
                }
            }

            if (ddlAlertMode.Items.Count>0)
                ddlAlertMode.SelectedIndex = iMode;
        }

        void Ranger1ValueMinChanged()
        {
            if (_loaded)
            {
                CameraControl.Camobject.detector.minsensitivity = ranger1.ValueMin;
                if (CameraControl.Camera != null)
                {
                    CameraControl.Camera.AlarmLevel = Helper.CalculateTrigger(ranger1.ValueMin);
                }
            }

        }

        void Ranger1ValueMaxChanged()
        {
            if (_loaded)
            {
                CameraControl.Camobject.detector.maxsensitivity = ranger1.ValueMax;
                if (CameraControl.Camera != null)
                {
                    CameraControl.Camera.AlarmLevelMax = Helper.CalculateTrigger(ranger1.ValueMax);
                }
            }
        }

        void Ranger1GainChanged()
        {
            if (_loaded)
            {
                CameraControl.Camobject.detector.gain = ranger1.Gain;
            }
        }


        

        private void RenderResources()
        {
            btnBack.Text = LocRm.GetString("Back");
            btnDelete.Text = LocRm.GetString("Delete");
            btnFinish.Text = LocRm.GetString("Finish");
            btnMaskImage.Text = "...";
            btnNext.Text = LocRm.GetString("Next");
            btnAdvanced.Text = LocRm.GetString("AdvProperties");
            btnSelectSource.Text = "...";
            btnUpdate.Text = LocRm.GetString("Update");
            llblClearAll.Text = LocRm.GetString("ClearAll");
            button2.Text = LocRm.GetString("Add");
            chkActive.Text = LocRm.GetString("CameraActive");
            chkFri.Text = LocRm.GetString("Fri");
            chkFTP.Text = LocRm.GetString("FtpEnabled");
            label22.Text = LocRm.GetString("Username");
            label42.Text = LocRm.GetString("Password");
            
            chkschedPTZ.Text = LocRm.GetString("SchedulePTZ");
            rdoMotion.Text = LocRm.GetString("WhenMotionDetected");
            rdoContinuous.Text = LocRm.GetString("Continuous");
            chkMon.Text = LocRm.GetString("Mon");
            chkMovement.Text = LocRm.GetString("AlertsEnabled");
            chkPublic.Text = LocRm.GetString("PubliccheckThisToMakeYour");
            rdoRecordDetect.Text = LocRm.GetString("RecordOnMovementDetection");
            rdoRecordAlert.Text = LocRm.GetString("RecordOnAlert");
            rdoNoRecord.Text = LocRm.GetString("NoRecord");
            chkRecordSchedule.Text = LocRm.GetString("RecordOnScheduleStart");
            chkSat.Text = LocRm.GetString("Sat");
            chkSchedule.Text = LocRm.GetString("ScheduleCamera");
            chkScheduleActive.Text = LocRm.GetString("ScheduleActive");
            chkScheduleAlerts.Text = LocRm.GetString("AlertsEnabled");
            chkScheduleRecordOnDetect.Text = LocRm.GetString("RecordOnDetect");
            chkRecordAlertSchedule.Text = LocRm.GetString("RecordOnAlert");
            chkSun.Text = LocRm.GetString("Sun");
            chkSuppressNoise.Text = LocRm.GetString("SupressNoise");
            chkThu.Text = LocRm.GetString("Thu");
            chkTue.Text = LocRm.GetString("Tue");
            chkWed.Text = LocRm.GetString("Wed");
            chkScheduleTimelapse.Text = LocRm.GetString("TimelapseEnabled");
            chkTimelapse.Text = LocRm.GetString("TimelapseEnabled");
            gbFTP.Text = LocRm.GetString("FtpDetails");
            gbZones.Text = LocRm.GetString("DetectionZones");
            gpbSubscriber2.Text = LocRm.GetString("WebServiceOptions");
            groupBox1.Text = LocRm.GetString("TimelapseRecording");
            groupBox3.Text = LocRm.GetString("VideoSource");
            groupBox4.Text = LocRm.GetString("RecordingSettings");
            groupBox5.Text = LocRm.GetString("Detector");
            label20.Text = gbLocal.Text = LocRm.GetString("Filename");
            label97.Text = LocRm.GetString("Seconds");
            label1.Text = LocRm.GetString("Name");
            label10.Text = ":";
            label11.Text = LocRm.GetString("TimeStamp");
            label12.Text = LocRm.GetString("UseDetector");
            label13.Text = LocRm.GetString("Seconds");
            label14.Text = LocRm.GetString("RecordTimelapse");
            label15.Text = LocRm.GetString("Intervals");
            label17.Text = LocRm.GetString("Frames");
            label19.Text = groupBox2.Text = LocRm.GetString("Microphone");
            label2.Text = LocRm.GetString("Source");
            
            label24.Text = LocRm.GetString("Seconds");
            label25.Text = LocRm.GetString("CalibrationDelay");
            label26.Text = LocRm.GetString("PrebufferFrames");
            label27.Text = LocRm.GetString("Seconds");
            label28.Text = LocRm.GetString("Seconds");
            label29.Text = LocRm.GetString("Buffer");
            label3.Text = LocRm.GetString("TriggerRange");
            label30.Text = LocRm.GetString("MaxRecordTime");
            label31.Text = LocRm.GetString("Seconds");
            label32.Text = LocRm.GetString("InactivityRecord");
            label34.Text = LocRm.GetString("MaxRecordTime");
            label53.Text = LocRm.GetString("MinRecordTime");
            label35.Text = LocRm.GetString("Seconds");
            label36.Text = LocRm.GetString("Seconds");
            label33.Text = LocRm.GetString("Seconds");
            label37.Text = rdoFTPInterval.Text = rdoSaveInterval.Text = LocRm.GetString("Interval");
            label38.Text = LocRm.GetString("MaxCalibrationDelay");
            label39.Text = LocRm.GetString("Seconds");
            label4.Text = LocRm.GetString("Mode");
            label40.Text = LocRm.GetString("InactivityRecord");
            label41.Text = LocRm.GetString("Seconds");
            label44.Text = LocRm.GetString("savesAFrameToAMovieFileNS");
            label46.Text = LocRm.GetString("DisplayStyle");
            label48.Text = LocRm.GetString("ColourFiltering");
            label49.Text = LocRm.GetString("Days");
            label50.Text = LocRm.GetString("ImportantMakeSureYourSche");
            label51.Text = LocRm.GetString("ProcessEvery");
            label56.Text = LocRm.GetString("Filename");
            label57.Text = label96.Text =LocRm.GetString("When");
            label58.Text = label99.Text = LocRm.GetString("Seconds");
            
            label60.Text = LocRm.GetString("Egimagesmycamimagejpg");
            label64.Text = LocRm.GetString("Frames");
            label67.Text = LocRm.GetString("Images");
            label68.Text = LocRm.GetString("Interval");
            label69.Text = LocRm.GetString("Seconds");
            label7.Text = LocRm.GetString("Start");
            label70.Text = LocRm.GetString("savesAFrameEveryNSecondsn");
            label71.Text = LocRm.GetString("Movie");
            label73.Text = LocRm.GetString("CameraModel");
            label75.Text = LocRm.GetString("ExtendedCommands");
            label76.Text = LocRm.GetString("ExitThisToEnableAlertsAnd");
            label77.Text = LocRm.GetString("Tags");
            label79.Text = LocRm.GetString("UploadViaWebsite");
            label8.Text = ":";
            label80.Text = LocRm.GetString("TipToCreateAScheduleOvern");
            label83.Text = LocRm.GetString("ClickAndDragTodraw");
            label84.Text = LocRm.GetString("MaskImage");
            label86.Text = label100.Text = LocRm.GetString("OverlayText");
            label9.Text = LocRm.GetString("Stop");
            linkLabel1.Text = LocRm.GetString("UsageTips");
            groupBox7.Text = LocRm.GetString("Upload");
            groupBox10.Text = LocRm.GetString("Save");
            label6.Text = label95.Text = LocRm.GetString("MinimumDelay");
            linkLabel2.Text = LocRm.GetString("ScriptToRenderThisImageOn");
            linkLabel6.Text = LocRm.GetString("GetLatestList");
            linkLabel8.Text = linkLabel14.Text = LocRm.GetString("help");
            pnlScheduler.Text = LocRm.GetString("Scheduler");
            chkLocalSaving.Text = LocRm.GetString("LocalSavingEnabled");
            linkLabel11.Text = LocRm.GetString("OpenLocalFolder");
            tabPage1.Text = LocRm.GetString("Camera");
            tabPage2.Text = rdoFTPAlerts.Text = rdoSaveAlerts.Text = LocRm.GetString("Alerts");
            tabPage3.Text = rdoFTPMotion.Text = rdoSaveMotion.Text = LocRm.GetString("MotionDetection");
            tabPage4.Text = LocRm.GetString("Recording");
            tabPage5.Text = LocRm.GetString("Scheduling");
            tabPage7.Text = LocRm.GetString("FTPImages");
            tabPage10.Text = LocRm.GetString("Images");
            tabPage8.Text = LocRm.GetString("Ptz");
            tabPage9.Text = LocRm.GetString("Cloud");
            toolTip1.SetToolTip(txtMaskImage, LocRm.GetString("ToolTip_CameraName"));
            toolTip1.SetToolTip(txtCameraName, LocRm.GetString("ToolTip_CameraName"));
            toolTip1.SetToolTip(ranger1, LocRm.GetString("ToolTip_MotionSensitivity"));
            toolTip1.SetToolTip(txtTimeLapseFrames, LocRm.GetString("ToolTip_TimeLapseFrames"));
            toolTip1.SetToolTip(txtTimeLapse, LocRm.GetString("ToolTip_TimeLapseVideo"));
            toolTip1.SetToolTip(txtMaxRecordTime, LocRm.GetString("ToolTip_MaxDuration"));
            toolTip1.SetToolTip(txtInactiveRecord, LocRm.GetString("ToolTip_InactiveRecord"));
            //toolTip1.SetToolTip(txtBuffer, LocRm.GetString("ToolTip_BufferFrames"));
            toolTip1.SetToolTip(txtCalibrationDelay, LocRm.GetString("ToolTip_DelayAlerts"));
            toolTip1.SetToolTip(lbSchedule, LocRm.GetString("ToolTip_PressDelete"));
            label16.Text = LocRm.GetString("PTZNote");
            //chkRotate90.Text = LocRm.GetString("Rotate90");
            chkPTZFlipX.Text = LocRm.GetString("Flipx");
            chkPTZFlipY.Text = LocRm.GetString("Flipy");
            chkPTZRotate90.Text = LocRm.GetString("Rotate90");
            label43.Text = LocRm.GetString("MaxFramerate");
            label47.Text = LocRm.GetString("WhenRecording");
            label74.Text = LocRm.GetString("Directory");
            
            llblHelp.Text = LocRm.GetString("help");
            
            chkSchedFTPEnabled.Text = LocRm.GetString("FtpEnabled");
            chkSchedSaveLocalEnabled.Text = LocRm.GetString("LocalSavingEnabled");

            chkColourProcessing.Text = LocRm.GetString("Apply");
            Text = LocRm.GetString("AddCamera");
            
            lblAccessGroups.Text = LocRm.GetString("AccessGroups");
            groupBox6.Text = LocRm.GetString("RecordingMode");
            llblEditPTZ.Text = LocRm.GetString("Edit");
            lblQuality.Text = lblQuality2.Text = lblQuality3.Text = LocRm.GetString("Quality");
            lblMinutes.Text = LocRm.GetString("Minutes");
            lblSaveEvery.Text = LocRm.GetString("SaveEvery");
            label61.Text = LocRm.GetString("Profile");
            label62.Text = LocRm.GetString("Framerate");
            linkLabel3.Text = LocRm.GetString("Plugins");
            
            linkLabel10.Text = LocRm.GetString("Reload");
            btnCrossbar.Text = LocRm.GetString("Inputs");
            label72.Text = LocRm.GetString("AutoOff");
            label82.Text = LocRm.GetString("Seconds");
            groupBox9.Text = LocRm.GetString("Actions");
            label89.Text = LocRm.GetString("When");
            rdoTrigger.Text = LocRm.GetString("ExternalTrigger");
            label90.Text = LocRm.GetString("TriggerRecording");

            label63.Text = LocRm.GetString("MediaLocation");
            label74.Text = LocRm.GetString("Directory");
            chkStorageManagement.Text = LocRm.GetString("EnableStorageManagement");
            label85.Text = LocRm.GetString("MaxFolderSizeMb");
            label94.Text = LocRm.GetString("MaxAgeHours");
            chkArchive.Text = LocRm.GetString("ArchiveInsteadOfDelete");
            btnRunNow.Text = LocRm.GetString("RunNow");

            chkUploadGrabs.Text = LocRm.GetString("AutomaticallyUploadImages");
            chkUploadRecordings.Text = LocRm.GetString("AutomaticallyUploadRecordings");


            LocRm.SetString(label3,"TriggerRange");
            LocRm.SetString(groupBox8, "Talk");
            LocRm.SetString(label23, "CameraModel");
            LocRm.SetString(linkLabel13, "Settings");
            LocRm.SetString(label65, "IPAddress");
            LocRm.SetString(label21, "Port");
            LocRm.SetString(label66, "Username");
            LocRm.SetString(label88, "Password");
            
            LocRm.SetString(label81,"FTPFileTip");
            LocRm.SetString(label102, "FTPFileTip");
            LocRm.SetString(label93, "CounterMax");
            LocRm.SetString(label101, "CounterMax");
            LocRm.SetString(label90, "TriggerRecording");
            LocRm.SetString(chkIgnoreAudio, "IgnoreAudio");
            LocRm.SetString(button9,"Options");
            LocRm.SetString(label52, "Server");
            LocRm.SetString(linkLabel5, "Servers");
            LocRm.SetString(btnPTZTrack, "TrackObjects");
            LocRm.SetString(btnPTZSchedule, "Scheduler");
            LocRm.SetString(label5, "PictureInPicture");
            LocRm.SetString(linkLabel4, "CopyTo");


            HideTab(tabPage3, Helper.HasFeature(Enums.Features.Motion_Detection));
            HideTab(tabPage2, Helper.HasFeature(Enums.Features.Alerts));
            HideTab(tabPage4, Helper.HasFeature(Enums.Features.Recording));
            HideTab(tabPage8, Helper.HasFeature(Enums.Features.PTZ));
            HideTab(tabPage7, Helper.HasFeature(Enums.Features.Save_Frames));
            HideTab(tabPage10, Helper.HasFeature(Enums.Features.Save_Frames));
            HideTab(tabPage9, Helper.HasFeature(Enums.Features.Cloud) && Helper.HasFeature(Enums.Features.Web_Settings));
            HideTab(tabPage5, Helper.HasFeature(Enums.Features.Scheduling));
            HideTab(tabPage6, Helper.HasFeature(Enums.Features.Storage));

            if (!Helper.HasFeature(Enums.Features.Web_Settings))
            {
                linkLabel9.Visible = false;
            }


        }
        private void HideTab(TabPage t, bool show)
        {
            if (!show)
            {
                tcCamera.TabPages.Remove(t);
            }
        }


        private void LoadPTZs()
        {
            ddlPTZ.Items.Clear();
            ddlPTZ.Items.Add(new ListItem(":: NONE", "-6"));
            ddlPTZ.Items.Add(new ListItem(":: DIGITAL", "-1"));
            ddlPTZ.Items.Add(new ListItem(":: IAM-CONTROL", "-2"));
            ddlPTZ.Items.Add(new ListItem(":: ONVIF", "-5"));
            ddlPTZ.Items.Add(new ListItem(":: PELCO-P", "-3"));
            ddlPTZ.Items.Add(new ListItem(":: PELCO-D", "-4"));


            foreach(ListItem li in ddlPTZ.Items)
            {
                if (li.Value == CameraControl.Camobject.ptz.ToString(CultureInfo.InvariantCulture))
                {
                    ddlPTZ.SelectedItem = li;
                    break;
                }
            }

            if (MainForm.PTZs != null)
            {
                var ptzEntries = new List<PTZEntry>();

                foreach (PTZSettings2Camera ptz in MainForm.PTZs)
                {
                    int j = 0;
                    foreach(var m in ptz.Makes)
                    {
                        string ttl = (m.Name+" "+m.Model).Trim();
                        var ptze = new PTZEntry(ttl,ptz.id,j);

                        if (!ptzEntries.Contains(ptze))
                            ptzEntries.Add(ptze);
                        j++;
                    }
                }
                foreach(var e in ptzEntries.OrderBy(p=>p.Entry))
                {
                    ddlPTZ.Items.Add(e);

                    if (CameraControl.Camobject.ptz == e.Id && CameraControl.Camobject.ptzentryindex==e.Index)
                    {
                        ddlPTZ.SelectedIndex = ddlPTZ.Items.Count-1;
                        if (CameraControl.Camobject.settings.ptzurlbase == "")
                            CameraControl.Camobject.settings.ptzurlbase = MainForm.PTZs.Single(p=>p.id==e.Id).CommandURL;
                    }
                }
                if (ddlPTZ.SelectedIndex == -1)
                {
                    ddlPTZ.SelectedIndex = 0;
                }
            }
        }

        private struct PTZEntry
        {
            public readonly string Entry;
            public readonly int Id;
            public readonly int Index;
            public PTZEntry(string entry, int id, int index)
            {
                Id = id;
                Entry = entry;
                Index = index;
            }
            public override string ToString()
            {
                return Entry;
            }
        }

        private void ShowSchedule(int selectedIndex)
        {
            lbSchedule.Items.Clear();
            int i = 0;
            foreach (string sched in CameraControl.ScheduleDetails)
            {
                if (sched != "")
                {
                    lbSchedule.Items.Add(new ListItem(sched, i.ToString(CultureInfo.InvariantCulture)));
                    i++;
                }
            }
            if (selectedIndex > -1 && selectedIndex < lbSchedule.Items.Count)
                lbSchedule.SelectedIndex = selectedIndex;
        }

        private void CameraNewFrame(object sender, NewFrameEventArgs e)
        {
            AreaControl.LastFrame = e.Frame;
            try
            {
                if (_filterForm != null)
                    _filterForm.ImageProcess = (Bitmap) e.Frame.Clone();

                if (_ctw != null && _ctw.TripWireEditor1 != null)
                {
                    _ctw.TripWireEditor1.LastFrame = e.Frame;
                }
                if (_pip != null && _pip.areaSelector1 != null)
                {
                    _pip.areaSelector1.LastFrame = e.Frame;
                }
            }
            catch(Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

        private void BtnNextClick(object sender, EventArgs e)
        {
            GoNext();
        }

        private void GoNext()
        {
            tcCamera.SelectedIndex++;
        }

        private void GoPrevious()
        {
            tcCamera.SelectedIndex--;
        }

        private bool CheckStep1()
        {
            string err = "";
            string name = txtCameraName.Text.Trim();
            if (name == "")
                err += LocRm.GetString("Validate_Camera_EnterName") + Environment.NewLine;
            if (MainForm.Cameras.FirstOrDefault(p => p.name.ToLower() == name.ToLower() && p.id != CameraControl.Camobject.id) != null)
                err += LocRm.GetString("Validate_Camera_NameInUse") + Environment.NewLine;

            if (string.IsNullOrEmpty(CameraControl.Camobject.settings.videosourcestring))
            {
                err += LocRm.GetString("Validate_Camera_SelectVideoSource") + Environment.NewLine;
            }

            if (err != "")
            {
                MessageBox.Show(err, LocRm.GetString("Error"));
                tcCamera.SelectedIndex = 0;
                return false;
            }
            return true;
        }

        private void BtnFinishClick(object sender, EventArgs e)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool Save()
        {
            //validate page 0
            if (!CheckStep1())
                return false;
            string err = "";
                
            if (txtBuffer.Text.Length < 1 || txtInactiveRecord.Text.Length < 1 ||
                txtCalibrationDelay.Text.Length < 1 || txtMaxRecordTime.Text.Length < 1)
            {
                err += LocRm.GetString("Validate_Camera_RecordingSettings") + Environment.NewLine;
            }
            if (err != "")
            {
                MessageBox.Show(err, LocRm.GetString("Error"));
                return false;
            }

            double ftpinterval = Convert.ToDouble(txtUploadEvery.Value);
            double saveinterval = Convert.ToDouble(numSaveInterval.Value);
            double ftpmindelay = Convert.ToDouble(numFTPMinimumDelay.Value);
            double savemindelay = Convert.ToDouble(numSaveDelay.Value);


            int timelapseframes = Convert.ToInt32(txtTimeLapseFrames.Value);
            int timelapsemovie = Convert.ToInt32(txtTimeLapse.Value);
            
            string localFilename=txtLocalFilename.Text.Trim();
            if (localFilename.IndexOf("\\", StringComparison.Ordinal)!=-1)
            {
                MessageBox.Show(LocRm.GetString("Validate_Camera_Local_Filename"));
                return false;
            }

            string audioip = txtAudioOutIP.Text.Trim();
                
                
            if (!String.IsNullOrEmpty(audioip))
            {
                IPAddress aip;
                if (!IPAddress.TryParse(audioip, out aip))
                {
                    try
                    {
                        IPHostEntry ipE = Dns.GetHostEntry(audioip);
                        IPAddress[] ipA = ipE.AddressList;
                        if (ipA==null || ipA.Length == 0)
                        {
                            MessageBox.Show(LocRm.GetString("Validate_Camera_Talk_Field"));
                            return false;
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(LocRm.GetString("Validate_Camera_Talk_Field")+" ("+ex.Message+")");
                        return false;
                    }
                }
            }

            CameraControl.Camobject.savelocal.intervalnew = saveinterval;
            CameraControl.Camobject.savelocal.minimumdelay = savemindelay;
            CameraControl.Camobject.savelocal.countermax = Convert.ToInt32(numSaveCounter.Value);

            int savemode = 0;
            if (rdoSaveAlerts.Checked)
                savemode = 1;
            if (rdoSaveInterval.Checked)
                savemode = 2;
            CameraControl.Camobject.savelocal.mode = savemode;
            CameraControl.Camobject.savelocal.quality = tbSaveQuality.Value;
            CameraControl.Camobject.savelocal.text = txtSaveOverlay.Text;
            CameraControl.Camobject.savelocal.filename = txtLocalFilename.Text.Trim();
            CameraControl.Camobject.savelocal.enabled = chkLocalSaving.Checked;


            CameraControl.Camobject.detector.processeveryframe = Convert.ToInt32(ddlProcessFrames.SelectedItem.ToString());
            CameraControl.Camobject.detector.motionzones = AreaControl.MotionZones;
            CameraControl.Camobject.detector.type = (string) _detectortypes[ddlMotionDetector.SelectedIndex];
            CameraControl.Camobject.detector.postprocessor = (string) _processortypes[ddlProcessor.SelectedIndex];
            CameraControl.Camobject.name = txtCameraName.Text.Trim();
            //update to plugin if connected and supported
            if (CameraControl.Camera != null && CameraControl.Camera.Plugin != null)
            {
                try
                {
                    var plugin = CameraControl.Camera.Plugin;
                    plugin.GetType().GetProperty("CameraName").SetValue(plugin, CameraControl.Camobject.name, null);
                }
                catch
                {
                }
            }

            CameraControl.Camobject.settings.ignoreaudio = chkIgnoreAudio.Checked;
            CameraControl.Camobject.alerts.active = chkMovement.Checked;
                
            CameraControl.Camobject.settings.ptzusername = txtPTZUsername.Text;
            CameraControl.Camobject.settings.ptzpassword = txtPTZPassword.Text;
            CameraControl.Camobject.settings.ptzchannel = txtPTZChannel.Text;
                
            CameraControl.Camobject.recorder.quality = tbQuality.Value;
            CameraControl.Camobject.recorder.timelapsesave = (int)numTimelapseSave.Value;
            CameraControl.Camobject.recorder.timelapseframerate = (int)numFramerate.Value;
                
            CameraControl.Camobject.ftp.quality = tbFTPQuality.Value;
            CameraControl.Camobject.ftp.countermax = (int) numMaxCounter.Value;
            CameraControl.Camobject.ftp.minimumdelay = ftpmindelay;                
            SetStorageManagement();

            CameraControl.Camobject.recorder.minrecordtime = (int)numMinRecordTime.Value;

            CameraControl.Camobject.detector.autooff = (int)numAutoOff.Value;
                                
            if (txtDirectory.Text.Trim() == "")
                txtDirectory.Text = MainForm.RandomString(5);

            var md = (ListItem)ddlMediaDirectory.SelectedItem;
            var newind = Convert.ToInt32(md.Value);


            string olddir = Helper.GetMediaDirectory(2, CameraControl.Camobject.id) + "video\\" + CameraControl.Camobject.directory + "\\";

            bool needsFileRefresh = (CameraControl.Camobject.directory != txtDirectory.Text || CameraControl.Camobject.settings.directoryIndex != newind);
                
            int tempidx = CameraControl.Camobject.settings.directoryIndex;
            CameraControl.Camobject.settings.directoryIndex = newind;

                
            string newdir = Helper.GetMediaDirectory(2, CameraControl.Camobject.id) + "video\\" + txtDirectory.Text + "\\";

            if (IsNew)
            {
                try
                {
                    if (!Directory.Exists(newdir))
                    {
                        Directory.CreateDirectory(newdir);
                    }
                    else
                    {
                        switch (
                            MessageBox.Show(this,
                                LocRm.GetString("Validate_Directory_Exists"),
                                LocRm.GetString("Confirm"), MessageBoxButtons.YesNoCancel))
                        {
                            case DialogResult.Yes:
                                Directory.Delete(newdir, true);
                                Directory.CreateDirectory(newdir);
                                break;
                            case DialogResult.Cancel:
                                CameraControl.Camobject.settings.directoryIndex = tempidx;
                                return false;
                            case DialogResult.No:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, LocRm.GetString("Validate_Directory_String") + Environment.NewLine + ex.Message);
                    CameraControl.Camobject.settings.directoryIndex = tempidx;
                    return false;
                }
            }
            else
            {
                if (newdir != olddir)
                {
                    try
                    {
                        if (!Directory.Exists(newdir))
                        {
                            if (Directory.Exists(olddir))
                            {
                                if (MessageBox.Show(this,"Copy Files?",LocRm.GetString("Confirm"),MessageBoxButtons.YesNo)== DialogResult.Yes)
                                    Helper.CopyFolder(olddir, newdir);
                            }
                            else
                            {
                                Directory.CreateDirectory(newdir);
                            }
                        }
                        else
                        {
                            switch (
                                MessageBox.Show(this,
                                    LocRm.GetString("Validate_Directory_Exists"),
                                    LocRm.GetString("Confirm"), MessageBoxButtons.YesNoCancel))
                            {
                                case DialogResult.Yes:
                                    if (Directory.Exists(olddir))
                                    {
                                        if (MessageBox.Show(this, "Copy Files?", LocRm.GetString("Confirm"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            Helper.CopyFolder(olddir,newdir);
                                    }
                                    else
                                    {
                                        Directory.Delete(newdir, true);
                                        Directory.CreateDirectory(newdir);
                                    }
                                    break;
                                case DialogResult.Cancel:
                                    CameraControl.Camobject.settings.directoryIndex = tempidx;
                                    return false;
                                case DialogResult.No:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, LocRm.GetString("Validate_Directory_String") + Environment.NewLine + ex.Message);
                        CameraControl.Camobject.settings.directoryIndex = tempidx;
                        return false;
                    }
                }
            }

            if (!Directory.Exists(newdir + "thumbs\\"))
            {
                Directory.CreateDirectory(newdir + "thumbs\\");
            }
            if (!Directory.Exists(newdir + "grabs\\"))
            {
                Directory.CreateDirectory(newdir + "grabs\\");
            }
                
            CameraControl.Camobject.directory = txtDirectory.Text;
                

            CameraControl.Camobject.schedule.active = chkSchedule.Checked;
            CameraControl.Camobject.settings.active = chkActive.Checked;

            int bufferseconds,
                calibrationdelay,
                inactiveRecord,
                maxrecord;
            int.TryParse(txtBuffer.Text, out bufferseconds);
            int.TryParse(txtCalibrationDelay.Text, out calibrationdelay);
            int.TryParse(txtInactiveRecord.Text, out inactiveRecord);
            int.TryParse(txtMaxRecordTime.Text, out maxrecord);

            CameraControl.Camobject.recorder.bufferseconds = bufferseconds;

            var m = MainForm.Microphones.SingleOrDefault(p => p.id == CameraControl.Camobject.settings.micpair);
            if (m != null)
                m.settings.buffer = CameraControl.Camobject.recorder.bufferseconds;

            CameraControl.Camobject.detector.calibrationdelay = calibrationdelay;
            CameraControl.Camobject.recorder.inactiverecord = inactiveRecord;
            CameraControl.Camobject.alerts.processmode = "continuous";
            if (rdoMotion.Checked)
                CameraControl.Camobject.alerts.processmode = "motion";
            if (rdoTrigger.Checked)
                CameraControl.Camobject.alerts.processmode = "trigger";
            CameraControl.Camobject.recorder.maxrecordtime = maxrecord;
            CameraControl.Camobject.recorder.timelapseenabled = chkTimelapse.Checked;

            CameraControl.Camobject.ftp.enabled = chkFTP.Checked;
            CameraControl.Camobject.ftp.intervalnew = ftpinterval;
            CameraControl.Camobject.ftp.filename = txtFTPFilename.Text;
            CameraControl.Camobject.ftp.text = txtFTPText.Text;
            int ftpmode = 0;
            if (rdoFTPAlerts.Checked)
                ftpmode = 1;
            if (rdoFTPInterval.Checked)
                ftpmode = 2;
            CameraControl.Camobject.ftp.mode = ftpmode;

            CameraControl.Camobject.recorder.timelapseframes = timelapseframes;
            CameraControl.Camobject.recorder.timelapse = timelapsemovie;
            CameraControl.Camobject.recorder.profile = ddlProfile.SelectedIndex;

            CameraControl.Camobject.settings.youtube.@public = chkPublic.Checked;
            CameraControl.Camobject.settings.youtube.tags = txtTags.Text;
            CameraControl.Camobject.settings.maxframeraterecord = (int)numMaxFRRecording.Value;

            CameraControl.Camobject.settings.accessgroups = txtAccessGroups.Text;
            CameraControl.Camobject.detector.recordonalert = rdoRecordAlert.Checked;
            CameraControl.Camobject.detector.recordondetect = rdoRecordDetect.Checked;

            CameraControl.UpdateFloorplans(false);

            CameraControl.Camobject.settings.audiomodel = ddlTalkModel.SelectedItem.ToString();
            CameraControl.Camobject.settings.audioport = (int)numTalkPort.Value;
            CameraControl.Camobject.settings.audioip = txtAudioOutIP.Text.Trim();
            CameraControl.Camobject.settings.audiousername = txtTalkUsername.Text;
            CameraControl.Camobject.settings.audiopassword = txtTalkPassword.Text;
            CameraControl.Camobject.recorder.trigger = ((ListItem)ddlTriggerRecording.SelectedItem).Value;
                
            CameraControl.SetVideoSize();

            if (ddlFTPServer.Enabled)
            {
                int i = ddlFTPServer.SelectedIndex;
                if (i > -1)
                {
                    var ftp = MainForm.Conf.FTPServers[i];
                    CameraControl.Camobject.ftp.ident = ftp.ident;
                }
            }
            CameraControl.Camobject.settings.cloudprovider.images = chkUploadGrabs.Checked;
            CameraControl.Camobject.settings.cloudprovider.recordings = chkUploadRecordings.Checked;

            if (CameraControl != null && CameraControl.Camera != null && CameraControl.Camera.VideoSource != null)
            {
                var vcd = CameraControl.Camera.VideoSource as VideoCaptureDevice;
                if (vcd != null && vcd.SupportsProperties)
                {
                    //save extended properties of local device
                    int b, c, h, s, sh, gam, ce, wb, bc, g;
                    VideoProcAmpFlags fb, fc, fh, fs, fsh, fgam, fce, fwb, fbc, fg;

                    vcd.GetProperty(VideoProcAmpProperty.Brightness, out b, out fb);
                    vcd.GetProperty(VideoProcAmpProperty.Contrast, out c, out fc);
                    vcd.GetProperty(VideoProcAmpProperty.Hue, out h, out fh);
                    vcd.GetProperty(VideoProcAmpProperty.Saturation, out s, out fs);
                    vcd.GetProperty(VideoProcAmpProperty.Sharpness, out sh, out fsh);
                    vcd.GetProperty(VideoProcAmpProperty.Gamma, out gam, out fgam);
                    vcd.GetProperty(VideoProcAmpProperty.ColorEnable, out ce, out fce);
                    vcd.GetProperty(VideoProcAmpProperty.WhiteBalance, out wb, out fwb);
                    vcd.GetProperty(VideoProcAmpProperty.BacklightCompensation, out bc, out fbc);
                    vcd.GetProperty(VideoProcAmpProperty.Gain, out g, out fg);
                            
                    string cfg = "";
                    cfg += "b=" + b + ",fb=" + (int) fb + ",";
                    cfg += "c=" + c + ",fc=" + (int) fc + ",";
                    cfg += "h=" + h + ",fh=" + (int) fh + ",";
                    cfg += "s=" + s + ",fs=" + (int) fs + ",";
                    cfg += "sh=" + sh + ",fsh=" + (int) fsh + ",";
                    cfg += "gam=" + gam + ",fgam=" + (int) fgam + ",";
                    cfg += "ce=" + ce + ",fce=" + (int) fce + ",";
                    cfg += "wb=" + wb + ",fwb=" + (int) fwb + ",";
                    cfg += "bc=" + bc + ",fbc=" + (int) fbc + ",";
                    cfg += "g=" + g + ",fg=" + (int) fg;

                    CameraControl.Camobject.settings.procAmpConfig = cfg;
                }
            }

            if (ddlCloudProviders.SelectedIndex > 0)
                CameraControl.Camobject.settings.cloudprovider.provider = ddlCloudProviders.SelectedItem.ToString();
            else
                CameraControl.Camobject.settings.cloudprovider.provider = "";

            CameraControl.Camobject.settings.cloudprovider.images = chkUploadGrabs.Checked;
            CameraControl.Camobject.settings.cloudprovider.recordings = chkUploadRecordings.Checked;
            CameraControl.Camobject.settings.cloudprovider.path = txtCloudPath.Text;
            CameraControl.Camobject.settings.messaging = chkMessaging.Checked;

            MainForm.NeedsSync = true;
            IsNew = false;
            if (needsFileRefresh)
            {
                CameraControl.GenerateFileList();
                MainForm.NeedsMediaRebuild = true;
                MainForm.NeedsMediaRefresh = Helper.Now;
            }

            return true;
        }

        private void ChkMovementCheckedChanged(object sender, EventArgs e)
        {
            pnlMovement.Enabled = (chkMovement.Checked);
            CameraControl.Camobject.alerts.active = chkMovement.Checked;
        }

        
        private void ChkScheduleCheckedChanged(object sender, EventArgs e)
        {
            pnlScheduler.Enabled = chkSchedule.Checked;
            btnDelete.Enabled = btnUpdate.Enabled = lbSchedule.SelectedIndex > -1;
            lbSchedule.Refresh();
        }

        private void TxtCameraNameKeyUp(object sender, KeyEventArgs e)
        {
            CameraControl.Camobject.name = txtCameraName.Text;
        }


        private void ChkActiveCheckedChanged(object sender, EventArgs e)
        {
            if (CameraControl.Camobject.settings.active != chkActive.Checked)
            {
                if (chkActive.Checked)
                {
                    CameraControl.Enable();
                }
                else
                {
                    CameraControl.Disable();
                }
            }
            btnAdvanced.Enabled = btnCrossbar.Enabled = false;


            if (CameraControl.Camera != null && CameraControl.Camera.VideoSource is VideoCaptureDevice)
            {
                btnAdvanced.Enabled = true;
                btnCrossbar.Enabled = ((VideoCaptureDevice) CameraControl.Camera.VideoSource).CheckIfCrossbarAvailable();
            }
        }

        private void TxtCameraNameTextChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.name = txtCameraName.Text;
        }

        private void AddCameraFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsNew)
            {
                if (!_forceClose && MessageBox.Show(this, LocRm.GetString("DiscardCamera"), LocRm.GetString("Confirm"), MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                if (CameraControl.VolumeControl!=null)
                    MainClass.RemoveMicrophone(CameraControl.VolumeControl, false);
                    
            }
            CameraControl.NewFrame -= CameraNewFrame;
            AreaControl.Dispose();
            CameraControl.IsEdit = false;
            if (CameraControl.VolumeControl != null)
                CameraControl.VolumeControl.IsEdit = false;
        }

        private void DdlMovementDetectorSelectedIndexChanged1(object sender, EventArgs e)
        {
            ddlProcessor.Enabled = rdoMotion.Enabled = (string) _detectortypes[ddlMotionDetector.SelectedIndex] != "None";
            if (!rdoMotion.Enabled)
                rdoContinuous.Checked = true;

            if (CameraControl.Camera != null && CameraControl.Camera.VideoSource != null)
            {
                if ((string) _detectortypes[ddlMotionDetector.SelectedIndex] != CameraControl.Camobject.detector.type)
                {
                    CameraControl.Camobject.detector.type = (string) _detectortypes[ddlMotionDetector.SelectedIndex];
                    CameraControl.SetDetector();
                }
            }
            CameraControl.Camobject.detector.type = (string) _detectortypes[ddlMotionDetector.SelectedIndex];
        }

        

        private void ChkSuppressNoiseCheckedChanged(object sender, EventArgs e)
        {
            if (CameraControl.Camera != null && CameraControl.Camera.VideoSource != null)
            {
                if (CameraControl.Camobject.settings.suppressnoise != chkSuppressNoise.Checked)
                {
                    CameraControl.Camobject.settings.suppressnoise = chkSuppressNoise.Checked;
                    CameraControl.SetDetector();
                }
            }
        }


        private void Button2Click(object sender, EventArgs e)
        {
            GoPrevious();
        }

        private void TcCameraSelectedIndexChanged(object sender, EventArgs e)
        {
            btnBack.Enabled = tcCamera.SelectedIndex != 0;

            btnNext.Enabled = tcCamera.SelectedIndex != tcCamera.TabCount - 1;
        }

        private void Button1Click1(object sender, EventArgs e)
        {
            
        }

        private void DdlProcessorSelectedIndexChanged(object sender, EventArgs e)
        {
            if (CameraControl.Camera != null && CameraControl.Camera.VideoSource != null &&
                CameraControl.Camera.MotionDetector != null)
            {
                if ((string) _processortypes[ddlProcessor.SelectedIndex] != CameraControl.Camobject.detector.postprocessor)
                {
                    CameraControl.Camobject.detector.postprocessor = (string) _processortypes[ddlProcessor.SelectedIndex];
                    CameraControl.SetProcessor();
                }
            }
            CameraControl.Camobject.detector.postprocessor = (string) _processortypes[ddlProcessor.SelectedIndex];
        }

        private void Button2Click1(object sender, EventArgs e)
        {
            List<objectsCameraScheduleEntry> scheds = CameraControl.Camobject.schedule.entries.ToList();
            var sched = new objectsCameraScheduleEntry();
            if (ConfigureSchedule(sched))
            {
                scheds.Add(sched);
                CameraControl.Camobject.schedule.entries = scheds.ToArray();
                ShowSchedule(CameraControl.Camobject.schedule.entries.Count() - 1);
            }
        }

        private bool ConfigureSchedule(objectsCameraScheduleEntry sched)
        {
            if (ddlHourStart.SelectedItem.ToString() != "-" && ddlMinuteStart.SelectedItem.ToString() == "-")
            {
                ddlMinuteStart.SelectedIndex = 1;
            }
            if (ddlHourEnd.SelectedItem.ToString() != "-" && ddlMinuteEnd.SelectedItem.ToString() == "-")
            {
                ddlMinuteEnd.SelectedIndex = 1;
            }

            if (ddlHourStart.SelectedItem.ToString() == "-" || ddlMinuteStart.SelectedItem.ToString() == "-")
            {
                sched.start = "-:-";
            }
            else
                sched.start = ddlHourStart.SelectedItem + ":" + ddlMinuteStart.SelectedItem;
            if (ddlHourEnd.SelectedItem.ToString() == "-" || ddlMinuteEnd.SelectedItem.ToString() == "-")
            {
                sched.stop = "-:-";
            }
            else
                sched.stop = ddlHourEnd.SelectedItem + ":" + ddlMinuteEnd.SelectedItem;

            sched.daysofweek = "";
            if (chkMon.Checked)
            {
                sched.daysofweek += "1,";
            }
            if (chkTue.Checked)
            {
                sched.daysofweek += "2,";
            }
            if (chkWed.Checked)
            {
                sched.daysofweek += "3,";
            }
            if (chkThu.Checked)
            {
                sched.daysofweek += "4,";
            }
            if (chkFri.Checked)
            {
                sched.daysofweek += "5,";
            }
            if (chkSat.Checked)
            {
                sched.daysofweek += "6,";
            }
            if (chkSun.Checked)
            {
                sched.daysofweek += "0,";
            }
            sched.daysofweek = sched.daysofweek.Trim(',');
            if (sched.daysofweek == "")
            {
                MessageBox.Show(LocRm.GetString("Validate_Camera_SelectOneDay"));
                return false;
            }

            sched.recordonstart = chkRecordSchedule.Checked;
            sched.active = chkScheduleActive.Checked;
            sched.recordondetect = chkScheduleRecordOnDetect.Checked;
            sched.recordonalert = chkRecordAlertSchedule.Checked;
            sched.alerts = chkScheduleAlerts.Checked;
            sched.timelapseenabled = chkScheduleTimelapse.Checked;
            sched.ftpenabled = chkSchedFTPEnabled.Checked;
            sched.savelocalenabled = chkSchedSaveLocalEnabled.Checked;
            sched.ptz = chkschedPTZ.Checked;
            sched.messaging = chkScheduleMessaging.Checked;

            return true;
        }

        private void LbScheduleKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSchedule();
            }
        }

        private void DeleteSchedule()
        {
            if (lbSchedule.SelectedIndex > -1)
            {
                int i = lbSchedule.SelectedIndex;
                List<objectsCameraScheduleEntry> scheds = CameraControl.Camobject.schedule.entries.ToList();
                scheds.RemoveAt(i);
                CameraControl.Camobject.schedule.entries = scheds.ToArray();
                int j = i;
                if (j == scheds.Count)
                    j--;
                if (j < 0)
                    j = 0;
                ShowSchedule(j);
                if (lbSchedule.Items.Count == 0)
                    btnDelete.Enabled = btnUpdate.Enabled = false;
                else
                    btnDelete.Enabled = btnUpdate.Enabled = (lbSchedule.SelectedIndex > -1);
            }
        }

        private void DdlHourStartSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LinkLabel1LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-motion-detection.aspx#2");
        }

        
        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            gbFTP.Enabled = chkFTP.Checked;
        }

        private void LinkLabel2LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/userguide-ftp.aspx");
        }

        private void DdlProcessFramesSelectedIndexChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.detector.processeveryframe = Convert.ToInt32(ddlProcessFrames.SelectedItem);
        }

        private void Login()
        {
            MainClass.Connect(MainForm.Website + "/subscribe.aspx", false);
        }
        
        private void Button3Click(object sender, EventArgs e)
        {
            DeleteSchedule();
        }

        private void LbScheduleSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSchedule.Items.Count == 0)
                btnDelete.Enabled = btnUpdate.Enabled = false;
            else
            {
                btnUpdate.Enabled = btnDelete.Enabled = (lbSchedule.SelectedIndex > -1);
                if (btnUpdate.Enabled)
                {
                    int i = lbSchedule.SelectedIndex;
                    objectsCameraScheduleEntry sched = CameraControl.Camobject.schedule.entries[i];

                    string[] start = sched.start.Split(':');
                    string[] stop = sched.stop.Split(':');


                    ddlHourStart.SelectedItem = start[0];
                    ddlHourEnd.SelectedItem = stop[0];
                    ddlMinuteStart.SelectedItem = start[1];
                    ddlMinuteEnd.SelectedItem = stop[1];

                    chkMon.Checked = sched.daysofweek.IndexOf("1", StringComparison.Ordinal) != -1;
                    chkTue.Checked = sched.daysofweek.IndexOf("2", StringComparison.Ordinal) != -1;
                    chkWed.Checked = sched.daysofweek.IndexOf("3", StringComparison.Ordinal) != -1;
                    chkThu.Checked = sched.daysofweek.IndexOf("4", StringComparison.Ordinal) != -1;
                    chkFri.Checked = sched.daysofweek.IndexOf("5", StringComparison.Ordinal) != -1;
                    chkSat.Checked = sched.daysofweek.IndexOf("6", StringComparison.Ordinal) != -1;
                    chkSun.Checked = sched.daysofweek.IndexOf("0", StringComparison.Ordinal) != -1;

                    chkRecordSchedule.Checked = sched.recordonstart;
                    chkScheduleActive.Checked = sched.active;
                    chkScheduleRecordOnDetect.Checked = sched.recordondetect;
                    chkScheduleAlerts.Checked = sched.alerts;
                    chkRecordAlertSchedule.Checked = sched.recordonalert;
                    chkScheduleTimelapse.Checked = sched.timelapseenabled;
                    chkSchedFTPEnabled.Checked = sched.ftpenabled;
                    chkSchedSaveLocalEnabled.Checked = sched.savelocalenabled;
                    chkschedPTZ.Checked = sched.ptz;
                    chkScheduleMessaging.Checked = sched.messaging;
                }
            }
        }

        private void PnlPtzMouseDown(object sender, MouseEventArgs e)
        {
            CameraControl.Camobject.settings.ptzusername = txtPTZUsername.Text;
            CameraControl.Camobject.settings.ptzpassword = txtPTZPassword.Text;
            CameraControl.Camobject.settings.ptzchannel = txtPTZChannel.Text;

            ProcessPtzInput(e.Location);
        }


        private void ProcessPtzInput(Point p)
        {
            var comm = Enums.PtzCommand.Center;
            bool cmd = false;

            if (p.X > 170 && p.Y < 45)
            {
                comm = Enums.PtzCommand.ZoomIn;
                cmd = true;
            }
            if (p.X > 170 && p.Y > 45 && p.Y < 90)
            {
                comm = Enums.PtzCommand.ZoomOut;
                cmd = true;
            }

            if (cmd)
            {
                CameraControl.Calibrating = true;
                CameraControl.PTZ.SendPTZCommand(comm);
            }
            else
            {
                double angle = Math.Atan2(86 - p.Y, 86 - p.X);
                CameraControl.Calibrating = true;
                CameraControl.PTZ.SendPTZDirection(angle);
            }

            
        }

        private void DdlPtzSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPTZ.SelectedItem is ListItem)
            {
                var li = (ListItem) ddlPTZ.SelectedItem;
                CameraControl.Camobject.ptz = Convert.ToInt32(li.Value);
                CameraControl.Camobject.ptzentryindex = -1;
                CameraControl.PTZ.PTZSettings = null;
            }
            else
            {
                var entry = (PTZEntry)ddlPTZ.SelectedItem;
                CameraControl.Camobject.ptz = entry.Id;
                CameraControl.Camobject.ptzentryindex = entry.Index;
            }

            lbExtended.Items.Clear();
            btnAddPreset.Visible = btnDeletePreset.Visible = false;



            if (CameraControl.Camobject.ptz > -1)
            {
                PTZSettings2Camera ptz = MainForm.PTZs.Single(p => p.id == CameraControl.Camobject.ptz);
                CameraControl.PTZ.PTZSettings = ptz;
                if (ptz.ExtendedCommands != null && ptz.ExtendedCommands.Command!=null)
                {
                    foreach (var extcmd in ptz.ExtendedCommands.Command)
                    {
                        lbExtended.Items.Add(new ListItem(extcmd.Name, extcmd.Value));
                    }
                }
                if (_loaded)    
                    txtPTZURL.Text = ptz.CommandURL;
            }
            if (CameraControl.Camobject.ptz==-3 || CameraControl.Camobject.ptz==-4)
            {
                foreach(string cmd in PTZController.PelcoCommands)
                {
                    lbExtended.Items.Add(new ListItem(cmd, cmd));
                }
                
            }

            if (CameraControl.Camobject.ptz == -5)
            {
                PopOnvifPresets();
            }
            switch (CameraControl.Camobject.ptz)
            {
                case -1:
                case -6:
                    tableLayoutPanel12.Enabled = false;
                    break;
                default:
                    tableLayoutPanel12.Enabled = true;
                    break;

            }
            

            bool bPelco = CameraControl.Camobject.ptz == -3 || CameraControl.Camobject.ptz == -4;
            bool bConfig = CameraControl.Camobject.ptz >= 0;

             txtPTZChannel.Visible =label91.Visible = txtPTZPassword.Visible = label42.Visible = txtPTZUsername.Visible =label22.Visible = txtPTZURL.Visible =label18.Visible = bConfig;

            btnConfigurePelco.Visible = bPelco;

        }

        private void PopOnvifPresets()
        {
            lbExtended.Items.Clear();
            btnAddPreset.Visible = btnDeletePreset.Visible = true;
            foreach (string cmd in CameraControl.PTZ.ONVIFPresets)
            {
                lbExtended.Items.Add(new ListItem(cmd, cmd));
            }
        }

        private void PnlPtzPaint(object sender, PaintEventArgs e)
        {
        }

        private void LbExtendedClick(object sender, EventArgs e)
        {
            if (lbExtended.SelectedIndex > -1)
            {
                var li = ((ListItem) lbExtended.SelectedItem);
                SendPtzCommand(li.Value, true);
            }
        }


        private void PnlPtzMouseUp(object sender, MouseEventArgs e)
        {
            PTZSettings2Camera ptz = MainForm.PTZs.SingleOrDefault(p => p.id == CameraControl.Camobject.ptz);
            if ((ptz != null && ptz.Commands.Stop!=""))
                SendPtzCommand(ptz.Commands.Stop,true);

            if (CameraControl.PTZ.IsContinuous)
                CameraControl.PTZ.SendPTZCommand(Enums.PtzCommand.Stop);
        }

        private void SendPtzCommand(string cmd, bool wait)
        {
            if (cmd == "")
            {
                MessageBox.Show(LocRm.GetString("CommandNotSupported"));
                return;
            }
            try
            {
                CameraControl.Calibrating = true;
                CameraControl.PTZ.SendPTZCommand(cmd, wait);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    LocRm.GetString("Validate_Camera_PTZIPOnly") + Environment.NewLine + Environment.NewLine +
                    ex.Message, LocRm.GetString("Error"));
            }
        }

        private void PnlPtzMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //todo: add drag to move cam around
            }
        }

        private void LbExtendedSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LinkLabel6LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var d = new downloader
            {
                Url = MainForm.Website + "/getcontent.aspx?name=PTZ2",
                SaveLocation = Program.AppDataPath + @"XML\PTZ2.xml"
            };
            d.ShowDialog(this);
            if (d.DialogResult == DialogResult.OK)
            {
                MainForm.PTZs = null;
                LoadPTZs();
            }
            d.Dispose();
        }

        private void TabPage9Click(object sender, EventArgs e)
        {
        }

        private void ShowSettings(int tabindex)
        {
            string lang = MainForm.Conf.Language;
            MainClass.ShowSettings(tabindex, this);
            if (lang != MainForm.Conf.Language)
                RenderResources();
        }


        private void DdlTimestampKeyUp(object sender, KeyEventArgs e)
        {
            CameraControl.Camobject.settings.timestampformatter = ddlTimestamp.Text;
        }

        private void BtnMaskImageClick(object sender, EventArgs e)
        {
            ofdDetect.FileName = "";
            ofdDetect.InitialDirectory = Program.AppPath + @"backgrounds\";
            ofdDetect.Filter = "Image Files (*.png)|*.png";
            ofdDetect.ShowDialog(this);
            if (ofdDetect.FileName != "")
            {
                txtMaskImage.Text = ofdDetect.FileName;
            }
        }

        private void TxtMaskImageTextChanged(object sender, EventArgs e)
        {
            if (File.Exists(txtMaskImage.Text))
            {
                try
                {
                    CameraControl.Camobject.settings.maskimage = txtMaskImage.Text;
                    if (CameraControl.Camera != null)
                        CameraControl.Camera.Mask = (Bitmap)Image.FromFile(txtMaskImage.Text);
                }
                catch
                {
                }
            }
            else
            {
                CameraControl.Camobject.settings.maskimage = "";
                if (CameraControl.Camera!=null)
                    CameraControl.Camera.Mask = null;
            }
        }


        //private void ChkFlipYCheckedChanged(object sender, EventArgs e)
        //{
        //    CameraControl.Camobject.flipy = chkFlipY.Checked;
        //}

        //private void ChkFlipXCheckedChanged(object sender, EventArgs e)
        //{
        //    CameraControl.Camobject.flipx = chkFlipX.Checked;
        //}

        private void BtnUpdateClick(object sender, EventArgs e)
        {
            int i = lbSchedule.SelectedIndex;
            objectsCameraScheduleEntry sched = CameraControl.Camobject.schedule.entries[i];

            if (ConfigureSchedule(sched))
            {
                ShowSchedule(i);
            }
        }

        private void ChkScheduleActiveCheckedChanged(object sender, EventArgs e)
        {
        }

        private void LbScheduleDrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            int i = e.Index;
            if (i >= 0)
            {
                objectsCameraScheduleEntry sched = CameraControl.Camobject.schedule.entries[i];

                Font f = sched.active ? new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold) : new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
                Brush b = !chkSchedule.Checked ? Brushes.Gray : Brushes.Black;

                e.Graphics.DrawString(lbSchedule.Items[i].ToString(), f, b, e.Bounds);
                e.DrawFocusRectangle();
            }
        }


        private void ChkRecordScheduleCheckedChanged(object sender, EventArgs e)
        {
            if (chkRecordSchedule.Checked)
            {
                chkScheduleRecordOnDetect.Checked = false;
                chkRecordAlertSchedule.Checked = false;
            }
        }

        private void ChkScheduleRecordOnDetectCheckedChanged(object sender, EventArgs e)
        {
            if (chkScheduleRecordOnDetect.Checked)
            {
                chkRecordSchedule.Checked = false;
                chkRecordAlertSchedule.Checked = false;
            }
        }

        private void LinkLabel8LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/userguide-pairing.aspx");
        }

        private void ChkRecordAlertScheduleCheckedChanged(object sender, EventArgs e)
        {
            if (chkRecordAlertSchedule.Checked)
            {
                chkRecordSchedule.Checked = false;
                chkScheduleRecordOnDetect.Checked = false;
                chkScheduleAlerts.Checked = true;
            }
        }

        private void ChkScheduleAlertsCheckedChanged(object sender, EventArgs e)
        {
            if (!chkScheduleAlerts.Checked)
                chkRecordAlertSchedule.Checked = false;
        }

        private void RdoFtpIntervalCheckedChanged(object sender, EventArgs e)
        {
            txtUploadEvery.Enabled = rdoFTPInterval.Checked;
        }

        private void rdoFTPAlerts_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Button3Click3(object sender, EventArgs e)
        {
            if (Helper.HasFeature(Enums.Features.Motion_Detection))
            {
                ConfigureSeconds cf;
                switch (ddlAlertMode.SelectedIndex)
                {
                    case 0:
                        cf = new ConfigureSeconds
                                 {
                                     Seconds = CameraControl.Camobject.detector.movementintervalnew
                                 };
                        cf.ShowDialog(this);
                        if (cf.DialogResult == DialogResult.OK)
                            CameraControl.Camobject.detector.movementintervalnew = cf.Seconds;
                        cf.Dispose();
                        return;
                    case 1:
                        cf = new ConfigureSeconds
                                 {
                                     Seconds = CameraControl.Camobject.detector.nomovementintervalnew
                                 };
                        cf.ShowDialog(this);
                        if (cf.DialogResult == DialogResult.OK)
                            CameraControl.Camobject.detector.nomovementintervalnew = cf.Seconds;
                        cf.Dispose();
                        return;
                    case 2:
                        var coc = new ConfigureObjectCount
                                      {
                                          Objects = CameraControl.Camobject.alerts.objectcountalert
                                      };
                        coc.ShowDialog(this);

                        if (coc.DialogResult == DialogResult.OK)
                            CameraControl.Camobject.alerts.objectcountalert = coc.Objects;
                        coc.Dispose();
                        return;
                }
            }

            if (ddlAlertMode.SelectedIndex == -1)
                return;

            switch (ddlAlertMode.SelectedItem.ToString())
            {
                case "Virtual Trip Wires":
                    _ctw = new ConfigureTripWires();
                    _ctw.TripWireEditor1.Init(CameraControl.Camobject.alerts.pluginconfig);
                    _ctw.ShowDialog(this);
                    CameraControl.Camobject.alerts.pluginconfig = _ctw.TripWireEditor1.Config;
                    if (CameraControl.Camera != null && CameraControl.Camera.VideoSource is KinectStream)
                    {
                        ((KinectStream) CameraControl.Camera.VideoSource).InitTripWires(
                            CameraControl.Camobject.alerts.pluginconfig);
                    }
                    _ctw.Dispose();
                    _ctw = null;
                    break;
                default:
                    if (CameraControl.Camera != null && CameraControl.Camera.Plugin != null)
                    {
                        CameraControl.ConfigurePlugin();
                    }
                    else
                    {
                        MessageBox.Show(this,
                                        LocRm.GetString("Validate_Initialise_Camera"));
                    }
                    break;
            }


        }        

        private void DdlAlertModeSelectedIndexChanged(object sender, EventArgs e)
        {
            string last = CameraControl.Camobject.alerts.mode;
            flowLayoutPanel5.Enabled = Helper.HasFeature(Enums.Features.Motion_Detection);
            if (flowLayoutPanel5.Enabled)
                flowLayoutPanel5.Enabled = ddlAlertMode.SelectedIndex > _alertmodes.Length-1;
            if (!flowLayoutPanel5.Enabled)
                rdoContinuous.Checked = true;

            if (Helper.HasFeature(Enums.Features.Motion_Detection) && ddlAlertMode.SelectedIndex < _alertmodes.Length)
            {
                CameraControl.Camobject.alerts.mode = _alertmodes[ddlAlertMode.SelectedIndex];
                if (ddlAlertMode.SelectedIndex==2)
                {
                    ddlProcessor.SelectedIndex = 1;
                }
            }
            else
            {
                CameraControl.Camobject.alerts.mode = ddlAlertMode.SelectedItem.ToString();
            }

            if (last != ddlAlertMode.SelectedItem.ToString())
            {
                if (CameraControl.Camera != null && CameraControl.Camera.Plugin != null)
                {
                    CameraControl.Camera.Plugin = null;
                    CameraControl.Camobject.alerts.pluginconfig = "";
                }
            }
            button3.Enabled = true;
        }

        private void ChkTimelapseCheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = chkTimelapse.Checked;
        }

        private void chkPublic_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void LinkLabel9LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login();
        }

        #region Nested type: ListItem

        private struct ListItem
        {
            private readonly string _name;
            internal readonly string Value;

            public ListItem(string name, string value)
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

        private void chkPTZFlipX_CheckedChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.ptzflipx = chkPTZFlipX.Checked;
        }

        private void chkPTZFlipY_CheckedChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.ptzflipy = chkPTZFlipY.Checked;
        }

        private void chkPTZRotate90_CheckedChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.ptzrotate90 = chkPTZRotate90.Checked;
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void txtPTZURL_TextChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.ptzurlbase = txtPTZURL.Text;
        }

        private void numMaxFR_ValueChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.maxframerate = (int)numMaxFR.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var cp = new ConfigureProcessor(CameraControl);
            if (cp.ShowDialog(this)== DialogResult.OK)
            {
                if (CameraControl.Camera != null && CameraControl.Camera.MotionDetector != null)
                {
                    CameraControl.SetDetector();
                }
            }
            cp.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ConfigFilter();
        }

        private void chkColourProcessing_CheckedChanged(object sender, EventArgs e)
        {
            if (chkColourProcessing.Checked)
            {
                if (String.IsNullOrEmpty(CameraControl.Camobject.detector.colourprocessing))
                {
                    if (!ConfigFilter())
                        chkColourProcessing.Checked = false;
                }
            }
            CameraControl.Camobject.detector.colourprocessingenabled = chkColourProcessing.Checked;
        }

        private bool ConfigFilter()
        {
            _filterForm = new HSLFilteringForm(CameraControl.Camobject.detector.colourprocessing) { ImageProcess = CameraControl.Camera==null?null: CameraControl.LastFrame };
            _filterForm.ShowDialog(this);
            if (_filterForm.DialogResult == DialogResult.OK)
            {
                CameraControl.Camobject.detector.colourprocessing = _filterForm.Configuration;
                if (CameraControl.Camera!=null)
                    CameraControl.Camera.FilterChanged();
                _filterForm.Dispose();
                _filterForm = null;
                chkColourProcessing.Checked = true;
                return true;
            }

            _filterForm.Dispose();
            _filterForm = null;
            return false;
        }

        private void AddCamera_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/userguide-camera-settings.aspx");
        }

        private void llblHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = MainForm.Website+"/userguide-camera-settings.aspx";
            switch (tcCamera.SelectedTab.Name)
            {
                case "tabPage1":
                    url=MainForm.Website+"/userguide-camera-settings.aspx";
                    break;
                case "tabPage3":
                    url = MainForm.Website+"/userguide-motion-detection.aspx";
                    break;
                case "tabPage2":
                    url = MainForm.Website+"/userguide-alerts.aspx";
                    break;
                case "tabPage4":
                    url = MainForm.Website+"/userguide-recording.aspx";
                    break;
                case "tabPage8":
                    url = MainForm.Website+"/userguide-ptz.aspx";
                    break;
                case "tabPage7":
                case "tabPage10":
                    url = MainForm.Website+"/userguide-ftp.aspx";
                    break;
                case "tabPage9":
                    url = MainForm.Website+"/userguide-youtube.aspx";
                    break;
                case "tabPage5":
                    url = MainForm.Website+"/userguide-scheduling.aspx";
                    break;
            }
            MainForm.OpenUrl( url);
        }

        private void btnTimestamp_Click(object sender, EventArgs e)
        {
            var ct = new ConfigureTimestamp
                         {
                             TimeStampLocation = CameraControl.Camobject.settings.timestamplocation,
                             Offset = CameraControl.Camobject.settings.timestampoffset,
                             TimestampForeColor = CameraControl.Camobject.settings.timestampforecolor.ToColor(),
                             TimestampBackColor = CameraControl.Camobject.settings.timestampbackcolor.ToColor(),
                             CustomFont = FontXmlConverter.ConvertToFont(CameraControl.Camobject.settings.timestampfont),
                             TimestampShowBack = CameraControl.Camobject.settings.timestampshowback,
                             TagsNV =  CameraControl.Camobject.settings.tagsnv
                         };

            if (ct.ShowDialog(this)== DialogResult.OK)
            {
                CameraControl.Camobject.settings.timestamplocation = ct.TimeStampLocation;
                CameraControl.Camobject.settings.timestampfont = ct.CustomFont.SerializeFontAttribute;
                CameraControl.Camobject.settings.timestampoffset = ct.Offset;
                CameraControl.Camobject.settings.timestampforecolor = ct.TimestampForeColor.ToRGBString();
                CameraControl.Camobject.settings.timestampbackcolor = ct.TimestampBackColor.ToRGBString();
                CameraControl.Camobject.settings.timestampshowback = ct.TimestampShowBack;
                CameraControl.Camobject.settings.tagsnv = ct.TagsNV;
                

                if (CameraControl.Camera != null)
                {
                    CameraControl.Camera.DrawFont = null;
                    CameraControl.Camera.ForeBrush = CameraControl.Camera.BackBrush = null;
                    CameraControl.Camera.Tags = null;
                }
            }
            ct.Dispose();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website+"/plugins.aspx");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void rdoContinuous_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ddlCopyFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCopyFrom.SelectedIndex>0)
            {
                var cam =
                    MainForm.Cameras.SingleOrDefault(
                        p => p.id == Convert.ToInt32(((ListItem) ddlCopyFrom.SelectedItem).Value));
                if (cam!=null)
                {
                    List<objectsCameraScheduleEntry> scheds = cam.schedule.entries.ToList();

                    CameraControl.Camobject.schedule.entries = scheds.ToArray();
                    ShowSchedule(CameraControl.Camobject.schedule.entries.Count() - 1);                    
                }
            }
        }

        private void llblEditPTZ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("Notepad.exe", Program.AppDataPath + @"XML\PTZ2.xml");
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.PTZs = null;
            LoadPTZs();
        }

        private void txtBuffer_ValueChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var dir = Helper.GetMediaDirectory(2, CameraControl.Camobject.id);
            string path = dir + "video\\" + txtDirectory.Text + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = dir + "video\\" + txtDirectory.Text + "\\grabs\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Process.Start(path);
        }

        private void ddlProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            //not sure why i was doing this, must have been a reason...
            //numMaxFRRecording.Enabled = ddlProfile.SelectedIndex < 3;
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            try
            {
                var vcd = CameraControl.Camera.VideoSource as VideoCaptureDevice;
                if (vcd!=null)
                {
                    vcd.DisplayPropertyPage(Handle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnCrossbar_Click(object sender, EventArgs e)
        {
            try {
                if (CameraControl.Camera!=null)
                    ((VideoCaptureDevice)CameraControl.Camera.VideoSource).DisplayCrossbarPropertyPage(Handle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMic_Click(object sender, EventArgs e)
        {
            var cms = new CameraMicSource
                          {
                              CameraControl = this.CameraControl,
                              StartPosition = FormStartPosition.CenterParent
                          };
            cms.ShowDialog(this);

            CameraControl.SetVolumeLevel(CameraControl.Camobject.settings.micpair);

            if (CameraControl.Camobject.settings.micpair>-1)
            {
                var m = MainForm.Microphones.SingleOrDefault(p => p.id == CameraControl.Camobject.settings.micpair);
                if (m != null)
                {
                    lblMicSource.Text = m.name;
                    m.settings.buffer = CameraControl.Camobject.recorder.bufferseconds;
                    CameraControl.SetVolumeLevelLocation();
                    chkIgnoreAudio.Checked = false;
                }
            }
            else
            {
                lblMicSource.Text = LocRm.GetString("None");
            }

            
        }

        private void ddlTimestamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.timestampformatter = ddlTimestamp.Text;
        }

        private void chkLocalSaving_CheckedChanged(object sender, EventArgs e)
        {
            gbLocal.Enabled = chkLocalSaving.Checked;
        }

        private void PopulateTalkDevices()
        {
            var models = new [] {"None", "Local Playback","Axis", "Foscam", "iSpyServer", "NetworkKinect", "IP Webcam (Android)"};
            foreach(string m in models)
            {
                ddlTalkModel.Items.Add(m);
            }
            try
            {
                ddlTalkModel.SelectedItem = CameraControl.Camobject.settings.audiomodel;
            }
            catch
            {
            }
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string lang = MainForm.Conf.Language;
            MainClass.ShowSettings(6,this);
            if (lang != MainForm.Conf.Language)
                RenderResources();
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-grant-access.aspx");
        }

        private void txtPTZPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label81_Click(object sender, EventArgs e)
        {

        }

        private void chkIgnoreAudio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIgnoreAudio.Checked)
            {
                if (CameraControl.VolumeControl!=null)
                {
                    MainClass.RemoveMicrophone(CameraControl.VolumeControl, false);
                }
            }
        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AreaControl.ClearRectangles();
            CameraControl.Camobject.detector.motionzones = AreaControl.MotionZones;
            CameraControl.SetMotionZones();            
            AreaControl.Invalidate();
        }

        private void txtPTZChannel_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtPTZChannel_Leave(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.ptzchannel = txtPTZChannel.Text;
        }

        private void btnConfigurePelco_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CameraControl.Camobject.settings.ptzpelcoconfig))
            {
                //default
                CameraControl.Camobject.settings.ptzpelcoconfig = "COM1|9600|8|One|Odd|1";
            }
            var pc = new PelcoConfig { Config = CameraControl.Camobject.settings.ptzpelcoconfig};
            if (pc.ShowDialog(this) == DialogResult.OK)
            {
                CameraControl.Camobject.settings.ptzpelcoconfig = pc.Config;
                CameraControl.PTZ.ConfigurePelco();
            }

            pc.Dispose();

        }

        private void btnAddPreset_Click(object sender, EventArgs e)
        {
            var p = new Prompt(LocRm.GetString("EnterName"),"");
            if (p.ShowDialog(this)==DialogResult.OK)
            {
                var s = p.Val.Trim();
                if (!String.IsNullOrEmpty(s))
                {
                    if (CameraControl.PTZ != null)
                    {
                        CameraControl.PTZ.AddPreset(s);
                        PopOnvifPresets();
                    }
                }
            }
            p.Dispose();
        }

        private void btnDeletePreset_Click(object sender, EventArgs e)
        {
            var p = lbExtended.SelectedItem;
            if (p!=null)
            {
                if (CameraControl.PTZ != null)
                {
                    var li = (ListItem) p;
                    CameraControl.PTZ.DeletePreset(li.Value);
                    PopOnvifPresets();
                }
            }

        }

        private void chkStorageManagement_CheckedChanged(object sender, EventArgs e)
        {
            tblStorage.Enabled = chkStorageManagement.Checked;

        }

        private void btnRunNow_Click(object sender, EventArgs e)
        {
            SetStorageManagement();

            MainClass.RunStorageManagement(true);
        }

        private void SetStorageManagement()
        {
            CameraControl.Camobject.settings.storagemanagement.enabled = chkStorageManagement.Checked;
            CameraControl.Camobject.settings.storagemanagement.maxage = (int)numMaxAge.Value;
            CameraControl.Camobject.settings.storagemanagement.maxsize = (int)numMaxFolderSize.Value;
            CameraControl.Camobject.settings.storagemanagement.archive = chkArchive.Checked;
        }

        private void rdoMotion_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void actionEditor1_Load(object sender, EventArgs e)
        {

        }

        private void ddlRotateFlip_SelectedIndexChanged(object sender, EventArgs e)
        {
            var li = (ListItem) ddlRotateFlip.SelectedItem;

            bool changed = CameraControl.Camobject.rotateMode != li.Value;
            
            if (changed)
            {
                RotateFlipType rmold,rmnew;
                Enum.TryParse(CameraControl.Camobject.rotateMode, out rmold);
                Enum.TryParse(li.Value, out rmnew);

                
                var bmp1 = new Bitmap(12, 6);
                bmp1.RotateFlip(rmold);
                var bmp2 = new Bitmap(12, 6);
                bmp2.RotateFlip(rmnew);

                var reset = bmp1.Width != bmp2.Width;

                bmp1.Dispose();
                bmp2.Dispose();

                CameraControl.Camobject.rotateMode = li.Value;
                
                if (CameraControl.Camobject.settings.active)
                {
                    if (reset)
                    {
                        chkActive.Enabled = true;
                        chkActive.Checked = false;
                        Thread.Sleep(1000); //allows unmanaged code to complete shutdown
                        chkActive.Checked = true;
                        CameraControl.NeedSizeUpdate = true;
                    }
                    else
                    {
                        if (CameraControl.Camera!=null)
                            CameraControl.Camera.RotateFlipType = rmnew;
                    }
                    
                }
            }           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainClass.ShowSettings(2, this);
            LoadMediaDirectories();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var vsa = new VideoSourceAdvanced { Camobject = CameraControl.Camobject };
            vsa.ShowDialog(this);
            vsa.Dispose();
        }

        private void ddlEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlActionType.SelectedIndex > -1)
            {
                string at = "alert";
                switch (ddlActionType.SelectedIndex)
                {
                    case 1:
                        at = "alertstopped";
                        break;
                    case 2:
                        at = "disconnect";
                        break;
                    case 3:
                        at = "reconnect";
                        break;
                    case 4:
                        at = "reconnectfailed";
                        break;
                    case 5:
                        at = "recordingstarted";
                        break;
                    case 6:
                        at = "recordingstopped";
                        break;

                }
                
                actionEditor1.Init(at,CameraControl.Camobject.id,2);
            }
        }

        private void rdoSaveInterval_CheckedChanged(object sender, EventArgs e)
        {
            numSaveInterval.Enabled = rdoSaveInterval.Checked;
        }

        private void flowLayoutPanel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowSettings(3);
            PopFTPServers();
        }

        private void PopFTPServers()
        {
            ddlFTPServer.Items.Clear();
            ddlFTPServer.Enabled = true;
            int i = -1, j=0;
            foreach (var ftp in MainForm.Conf.FTPServers)
            {
                ddlFTPServer.Items.Add(ftp.name);
                if (CameraControl.Camobject.ftp.ident == ftp.ident)
                {
                    i = j;
                }
                j++;
            }

            if (i > -1)
                ddlFTPServer.SelectedIndex = i;
            else
            {
                if (ddlFTPServer.Items.Count > 0)
                    ddlFTPServer.SelectedIndex = 0;
                else
                {
                    ddlFTPServer.Items.Add(LocRm.GetString("None"));
                    ddlFTPServer.Enabled = false;
                }
            }
            
        }

        private void btnAuthorise_Click(object sender, EventArgs e)
        {
            switch (ddlCloudProviders.SelectedItem.ToString())
            {
                case "Google Drive":
                    if (Cloud.Drive.Authorise())
                        MessageBox.Show(this, "OK");
                    else
                        MessageBox.Show(this, LocRm.GetString("Failed"));
                    return;
                case "Dropbox":
                    if (Cloud.Dropbox.Authorise())
                        MessageBox.Show(this, "OK");
                    else
                        MessageBox.Show(this, LocRm.GetString("Failed"));
                    return;
            }
        }

        private void ddlCloudProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAuthorise.Enabled = ddlCloudProviders.SelectedIndex > 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var t = new PTZTracking {CameraControl = CameraControl};
            t.ShowDialog(this);
            if (CameraControl.Camobject.settings.ptzautotrack)
            {
                ddlMotionDetector.SelectedIndex = 0;
                ddlProcessor.SelectedIndex = 1;
                CameraControl.SetDetector();
            }
            t.Dispose();

        }

        private void btnPTZSchedule_Click(object sender, EventArgs e)
        {
            var s = new PTZScheduler {CameraControl = CameraControl};
            s.ShowDialog(this);
            s.Dispose();
        }

        private void btnPiP_Click(object sender, EventArgs e)
        {
            _pip = new PiPConfig {pip = CameraControl.Camobject.settings.pip, CW = CameraControl};
            _pip.ShowDialog(this);
            _pip.Dispose();
            _pip = null;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Save())
            {
                using (var ct = new CopyTo {OC = CameraControl.Camobject})
                {
                    ct.ShowDialog(this);
                }
            }

        }

        private void btnAuthoriseYouTube_Click(object sender, EventArgs e)
        {
            if (YouTubeUploader.Authorise())
            {
                MessageBox.Show(this, LocRm.GetString("OK"));
            }
            else
                MessageBox.Show(this, LocRm.GetString("Failed"));
        }

    }
}