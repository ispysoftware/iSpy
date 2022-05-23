using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Utilities;
using Timer = System.Timers.Timer;

namespace iSpyApplication.Controls
{
    /// <summary>
    /// Summary description for GridView.
    /// </summary>
    public sealed class GridView : PictureBox
    {
        #region Private

        public Font Drawfont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel);
        public Font Iconfont = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold, GraphicsUnit.Pixel);
        public Brush IconBrush = new SolidBrush(Color.White);
        public Brush IconBrushActive = new SolidBrush(Color.Red);
        public Brush OverlayBrush = new SolidBrush(Color.White);
        public Brush RecordBrush = new SolidBrush(Color.Red);
        public Brush Lgb = new SolidBrush(MainForm.VolumeLevelColor);
        
        private readonly Pen _pline = new Pen(Color.Gray, 2);
        private readonly Pen _vline = new Pen(Color.Green, 2);
        private readonly Pen _pAlert = new Pen(Color.Red, 2);

        private readonly iSpyApplication.GridView _owner;

        internal MainForm MainClass;
        private DateTime _lastRun = Helper.Now;
        private Timer _tmrUpdateList;

        private const int Itempadding = 5;
        private int _maxItems = 36;
        private List<GridViewConfig> _controls;
        private int _itemwidth;
        private int _itemheight;
        private Bitmap _brandImage;

        private const int ButtonCount = 11;
        private Rectangle ButtonPanel
        {
            get
            {
                int w = ButtonCount * 22 + 3;
                int h = 28;

                if (MainForm.Conf.BigButtons)
                {
                    w = ButtonCount * 31;
                    h = 34;

                }
                return new Rectangle(0,0,w,h);

            }
        }
        
        public configurationGrid Cg;
        private int _cols = 1, _rows = 1;
        private GridViewConfig _maximised;

        private readonly object _objlock = new object();

        private int _overControlIndex = -1;
        

        

        #endregion
        
        public GridView(MainForm main, ref configurationGrid cg, iSpyApplication.GridView owner)
        {
            Cg = cg;
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 5, 5);
            BorderStyle = BorderStyle.None;
            BackColor = MainForm.Conf.BackColor.ToColor();
            MainClass = main;
            _owner = owner;
            Program.AppIdle.ApplicationLoopDoWork += HandlerApplicationLoopDoWork;
            Init();
        }


        public void Init()
        {
            if (_tmrUpdateList != null)
            {
                _tmrUpdateList.Elapsed -= TmrUpdateLayoutElapsed;
                _tmrUpdateList.Stop();
                _tmrUpdateList.Dispose();
                _tmrUpdateList = null;
            }

            _maxItems = 36;
            
            Text = Cg.name;
            _controls = new List<GridViewConfig>();
            switch (Cg.ModeIndex)
            {
                case 0:
                    
                    _cols = Cg.Columns;
                    _rows = Cg.Rows;
            
                    _maxItems = _cols*_rows;
                    ClearControls();
                    AddItems();
                    break;
                case 1:
                case 2:
                    _cols = _rows = 1;

                    if (!string.IsNullOrEmpty(Cg.ModeConfig))
                    {
                        //add default camera
                        string[] cfg = Cg.ModeConfig.Split(',');
                        if (cfg.Length > 1)
                        {
                            if (cfg[1] != "")
                            {
                                var gvi = new GridViewItem("", Convert.ToInt32(cfg[1]), 2);
                                _controls[0] =
                                    new GridViewConfig(
                                        new List<GridViewItem> { gvi  }, 1000) {Hold = true};
                            }
                            if (cfg.Length > 2)
                            {
                                _maxItems = Convert.ToInt32(cfg[2]);
                            }
                        }

                    }
                    ClearControls();
                    _tmrUpdateList = new Timer(1000);
                    _tmrUpdateList.Elapsed += TmrUpdateLayoutElapsed;
                    _tmrUpdateList.Start();
                    break;
            }

        }

        private void ClearControls()
        {
            if (_controls != null)
            {
                _controls.Clear();

                for (int i = 0; i < _maxItems; i++)
                    _controls.Add(null);
            }

        }

        

        void HandlerApplicationLoopDoWork(object sender, EventArgs e)
        {
            if ((Helper.Now - _lastRun).TotalMilliseconds > (1000d/Cg.Framerate))
            {
                _lastRun = Helper.Now;
                Invalidate();
            }
        }

        void AddItems()
        {
            if (Cg.GridItem == null)
                Cg.GridItem = new configurationGridGridItem[] { };

            foreach (var o in Cg.GridItem)
            {
                if (o.Item != null)
                {
                    if (o.GridIndex < _controls.Count)
                    {
                        var li = o.Item.Select(c => new GridViewItem("", c.ObjectID, c.TypeID)).ToList();

                        if (li.Count == 0)
                            _controls[o.GridIndex] = null;
                        else
                        {
                            _controls[o.GridIndex] = new GridViewConfig(li, o.CycleDelay);
                        }
                    }
                }
            }
        }

        void TmrUpdateLayoutElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock(_objlock)
            {
                int del = 10;
                bool sr = false;
                if (!string.IsNullOrEmpty(Cg.ModeConfig))
                {
                    var cfg = Cg.ModeConfig.Split(',');
                    if (cfg.Length > 0)
                    {
                        del = Convert.ToInt32(cfg[0]);
                        if (cfg.Length >= 4)
                        {
                            bool.TryParse(cfg[3], out sr);
                        }
                    }
                }
                int i;
                for(int k=0;k<_controls.Count;k++)
                {
                    var c = _controls[k];
                    if (c == null || c.Hold) continue;
                    _controls[k] = null;
                }

                foreach(var cam in MainForm.Cameras)
                {
                    var ctrl = MainClass.GetCameraWindow(cam.id);

                    bool add = (Cg.ModeIndex == 1 && ctrl.LastMovementDetected > Helper.Now.AddSeconds(0 - del)) || (Cg.ModeIndex == 2 && ctrl.LastAlerted > Helper.Now.AddSeconds(0 - del));

                    if (add)
                    {
                        foreach (var c in _controls)
                        {
                            if (c != null && c.ObjectIDs.Any(o => o.ObjectID == cam.id && o.TypeID == 2))
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
                            i = 0;
                            while (_controls[i] != null && i < _maxItems)
                                i++;
                            if (i == _maxItems)
                                break;
                            _controls[i] = new GridViewConfig(new List<GridViewItem> { new GridViewItem("", cam.id, 2) }, 1000);
                            
                        }                           
                    }
                }

                foreach (var mic in MainForm.Microphones)
                {
                    var ctrl = MainClass.GetVolumeLevel(mic.id);
                    //only want to display mics without associated camera controls
                    if (ctrl.CameraControl == null)
                    {

                        bool add = (Cg.ModeIndex == 1 && ctrl.LastSoundDetected > Helper.Now.AddSeconds(0 - del)) ||
                                    (Cg.ModeIndex == 2 && ctrl.LastAlerted > Helper.Now.AddSeconds(0 - del));
                        if (add)
                        {
                            foreach (var c in _controls)
                            {
                                if (c!=null && c.ObjectIDs.Any(o => o.ObjectID == mic.id && o.TypeID == 1))
                                {
                                    add = false;
                                }
                            }
                            if (add)
                            {
                                i = 0;
                                while (_controls[i] != null && i < _maxItems)
                                    i++;
                                if (i == _maxItems)
                                    break;
                                _controls[i] = new GridViewConfig(new List<GridViewItem> { new GridViewItem("", mic.id, 1) },1000);
                            }
                        }
                    }
                }

                i = _controls.Count(p => p != null);
                if (i == 0)
                {
                    _cols = 1;
                    _rows = 1;
                }
                else
                {
                    _cols = (int) Math.Sqrt(i);
                    _rows = (int) Math.Ceiling(i/(float) _cols);
                    if (sr)
                    {
                        if (_lastRestored < DateTime.UtcNow.AddSeconds(-5))
                        {
                            _lastRestored = DateTime.UtcNow;
                            _owner.ShowForm();
                        }
                    }
                }
            }

        }
        
        private DateTime _lastRestored = DateTime.MinValue;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Program.AppIdle.ApplicationLoopDoWork -= HandlerApplicationLoopDoWork;
            }

            _pAlert.Dispose();
            _pline.Dispose();
            _vline.Dispose();

            Drawfont.Dispose();
            Iconfont.Dispose();
            IconBrush.Dispose();
            IconBrushActive.Dispose();
            OverlayBrush.Dispose();
            RecordBrush.Dispose();
            if (_tmrUpdateList != null)
            {
                _tmrUpdateList.Stop();
                _tmrUpdateList.Dispose();
                _tmrUpdateList = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (Cg.ModeIndex != 0)
            {
                lock (_objlock)
                {
                    DoPaint(pe);
                }
            }
            else
            {
                DoPaint(pe);
            }

            base.OnPaint(pe);
        }

        private Rectangle ScaleRect(Image img, Rectangle r)
        {
            var ar = Convert.ToDouble(img.Width) / img.Height;
            var w = Math.Min(img.Width, r.Width);
            var h = Convert.ToInt32(w / ar);
            if (h > r.Height)
            {
                h = r.Height;
                w = Convert.ToInt32(h * ar);
            }
            return new Rectangle(r.Left + r.Width/2 - w/2,r.Top + r.Height/2 - h/2, w, h);
        }

        private void DoPaint(PaintEventArgs pe)
        {
            Graphics gGrid = pe.Graphics;
            gGrid.CompositingMode = CompositingMode.SourceOver;
            gGrid.CompositingQuality = CompositingQuality.HighSpeed;
            gGrid.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            gGrid.SmoothingMode = SmoothingMode.None;
            gGrid.InterpolationMode = InterpolationMode.Default;
            try
            {
                int cols = _cols;
                int rows = _rows;

                if (_maximised != null)
                {
                    cols = 1;
                    rows = 1;

                }
                Rectangle rc = ClientRectangle;
                _itemwidth = (rc.Width - cols * Itempadding) / cols;
                _itemheight = (rc.Height - rows * Itempadding) / rows;

                //draw lines
                for (var i = 0; i < cols; i++)
                {
                    var x = (i * (_itemwidth + Itempadding) - Itempadding / 2);
                    gGrid.DrawLine(_pline, x, 0, x, rc.Height);
                }
                for (var i = 0; i < rows; i++)
                {
                    var y = (i * (_itemheight + Itempadding) - Itempadding / 2);

                    gGrid.DrawLine(_pline, 0, y, rc.Width, y);
                }

                var ind = 0;
                var j = 0;
                var k = 0;
                for (var i = 0; i < cols * rows; i++)
                {
                    var x = j * (_itemwidth + Itempadding);
                    var y = k * (_itemheight + Itempadding);
                    var r = new Rectangle(x, y, _itemwidth, _itemheight);
                    var gvc = _controls[ind];
                    if (_maximised!=null)
                        gvc = _maximised;
                    ISpyControl alerted = null;

                    var rect = new Rectangle(r.X, r.Y + r.Height - 20, r.Width, 20);
                    if (gvc == null || gvc.ObjectIDs.Count == 0)
                    {
                        gGrid.FillRectangle(MainForm.OverlayBackgroundBrush, rect);
                        switch (Cg.ModeIndex)
                        {
                            case 0:
                            {
                                    if (!string.IsNullOrEmpty(MainForm.Conf.BrandPath))
                                    {
                                        if (_brandImage == null)
                                        {
                                            try
                                            {
                                                _brandImage = (Bitmap)Image.FromFile(MainForm.Conf.BrandPath);
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.LogException(ex,"Brand Image invalid - resetting");
                                                MainForm.Conf.BrandPath = "";
                                            }
                                        }
                                        if (_brandImage != null)
                                            gGrid.DrawImage(_brandImage, ScaleRect(_brandImage, new Rectangle(r.Location, new Size(r.Width, r.Height-20))));

                                    }
                                    else
                                    {
                                        //    gGrid.DrawImage()
                                        string m = LocRm.GetString("AddObjects");
                                        int txtOffline = Convert.ToInt32(gGrid.MeasureString(m,
                                                                                             Iconfont).Width);

                                        gGrid.DrawString(m, Iconfont, OverlayBrush,
                                                         x + _itemwidth / 2 - (txtOffline / 2),
                                                         y + _itemheight / 2);
                                    }
                            }
                                break;
                            case 1:
                                {
                                    string m = LocRm.GetString("NoMotionOrSound");
                                    int txtOffline = Convert.ToInt32(gGrid.MeasureString(m,
                                                                                         Iconfont).Width);

                                    gGrid.DrawString(m, Iconfont, OverlayBrush,
                                                     x + _itemwidth / 2 - (txtOffline / 2),
                                                     y + _itemheight / 2);
                                }
                                break;
                            case 2:
                            {
                                string m = LocRm.GetString("NoAlerts");
                                int txtOffline = Convert.ToInt32(gGrid.MeasureString(m,
                                                                                     Iconfont).Width);

                                gGrid.DrawString(m, Iconfont, OverlayBrush,
                                                 x + _itemwidth / 2 - (txtOffline / 2),
                                                 y + _itemheight / 2);
                            }
                                break;
                        }
                    }
                    else
                    {
                        if ((Helper.Now - gvc.LastCycle).TotalSeconds > gvc.Delay)
                        {
                            if (!gvc.Hold)
                            {
                                gvc.CurrentIndex++;
                                gvc.LastCycle = Helper.Now;
                            }
                        }
                        if (gvc.CurrentIndex >= gvc.ObjectIDs.Count)
                        {
                            gvc.CurrentIndex = 0;
                        }
                        var obj = gvc.ObjectIDs[gvc.CurrentIndex];
                        
                        var rFeed = r;
                        
                        switch (obj.TypeID)
                        {
                            case 1:
                                var vl = MainClass.GetVolumeLevel(obj.ObjectID);
                                if (vl != null)
                                {
                                    
                                    string txt = vl.Micobject.name;
                                    if (vl.IsEnabled && vl.Levels != null && !vl.AudioSourceErrorState)
                                    {
                                        int bh = (rFeed.Height) / vl.Micobject.settings.channels - (vl.Micobject.settings.channels - 1) * 2;
                                        if (bh <= 2)
                                            bh = 2;
                                        for (int m = 0; m < vl.Micobject.settings.channels; m++)
                                        {
                                            float f = 0f;
                                            if (m < vl.Levels.Length)
                                                f = vl.Levels[m];
                                            if (f > 1) f = 1;
                                            int drawW = Convert.ToInt32(Convert.ToDouble(rFeed.Width * f) - 1.0);
                                            if (drawW < 1)
                                                drawW = 1;

                                            gGrid.FillRectangle(Lgb, rFeed.X + 2, rFeed.Y + 2 + m * bh + (m * 2), drawW - 4, bh);

                                        }
                                        var d =  ((Convert.ToDouble(rFeed.Width - 4)/100.00));
                                        var mx1 = rFeed.X + (float) (d*Convert.ToDouble(vl.Micobject.detector.minsensitivity));
                                        var mx2 = rFeed.X + (float)(d * Convert.ToDouble(vl.Micobject.detector.maxsensitivity));
                                        gGrid.DrawLine(_vline, mx1, rFeed.Y+1, mx1, rFeed.Y + rFeed.Height-2);
                                        gGrid.DrawLine(_vline, mx2, rFeed.Y + 1, mx2, rFeed.Y + rFeed.Height - 2);

                                        if (vl.Recording)
                                        {
                                            gGrid.FillEllipse(RecordBrush, new Rectangle(rFeed.X + rFeed.Width - 12, rFeed.Y + 4, 8, 8));
                                        }
                                        if (Cg.Overlays)
                                        {
                                            gGrid.FillRectangle(MainForm.OverlayBackgroundBrush, rect);
                                            gGrid.DrawString(txt, Drawfont, OverlayBrush,
                                                new PointF(rect.X + 5, rect.Y + 2));
                                        }
                                        alerted = vl;
                                    }
                                    else
                                    {
                                        txt += ": " + (vl.Micobject.schedule.active ? LocRm.GetString("Scheduled") : LocRm.GetString("Offline"));
                                        gGrid.FillRectangle(MainForm.OverlayBackgroundBrush, rect);
                                        gGrid.DrawString(txt, Drawfont, OverlayBrush, new PointF(rect.X + 5, rect.Y + 2));
                                        gGrid.DrawString(vl.Micobject.name, Drawfont, OverlayBrush, new PointF(r.X + 5, r.Y + 2));
                                        gGrid.DrawString(vl.SourceType, Drawfont, OverlayBrush, new PointF(r.X + 5, r.Y + 20));
                                    }
                                    
                                    if (vl.IsEnabled && (vl.AudioSourceErrorState || vl.Levels == null))
                                    {
                                        var img = Properties.Resources.connecting;
                                        gGrid.DrawImage(img, x + _itemwidth - img.Width - 2, y + 2, img.Width, img.Height);
                                    }

                                }
                                else
                                {
                                    gvc.ObjectIDs.Remove(gvc.ObjectIDs[gvc.CurrentIndex]);
                                }
                                gGrid.DrawRectangle(new Pen(vl.BorderColor), rFeed);
                                break;
                            case 2:
                                var cw = MainClass.GetCameraWindow(obj.ObjectID);
                                if (cw != null)
                                {
                                    string txt = cw.Camobject.name;
                                    if (cw.IsEnabled)
                                    {
                                        if (cw.Camera != null && cw.GotImage)
                                        {
                                            var bmp = cw.LastFrame;
                                            if (bmp != null)
                                            {
                                                if (!Cg.Fill)
                                                {
                                                    rFeed = GetArea(x, y, _itemwidth, _itemheight, bmp.Width, bmp.Height);
                                                }
                                                gGrid.CompositingMode = CompositingMode.SourceCopy;
                                                gGrid.DrawImage(bmp, rFeed);
                                                gGrid.CompositingMode = CompositingMode.SourceOver;
                                            }


                                            alerted = cw;
                                            if (cw.Recording)
                                                gGrid.FillEllipse(RecordBrush,
                                                    new Rectangle(rFeed.X + rFeed.Width - 12, rFeed.Y + 4, 8, 8));
                                        }

                                        if (Cg.Overlays)
                                        {
                                            gGrid.FillRectangle(MainForm.OverlayBackgroundBrush, rect);
                                            gGrid.DrawString(txt, Drawfont, OverlayBrush,
                                                new PointF(rect.X + 5, rect.Y + 2));
                                        }

                                    }
                                    else
                                    {
                                        txt += ": " + (cw.Camobject.schedule.active ? LocRm.GetString("Scheduled") : LocRm.GetString("Offline"));
                                        gGrid.FillRectangle(MainForm.OverlayBackgroundBrush, rect);
                                        gGrid.DrawString(txt, Drawfont, OverlayBrush, new PointF(rect.X + 5, rect.Y + 2));
                                        gGrid.DrawString(cw.Camobject.name, Drawfont, OverlayBrush, new PointF(r.X + 5, r.Y + 2));
                                        gGrid.DrawString(cw.SourceType, Drawfont, OverlayBrush, new PointF(r.X + 5, r.Y + 20));
                                        
                                    }

                                    if (cw.IsEnabled && (cw.VideoSourceErrorState || !cw.GotImage))
                                    {
                                        var img = Properties.Resources.connecting;
                                        gGrid.DrawImage(img, x+ _itemwidth - img.Width - 2, y + 2, img.Width, img.Height);
                                    }
                                    gGrid.DrawRectangle(new Pen(cw.BorderColor), rFeed);
                                }
                                else
                                {
                                    gvc.ObjectIDs.Remove(gvc.ObjectIDs[gvc.CurrentIndex]);
                                }
                                break;
                            case 3:
                                var fp = MainClass.GetFloorPlan(obj.ObjectID);
                                if (fp != null)
                                {
                                    if (fp.Fpobject != null && fp.ImgPlan != null)
                                    {
                                        var bmp = fp.ImgView;
                                        if (!Cg.Fill)
                                        {
                                            rFeed = GetArea(x, y, _itemwidth, _itemheight, bmp.Width, bmp.Height);
                                        }
                                        if (bmp != null)
                                        {
                                            gGrid.DrawImage(bmp, rFeed);
                                        }
                                        if (Cg.Overlays)
                                        {
                                            gGrid.FillRectangle(MainForm.OverlayBackgroundBrush, r.X,
                                                r.Y + r.Height - 20, r.Width, 20);
                                            gGrid.DrawString(fp.Fpobject.name, Drawfont, OverlayBrush,
                                                r.X + 5,
                                                r.Y + r.Height - 16);
                                        }
                                    }

                                }
                                else
                                {
                                    gvc.ObjectIDs.Remove(gvc.ObjectIDs[gvc.CurrentIndex]);
                                }
                                break;
                        }
                    }
                    
                    

                    

                    if (_overControlIndex == ind && MainForm.Conf.ShowOverlayControls)
                    {
                        var rBp = ButtonPanel;
                        rBp.X = r.X;
                        rBp.Y = r.Y + r.Height - 20 - rBp.Height;
                        if (rBp.Width > _itemwidth)
                            rBp.Width = _itemwidth;
                        

                        if (gvc != null && gvc.ObjectIDs.Count != 0)
                        {
                            gGrid.FillRectangle(MainForm.OverlayBackgroundBrush, rBp);
                            var c = gvc.ObjectIDs[gvc.CurrentIndex];
                            var ctrl = MainClass.GetISpyControl(c.TypeID, c.ObjectID);
                            for (int b = 0; b < ButtonCount; b++)
                                DrawButton(gGrid, b, ctrl, Cg.ModeIndex,gvc,rBp.X,rBp.Y);
                        }
                    }


                    ind++;
                    j++;
                    if (j == cols)
                    {
                        j = 0;
                        k++;
                    }

                    if (alerted!=null)
                    {
                        var ra = new Rectangle(r.X,r.Y,r.Width,r.Height);
                        ra.X += 1;
                        ra.Y += 1;
                        using (var p = new Pen(alerted.BorderColor))
                        {
                            gGrid.DrawRectangle(p, ra);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void DrawButton(Graphics gCam, int buttonIndex, ISpyControl ctrl, int modeIndex, GridViewConfig gvc, int x, int y)
        {
            Rectangle rDest;
            Rectangle rSrc = GetButtonByIndex(buttonIndex, ctrl, modeIndex, gvc, out rDest);

            if (rDest.X + rDest.Width < _itemwidth)
            {
                rDest.X = x + rDest.X;
                rDest.Y = y + 4;

                gCam.DrawImage(MainForm.Conf.BigButtons ? Properties.Resources.icons_big : Properties.Resources.icons,
                    rDest, rSrc, GraphicsUnit.Pixel);
            }
        }

        private Rectangle GetButtonByIndex(int buttonIndex, ISpyControl ctrl, int modeIndex, GridViewConfig gvc, out Rectangle destRect)
        {
            Rectangle rSrc = Rectangle.Empty;
            bool b = ctrl!=null && ctrl.IsEnabled;
            switch (buttonIndex)
            {
                case 0:
                    rSrc = (modeIndex == 0) ? MainForm.RAdd : MainForm.RAddOff;
                    break;
                case 1:
                    rSrc = gvc.Hold ? MainForm.RHoldOn : MainForm.RHold;
                    break;
                case 2:
                    rSrc = (gvc.ObjectIDs.Count > 1) ? MainForm.RNext : MainForm.RNextOff;
                    break;
                case 5://settings
                    rSrc = MainForm.REdit;
                    break;
                case 6://web
                    rSrc = Helper.HasFeature(Enums.Features.Access_Media) ? MainForm.RWeb : MainForm.RWebOff;
                    break;
            }
            if (ctrl != null)
            {
                switch (buttonIndex)
                {
                    case 3: //power
                        rSrc = ctrl.CanEnable ? (b ? MainForm.RPowerOn : MainForm.RPower) : MainForm.RPowerOff;
                        break;
                    case 4: //record
                        rSrc = (ctrl.CanRecord && b)
                            ? (ctrl.Recording ? MainForm.RRecordOn : MainForm.RRecord)
                            : MainForm.RRecordOff;
                        break;
                    case 7: //grab
                        rSrc = (ctrl.CanGrab && b) ? MainForm.RGrab : MainForm.RGrabOff;
                        break;
                    case 8: //talk
                        rSrc = (ctrl.CanTalk && b) ? (ctrl.Talking ? MainForm.RTalkOn : MainForm.RTalk) : MainForm.RTalkOff;
                        break;
                    case 9: //listen
                        rSrc = (ctrl.CanListen && b)
                            ? (ctrl.Listening ? MainForm.RListenOn : MainForm.RListen)
                            : MainForm.RListenOff;
                        break;
                    case 10: //files
                        rSrc = ctrl.HasFiles ? MainForm.RFolder : MainForm.RFolderOff;
                        break;

                }
            }
            if (MainForm.Conf.BigButtons)
            {
                rSrc.X -= 2;
                rSrc.Width += 8;
                rSrc.Height += 8;
            }

            var bp = ButtonPanel;
            destRect = new Rectangle(bp.X + buttonIndex * (bp.Width/ButtonCount) + 5, Height - 25 - rSrc.Height - 6, rSrc.Width, rSrc.Height);
            return rSrc;
        }

        private int GetButtonIndexByLocation(Point xy)
        {
            var rBp = ButtonPanel;
            if (xy.X >= rBp.X && xy.Y > rBp.Y && xy.X <= rBp.X + rBp.Width && xy.Y <= rBp.Y + rBp.Height)
            {
                double x = xy.X - rBp.X;
                return Convert.ToInt32(Math.Ceiling((x / rBp.Width) * ButtonCount)) - 1;
            }
            return -999;//nothing
        }

        private Rectangle GetArea(int x, int y, int contW, int contH, int imageW, int imageH)
        {
            if (Height > 0 && Width > 0)
            {
                double arw = Convert.ToDouble(contW) / Convert.ToDouble(imageW);
                double arh = Convert.ToDouble(contH) / Convert.ToDouble(imageH);
                int w;
                int h;
                if (arh <= arw)
                {
                    w = Convert.ToInt32(((Convert.ToDouble(contW) * arh) / arw));
                    h = contH;
                }
                else
                {
                    w = contW;
                    h = Convert.ToInt32((Convert.ToDouble(contH) * arw) / arh);
                }
                int x2 = x+((contW - w) / 2);
                int y2 = y+((contH - h) / 2);
                return new Rectangle(x2, y2, w, h);
            }
            return Rectangle.Empty;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var row = -1;
            var col = -1;
            var x = e.Location.X;
            var y = e.Location.Y;

            for (var i = 1; i <= _cols; i++)
            {
                if (i * (_itemwidth + Itempadding) - Itempadding / 2 > x)
                {
                    col = i - 1;
                    break;
                }
            }
            for (var i = 1; i <= _rows; i++)
            {
                if (i * (_itemheight + Itempadding) - Itempadding / 2 > y)
                {
                    row = i - 1;
                    break;
                }
            }

            if (row != -1 && col != -1)
            {
                var io = row*_cols + col;
                _overControlIndex = io;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _overControlIndex = -1;
        }

        public void QuickSelect(Point p)
        {
            GridViewConfig cgv;
            int bpi, io;
            GetInfoByLocation(p, out cgv, out bpi, out io);

            if (Cg.ModeIndex == 0)
            {
                List(cgv, io);                
            }
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Select();

            if (e.Button==MouseButtons.Left)
            {
                GridViewConfig cgv;
                int bpi,io;
                GetInfoByLocation(e.Location, out cgv, out bpi, out io);

                if (Cg.ModeIndex == 0)
                {
                    if (cgv == null || cgv.ObjectIDs.Count == 0)
                    {
                        List(cgv, io);
                        return;
                    }
                }

                if (cgv != null)
                {
                    var gv = cgv.ObjectIDs[cgv.CurrentIndex];
                    var ctrl = MainClass.GetISpyControl(gv.TypeID, gv.ObjectID);
                    ((Control)ctrl).Focus();
                    switch (bpi)
                    {
                        case 0:
                            if (Cg.ModeIndex == 0)
                                List(cgv, io);
                            break;
                        case 1:
                            cgv.Hold = !cgv.Hold;
                            break;
                        case 2:
                            if (Cg.ModeIndex == 0)
                            {
                                int i = cgv.CurrentIndex + 1;
                                if (i > cgv.ObjectIDs.Count)
                                    i = 0;
                                cgv.CurrentIndex = i;
                                cgv.LastCycle = Helper.Now;
                            }
                            break;
                        case 3:
                            if (ctrl.IsEnabled)
                            {
                                ctrl.Disable();
                            }
                            else
                            {
                                ctrl.Enable();
                            }
                            break;
                        case 4:
                            if (ctrl.IsEnabled && ctrl.CanRecord)
                            {
                                ctrl.RecordSwitch(!ctrl.Recording);
                            }
                            break;
                        case 5:
                            MainClass.EditObject(ctrl, _owner);
                            break;
                        case 6:
                            if (Helper.HasFeature(Enums.Features.Access_Media))
                            {
                                string url = MainForm.Webpage;
                                if (WsWrapper.WebsiteLive && MainForm.Conf.ServicesEnabled)
                                {
                                    MainForm.OpenUrl(url);
                                }
                                else
                                    MainClass.Connect(url, false);
                            }
                            break;
                        case 7:
                            if (ctrl.CanGrab && ctrl.IsEnabled)
                            {
                                string fn = ctrl.SaveFrame();
                                if (fn != "" && MainForm.Conf.OpenGrabs)
                                    MainForm.OpenUrl(fn);
                            }
                            break;
                        case 8:
                            if (ctrl.CanTalk && ctrl.IsEnabled)
                                ctrl.Talk();
                            break;
                        case 9:
                            if (ctrl.CanListen && ctrl.IsEnabled)
                                ctrl.Listen();
                            break;
                        case 10://files
                            if (ctrl.HasFiles)
                                MainClass.ShowFiles(ctrl);
                            break;
                    }
                }
            }

        }

        private void GetInfoByLocation(Point p, out GridViewConfig cgv, out int bpi, out int io)
        {
            var row = -1;
            var col = -1;
            var x = p.X;
            var y = p.Y;
            io = 0;

            cgv = null;
            if (_maximised != null)
            {
                cgv = _maximised;
                row = 0;
                col = 0;
                int j = 0;
                foreach (var obj in _controls)
                {
                    if (obj != null && obj.Equals(cgv))
                    {
                        io = j;
                        break;
                    }
                    j++;
                }
            }
            else
            {

                for (var i = 1; i <= _cols; i++)
                {
                    if (i * (_itemwidth + Itempadding) - Itempadding / 2 > x)
                    {
                        col = i - 1;
                        break;
                    }
                }
                for (var i = 1; i <= _rows; i++)
                {
                    if (i * (_itemheight + Itempadding) - Itempadding / 2 > y)
                    {
                        row = i - 1;
                        break;
                    }
                }

                if (row != -1 && col != -1)
                {
                    cgv = _controls[row * _cols + col];
                    io = row * _cols + col;
                }

            }

            int rx = col * (_itemwidth + Itempadding);
            int ry = row * (_itemheight + Itempadding);

            int ox = x - rx;
            int oy = (ry + _itemheight) - y - 20;

            bpi = GetButtonIndexByLocation(new Point(ox, oy));
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (_maximised!=null)
            {
                _maximised = null;
                return;
            }

            var row = -1;
            var col = -1;
            var x = e.Location.X;
            var y = e.Location.Y;

            for (var i = 1; i <= _cols; i++)
            {
                if (i * (_itemwidth + Itempadding) - Itempadding / 2 > x)
                {
                    col = i - 1;
                    break;
                }
            }
            for (var i = 1; i <= _rows; i++)
            {
                if (i * (_itemheight + Itempadding) - Itempadding / 2 > y)
                {
                    row = i - 1;
                    break;
                }
            }

            if (row != -1 && col != -1)
            {
                var io = row * _cols + col;
                int ry = row * (_itemheight + Itempadding);
                
                if ((ry + _itemheight) - y > 38)
                {
                    //only maximise if clicking above buttons
                    if (_controls[io].CurrentIndex > -1)
                        _maximised = _controls[io];
                }

                
            }
        }

        protected override void  OnMouseWheel(MouseEventArgs e)
        {
            var row = -1;
            var col = -1;
            var x = e.Location.X;
            var y = e.Location.Y;

            GridViewConfig cgv = null;
            if (_maximised != null)
            {
                cgv = _maximised;
                row = 0;
                col = 0;
            }
            else
            {

                for (var i = 1; i <= _cols; i++)
                {
                    if (i * (_itemwidth + Itempadding) - Itempadding / 2 > x)
                    {
                        col = i - 1;
                        break;
                    }
                }
                for (var i = 1; i <= _rows; i++)
                {
                    if (i * (_itemheight + Itempadding) - Itempadding / 2 > y)
                    {
                        row = i - 1;
                        break;
                    }
                }

                if (row != -1 && col != -1)
                {
                    cgv = _controls[row * _cols + col];
                }

            }
            if (cgv != null)
            {
                var gv = cgv.ObjectIDs[cgv.CurrentIndex];

                if (gv.TypeID != 2)
                {
                    return;
                }
                var cameraControl = MainClass.GetCameraWindow(gv.ObjectID);
                cameraControl.PTZNavigate = false;
                if (cameraControl.PTZ != null)
                {
                    cgv.Hold = true;

                    if (!cameraControl.PTZ.DigitalZoom)
                    {
                        cameraControl.Calibrating = true;
                        cameraControl.PTZ.SendPTZCommand(
                            e.Delta > 0 ? Enums.PtzCommand.ZoomIn : Enums.PtzCommand.ZoomOut);

                        cameraControl.PTZ.CheckSendStop();
                    }
                    else
                    {
                        Rectangle r = cameraControl.Camera.ViewRectangle;
                        //map location to point in the view rectangle

                        var pCell = new Point(col*(_itemwidth + Itempadding), row*(_itemheight + Itempadding));

                        var ox =
                            Convert.ToInt32((Convert.ToDouble(e.Location.X-pCell.X)/Convert.ToDouble(_itemwidth))*
                                            Convert.ToDouble(r.Width));
                        var oy =
                            Convert.ToInt32((Convert.ToDouble(e.Location.Y - pCell.Y) / Convert.ToDouble(_itemheight)) *
                                            Convert.ToDouble(r.Height));

                        cameraControl.Camera.ZPoint = new Point(r.Left + ox, r.Top + oy);
                        var f = cameraControl.Camera.ZFactor;
                        if (e.Delta > 0)
                        {
                            f += 0.2f;
                        }
                        else
                            f -= 0.2f;
                        if (f < 1)
                            f = 1;
                        cameraControl.Camera.ZFactor = f;
                    }
                    ((HandledMouseEventArgs) e).Handled = true;

                }
            }

        }

        private void List(GridViewConfig cgv, int io)
        {
            using (var gvc = new GridViewCamera())
            {
                if (cgv != null)
                {
                    gvc.Delay = cgv.Delay;
                    gvc.SelectedIDs = cgv.ObjectIDs;
                }
                else
                {
                    gvc.SelectedIDs = new List<GridViewItem>();
                }
                if (gvc.ShowDialog(this) == DialogResult.OK)
                {
                    cgv = gvc.SelectedIDs.Count > 0 ? new GridViewConfig(gvc.SelectedIDs, gvc.Delay) : null;

                    if (Cg != null)
                    {
                        var gi = Cg.GridItem.FirstOrDefault(p => p.GridIndex == io);
                        if (gi == null)
                        {
                            gi = new configurationGridGridItem {CycleDelay = gvc.Delay, GridIndex = io};
                            var lgi = Cg.GridItem.ToList();
                            lgi.Add(gi);
                            Cg.GridItem = lgi.ToArray();
                        }


                        gi.CycleDelay = gvc.Delay;

                        gi.Item =
                            gvc.SelectedIDs.Select(
                                i => new configurationGridGridItemItem {ObjectID = i.ObjectID, TypeID = i.TypeID})
                                .ToArray();
                    }
                    _controls[io] = cgv;
                    Invalidate();
                }
            }
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GridView
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MinimumSize = new System.Drawing.Size(160, 120);
            this.Size = new System.Drawing.Size(160, 120);
            this.ResumeLayout(false);
        }

        #endregion


        private class GridViewConfig
        {
            public readonly int Delay;
            public readonly List<GridViewItem> ObjectIDs;
            public DateTime LastCycle;
            public int CurrentIndex;
            public bool Hold;


            public GridViewConfig(List<GridViewItem> objectIDs, int delay)
            {
                ObjectIDs = objectIDs;
                Delay = delay;
                LastCycle = Helper.Now;
                Hold = false;
            }
        }


   }


}