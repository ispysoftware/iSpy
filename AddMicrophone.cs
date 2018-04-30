using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Controls;
using NAudio.Wave;

namespace iSpyApplication
{
    public partial class AddMicrophone : Form
    {
        private readonly object[] _actiontypes = { "Alert", "AlertStopped", "Connection Lost", "Reconnect", "ReconnectFailed", "RecordingAlertStarted", "RecordingAlertStopped" };
        public VolumeLevel VolumeLevel;
        private bool _loaded;
        public bool IsNew;
        public MainForm MainClass;

        public AddMicrophone()
        {
            InitializeComponent();
            RenderResources();
        }

        private void BtnSelectSourceClick(object sender, EventArgs e)
        {
            SelectSource();
        }

        private bool SelectSource()
        {
            bool success = false;
            var ms = new MicrophoneSource {Mic = VolumeLevel.Micobject};

            ms.ShowDialog(this);
            if (ms.DialogResult == DialogResult.OK)
            {
                chkActive.Enabled = true;
                chkActive.Checked = false;
                Application.DoEvents();
                VolumeLevel.Micobject.settings.needsupdate = true;
                lblAudioSource.Text = VolumeLevel.Micobject.settings.sourcename;
                chkActive.Checked = true;
                success = true;
            }
            ms.Dispose();
            return success;
        }

        void Ranger1ValueMinChanged()
        {
            VolumeLevel.Micobject.detector.minsensitivity = ranger1.ValueMin;
        }

        void Ranger1ValueMaxChanged()
        {
            VolumeLevel.Micobject.detector.maxsensitivity = ranger1.ValueMax;
        }

        void Ranger1GainChanged()
        {
            VolumeLevel.Micobject.detector.gain = ranger1.Gain;
        }

