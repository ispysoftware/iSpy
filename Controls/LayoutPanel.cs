using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Utilities;

namespace iSpyApplication.Controls
{
    public class LayoutPanel:Panel
    {
        private static readonly List<LayoutItem> SavedLayout = new List<LayoutItem>();
        private ISpyControl _maximised = null;
        public static bool NeedsRedraw;

        public LayoutPanel()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.ResizeRedraw | 
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);

            UpdateStyles();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            if (BrandedImage != null)
            {
                BrandedImage.Left = Width / 2 - BrandedImage.Width / 2;
                BrandedImage.Top = Height / 2 - BrandedImage.Height / 2;
            }
            Invalidate();
            base.OnScroll(se);
        }

        public PictureBox BrandedImage;
        

        private static void GetRowsCols(int controls, double width, double height, out int rows, out int cols)
        {
            rows = 0;
            cols = 0;
            if (controls == 0)
                return;
            bool favourH = width > height;
            rows = Convert.ToInt32(Math.Ceiling(Math.Sqrt(controls)));
            double d = Convert.ToDouble(controls) / rows;
            cols = Convert.ToInt32(Math.Ceiling(d));

            if (favourH && rows != cols)
            {
                int i = cols;
                cols = rows;
                rows = i;
            }
        }

        private int GridPadding = 2;
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (BrandedImage != null)
            {
                BrandedImage.Left = Width / 2 - BrandedImage.Width / 2;
                BrandedImage.Top = Height / 2 - BrandedImage.Height / 2;
            }
            
            AutoGrid();
            
            base.OnPaint(pe);
        }

        public void AutoGrid()
        {
            AutoScroll = MainForm.LayoutMode != Enums.LayoutMode.AutoGrid;

            if (MainForm.LayoutMode == Enums.LayoutMode.AutoGrid)
            {
                HorizontalScroll.Value = 0;
                VerticalScroll.Value = 0;

                if (_maximised != null)
                {
                    Maximise(_maximised, false);
                }
                else
                    LayoutControlsInGrid();
            }
        }

        public void LayoutControlsInGrid()
        {
            var lc = new List<ISpyControl>();
            foreach (var c in Controls)
            {
                if (c is CameraWindow || c is FloorPlanControl)
                {
                    lc.Add((ISpyControl) c);
                    continue;
                }
                var vl = c as VolumeLevel;
                if (vl?.Paired == false)
                    lc.Add(vl);
            }

            int aw = DisplayRectangle.Width - Padding.Horizontal;
            int ah = DisplayRectangle.Height - Padding.Vertical;

            int rows;
            int cols;
            GetRowsCols(lc.Count, aw, ah, out rows, out cols);
            double w = Convert.ToDouble(aw)/cols;
            double h = Convert.ToDouble(ah)/rows;

            int row = 0;
            int col = 0;
            var l = lc.OrderBy(p => p.Order).ToList();
            int ind = 0;
            foreach (var io in l)
            {
                io.Order = ind;
                ind++;
                var c = (PictureBox) io;
                c.Location = new Point(Convert.ToInt32(col*w), Convert.ToInt32(row*h));
                c.Width = Convert.ToInt32(Math.Max(2, w - GridPadding));
                var hc = Convert.ToInt32(Math.Max(2, h - GridPadding));
                var cw = c as CameraWindow;
                var vc = cw?.VolumeControl;
                if (vc?.IsDisposed == false)
                {
                    hc = Math.Max(40, hc - 40);
                    vc.Location = new Point(Convert.ToInt32(col*w), Convert.ToInt32(row*h) + hc);
                    vc.Width = c.Width;
                    vc.Height = 40;
                }
                c.Height = hc;

                col++;
                if (col < cols) continue;
                row++;
                col = 0;
            }
        }

        public void LayoutObjects(int w, int h)
        {
            if (MainForm.LayoutMode == Enums.LayoutMode.AutoGrid)
                return;

            HorizontalScroll.Value = 0;
            VerticalScroll.Value = 0;
            Refresh();
            int num = Controls.Count;
            if (num == 0)
                return;
            // Get data.
            var rectslist = new List<Rectangle>();

            foreach (Control c in Controls)
            {
                bool skip = false;
                if (!(c is ISpyControl)) continue;
                var p = (PictureBox)c;
                if (w > 0)
                {
                    p.Width = w;
                    p.Height = h;
                }
                if (w == -1)
                {
                    var window = c as CameraWindow;
                    if (window != null)
                    {
                        var cw = window;

                        if (cw.Camera != null)
                        {
                            var bmp = cw.LastFrame;
                            if (bmp != null)
                            {
                                p.Width = bmp.Width + 2;
                                p.Height = bmp.Height + 32;
                            }
                        }
                    }
                    else
                    {
                        p.Width = c.Width;
                        p.Height = c.Height;
                    }
                }
                int nh = p.Height;
                var cameraWindow = c as CameraWindow;
                if (cameraWindow?.VolumeControl != null)
                    nh += 40;
                var level = c as VolumeLevel;
                if (level != null)
                {
                    if (level.Paired)
                        skip = true;
                }
                if (!skip)
                {
                    rectslist.Add(new Rectangle(0, 0, p.Width, nh));
                }
            }
            // Arrange the rectangles.
            Rectangle[] rects = rectslist.ToArray();
            int binWidth = Width;
            var proc = new C2BpProcessor();
            proc.SubAlgFillOneColumn(binWidth, rects);
            rectslist = rects.ToList();
            bool assigned = true;
            var indexesassigned = new List<int>();
            while (assigned)
            {
                assigned = false;
                foreach (Rectangle r in rectslist)
                {
                    for (int i = 0; i < Controls.Count; i++)
                    {
                        Control c = Controls[i];
                        if (c is ISpyControl)
                        {
                            bool skip = false;
                            int hoffset = 0;
                            if (!indexesassigned.Contains(i))
                            {
                                var window = c as CameraWindow;
                                var cw = window;
                                if (cw?.VolumeControl != null)
                                    hoffset = 40;
                                var level = c as VolumeLevel;
                                if (level != null)
                                {
                                    if (level.Paired)
                                        skip = true;
                                }
                                if (!skip && c.Width == r.Width && c.Height + hoffset == r.Height)
                                {
                                    PositionPanel((PictureBox)c, new Point(r.X, r.Y), r.Width, r.Height - hoffset);
                                    rectslist.Remove(r);
                                    assigned = true;
                                    indexesassigned.Add(i);
                                    break;
                                }
                            }
                        }
                    }
                    if (assigned)
                        break;
                }
            }
            NeedsRedraw = true;
        }

        public void PositionPanel(PictureBox p, Point xy, int w, int h)
        {
            p.Width = w;
            p.Height = h;
            p.Location = new Point(xy.X, xy.Y);
        }

        public void ResetLayout()
        {
            foreach (LayoutItem li in SavedLayout)
            {
                switch (li.ObjectTypeId)
                {
                    case 1:
                        VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(li.ObjectId);
                        if (vl != null)
                        {
                            vl.Location = new Point(li.LayoutRectangle.X, li.LayoutRectangle.Y);
                            vl.Size = new Size(li.LayoutRectangle.Width, li.LayoutRectangle.Height);
                        }
                        break;
                    case 2:
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(li.ObjectId);
                        if (cw != null)
                        {
                            cw.Location = new Point(li.LayoutRectangle.X, li.LayoutRectangle.Y);
                            cw.Size = new Size(li.LayoutRectangle.Width, li.LayoutRectangle.Height);
                        }
                        break;
                    case 3:
                        FloorPlanControl fp = MainForm.InstanceReference.GetFloorPlan(li.ObjectId);
                        if (fp != null)
                        {
                            fp.Location = new Point(li.LayoutRectangle.X, li.LayoutRectangle.Y);
                            fp.Size = new Size(li.LayoutRectangle.Width, li.LayoutRectangle.Height);
                        }
                        break;
                }
            }
        }

        public void SaveLayout()
        {
            //save layout
            SavedLayout.Clear();

            foreach (Control c in Controls)
            {
                var r = new Rectangle(c.Location.X, c.Location.Y, c.Width, c.Height);
                var window = c as CameraWindow;
                if (window != null)
                {
                    SavedLayout.Add(new LayoutItem
                    {
                        LayoutRectangle = r,
                        ObjectId = window.Camobject.id,
                        ObjectTypeId = 2
                    });
                    continue;
                }
                var control = c as FloorPlanControl;
                if (control != null)
                {
                    SavedLayout.Add(new LayoutItem
                    {
                        LayoutRectangle = r,
                        ObjectId = control.Fpobject.id,
                        ObjectTypeId = 3
                    });
                    continue;
                }
                var level = c as VolumeLevel;
                if (level != null)
                {
                    SavedLayout.Add(new LayoutItem
                    {
                        LayoutRectangle = r,
                        ObjectId = level.Micobject.id,
                        ObjectTypeId = 1
                    });
                }

            }
            
        }

        public void Maximise(object obj)
        {
            Maximise(obj, true);
        }

        private delegate void MaximiseDelegate(object obj, bool minimiseIfMaximised);

        public void Maximise(object obj, bool minimiseIfMaximised)
        {
            if (obj == null)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new MaximiseDelegate(Maximise), obj, minimiseIfMaximised);
                return;
            }
            var window = obj as CameraWindow;
            if (window != null)
            {

                var cameraControl = window;
                cameraControl.BringToFront();

                try
                {
                    var r = cameraControl.RestoreRect;
                    if (r.IsEmpty)
                    {
                        var s = "320x240";
                        if (!string.IsNullOrEmpty(cameraControl.Camobject.resolution))
                            s = cameraControl.Camobject.resolution;
                        var wh = s.Split('x');

                        cameraControl.RestoreRect = new Rectangle(cameraControl.Location.X, cameraControl.Location.Y,
                                                                  cameraControl.Width, cameraControl.Height);

                        double wFact = Convert.ToDouble(Width) / Convert.ToDouble(wh[0]);
                        double hFact = Convert.ToDouble(Height) / Convert.ToDouble(wh[1]);
                        if (cameraControl.VolumeControl != null)
                            hFact = Convert.ToDouble((Height - 40)) / Convert.ToDouble(wh[1]);
                        if (hFact <= wFact)
                        {
                            cameraControl.Width = Convert.ToInt32(((Convert.ToDouble(Width) * hFact) / wFact));
                            cameraControl.Height = Height;
                        }
                        else
                        {
                            cameraControl.Width = Width;
                            cameraControl.Height = Convert.ToInt32((Convert.ToDouble(Height) * wFact) / hFact);
                        }
                        cameraControl.Location = new Point(((Width - cameraControl.Width) / 2),
                                                           ((Height - cameraControl.Height) / 2));
                        if (cameraControl.VolumeControl != null)
                            cameraControl.Height -= 40;
                        _maximised = cameraControl;
                    }
                    else
                    {
                        if (minimiseIfMaximised)
                        {
                            Minimize(window, false);
                            cameraControl.RestoreRect = Rectangle.Empty;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                return;
            }

            var level = obj as VolumeLevel;
            if (level != null)
            {
                var vf = level;
                vf.BringToFront();
                if (vf.Paired)
                {
                    var c = MainForm.Cameras.Single(p => p.settings.micpair == vf.Micobject.id).id;
                    CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(c);
                    if (vf.Width == Width)
                    {
                        if (minimiseIfMaximised)
                            Minimize(cw, false);
                    }
                    else
                        Maximise(cw);
                }
                else
                {
                    var r = vf.RestoreRect;
                    if (r.IsEmpty)
                    {
                        vf.RestoreRect = new Rectangle(vf.Location.X, vf.Location.Y,
                                                                  vf.Width, vf.Height);
                        vf.Location = new Point(0, 0);
                        vf.Width = Width;
                        vf.Height = Height;
                        _maximised = vf;

                    }
                    else
                    {

                        if (minimiseIfMaximised)
                            Minimize(vf, false);
                        vf.RestoreRect = Rectangle.Empty;
                    }
                }
                return;
            }

            var control = obj as FloorPlanControl;
            if (control != null)
            {
                var fp = control;
                fp.BringToFront();
                var r = fp.RestoreRect;
                if (r.IsEmpty)
                {
                    fp.RestoreRect = new Rectangle(fp.Location.X, fp.Location.Y,
                                                              fp.Width, fp.Height);
                    var wFact = Convert.ToDouble(Width) / fp.Width;
                    var hFact = Convert.ToDouble(Height) / fp.Height;

                    if (hFact <= wFact)
                    {
                        fp.Width = (int)(Width / wFact * hFact);
                        fp.Height = Height;
                    }
                    else
                    {
                        fp.Width = Width;
                        fp.Height = (int)(Height / hFact * wFact);
                    }
                    fp.Location = new Point(((Width - fp.Width) / 2), ((Height - fp.Height) / 2));
                    _maximised = fp;
                }
                else
                {
                    if (minimiseIfMaximised)
                        Minimize(control, false);
                    fp.RestoreRect = Rectangle.Empty;
                }
                return;
            }
        }

        public void Minimize(object obj, bool tocontents)
        {
            _maximised = null;
            if (obj == null)
                return;
            var window = obj as CameraWindow;
            if (window != null)
            {
                var cw = window;
                Rectangle r = cw.RestoreRect;
                if (r != Rectangle.Empty && !tocontents)
                {
                    cw.Location = r.Location;
                    cw.Height = r.Height;
                    cw.Width = r.Width;
                }
                else
                {
                    if (cw.Camera != null)
                    {
                        Bitmap bmp = cw.LastFrame;
                        if (bmp != null)
                        {
                            cw.Width = bmp.Width + 2;
                            cw.Height = bmp.Height + 26;
                            bmp.Dispose();
                        }
                    }
                    else
                    {
                        cw.Width = 322;
                        cw.Height = 266;
                    }
                }
                cw.Invalidate();
            }

            var level = obj as VolumeLevel;
            if (level != null)
            {
                var cw = level;
                Rectangle r = cw.RestoreRect;
                if (r != Rectangle.Empty && !tocontents)
                {
                    cw.Location = r.Location;
                    cw.Height = r.Height;
                    cw.Width = r.Width;
                }
                else
                {
                    cw.Width = 160;
                    cw.Height = 40;
                }
                cw.Invalidate();
            }

            var control = obj as FloorPlanControl;
            if (control != null)
            {
                var fp = control;
                Rectangle r = fp.RestoreRect;
                if (r != Rectangle.Empty && !tocontents)
                {
                    fp.Location = r.Location;
                    fp.Height = r.Height;
                    fp.Width = r.Width;
                    fp.Invalidate();
                }
                else
                {
                    if (fp.ImgPlan != null)
                    {
                        fp.Width = fp.ImgPlan.Width + 2;
                        fp.Height = fp.ImgPlan.Height + 26;
                    }
                    else
                    {
                        fp.Width = 322;
                        fp.Height = 266;
                    }
                }
            }
        }

        public void LayoutOptimised()
        {
            double numberCameras = MainForm.Cameras.Count;
            int useX = 320, useY = 200;
            int dispArea = Width * Height;
            int lastArea = dispArea;


            for (int y = 1; y <= numberCameras; y++)
            {
                int camX = y;
                var camY = (int)Math.Round((numberCameras / y) + 0.499999999, 0);

                int dispWidth = Width / camX;
                int dispHeight = dispWidth / 4 * 3;
                int camArea = (int)numberCameras * (dispWidth * (dispHeight + 40));
                if (((dispArea - camArea) <= lastArea) && ((dispArea - camArea) > 0) && (((camY * (dispHeight + 40)) < Height)))
                {
                    useX = dispWidth;
                    useY = dispHeight;
                    lastArea = dispArea - camArea;
                }

                dispHeight = (Height - (camY * 40)) / camY;
                dispWidth = dispHeight * 4 / 3;
                camArea = (int)numberCameras * (dispWidth * (dispHeight + 40));
                if (((dispArea - camArea) <= lastArea) && ((dispArea - camArea) > 0) && (((camX * dispWidth) < Width)))
                {
                    useX = dispWidth;
                    useY = dispHeight;
                    lastArea = dispArea - camArea;
                }
            }
            LayoutObjects(useX, useY);
        }

        internal void ClearHighlights()
        {
            foreach (Control c in Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    window.Highlighted = false;
                }
                var control = c as FloorPlanControl;
                if (control != null)
                {
                    control.Highlighted = false;
                }
                var level = c as VolumeLevel;
                if (level != null)
                {
                    level.Highlighted = false;
                }

            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.SizeChanged += new System.EventHandler(this.LayoutPanel_SizeChanged);
            this.ResumeLayout(false);

        }

        private bool _mDown;
        private ISpyControl _dControl;

        public void ISpyControlDown(Point p)
        {
            if (MainForm.LayoutMode == Enums.LayoutMode.AutoGrid)
            {
                _mDown = true;
                _dControl = GetiSpyControl(p);
            }
        }

        private ISpyControl GetiSpyControl(Point p)
        {
            int x = p.X;
            int y = p.Y;
            foreach (var c in Controls)
            {
                var ctrl = c;
                var pb = ctrl as PictureBox;
                if (pb != null && c is ISpyControl)
                {
                    if (ctrl is VolumeLevel)
                    {
                        var vl = ctrl as VolumeLevel;
                        if (vl.Paired)
                            ctrl = vl.CameraControl;
                    }
                    if (x > pb.Left && x < pb.Left + pb.Width && y > pb.Top && y < pb.Top + pb.Height)
                        return ctrl as ISpyControl;
                }
            }
            return null;
        }


        public void ISpyControlUp(Point p)
        {
            
            if (MainForm.LayoutMode == Enums.LayoutMode.AutoGrid)
            {
                var c = GetiSpyControl(p);
                if (_mDown && c!=null)
                {
                    if (_dControl != null)
                    {
                        var uControl = c;
                        var i = uControl.Order;
                        var j = _dControl.Order;
                        _dControl.Order = i;
                        uControl.Order = j;
                        Invalidate();
                    }
                }
                _mDown = false;
            }
        }

        private void LayoutPanel_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
