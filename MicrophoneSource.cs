using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Realtime;
using iSpyApplication.Sources.Video;
using iSpyApplication.Utilities;
using NAudio.Wave;

namespace iSpyApplication
{
    public partial class MicrophoneSource : Form
    {
        private object[] SampleRates = {8000, 11025, 12000, 16000, 22050, 32000, 44100, 48000};
        private readonly string _noDevices = LocRm.GetString("NoAudioDevices");
        public objectsMicrophone Mic;

        public MicrophoneSource()
        {
            InitializeComponent();
            RenderResources();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Finish();
        }
        private int GetSourceIndex()
        {
            int sourceIndex = 0;
            if (tcAudioSource.SelectedTab.Equals(tabPage1))
                sourceIndex = 0;
            if (tcAudioSource.SelectedTab.Equals(tabPage3))
                sourceIndex = 1;
            if (tcAudioSource.SelectedTab.Equals(tabPage2))
                sourceIndex = 2;
            if (tcAudioSource.SelectedTab.Equals(tabPage4))
                sourceIndex = 3;
            if (tcAudioSource.SelectedTab.Equals(tabPage5))
                sourceIndex = 4;
            if (tcAudioSource.SelectedTab.Equals(tabPage6))
                sourceIndex = 5;
            if (tcAudioSource.SelectedTab.Equals(tabPage7))
                sourceIndex = 6;
          
            return sourceIndex;
        }
        private void Finish()
        {
            int sourceIndex = GetSourceIndex();
            switch (sourceIndex)
            {
                case 0:
                    if (!ddlDevice.Enabled)
                    {
                        Close();
                        return;
                    }
                    Mic.settings.sourcename = ddlDevice.SelectedItem.ToString();

                    int i = 0, selind = -1;
                    for (int n = 0; n < WaveIn.DeviceCount; n++)
                    {
                        ddlDevice.Items.Add(WaveIn.GetCapabilities(n).ProductName);
                        if (WaveIn.GetCapabilities(n).ProductName == Mic.settings.sourcename)
                            selind = i;
                        i++;
                    }

                    int channels = WaveIn.GetCapabilities(selind).Channels;

                    Mic.settings.channels = channels;
                    Mic.settings.samples = Convert.ToInt32(ddlSampleRate.SelectedItem);
                    Mic.settings.bits = 16;

                    break;
                case 1:
                    try
                    {
                        var url = new Uri(txtNetwork.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    Mic.settings.sourcename = txtNetwork.Text;

                    //set format
                    Mic.settings.channels = 1;
                    Mic.settings.samples = 22050;
                    Mic.settings.bits = 16;
                    break;
                case 2:
                {
                    string t = cmbVLCURL.Text.Trim();
                    if (t == String.Empty)
                    {
                        MessageBox.Show(LocRm.GetString("Validate_Microphone_SelectSource"), LocRm.GetString("Error"));
                        return;
                    }
                    Mic.settings.sourcename = t;
                }
                    break;
                case 3:
                    try
                    {
                        var url = new Uri(cmbFFMPEGURL.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    Mic.settings.sourcename = cmbFFMPEGURL.Text;
                    break;
                case 5:
                    if (ddlCloneMicrophone.SelectedIndex > -1)
                    {
                        int micid = (int)((MainForm.ListItem) ddlCloneMicrophone.SelectedItem).Value;
                        Mic.settings.sourcename = micid.ToString(CultureInfo.InvariantCulture);
                        var mic = MainForm.Microphones.First(p => p.id == micid);
                        Mic.name = "Clone: " + mic.name;
                    }
                    else
                    {
                        MessageBox.Show(this, LocRm.GetString("SelectMicrophoneToClone"));
                        return;
                    }
                    break;
                case 6:
                    try
                    {
                        var url = new Uri(txtWavStreamURL.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    Mic.settings.sourcename = txtWavStreamURL.Text;

                    //set default format
                    Mic.settings.channels = (int)numChannels.Value;
                    Mic.settings.samples = (int)numSamples.Value;
                    Mic.settings.bits = 16;
                    break;
            }

            MainForm.Conf.VLCURL = cmbVLCURL.Text.Trim();
            if (!MainForm.Conf.RecentVLCList.Contains(MainForm.Conf.VLCURL) &&
                MainForm.Conf.VLCURL != "")
            {
                MainForm.Conf.RecentVLCList =
                    (MainForm.Conf.RecentVLCList + "|" + MainForm.Conf.VLCURL).Trim('|');
            }

            Mic.settings.typeindex = sourceIndex;
            Mic.settings.decompress = true; // chkDecompress.Checked;
            Mic.settings.vlcargs = txtVLCArgs.Text.Trim();

            Mic.settings.analyzeduration = (int)numAnalyseDuration.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            //cancel
            Close();
        }
        
        private object[] ObjectList(string str)
        {
            string[] ss = str.Split('|');
            var o = new object[ss.Length];
            int i = 0;
            foreach (string s in ss)
            {
                o[i] = s;
                i++;
            }
            return o;
        }

        private void SetSourceIndex(int sourceIndex)
        {
            switch (sourceIndex)
            {
                case 0:
                    tcAudioSource.SelectedTab = tabPage1;
                    break;
                case 1:
                    tcAudioSource.SelectedTab = tabPage3;
                    break;
                case 2:
                    tcAudioSource.SelectedTab = tabPage2;
                    break;
                case 3:
                    tcAudioSource.SelectedTab = tabPage4;
                    break;
                case 4:
                    tcAudioSource.SelectedTab = tabPage5;
                    break;
                case 5:
                    tcAudioSource.SelectedTab = tabPage6;
                    break;
                case 6:
                    tcAudioSource.SelectedTab = tabPage7;
                    break;
            }

            if (tcAudioSource.SelectedTab == null)
            {
                if (tcAudioSource.TabCount == 0)
                {
                    MessageBox.Show(this, LocRm.GetString("CouldNotDisplayControls"));
                    Close();
                }
                else
                {
                    tcAudioSource.SelectedIndex = 0;
                }
            }
        }

        private void MicrophoneSourceLoad(object sender, EventArgs e)
        {
            UISync.Init(this);
            tableLayoutPanel2.Enabled = VlcHelper.VlcInstalled;
            linkLabel3.Visible = lblInstallVLC.Visible = !tableLayoutPanel2.Enabled;
            cmbVLCURL.Text = MainForm.Conf.VLCURL;
            cmbVLCURL.Items.AddRange(ObjectList(MainForm.Conf.RecentVLCList));
            cmbFFMPEGURL.Items.AddRange(ObjectList(MainForm.Conf.RecentVLCList));
            ddlSampleRate.Items.AddRange(SampleRates);
            try
            {
                int selind = -1;
                
                for (int n = 0; n < WaveIn.DeviceCount; n++)
                {
                    ddlDevice.Items.Add(WaveIn.GetCapabilities(n).ProductName);
                    if (WaveIn.GetCapabilities(n).ProductName == Mic.settings.sourcename)
                        selind = n;
                }
                

                ddlDevice.Enabled = true;

                if (selind > -1)
                    ddlDevice.SelectedIndex = selind;
                else
                {
                    if (ddlDevice.Items.Count == 0)
                    {
                        ddlDevice.Items.Add(_noDevices);
                        ddlDevice.Enabled = false;
                    }
                    else
                        ddlDevice.SelectedIndex = 0;
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogException(ex);
                ddlDevice.Items.Add(_noDevices);
                ddlDevice.Enabled = false;
            }
            ddlSampleRate.SelectedIndex = 0;
            foreach (var mic in MainForm.Microphones)
            {
                if (mic.id != Mic.id && mic.settings.typeindex != 5) //dont allow a clone of a clone as the events get too complicated (and also it's pointless)
                    ddlCloneMicrophone.Items.Add(new MainForm.ListItem(mic.name, mic.id));
            }

            SetSourceIndex(Mic.settings.typeindex);
            numSamples.Value = Mic.settings.samples;
            numChannels.Value = Mic.settings.channels;

            switch (Mic.settings.typeindex)
            {
                case 0:
                    if (ddlDevice.Items.Count > 0)
                    {
                        tcAudioSource.SelectedIndex = 0;
                        int j = 0;
                        foreach(int s in ddlSampleRate.Items)
                        {
                            if (s == Mic.settings.samples)
                                ddlSampleRate.SelectedIndex = j;
                            j++;
                        }
                    }
                    break;
                case 1:
                    txtNetwork.Text = Mic.settings.sourcename;
                    break;
                case 2:
                    cmbVLCURL.Text = Mic.settings.sourcename;
                    break;
                case 3:
                    cmbFFMPEGURL.Text = Mic.settings.sourcename;
                    break;
                case 4:
                    int i;
                    Int32.TryParse(Mic.settings.sourcename, out i);
                    var c = MainForm.Cameras.SingleOrDefault(p => p.id == i);
                    lblCamera.Text = c == null ? LocRm.GetString("Removed") : c.name;
                    break;
                case 5:
                    int id;
                    if (Int32.TryParse(Mic.settings.sourcename, out id))
                    {
                        foreach (MainForm.ListItem li in ddlCloneMicrophone.Items)
                        {
                            if ((int)li.Value == id)
                            {
                                ddlCloneMicrophone.SelectedItem = li;
                                break;
                            }
                        }
                    }
                    break;
                case 6:
                    txtWavStreamURL.Text = Mic.settings.sourcename;
                    break;
            }
            

            txtVLCArgs.Text = Mic.settings.vlcargs.Replace("\r\n", "\n").Replace("\n\n", "\n").Replace("\n", Environment.NewLine);

            numAnalyseDuration.Value = Mic.settings.analyzeduration;
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("Microphonesource");
            button1.Text = LocRm.GetString("Ok");
            button2.Text = LocRm.GetString("Cancel");
            label8.Text = LocRm.GetString("Device");
            label9.Text = LocRm.GetString("Url");
            tabPage1.Text = LocRm.GetString("LocalDevice");
            tabPage3.Text = LocRm.GetString("iSpyServer");
            tabPage2.Text = LocRm.GetString("VLCPlugin");

            label18.Text = LocRm.GetString("Arguments");
            lblInstallVLC.Text = LocRm.GetString("VLCConnectInfo");
            linkLabel3.Text = LocRm.GetString("DownloadVLC");
            linkLabel1.Text = LocRm.GetString("UseiSpyServerText");

            llblHelp.Text = LocRm.GetString("help");
            LocRm.SetString(label21,"FileURL");
            LocRm.SetString(label18, "Arguments");
            LocRm.SetString(lblInstallVLC, "VLCHelp");
            lblInstallVLC.Text = lblInstallVLC.Text.Replace("x86", Program.Platform);
            LocRm.SetString(label2, "FileURL");
            LocRm.SetString(btnTest, "Test");
            LocRm.SetString(lblCamera, "NoCamera");

            LocRm.SetString(tabPage5, "Camera");
            LocRm.SetString(label1,"SampleRate");
            LocRm.SetString(label7, "AnalyseDurationMS");
            LocRm.SetString(label14, "Microphone");

        }


        private void DdlDeviceSelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        private void TextBox1TextChanged(object sender, EventArgs e)
        {
        }

        private void LinkLabel3LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Program.Platform == "x64")
                MessageBox.Show(this, LocRm.GetString("InstallVLCx64").Replace("[DIR]", Environment.NewLine + Program.AppPath + "VLC64" + Environment.NewLine));
            else
                MessageBox.Show(this, LocRm.GetString("InstallVLCx86"));
            MainForm.OpenUrl(Program.Platform == "x64" ? MainForm.VLCx64 : MainForm.VLCx86);
        }

        private void LinkLabel1LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/download_ispyserver.aspx");
        }

        private void llblHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = MainForm.Website+"/userguide-microphones.aspx";
            switch (GetSourceIndex())
            {
                case 0:
                    url = MainForm.Website+"/userguide-microphones.aspx#1";
                    break;
                case 1:
                    url = MainForm.Website+"/userguide-microphones.aspx#3";
                    break;
                case 2:
                    url = MainForm.Website+"/userguide-microphones.aspx#2";
                    break;
            }
            MainForm.OpenUrl( url);
        }

        public bool NoBuffer;
        private MediaStream afr;
        private void Test_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;

            try
            {               
                string source = cmbFFMPEGURL.Text;
                int i = source.IndexOf("://", StringComparison.Ordinal);
                if (i > -1)
                {
                    source = source.Substring(0, i).ToLower() + source.Substring(i);
                }
                Mic.settings.sourcename = source;
                Mic.settings.analyzeduration = (int) numAnalyseDuration.Value;
                afr = new MediaStream(Mic);

                afr.DataAvailable += Afr_AudioAvailable;
                afr.ErrorHandler += Afr_ErrorHandler;
                afr.PlayingFinished += Afr_PlayingFinished;
                afr.Start();            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        #region Nested type: UISync

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try
                {
                    _sync.BeginInvoke(action, null);
                }
                catch
                {
                }
            }
        }

        #endregion

        private void Afr_PlayingFinished(object sender, Sources.PlayingFinishedEventArgs e)
        {
            UISync.Execute(() =>
                           {
                               btnTest.Enabled = true;
                           });
        }

        private void Afr_ErrorHandler(string message)
        {
            UISync.Execute(() => {
                                     MessageBox.Show(this, "Connection Failed");
            });
            afr.Close();
        }

        private void Afr_AudioAvailable(object sender, Sources.Audio.DataAvailableEventArgs e)
        {
            UISync.Execute(() => {
                                     MessageBox.Show(this, "Connected!"); });
            afr.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Audio Files|*.*";
            ofd.InitialDirectory = Program.AppPath;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                cmbVLCURL.Text = ofd.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Audio Files|*.*";
            ofd.InitialDirectory = Program.AppPath;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                cmbFFMPEGURL.Text = ofd.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var vsa = new MicrophoneSourceAdvanced { Micobject = Mic };
            vsa.ShowDialog(this);
            vsa.Dispose();
        }
    }
}