        private void AddMicrophoneLoad(object sender, EventArgs e)
        {
            if (VolumeLevel.Micobject.id == -1)
            {
                if (!SelectSource())
                {
                    Close();
                    return;
                }
            }
            if (VolumeLevel.Micobject.id == -1)
            {
                VolumeLevel.Micobject.id = MainForm.NextMicrophoneId;
            }
            VolumeLevel.IsEdit = true;
            if (VolumeLevel.CameraControl != null)
                VolumeLevel.CameraControl.IsEdit = true;
            
            btnBack.Enabled = false;
            txtMicrophoneName.Text = VolumeLevel.Micobject.name;
            //tbGain.Value = (int)(VolumeLevel.Micobject.settings.gain * 100);

            chkSound.Checked = VolumeLevel.Micobject.alerts.active;
            rdoRecordDetect.Checked = VolumeLevel.Micobject.detector.recordondetect;
            rdoRecordAlert.Checked = VolumeLevel.Micobject.detector.recordonalert;
            rdoNoRecord.Checked = !rdoRecordDetect.Checked && !rdoRecordAlert.Checked;

            if (VolumeLevel.Micobject.alerts.mode == "sound")
                rdoMovement.Checked = true;
            else
                rdoNoMovement.Checked = true;

            chkSchedule.Checked = VolumeLevel.Micobject.schedule.active;
            chkActive.Checked = VolumeLevel.Micobject.settings.active;

            chkActive.Enabled = VolumeLevel.Micobject.settings.sourcename != "";

            if (VolumeLevel.Micobject.settings.sourcename != "")
            {
                lblAudioSource.Text = "";
                if (VolumeLevel.Micobject.settings.typeindex == 4)
                {
                    int icam;
                    if (Int32.TryParse(VolumeLevel.Micobject.settings.sourcename, out icam))
                    {
                        var c = MainForm.Cameras.SingleOrDefault(p => p.id == icam);
                        if (c != null)
                            lblAudioSource.Text = c.name;
                    }

                }
                if (lblAudioSource.Text=="")
                    lblAudioSource.Text = VolumeLevel.Micobject.settings.sourcename;
            }
            else
            {
                lblAudioSource.Text = LocRm.GetString("NoSource");
                chkActive.Checked = false;
            }


            
            Text = LocRm.GetString("EditMicrophone");
            if (VolumeLevel.Micobject.id > -1)
                Text += string.Format(" (ID: {0}, DIR: {1})", VolumeLevel.Micobject.id, VolumeLevel.Micobject.directory);

            txtNoSound.Text = VolumeLevel.Micobject.detector.nosoundinterval.ToString(CultureInfo.InvariantCulture);
            txtSound.Text = VolumeLevel.Micobject.detector.soundinterval.ToString(CultureInfo.InvariantCulture);
            pnlSound.Enabled = chkSound.Checked;

            txtBuffer.Text = VolumeLevel.Micobject.settings.buffer.ToString(CultureInfo.InvariantCulture);
            txtInactiveRecord.Text = VolumeLevel.Micobject.recorder.inactiverecord.ToString(CultureInfo.InvariantCulture);
            txtMaxRecordTime.Text = VolumeLevel.Micobject.recorder.maxrecordtime.ToString(CultureInfo.InvariantCulture);

            scheduleEditor1.Io = VolumeLevel;

            txtAccessGroups.Text = VolumeLevel.Micobject.settings.accessgroups;
            txtDirectory.Text = VolumeLevel.Micobject.directory;
            

            tblStorage.Enabled = chkStorageManagement.Checked = VolumeLevel.Micobject.settings.storagemanagement.enabled;
            numMaxAge.Value = VolumeLevel.Micobject.settings.storagemanagement.maxage;
            numMaxFolderSize.Value = VolumeLevel.Micobject.settings.storagemanagement.maxsize;
            numMinRecord.Value = VolumeLevel.Micobject.recorder.minrecordtime;

            ddlCopyFrom.Items.Clear();
            ddlCopyFrom.Items.Add(new ListItem(LocRm.GetString("CopyFrom"), "-1"));
            foreach (objectsMicrophone c in MainForm.Microphones)
            {
                if (c.id!=VolumeLevel.Micobject.id)
                    ddlCopyFrom.Items.Add(new ListItem(c.name, c.id.ToString(CultureInfo.InvariantCulture)));
            }
            ddlCopyFrom.SelectedIndex = 0;


            int i = 0, j = 0;
            foreach(var dev in DirectSoundOut.Devices)
            {
                ddlPlayback.Items.Add(new ListItem(dev.Description, dev.Guid.ToString()));
                if (dev.Guid.ToString() == VolumeLevel.Micobject.settings.deviceout)
                    i = j;
                j++;
            }
            if (ddlPlayback.Items.Count>0)
                ddlPlayback.SelectedIndex = i;
            else
            {
                ddlPlayback.Items.Add(LocRm.GetString("NoAudioDevices"));
                ddlPlayback.Enabled = false;
            }

            tmrUpdateSourceDetails.Start();

            foreach (string dt in _actiontypes)
            {
                ddlActionType.Items.Add(LocRm.GetString(dt));
            }
            ddlActionType.SelectedIndex = 0;

            string t2 = VolumeLevel.Micobject.recorder.trigger ?? "";

            
            ddlTriggerRecording.Items.Add(new ListItem("None", ""));

            foreach (var c in MainForm.Cameras.Where(p => p.settings.micpair!=VolumeLevel.Micobject.id))
            {   
                ddlTriggerRecording.Items.Add(new ListItem(c.name, "2," + c.id));   
            }
            foreach (var c in MainForm.Microphones.Where(p=>p.id != VolumeLevel.Micobject.id))
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

            if (VolumeLevel.CameraControl != null)
            {
                txtBuffer.Enabled = false;
                toolTip1.SetToolTip(txtBuffer,"Change the buffer on the paired camera to update");
            }

            actionEditor1.LoginRequested += ActionEditor1LoginRequested;
            chkArchive.Checked = VolumeLevel.Micobject.settings.storagemanagement.archive;

            LoadMediaDirectories();

            ranger1.Maximum = 100;
            ranger1.Minimum = 0.001;
            ranger1.ValueMin = VolumeLevel.Micobject.detector.minsensitivity;
            ranger1.ValueMax = VolumeLevel.Micobject.detector.maxsensitivity;
            ranger1.Gain = VolumeLevel.Micobject.detector.gain;
            ranger1.ValueMinChanged += Ranger1ValueMinChanged;
            ranger1.ValueMaxChanged += Ranger1ValueMaxChanged;
            ranger1.GainChanged += Ranger1GainChanged;
            ranger1.SetText();

            intervalConfig1.Init(VolumeLevel);

            chkMessaging.Checked = VolumeLevel.Micobject.settings.messaging;
            _loaded = true;

        }

        void ActionEditor1LoginRequested(object sender, EventArgs e)
        {
            Login();
        }

