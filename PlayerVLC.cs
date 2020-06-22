using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LibVLCSharp.WinForms;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;
using LibVLCSharp.Shared;

namespace iSpyApplication
{
    public partial class PlayerVLC : Form
    {
        private readonly string _titleText;
        public int ObjectID = -1;
        public MainForm MF;

        private readonly FolderSelectDialog _fbdSaveTo = new FolderSelectDialog
        {
            Title = "Select a folder to copy the file to"
        };

        private void RenderResources()
        {
            openFolderToolStripMenuItem.Text = LocRm.GetString("OpenLocalFolder");
            saveAsToolStripMenuItem.Text = LocRm.GetString("SaveAs");
            chkRepeatAll.Text = LocRm.GetString("PlayAll");
            Text = LocRm.GetString("VideoPlayback");
            fileToolStripMenuItem.Text = LocRm.GetString("File");
            saveAsToolStripMenuItem.Text = LocRm.GetString("SaveAs");
            openFolderToolStripMenuItem.Text = LocRm.GetString("OpenLocation");

        }

         public PlayerVLC(string titleText, MainForm mf)
         {
            MF = mf;
            InitializeComponent();           
             RenderResources();
             _titleText = titleText;
             chkRepeatAll.Checked = MainForm.VLCRepeatAll;

            
            videoView1.MediaPlayer = new MediaPlayer(LibVLC);
            
            videoView1.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            videoView1.MediaPlayer.TimeChanged += MediaPlayer_TimeChanged; ;
            videoView1.MediaPlayer.EndReached += EventsMediaEnded;
            videoView1.MediaPlayer.Stopped+= EventsPlayerStopped;

            try
            {
                trackBar2.Value = videoView1.MediaPlayer.Volume;
                tbSpeed.Value = Convert.ToInt32(videoView1.MediaPlayer.Rate * 10);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex, "Media player init");
            }

        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            UISync.Execute(() => lblTime.Text = TimeSpan.FromMilliseconds(e.Time).ToString().Substring(0, 8));
        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            var newpos = (int)(e.Position * 100);
            if (newpos < 0)
                newpos = 0;
            if (newpos > 100)
                newpos = 100;
            UISync.Execute(() => vNav.Value = newpos);
        }

        private void Player_Load(object sender, EventArgs e)
        {
            UISync.Init(this);
            vNav.Seek += vNav_Seek;
            Text = _titleText;
        }

        void vNav_Seek(object sender, float percent)
        {
            if (!videoView1.MediaPlayer.IsPlaying)
            {
                if (videoView1.MediaPlayer.WillPlay)
                    videoView1.MediaPlayer.Play();
                else
                    Play(_filename, Text);
            }

            videoView1.MediaPlayer.Position = percent / 100;
        }

        private LibVLC _libVLC = null;
        private static bool _coreInitialized = false;
        private static object _coreLock = new object();
        private LibVLC LibVLC
        {
            get
            {
                if (_libVLC != null) return _libVLC;

                if (!_coreInitialized)
                {
                    lock (_coreLock)
                    {
                        if (!_coreInitialized)
                        {
                            try
                            {
                                Core.Initialize(VlcHelper.VLCLocation);
                                _coreInitialized = true;
                            }
                            catch (VLCException vlcex)
                            {
                                Logger.LogException(vlcex);
                                throw new ApplicationException("VLC not found (v3). Set location in settings.");
                            }
                        }
                    }


                }
                try
                {
                    _libVLC = new LibVLC();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "VLC Setup");
                    throw new ApplicationException("VLC not found (v3). Set location in settings.");
                }
                return _libVLC;
            }
        }

        private string _filename = "";

        private delegate void PlayDelegate(string filename, string titleText);
        public void Play(string filename, string titleText)
        {
            if (InvokeRequired)
                Invoke(new PlayDelegate(Play), filename, titleText);
            else
            {
                if (!File.Exists(filename))
                {
                    MessageBox.Show(this, LocRm.GetString("FileNotFound")+Environment.NewLine + filename);
                    return;
                }
                _filename = filename;
                Media m = new Media(LibVLC, filename, FromType.FromPath);
                if (!videoView1.MediaPlayer.Play(m))
                {
                    MessageBox.Show(this, LocRm.GetString("CouldNotOpen") + Environment.NewLine + filename);
                    return;
                }
                

                string[] parts = filename.Split('\\');
                string fn = parts[parts.Length - 1];
                FilesFile ff = null;
                if (fn.EndsWith(".mp3") || fn.EndsWith(".wav"))
                {
                    var vl = ((MainForm)Owner).GetVolumeLevel(ObjectID);
                    if (vl != null)
                    {
                        ff = vl.FileList.FirstOrDefault(p => p.Filename.EndsWith(fn));
                    }
                    vNav.IsAudio = true;
                    videoView1.BackgroundImage = Properties.Resources.ispy1audio;
                }
                else
                {
                    var cw = ((MainForm)Owner).GetCameraWindow(ObjectID);
                    if (cw!=null)   {
                        ff = cw.FileList.FirstOrDefault(p => p.Filename.EndsWith(fn));
                    }
                    vNav.IsAudio = false;
                    videoView1.BackgroundImage = Properties.Resources.ispy1;
                }
                
                if (ff!=null)
                    vNav.Init(ff);
                Text = titleText;
            }
        }
        void EventsPlayerStopped(object sender, EventArgs e)
        {
            UISync.Execute(InitControls);
        }

        void EventsMediaEnded(object sender, EventArgs e)
        {
            UISync.Execute(InitControls);
        }

        private void InitControls()
        {
            lblTime.Text = "00:00:00";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            videoView1.MediaPlayer.Volume = trackBar2.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (videoView1.MediaPlayer.IsPlaying)
                videoView1.MediaPlayer.Pause();
            else
                videoView1.MediaPlayer.Play();
        }

        private class UISync
        {
            private static ISynchronizeInvoke Sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                Sync = sync;
            }

            public static void Execute(Action action)
            {
                try {Sync.BeginInvoke(action, null);}
                catch{}
            }
        }

        private void Player_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
            videoView1.MediaPlayer.SetRate(((float) tbSpeed.Value)/10);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fi = new FileInfo(_filename);

                if (_fbdSaveTo.ShowDialog(Handle))
                {
                    File.Copy(_filename, _fbdSaveTo.FileName + @"\" + fi.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string argument = @"/select, " + _filename;
            Process.Start("explorer.exe", argument);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Go(-1);

        }

        private void Go(int n)
        {
            int j = 0;
            lock (MF.flowPreview.Controls)
            {
                var lb = (from Control c in MF.flowPreview.Controls select c as PreviewBox into pb where pb != null && pb.Selected select pb).ToList();
                if (lb.Count==0)
                    lb = (from Control c in MF.flowPreview.Controls select c as PreviewBox into pb where pb != null select pb).ToList();
                btnNext.Enabled = btnPrevious.Enabled = lb.Count > 1;
                for (int i = lb.Count-1; i >-1 ; i--)
                {
                    var pb = lb[i];
                    if (pb.FileName == _filename)
                    {
                        j = i - n;
                        if (j < 0)
                        {
                            j = lb.Count - 1;
                            break;
                        }
                        if (j >= lb.Count)
                            j = 0;
                        break;
                    }
                }
                if (j > -1 && j<lb.Count)
                {
                    UISync.Execute(() => lb[j].PlayMedia(Enums.PlaybackMode.iSpy));
                }
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            Go(1);
        }

        private void chkRepeatAll_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.VLCRepeatAll = chkRepeatAll.Checked;
        }
    }
}
