using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using iSpyApplication.Utilities;

namespace iSpyApplication.Controls
{
    public sealed partial class FloorPlanControl : PictureBox, ISpyControl
    {
        #region Public

        public MainForm MainClass;
        public bool NeedSizeUpdate;
        public bool ResizeParent;
        public objectsFloorplan Fpobject;
        public double LastAlertTimestamp;
        private Point _mouseLoc = Point.Empty;
        public double LastRefreshTimestamp;
        public int LastOid;
        public int LastOtid;
        public bool IsAlert;
        public Rectangle RestoreRect = Rectangle.Empty;
        private readonly ToolTip _toolTipFp;
        private int _ttind = -1;
        private readonly object _lockobject = new object();

        public bool ForcedRecording => false;

        private readonly SolidBrush _alertBrush = new SolidBrush(Color.FromArgb(200, 255, 0, 0));
        private readonly SolidBrush _noalertBrush = new SolidBrush(Color.FromArgb(200, 75, 172, 21));
        private readonly SolidBrush _offlineBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));

        private readonly SolidBrush _alertBrushScanner = new SolidBrush(Color.FromArgb(50, 255, 0, 0));
        private readonly SolidBrush _noalertBrushScanner = new SolidBrush(Color.FromArgb(50, 75, 172, 21));

        private readonly SolidBrush _drawBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private readonly SolidBrush _sbTs = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

        private readonly Font _drawFont = new Font(FontFamily.GenericSansSerif, 9);

        private const int ButtonCount = 2;

        public int ObjectTypeID => 3;

        public int ObjectID => Fpobject.id;

        private Rectangle ButtonPanel
        {
            get
            {
                int w = ButtonCount * 22 + 3;
                int h = 28;
                if (MainForm.Conf.BigButtons)
                {
                    w = ButtonCount * 31 + 3;
                    h = 34;

                }
                return new Rectangle(Width / 2 - w / 2, Height - 25 - h, w, h);

            }
        }

        public void Alert(object sender, EventArgs e)
        {

        }
        public void Detect(object sender, EventArgs e)
        {

        }
        public Bitmap ImgPlan
        {
            
            get
            {
                return _imgplan;

            }
            set
            {
                lock (_lockobject)
                {
                    _imgview?.Dispose();
                    _imgplan?.Dispose();
                    _imgplan = value;
                    if (_imgplan!=null)
                        _imgview = (Bitmap)_imgplan.Clone();
                }
            }
        }

        public void ReloadSchedule()
        {

        }

        public int Order
        {
            get { return Fpobject.order; }
            set { Fpobject.order = value; }
        }

        public Bitmap ImgView => _imgview;

        public bool NeedsRefresh = true, RefreshImage = true;


        #endregion
        private DateTime _mouseMove = DateTime.MinValue;
        private Bitmap _imgplan, _imgview;
        #region SizingControls

        public void UpdatePosition()
        {
            Monitor.Enter(this);

            if (Parent != null && ImgPlan != null)
            {
                int width = ImgPlan.Width;
                int height = ImgPlan.Height;

                SuspendLayout();
                Size = new Size(width + 2, height + 26);
                ResumeLayout();
                NeedSizeUpdate = false;
            }
            Monitor.Exit(this);
        }

        private MousePos GetMousePos(Point location)
        {
            var result = MousePos.NoWhere;
            int rightSize = Padding.Right;
            int bottomSize = Padding.Bottom;
            var testRect = new Rectangle(Width - rightSize, 0, Width - rightSize, Height - bottomSize);
            if (testRect.Contains(location)) result = MousePos.Right;
            testRect = new Rectangle(0, Height - bottomSize, Width - rightSize, Height);
            if (testRect.Contains(location)) result = MousePos.Bottom;
            testRect = new Rectangle(Width - rightSize, Height - bottomSize, Width, Height);
            if (testRect.Contains(location)) result = MousePos.BottomRight;
            return result;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Invalidate();
            }
            _toolTipFp.RemoveAll();
            _toolTipFp.Dispose();

            _alertBrush.Dispose();
            _noalertBrush.Dispose();
            _offlineBrush.Dispose();

            _alertBrushScanner.Dispose();
            _noalertBrushScanner.Dispose();
            _drawBrush.Dispose();
            _sbTs.Dispose();
            _drawFont.Dispose();
            
            base.Dispose(disposing);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {

            base.OnMouseUp(e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    ((LayoutPanel)Parent).ISpyControlUp(new Point(this.Left + e.X, this.Top + e.Y));
                    break;
            }

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Select();
            IntPtr hwnd = Handle;
            if ((ResizeParent) && (Parent != null) && (Parent.IsHandleCreated))
            {
                hwnd = Parent.Handle;
            }
            
            if (e.Button == MouseButtons.Left)
            {
                MousePos mousePos = GetMousePos(e.Location);
                if (mousePos== MousePos.NoWhere)
                {
                    if (MainForm.Conf.ShowOverlayControls)
                    {
                        int bpi = GetButtonIndexByLocation(e.Location);
                        switch (bpi)
                        {
                            case -999:
                                var layoutPanel = (LayoutPanel)Parent;
                                layoutPanel?.ISpyControlDown(new Point(this.Left + e.X, this.Top + e.Y));
                                break;
                            case 0:
                               MainClass.EditFloorplan(Fpobject);
                                break;
                            case 1:
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
                        }
                    }
                }
                if (MainForm.LockLayout) return;
                switch (mousePos)
                {
                    case MousePos.Right:
                        {
                            NativeCalls.ReleaseCapture(hwnd);
                            NativeCalls.SendMessage(hwnd, NativeCalls.WmSyscommand, NativeCalls.ScDragsizeE, IntPtr.Zero);
                        }
                        break;
                    case MousePos.Bottom:
                        {
                            NativeCalls.ReleaseCapture(hwnd);
                            NativeCalls.SendMessage(hwnd, NativeCalls.WmSyscommand, NativeCalls.ScDragsizeS, IntPtr.Zero);
                        }
                        break;
                    case MousePos.BottomRight:
                        {
                            NativeCalls.ReleaseCapture(hwnd);
                            NativeCalls.SendMessage(hwnd, NativeCalls.WmSyscommand, NativeCalls.ScDragsizeSe,
                                                    IntPtr.Zero);
                        }
                        break;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_mouseLoc.X == e.X && _mouseLoc.Y == e.Y)
                return;
            _mouseMove = Helper.Now;
            MousePos mousePos = GetMousePos(e.Location);
            switch (mousePos)
            {
                case MousePos.Right:
                    Cursor = Cursors.SizeWE;
                    break;
                case MousePos.Bottom:
                    Cursor = Cursors.SizeNS;
                    break;
                case MousePos.BottomRight:
                    Cursor = Cursors.SizeNWSE;
                    break;
                default:
                    Cursor = Cursors.Hand;

                    if (MainForm.Conf.ShowOverlayControls)
                    {
                        var rBp = ButtonPanel;
                        var toolTipLocation = new Point(e.Location.X, rBp.Y + rBp.Height + 1);
                        int bpi = GetButtonIndexByLocation(e.Location);
                        if (_ttind != bpi)
                        {
                            switch (bpi)
                            {
                                case 0:
                                    _toolTipFp.Show(LocRm.GetString("Edit"), this,toolTipLocation, 1000);
                                    _ttind = 0;
                                    break;
                                case 1:
                                    if (Helper.HasFeature(Enums.Features.Access_Media))
                                    {
                                        _toolTipFp.Show(LocRm.GetString("MediaoverTheWeb"), this, toolTipLocation, 1000);
                                        _ttind = 1;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            base.OnMouseMove(e);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                double ar = Convert.ToDouble(MinimumSize.Width)/Convert.ToDouble(MinimumSize.Height);
                if (ImgPlan != null)
                    ar = Convert.ToDouble(ImgPlan.Width)/Convert.ToDouble(ImgPlan.Height);
                Width = Convert.ToInt32(ar*Height);
            }

            base.OnResize(eventargs);
            if (Width < MinimumSize.Width) Width = MinimumSize.Width;
            if (Height < MinimumSize.Height) Height = MinimumSize.Height;
            _minimised = Size.Equals(MinimumSize);

        }

        private bool _minimised;

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor = Cursors.Default;
            _mouseMove = DateTime.MinValue;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Hand;
            Invalidate();
        }

        #region Nested type: MousePos

        private enum MousePos
        {
            NoWhere,
            Right,
            Bottom,
            BottomRight
        }

        #endregion

        
        #endregion

        public string Folder { get; set; }
        public FloorPlanControl(objectsFloorplan ofp, MainForm mainForm)
        {
            MainClass = mainForm;
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 5, 5);
            BorderStyle = BorderStyle.None;
            BackColor = MainForm.BackgroundColor;
            Fpobject = ofp;
            MouseClick += FloorPlanControlClick;

            _toolTipFp = new ToolTip { AutomaticDelay = 500, AutoPopDelay = 1500 };
        }

        private void FloorPlanControlClick(object sender, MouseEventArgs e)
        {
            var local = new Point(e.X, e.Y);
            double xRat = Convert.ToDouble(Width)/ImageWidth;
            double yRat = Convert.ToDouble(Height)/ImageHeight;
            double hittargetw = 22*xRat;
            double hittargeth = 22*yRat;

            double wrat = Convert.ToDouble(ImageWidth) / 533d;
            double hrat = Convert.ToDouble(ImageHeight) / 400d;


            bool changeHighlight = true;

            if (Highlighted)
            {
                foreach (objectsFloorplanObjectsEntry fpoe in Fpobject.objects.@object)
                {
                    if (((fpoe.x*wrat) - hittargetw)*xRat <= local.X && ((fpoe.x*wrat) + hittargetw)*xRat > local.X &&
                        ((fpoe.y*hrat) - hittargeth)*yRat <= local.Y && ((fpoe.y*hrat) + hittargeth)*yRat > local.Y)
                    {
                        switch (fpoe.type)
                        {
                            case "camera":
                                CameraWindow cw = MainClass.GetCameraWindow(fpoe.id);
                                if (cw != null)
                                {
                                    //cw.Location = new Point(Location.X + e.X, Location.Y + e.Y);
                                    cw.BringToFront();
                                    cw.Focus();
                                }

                                changeHighlight = false;
                                break;
                            case "microphone":
                                VolumeLevel vl = MainClass.GetVolumeLevel(fpoe.id);
                                if (vl != null)
                                {
                                    //vl.Location = new Point(Location.X + e.X, Location.Y + e.Y);
                                    vl.BringToFront();
                                    vl.Focus();
                                }

                                changeHighlight = false;
                                break;
                        }
                        break;
                    }                   
                }
            }

            if (changeHighlight)
            {
                bool hl = Highlighted;
                MainClass._pnlCameras.ClearHighlights();

                Highlighted = !hl;
            }
            if (Highlighted)
            {
                foreach (objectsFloorplanObjectsEntry fpoe in Fpobject.objects.@object)
                {
                    switch (fpoe.type)
                    {
                        case "camera":
                            CameraWindow cw = MainClass.GetCameraWindow(fpoe.id);
                            if (cw!=null)
                                cw.Highlighted = true;
                            break;
                        case "microphone":
                            VolumeLevel vl = MainClass.GetVolumeLevel(fpoe.id);
                            if (vl!=null)
                                vl.Highlighted = true;

                            break;
                    }
                }
            }

            MainClass.Invalidate(true);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            MainForm.InstanceReference.LastFocussedControl = this;
            Invalidate();
            base.OnGotFocus(e);
        }

        public bool Highlighted { get; set; }

        public void LoadFileList()
        {
            //no files
        }

        public void SaveFileList()
        {
            //no files
        }

        public Color BorderColor
        {
            get
            {
                if (Highlighted)
                    return MainForm.FloorPlanHighlightColor;

                if (Focused)
                    return MainForm.BorderHighlightColor;

                return MainForm.BorderDefaultColor;

            }
        }

        public int BorderWidth => Highlighted ? 2 : 1;

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics gPlan = pe.Graphics;
            Rectangle rc = ClientRectangle;
           
            int textpos = rc.Height - 20;

           
            
            
            
            try
            {
                


                if (_imgview != null)
                {
                    if (!_minimised)
                        gPlan.DrawImage(_imgview, rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 26);

                    gPlan.CompositingMode = CompositingMode.SourceOver;
                    gPlan.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    gPlan.DrawString(LocRm.GetString("FloorPlan") + ": " + Fpobject.name, _drawFont,
                                _drawBrush,
                                new PointF(5, textpos));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            if (_mouseMove > Helper.Now.AddSeconds(-3) && MainForm.Conf.ShowOverlayControls)
            {
                DrawOverlay(gPlan);
            }

            using (var grabBrush = new SolidBrush(BorderColor))
            using (var borderPen = new Pen(grabBrush, BorderWidth))
            {
                gPlan.DrawRectangle(borderPen, 0, 0, rc.Width - 1, rc.Height - 1);

                if (!MainForm.LockLayout)
                {
                    var borderPoints = new[]
                                        {
                                            new Point(rc.Width - 15, rc.Height), new Point(rc.Width, rc.Height - 15),
                                            new Point(rc.Width, rc.Height)
                                        };
                    gPlan.FillPolygon(grabBrush, borderPoints);
                    
                }
            }

            base.OnPaint(pe);
        }

        private int GetButtonIndexByLocation(Point xy)
        {
            var rBp = ButtonPanel;
            if (xy.X >= rBp.X && xy.Y > rBp.Y - 25 && xy.X <= rBp.X + rBp.Width && xy.Y <= rBp.Y + rBp.Height)
            {
                if (xy.Y < rBp.Y)
                    return -1;//seek

                if (xy.Y > 25)
                {
                    double x = xy.X - rBp.X;
                    return Convert.ToInt32(Math.Ceiling((x / rBp.Width) * ButtonCount)) - 1;
                }
            }
            return -999;//nothing
        }

        private Rectangle GetButtonByIndex(int buttonIndex, out Rectangle destRect)
        {
            Rectangle rSrc = Rectangle.Empty;
            switch (buttonIndex)
            {
                case 0://edit
                    rSrc = MainForm.REdit;
                    break;
                case 1://web
                    rSrc = Helper.HasFeature(Enums.Features.Access_Media) ? MainForm.RWeb : MainForm.RWebOff;
                    break;
            }

            if (MainForm.Conf.BigButtons)
            {
                rSrc.X -= 2;
                rSrc.Width += 8;
                rSrc.Height += 8;
            }

            destRect = new Rectangle(ButtonPanel.X + buttonIndex * (rSrc.Width + 5) + 5, Height - 25 - rSrc.Height - 6, rSrc.Width, rSrc.Height);
            return rSrc;
        }


        private void DrawButton(Graphics gCam, int buttonIndex)
        {
            Rectangle rDest;
            Rectangle rSrc = GetButtonByIndex(buttonIndex, out rDest);

            gCam.DrawImage(MainForm.Conf.BigButtons ? Properties.Resources.icons_big : Properties.Resources.icons, rDest, rSrc, GraphicsUnit.Pixel);
        }

        private void DrawOverlay(Graphics gCam)
        {
            var rPanel = ButtonPanel;
            gCam.FillRectangle(MainForm.OverlayBackgroundBrush, rPanel);
            for (int i = 0; i < ButtonCount; i++)
                DrawButton(gCam, i);
        }


        public int ImageWidth
        {
            get
            {
                if (_imgview != null)
                    return _imgview.Width;
                return 533;
            }
        }

        public int ImageHeight
        {
            get
            {
                if (_imgview != null)
                    return _imgview.Height;
                return 400;
            }
        }

        public void Tick()
        {
            if (NeedSizeUpdate)
            {
                UpdatePosition();
            }

            if (NeedsRefresh)
            {
                bool alert = false;
                lock (this)
                {
                    if (RefreshImage || (_imgplan == null && !string.IsNullOrEmpty(Fpobject.image)))
                    {
                        if (_imgplan!=null)
                        {
                            try
                            {
                                _imgplan.Dispose();
                            }
                            catch
                            {
                                // ignored
                            }
                            _imgplan = null;
                        }
                        if (_imgview != null)
                        {
                            try
                            {
                                _imgview.Dispose();
                            }
                            catch
                            {
                                // ignored
                            }
                            _imgview = null;
                        }
                        Bitmap img=null;
                        try
                        {
                            img = (Bitmap) Image.FromFile(Fpobject.image);
                        }
                        catch
                        {
                            // ignored
                        }
                        if (img != null)
                        {
                            if (!Fpobject.originalsize)
                            {
                                var rf = new ResizeBilinear(533, 400);
                                _imgplan = rf.Apply(img);
                                _imgview = (Bitmap) _imgplan.Clone();
                            }
                            else
                            {
                                _imgplan = img;
                                _imgview = (Bitmap) _imgplan.Clone();
                            }
                        }
                        RefreshImage = false;
                    }
                    if (_imgplan == null)
                        return;

                    
                    

                    Graphics gLf = Graphics.FromImage(_imgview);
                    gLf.DrawImage(_imgplan, 0, 0,_imgplan.Width,_imgplan.Height);

                    bool itemRemoved = false;
                    double wrat = Convert.ToDouble(ImageWidth) / 533d;
                    double hrat = Convert.ToDouble(ImageHeight) / 400d;

                    foreach (objectsFloorplanObjectsEntry fpoe in Fpobject.objects.@object)
                    {
                        var p = new Point(fpoe.x, fpoe.y);
                        if (Fpobject.originalsize)
                        {
                            p.X = Convert.ToInt32(p.X*wrat);
                            p.Y = Convert.ToInt32(p.Y*hrat);
                        }
                        if (fpoe.fov == 0)
                            fpoe.fov = 135;
                        if (fpoe.radius == 0)
                            fpoe.radius = 80;
                        switch (fpoe.type)
                        {
                            case "camera":
                                {
                                    var cw = MainClass.GetCameraWindow(fpoe.id);
                                    if (cw != null)
                                    {
                                        double drad = (fpoe.angle - 180) * Math.PI / 180;
                                        var points = new[]
                                            {
                                                new Point(p.X + 11+Convert.ToInt32(20*Math.Cos(drad)), p.Y + 11 + Convert.ToInt32((20* Math.Sin(drad)))),
                                                new Point(p.X + 11+Convert.ToInt32(20*Math.Cos(drad+(135*Math.PI/180))), p.Y + 11 + Convert.ToInt32((20* Math.Sin(drad+(135*Math.PI/180))))),
                                                new Point(p.X + 11+Convert.ToInt32(10*Math.Cos(drad+(180*Math.PI/180))), p.Y + 11 + Convert.ToInt32((10* Math.Sin(drad+(180*Math.PI/180))))),
                                                new Point(p.X + 11+Convert.ToInt32(20*Math.Cos(drad-(135*Math.PI/180))), p.Y + 11 + Convert.ToInt32((20* Math.Sin(drad-(135*Math.PI/180)))))
                                            };
                                        if (cw.Camobject.settings.active && !cw.VideoSourceErrorState)
                                        {
                                            int offset = (fpoe.radius / 2) - 11;
                                            if (cw.Alerted)
                                            {
                                                gLf.FillPolygon(_alertBrush, points);

                                                gLf.FillPie(_alertBrushScanner, p.X - offset, p.Y - offset, fpoe.radius, fpoe.radius,
                                                            (float)(fpoe.angle - 180 - (fpoe.fov / 2)), fpoe.fov);
                                                alert = true;
                                            }
                                            else
                                            {
                                                gLf.FillPolygon(_noalertBrush, points);
                                                gLf.FillPie(_noalertBrushScanner, p.X - offset, p.Y - offset, fpoe.radius, fpoe.radius,
                                                            (float)(fpoe.angle - 180 - (fpoe.fov / 2)), fpoe.fov);
                                            }
                                        }
                                        else
                                        {
                                            gLf.FillPolygon(_offlineBrush, points);
                                        }

                                    }
                                    else
                                    {
                                        fpoe.id = -2;
                                        itemRemoved = true;
                                    }
                                }
                                break;
                            case "microphone":
                                {
                                    var vw = MainClass.GetVolumeLevel(fpoe.id);
                                    if (vw != null)
                                    {
                                        if (vw.Micobject.settings.active && !vw.AudioSourceErrorState)
                                        {
                                            if (vw.Alerted)
                                            {
                                                gLf.FillEllipse(_alertBrush, p.X - 20, p.Y - 20, 40, 40);
                                                alert = true;
                                            }
                                            else
                                            {
                                                gLf.FillEllipse(_noalertBrush, p.X - 15, p.Y - 15, 30, 30);
                                            }
                                        }
                                        else
                                        {
                                            gLf.FillEllipse(_offlineBrush, p.X - 15, p.Y - 15, 30, 30);
                                        }
                                    }
                                    else
                                    {
                                        fpoe.id = -2;
                                        itemRemoved = true;
                                    }
                                }
                                break;
                        }
                    }

                    if (itemRemoved)
                        Fpobject.objects.@object = Fpobject.objects.@object.Where(fpoe => fpoe.id > -2).ToArray();
                    

                    gLf.Dispose();
                }
                Invalidate();
                LastRefreshTimestamp = Helper.Now.UnixTicks();
                NeedsRefresh = false;
                IsAlert = alert;
            }
        }

        public void Apply()
        {
        }

        #region Nested type: ThreadSafeCommand

        public class ThreadSafeCommand : EventArgs
        {
            public string Command;
            // Constructor
            public ThreadSafeCommand(string command)
            {
                Command = command;
            }
        }

        #endregion

        public bool IsEnabled => true;
        public bool Talking => false;
        public bool Listening => false;
        public bool Recording => false;

        public string ObjectName => Fpobject.name;

        public bool CanTalk => false;

        public bool CanListen => false;

        public bool CanRecord => false;

        public bool CanEnable => false;

        public bool CanGrab => false;
        public bool HasFiles => false;

        public void Disable(bool stopSource = true)
        {
            //throw new NotImplementedException();
        }

        public void Enable()
        {
            //throw new NotImplementedException();
        }

        public string RecordSwitch(bool record)
        {
            //throw new NotImplementedException();
            return "";
        }

        public void Talk(IWin32Window f = null)
        {
            //throw new NotImplementedException();
        }

        public void Listen()
        {
            //throw new NotImplementedException();
        }

        public string SaveFrame(Bitmap bmp = null)
        {
            //throw new NotImplementedException();
            return "";
        }
    }
}