        private void RenderResources()
        {
            btnBack.Text = LocRm.GetString("Back");
            btnFinish.Text = LocRm.GetString("Finish");
            btnNext.Text = LocRm.GetString("Next");
            btnSelectSource.Text = "...";
            chkActive.Text = LocRm.GetString("MicrophoneActive");
            groupBox1.Text = LocRm.GetString("RecordingSettings");
            groupBox6.Text = LocRm.GetString("RecordingSettings");
            groupBox6.Text = LocRm.GetString("RecordingMode");
            rdoRecordDetect.Text = LocRm.GetString("RecordOnSoundDetection");
            rdoRecordAlert.Text = LocRm.GetString("RecordOnAlert");
            rdoNoRecord.Text = LocRm.GetString("NoRecord");
            chkSchedule.Text = LocRm.GetString("ScheduleMicrophone");
            chkSound.Text = LocRm.GetString("AlertsEnabled");
            label1.Text = LocRm.GetString("Name");
            label12.Text = LocRm.GetString("MaxRecordTime");
            label6.Text = LocRm.GetString("MinRecordTime");
            label13.Text = LocRm.GetString("Seconds");
            label14.Text = LocRm.GetString("Seconds");
            label15.Text = LocRm.GetString("DistinctAlertInterval");
            label16.Text = LocRm.GetString("Seconds");
            label17.Text = LocRm.GetString("Seconds");
            label19.Text = LocRm.GetString("InactivityRecord");
            label2.Text = LocRm.GetString("Source");
            label20.Text = LocRm.GetString("BufferAudio");
            label21.Text = LocRm.GetString("ExitThisToEnableAlertsAnd");
            label3.Text = LocRm.GetString("Sensitivity");
            label4.Text = LocRm.GetString("WhenSound");
            label48.Text = LocRm.GetString("Seconds");
            label5.Text = LocRm.GetString("Seconds");
            label15.Text = LocRm.GetString("Intervals");
            
            lblAudioSource.Text = LocRm.GetString("Audiosource");
            rdoMovement.Text = LocRm.GetString("IsDetectedFor");
            rdoNoMovement.Text = LocRm.GetString("IsNotDetectedFor");
            tabPage1.Text = LocRm.GetString("Microphone");
            tabPage2.Text = LocRm.GetString("Alerts");
            tabPage3.Text = LocRm.GetString("Scheduling");
            tabPage4.Text = LocRm.GetString("Recording");
            Text = LocRm.GetString("Addmicrophone");

            toolTip1.SetToolTip(txtMicrophoneName, LocRm.GetString("ToolTip_MicrophoneName"));
            toolTip1.SetToolTip(txtInactiveRecord, LocRm.GetString("ToolTip_InactiveRecordAudio"));
            toolTip1.SetToolTip(txtBuffer, LocRm.GetString("ToolTip_BufferAudio"));
            llblHelp.Text = LocRm.GetString("help");
            lblAccessGroups.Text = LocRm.GetString("AccessGroups");
            toolTip1.SetToolTip(ranger1, LocRm.GetString("ToolTip_MotionSensitivity"));
            label74.Text = LocRm.GetString("Directory");

            LocRm.SetString(label23,"Listen");
            LocRm.SetString(label22, "TriggerRecording");

            label11.Text = LocRm.GetString("MediaLocation");
            label74.Text = LocRm.GetString("Directory");
            chkStorageManagement.Text = LocRm.GetString("EnableStorageManagement");
            label85.Text = LocRm.GetString("MaxFolderSizeMb");
            label94.Text = LocRm.GetString("MaxAgeHours");
            chkArchive.Text = LocRm.GetString("ArchiveInsteadOfDelete");
            btnRunNow.Text = LocRm.GetString("RunNow");

            HideTab(tabPage2, Helper.HasFeature(Enums.Features.Alerts));
            HideTab(tabPage4, Helper.HasFeature(Enums.Features.Recording));
            HideTab(tabPage3, Helper.HasFeature(Enums.Features.Scheduling));
            HideTab(tabPage5, Helper.HasFeature(Enums.Features.Storage));
            LocRm.SetString(linkLabel4, "CopyTo");


        }

        private void HideTab(TabPage t, bool show)
        {
            if (!show)
            {
                tcMicrophone.TabPages.Remove(t);
            }
        }

