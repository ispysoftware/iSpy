using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iSpyApplication.Utilities;
using DateTime = System.DateTime;

namespace iSpyApplication
{
    public partial class Merger : Form
    {
        public MainForm MainClass;
        public Merger()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            label1.Text = LocRm.GetString("Object");
            label2.Text = LocRm.GetString("From");
            label3.Text = LocRm.GetString("To");
            label4.Text = LocRm.GetString("OnlyMergeFilesWithTheSameResolution");
            button1.Text = LocRm.GetString("OK");
            Text = LocRm.GetString("MergeRecordings");
        }

        private void Merger_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now;

            foreach (var c in MainForm.Cameras)
            {
                var l = new Li { Name = c.name, Ot = 2, ID = c.id };
                ddlObject.Items.Add(l);
            }
            foreach (var c in MainForm.Microphones)
            {
                var l = new Li { Name = c.name, Ot = 1, ID = c.id };
                ddlObject.Items.Add(l);
            }
            ddlObject.Items.Insert(0,LocRm.GetString("PleaseSelect"));
            ddlObject.SelectedIndex = 0;
        }

        private struct Li
        {
            public string Name;
            public int Ot;
            public int ID;
            public override string ToString()
            {
                return Name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ddlObject.SelectedIndex > 0)
            {
                MergeMedia();
            }
            else
            {
                MessageBox.Show(this, LocRm.GetString("Validate_SelectCamera"));
            }
        }

        private Process _ffmpegProcess;
        private List<FilesFile> _pbMerge;
        private string _outfile;
        private string _dir;
        private Li _currentObject;


        internal void MergeMedia()
        {
            if (_ffmpegProcess != null)
                return;

            if (!Helper.HasFeature(Enums.Features.Recording))
                return;
            _pbMerge = new List<FilesFile>();

            _currentObject = ((Li)ddlObject.SelectedItem);
            if (_currentObject.Ot == 1)
            {
                var vl = MainClass.GetVolumeLevel(_currentObject.ID);
                _pbMerge = vl.FileList.Where(
                    p =>p.CreatedDateTicks >= dateTimePicker1.Value.Ticks &&
                        p.CreatedDateTicks <= dateTimePicker2.Value.Ticks).OrderBy(p => p.CreatedDateTicks).ToList();
            }
            else
            {
                var cw = MainClass.GetCameraWindow(_currentObject.ID);
                _pbMerge = cw.FileList.Where(
                    p => p.CreatedDateTicks >= dateTimePicker1.Value.Ticks &&
                        p.CreatedDateTicks <= dateTimePicker2.Value.Ticks).OrderBy(p => p.CreatedDateTicks).ToList();
            }

            if (_pbMerge.Count > 0)
            {
                var first = _pbMerge.First();
                string ext = first.Filename.Substring(first.Filename.LastIndexOf(".", StringComparison.Ordinal) + 1);
                var date = DateTime.Now;

                _dir = Helper.GetFullPath(_currentObject.Ot, _currentObject.ID);
                _outfile = _currentObject.ID + "_" +
                           $"Merge_{date.Year}-{Helper.ZeroPad(date.Month)}-{Helper.ZeroPad(date.Day)}_{Helper.ZeroPad(date.Hour)}-{Helper.ZeroPad(date.Minute)}-{Helper.ZeroPad(date.Second)}" +"."+ext;

                string filelist = _pbMerge.Aggregate("",
                    (current, file) => current + ("file '" + _dir + file.Filename + "'" + Environment.NewLine));

                File.WriteAllText(Program.AppDataPath + "concat.txt", filelist);
                if (filelist != "")
                {
                    var startInfo = new ProcessStartInfo
                                    {
                                        FileName = "\""+Program.AppPath + "ffmpeg.exe\"",
                                        Arguments =
                                            "-f concat -safe 0  -i \"" + Program.AppDataPath + "concat.txt" +
                                            "\" -codec copy \"" + _dir + _outfile + "\"",
                                        RedirectStandardOutput = false,
                                        RedirectStandardError = false,
                                        UseShellExecute = false,
                                        CreateNoWindow = true
                                    };
                    Logger.LogMessage("Merge: " + startInfo.FileName + " " + startInfo.Arguments);
                    _ffmpegProcess = new Process {StartInfo = startInfo, EnableRaisingEvents = true};
                    _ffmpegProcess.Exited += FfmpegMergeProcessExited;
                    _ffmpegProcess.ErrorDataReceived += FfmpegMergeProcessErrorDataReceived;
                    try
                    {
                        _ffmpegProcess.Start();
                    }
                    catch (Exception e)
                    {
                        _ffmpegProcess = null;
                        Logger.LogException(e);
                    }
                }
                button1.Enabled = false;

            }
        }

        void FfmpegMergeProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.LogError("merge process error: "+e.Data);
            DoClose();
        }

        void FfmpegMergeProcessExited(object sender, EventArgs e)
        {
            if (_ffmpegProcess.ExitCode == 0)
            {
                if (_pbMerge.Count>0)
                {
                    var ma = 0d;
                    var fi = new FileInfo(_dir+_outfile);


                    var alertData = new StringBuilder();
                    int durationSeconds = 0;
                    foreach (var m in _pbMerge)
                    {
                        if (m.AlertData != "")
                        {
                            alertData.Append(m.AlertData);
                            alertData.Append(",");
                        }
                        durationSeconds += m.DurationSeconds;

                        if (m.MaxAlarm > ma)
                            ma = m.MaxAlarm;
                    }

                    var ff = new FilesFile
                             {
                                 CreatedDateTicks = DateTime.Now.Ticks,
                                 DurationSeconds = durationSeconds,
                                 IsTimelapse = false,
                                 TriggerLevel = _pbMerge.First().TriggerLevel,
                                 TriggerLevelMax = _pbMerge.First().TriggerLevelMax,
                                 Filename = _outfile,
                                 AlertData = Helper.GetMotionDataPoints(alertData),
                                 MaxAlarm = ma,
                                 SizeBytes = fi.Length,
                                 IsMergeFile = true,
                                 IsMergeFileSpecified = true
                             };

                    string name;
                    if (_currentObject.Ot == 1)
                    {
                        var vl = MainClass.GetVolumeLevel(_currentObject.ID);
                        vl.AddFile(ff);
                        name = vl.Micobject.name;
                    }
                    else
                    {
                        var cw = MainClass.GetCameraWindow(_currentObject.ID);
                        cw.AddFile(ff);
                        name = cw.Camobject.name;

                        var fpv = _pbMerge.First();
                        //get preview image
                        string imgname = fpv.Filename.Substring(0,
                            fpv.Filename.LastIndexOf(".", StringComparison.Ordinal));
                        var imgpath = _dir + "thumbs/" + imgname+"_large.jpg";
                        
                        if (File.Exists(imgpath))
                        {
                            Image bmpPreview = Image.FromFile(imgpath);
                           
                            string jpgname = _dir + "thumbs\\" + ff.Filename.Substring(0,
                                ff.Filename.LastIndexOf(".", StringComparison.Ordinal));

                            bmpPreview.Save(jpgname + "_large.jpg", MainForm.Encoder,
                                MainForm.EncoderParams);
                            
                            Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
                            Image myThumbnail = bmpPreview.GetThumbnailImage(96, 72, myCallback, IntPtr.Zero);

                            myThumbnail.Save(jpgname + ".jpg", MainForm.Encoder,
                                MainForm.EncoderParams);

                            myThumbnail.Dispose();
                            bmpPreview.Dispose();
                        }


                    }

                    var fp = new FilePreview(_outfile, durationSeconds, name, ff.CreatedDateTicks, _currentObject.Ot,_currentObject.ID, ma,false,true);
                    MainForm.MasterFileAdd(fp);
                    MainForm.NeedsMediaRefresh = Helper.Now;
                }
                _pbMerge.Clear();
            }
            else
            {
                Logger.LogError("FFMPEG process exited with code " + _ffmpegProcess.ExitCode);
                MessageBox.Show(this, LocRm.GetString("ErrorCheckLogFile"));
            }
            DoClose();
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        private void DoClose()
        {

            if (InvokeRequired)
            {
                Invoke(new Delegates.SimpleDelegate(DoClose));
                return;
            }  
            Close();
        }
    }
}