        private void BtnNextClick(object sender, EventArgs e)
        {
            GoNext();
        }

        private void GoNext()
        {
            tcMicrophone.SelectedIndex++;
        }

        private void GoPrevious()
        {
            tcMicrophone.SelectedIndex--;
        }

        private bool CheckStep1()
        {
            string err = "";
            string name = txtMicrophoneName.Text.Trim();
            if (name == "")
                err += LocRm.GetString("Validate_Microphone_EnterName") + Environment.NewLine;
            if (
                MainForm.Microphones.SingleOrDefault(
                    p => p.name.ToLower() == name.ToLower() && p.id != VolumeLevel.Micobject.id) != null)
                err += LocRm.GetString("Validate_Microphone_NameInUse") + Environment.NewLine;


            if (VolumeLevel.Micobject.settings.sourcename == "")
            {
                err += LocRm.GetString("Validate_Microphone_SelectSource"); //"";
            }
            if (err != "")
            {
                MessageBox.Show(err, LocRm.GetString("Error"));
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
            if (!CheckStep1())
                return false;

            string err = "";
                
            int nosoundinterval;
            if (!int.TryParse(txtNoSound.Text, out nosoundinterval))
                err += LocRm.GetString("Validate_Microphone_NoSound") + Environment.NewLine;
            int soundinterval;
            if (!int.TryParse(txtSound.Text, out soundinterval))
                err += LocRm.GetString("Validate_Microphone_Sound") + Environment.NewLine;


            if (txtBuffer.Text.Length < 1 || txtInactiveRecord.Text.Length < 1 ||
                txtMaxRecordTime.Text.Length < 1)
            {
                err += LocRm.GetString("Validate_Camera_RecordingSettings") + Environment.NewLine;
            }

            if (err != "")
            {
                MessageBox.Show(err, LocRm.GetString("Error"));
                return false;
            }


            VolumeLevel.Micobject.settings.buffer = Convert.ToInt32(txtBuffer.Value);
            VolumeLevel.Micobject.recorder.inactiverecord = Convert.ToInt32(txtInactiveRecord.Value);
            VolumeLevel.Micobject.recorder.maxrecordtime = Convert.ToInt32(txtMaxRecordTime.Value);

            VolumeLevel.Micobject.name = txtMicrophoneName.Text.Trim();

            VolumeLevel.Micobject.alerts.active = chkSound.Checked;
                
            VolumeLevel.Micobject.alerts.mode = "sound";
            if (rdoNoMovement.Checked)
                VolumeLevel.Micobject.alerts.mode = "nosound";
            VolumeLevel.Micobject.detector.nosoundinterval = nosoundinterval;
            VolumeLevel.Micobject.detector.soundinterval = soundinterval;
                
            VolumeLevel.Micobject.schedule.active = chkSchedule.Checked;
            VolumeLevel.Micobject.width = VolumeLevel.Width;
            VolumeLevel.Micobject.height = VolumeLevel.Height;

            VolumeLevel.Micobject.settings.active = chkActive.Checked;
            VolumeLevel.Micobject.detector.recordondetect = rdoRecordDetect.Checked;
            VolumeLevel.Micobject.detector.recordonalert = rdoRecordAlert.Checked;
            VolumeLevel.Micobject.recorder.minrecordtime = (int)numMinRecord.Value;
                
            VolumeLevel.Micobject.settings.accessgroups = txtAccessGroups.Text;
            VolumeLevel.Micobject.settings.messaging = chkMessaging.Checked;


            if (txtDirectory.Text.Trim() == "")
                txtDirectory.Text = MainForm.RandomString(5);

            var md = (ListItem)ddlMediaDirectory.SelectedItem;
            var newind = Convert.ToInt32(md.Value);

            string olddir = Helper.GetMediaDirectory(1, VolumeLevel.Micobject.id) + "video\\" + VolumeLevel.Micobject.directory + "\\";

            bool needsFileRefresh = (VolumeLevel.Micobject.directory != txtDirectory.Text || VolumeLevel.Micobject.settings.directoryIndex != newind);

            int tempidx = VolumeLevel.Micobject.settings.directoryIndex;
            VolumeLevel.Micobject.settings.directoryIndex = newind;
                
            string newdir = Helper.GetMediaDirectory(1, VolumeLevel.Micobject.id) + "video\\" + txtDirectory.Text + "\\";

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
                                VolumeLevel.Micobject.settings.directoryIndex = tempidx;
                                return false;
                            case DialogResult.No:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, LocRm.GetString("Validate_Directory_String") + Environment.NewLine + ex.Message);
                    VolumeLevel.Micobject.settings.directoryIndex = tempidx;
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
                                if (MessageBox.Show(this, "Copy Files?", LocRm.GetString("Confirm"), MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                                            Helper.CopyFolder(olddir, newdir);
                                    }
                                    else
                                    {
                                        Directory.Delete(newdir, true);
                                        Directory.CreateDirectory(newdir);
                                    }
                                    break;
                                case DialogResult.Cancel:
                                    VolumeLevel.Micobject.settings.directoryIndex = tempidx;
                                    return false;
                                case DialogResult.No:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, LocRm.GetString("Validate_Directory_String") + Environment.NewLine + ex.Message);
                        VolumeLevel.Micobject.settings.directoryIndex = tempidx;
                        return false;
                    }
                }
            }

                
            VolumeLevel.Micobject.directory = txtDirectory.Text;
            VolumeLevel.Micobject.recorder.trigger = ((ListItem)ddlTriggerRecording.SelectedItem).Value;

            SetStorageManagement();

            MainForm.NeedsSync = true;

            if (needsFileRefresh)
            {
                VolumeLevel.GenerateFileList();
                MainForm.NeedsMediaRebuild = true;
                MainForm.NeedsMediaRefresh = Helper.Now;
            }
            return true;
            
        }

        public bool IsNumeric(string numberString)
        {
            return numberString.All(char.IsNumber);
        }

        private void ChkSoundCheckedChanged(object sender, EventArgs e)
        {
            pnlSound.Enabled = chkSound.Checked;
            VolumeLevel.Micobject.alerts.active = chkSound.Checked;
        }

        private void ChkScheduleCheckedChanged(object sender, EventArgs e)
        {
            scheduleEditor1.Enabled = chkSchedule.Checked;
        }

        private void TxtMicrophoneNameTextChanged(object sender, EventArgs e)
        {
            VolumeLevel.Micobject.name = txtMicrophoneName.Text;
        }

        
        private void AddMicrophoneFormClosing(object sender, FormClosingEventArgs e)
        {
            VolumeLevel.IsEdit = false;
            if (VolumeLevel.CameraControl != null)
                VolumeLevel.CameraControl.IsEdit = false;
            tmrUpdateSourceDetails.Stop();
            tmrUpdateSourceDetails.Dispose();
        }

        private void ChkActiveCheckedChanged(object sender, EventArgs e)
        {
            if (chkActive.Checked != VolumeLevel.Micobject.settings.active)
            {
                if (chkActive.Checked)
                    VolumeLevel.Enable();
                else
                    VolumeLevel.Disable();
            }
        }

        private void RdoMovementCheckedChanged(object sender, EventArgs e)
        {
            if (VolumeLevel.Micobject.alerts.mode != "sound" && rdoMovement.Checked)
            {
                VolumeLevel.Micobject.alerts.mode = "sound";
            }
        }

        private void RdoNoMovementCheckedChanged(object sender, EventArgs e)
        {
            if (VolumeLevel.Micobject.alerts.mode != "nosound" && rdoNoMovement.Checked)
            {
                VolumeLevel.Micobject.alerts.mode = "nosound";
            }
        }

        private void Button1Click(object sender, EventArgs e)
        {
            GoPrevious();
        }


        private void TcMicrophoneSelectedIndexChanged(object sender, EventArgs e)
        {
            btnBack.Enabled = tcMicrophone.SelectedIndex != 0;

            btnNext.Enabled = tcMicrophone.SelectedIndex != tcMicrophone.TabCount - 1;
        }


        private void Login()
        {
            MainClass.Connect(MainForm.Website + "/subscribe.aspx", false);
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

        private void llblHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = MainForm.Website+"/userguide-microphone-settings.aspx";
            switch (tcMicrophone.SelectedTab.Name)
            {
                case "tabPage1":
                    url = MainForm.Website+"/userguide-microphone-settings.aspx#1";
                    break;
                case "tabPage2":
                    url = MainForm.Website+"/userguide-microphone-alerts.aspx";
                    break;
                case "tabPage4":
                    url = MainForm.Website+"/userguide-microphone-recording.aspx#2";
                    break;
                case "tabPage3":
                    url = MainForm.Website+"/userguide-scheduling.aspx#6";
                    break;
            }
            MainForm.OpenUrl( url);
        }

        private void ddlCopyFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCopyFrom.SelectedIndex > 0)
            {
                if (MessageBox.Show(LocRm.GetString("Confirm"), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var mic =
                    MainForm.Microphones.SingleOrDefault(
                        p => p.id == Convert.ToInt32(((ListItem)ddlCopyFrom.SelectedItem).Value));
                    if (mic != null)
                    {
                        MainForm.Schedule.RemoveAll(p => p.objectid == VolumeLevel.ObjectID && p.objecttypeid == 1);
                        List<objectsScheduleEntry> scheds = MainForm.Schedule.Where(p => p.objecttypeid == 1 && p.objectid == mic.id).ToList();
                        foreach (var s in scheds)
                        {
                            var t = new objectsScheduleEntry
                            {
                                active = s.active,
                                daysofweek = s.daysofweek,
                                objectid = VolumeLevel.ObjectID,
                                objecttypeid = 2,
                                parameter = s.parameter,
                                time = s.time,
                                typeid = s.typeid
                            };
                            MainForm.Schedule.Add(t);
                        }
                        scheduleEditor1.RenderSchedule();

                    }
                }
                ddlCopyFrom.SelectedIndex = 0;
            }
        }

        private void tmrUpdateSourceDetails_Tick(object sender, EventArgs e)
        {
            if (VolumeLevel.Micobject.settings.needsupdate)
                lblFormat.Text = "...";
            else
            {
                string txt = VolumeLevel.Micobject.settings.samples + " hz (" +
                                 VolumeLevel.Micobject.settings.channels + " Channel";
                if (VolumeLevel.Micobject.settings.channels != 1)
                    txt += "s";
                txt += ")";
                lblFormat.Text = txt;
            }
            
        }

        private void ddlPlayback_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loaded && ddlPlayback.SelectedIndex>-1 && ddlPlayback.Enabled)
            {
                var g = new Guid(((ListItem) ddlPlayback.SelectedItem).Value);
                VolumeLevel.Micobject.settings.deviceout = g.ToString();
                bool listen = false;
                if (VolumeLevel.WaveOut!=null)
                {
                    if (VolumeLevel.Listening)
                    {
                        listen = true;
                        VolumeLevel.Listening = false;
                        Application.DoEvents();
                    }
                }
                VolumeLevel.WaveOut = new DirectSoundOut(g, 100);
                VolumeLevel.Listening = listen;
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
            VolumeLevel.Micobject.settings.storagemanagement.enabled = chkStorageManagement.Checked;
            VolumeLevel.Micobject.settings.storagemanagement.maxage = (int)numMaxAge.Value;
            VolumeLevel.Micobject.settings.storagemanagement.maxsize = (int)numMaxFolderSize.Value;
            VolumeLevel.Micobject.settings.storagemanagement.archive = chkArchive.Checked;
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-grant-access.aspx");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainClass.ShowSettings(2, this);
            LoadMediaDirectories();
        }

        private void LoadMediaDirectories()
        {
            ddlMediaDirectory.Items.Clear();
            foreach (var s in MainForm.Conf.MediaDirectories)
            {
                ddlMediaDirectory.Items.Add(new ListItem(s.Entry, s.ID.ToString(CultureInfo.InvariantCulture)));
                if (s.ID == VolumeLevel.Micobject.settings.directoryIndex)
                    ddlMediaDirectory.SelectedItem = ddlMediaDirectory.Items[ddlMediaDirectory.Items.Count - 1];
            }
            if (ddlMediaDirectory.SelectedIndex == -1)
                ddlMediaDirectory.SelectedIndex = 0;
        }

        private void ddlActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlActionType.SelectedIndex > -1)
            {
                string at = "alert";
                switch (ddlActionType.SelectedIndex)
                {
                    case 1:
                        at = "disconnect";
                        break;
                    case 2:
                        at = "reconnect";
                        break;
                    case 3:
                        at = "reconnectfailed";
                        break;
                }


                actionEditor1.Init(at, VolumeLevel.Micobject.id, 1);
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Save())
            {
                using (var ct = new CopyTo { OM = VolumeLevel.Micobject})
                {
                    ct.ShowDialog(this);
                }
            }
        }
    }
}