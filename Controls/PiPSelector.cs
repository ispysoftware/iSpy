using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed partial class PiPSelector : Panel
    {
        private Point _rectStart = Point.Empty;
        private Point _rectStop = Point.Empty;
        private Point _hoverPoint = Point.Empty;

        public int CurrentCameraID = -1;

        private bool _bMouseDown;
        private List<Pipconfig> _configs;

        private Bitmap _lastFrame;
        private readonly object _lockobject = new object();
        public Bitmap LastFrame
        {
            get
            {
                lock (_lockobject)
                {
                    return (Bitmap) _lastFrame?.Clone();
                }
            }
            set
            {
                lock (_lockobject)
                {
                    _lastFrame?.Dispose();
                    if (value != null)
                        _lastFrame = (Bitmap)value.Clone();
                    else
                    {
                        _lastFrame = null;
                    }
                }
                Invalidate();
            }
        }


        private int _rectIndex = -1;
        private Rectangle _rectOrig;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _bMouseDown = true;

            if (_hoverPoint!=Point.Empty)
            {
                return;
            }
            //clicked on an existing rectangle?
            
            int startX = Convert.ToInt32((e.X * 1.0) / (Width * 1.0) * 100);
            int startY = Convert.ToInt32((e.Y * 1.0) / (Height * 1.0) * 100);

            if (startX > 100)
                startX = 100;
            if (startY > 100)
                startY = 100;

            int i = 0;
            foreach (var r in _configs)
            {
                if (startX > r.Rect.X && startX < r.Rect.X + r.Rect.Width && startY > r.Rect.Y && startY < r.Rect.Y + r.Rect.Height)
                {
                    _rectIndex = i;
                    _rectOrig = new Rectangle(r.Rect.X, r.Rect.Y, r.Rect.Width, r.Rect.Height);
                    _rectStart = new Point(startX, startY);
                    return;
                }
                i++;
            }
            _rectIndex = -1;

            _rectStop = new Point(startX, startY);
            _rectStart = new Point(startX, startY);
            OnBoundsChanged();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            int endX = Convert.ToInt32((e.X * 1.0) / (Width * 1.0) * 100);
            int endY = Convert.ToInt32((e.Y * 1.0) / (Height * 1.0) * 100);
            
            _rectStop = new Point(endX, endY);
            _bMouseDown = false;
            if (_rectIndex>-1)
            {
                _rectStart = Point.Empty;
                _rectStop = Point.Empty;
                if (endX>100 || endY>100 || endX<0 || endY<0)
                {
                    _configs.RemoveAt(_rectIndex);
                }
                _rectIndex = -1;
                OnBoundsChanged();
                return;
            }
            if (endX > 100)
                endX = 100;
            if (endY > 100)
                endY = 100;
            if (Math.Sqrt(Math.Pow(endX - _rectStart.X, 2) + Math.Pow(endY - _rectStart.Y, 2)) < 5)
            {
                //ignore
                _rectStart = Point.Empty;
                _rectStop = Point.Empty;
                return;
            }
            var start = new Point();
            var stop = new Point();

            start.X = _rectStart.X;
            if (_rectStop.X < _rectStart.X)
                start.X = _rectStop.X;
            start.Y = _rectStart.Y;
            if (_rectStop.Y < _rectStart.Y)
                start.Y = _rectStop.Y;

            stop.X = _rectStop.X;
            if (_rectStop.X < _rectStart.X)
                stop.X = _rectStart.X;
            stop.Y = _rectStop.Y;
            if (_rectStop.Y < _rectStart.Y)
                stop.Y = _rectStart.Y;

            var size = new Size(stop.X - start.X, stop.Y - start.Y);
            if (CurrentCameraID > -1)
            {
                var m = _configs.FirstOrDefault(p => p.CameraID == CurrentCameraID);
                if (m == null)
                {
                    m = new Pipconfig {CameraID = CurrentCameraID, Rect = new Rectangle(start, size)};
                    _configs.Add(m);
                }
                else
                {
                    m.Rect = new Rectangle(start, size);
                }

            }
            _rectStart = Point.Empty;
            _rectStop = Point.Empty;
            OnBoundsChanged();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            double wmulti = Convert.ToDouble(Width) / Convert.ToDouble(100);
            double hmulti = Convert.ToDouble(Height) / Convert.ToDouble(100);

            var p = new Point(Convert.ToInt32(e.Location.X / wmulti), Convert.ToInt32(e.Location.Y / hmulti));

            if (_bMouseDown)
            {
                if (_hoverPoint!=Point.Empty)
                {
                    var r = _configs[_rectIndex];

                    Rectangle rnew = Rectangle.Empty;
                    if (_hoverPoint.X==r.Rect.Left)
                    {
                        rnew = _hoverPoint.Y == r.Rect.Top ? NormRect(new Point(p.X, p.Y), new Point(r.Rect.Right, r.Rect.Bottom)) : NormRect(new Point(p.X, r.Rect.Top), new Point(r.Rect.Right, p.Y));
                    }
                    if (_hoverPoint.X == r.Rect.Right)
                    {
                        rnew = _hoverPoint.Y == r.Rect.Top ? NormRect(new Point(r.Rect.X, p.Y), new Point(p.X, r.Rect.Bottom)) : NormRect(new Point(r.Rect.X, r.Rect.Y), new Point(p.X, p.Y));                        
                    }
                    _hoverPoint = new Point(p.X,p.Y);
                    _configs[_rectIndex] = new Pipconfig {CameraID = r.CameraID, Rect = rnew};
                    Invalidate();
                    return;
                }
                int endX = Convert.ToInt32((e.X * 1.0) / (Width * 1.0) * 100);
                int endY = Convert.ToInt32((e.Y * 1.0) / (Height * 1.0) * 100);
                if (endX > 100)
                    endX = 100;
                if (endY > 100)
                    endY = 100;

                _rectStop = new Point(endX, endY);
                if (_rectIndex>-1)
                {
                    var mz = _configs[_rectIndex];
                    mz.Rect.X = _rectOrig.X + (_rectStop.X - _rectStart.X);
                    mz.Rect.Y = _rectOrig.Y + (_rectStop.Y - _rectStart.Y);
                    _configs[_rectIndex] = mz;
                }
            }
            else
            {
                _hoverPoint = Point.Empty;

                for (int i = 0; i < _configs.Count; i++)
                {
                    var r = _configs[i];
                    if (CalcDist(r.Rect.Left, r.Rect.Top, p) < 5)
                    {
                        _hoverPoint = new Point(r.Rect.Left, r.Rect.Top);
                        _rectIndex = i;
                        break;
                    }
                    if (CalcDist(r.Rect.Right, r.Rect.Top, p) < 5)
                    {
                        _hoverPoint = new Point(r.Rect.Right, r.Rect.Top);
                        _rectIndex = i;
                        break;
                    }
                    if (CalcDist(r.Rect.Right, r.Rect.Bottom, p) < 5)
                    {
                        _hoverPoint = new Point(r.Rect.Right, r.Rect.Bottom);
                        _rectIndex = i;
                        break;
                    }
                    if (CalcDist(r.Rect.Left, r.Rect.Bottom, p) < 5)
                    {
                        _hoverPoint = new Point(r.Rect.Left, r.Rect.Bottom);
                        _rectIndex = i;
                        break;
                    }
                }
            }
            Invalidate();
        }

        internal Rectangle NormRect(Point p1, Point p2)
        {
            int x = p1.X, y = p1.Y;
            var w = Math.Abs(p1.X - p2.X);
            var h = Math.Abs(p1.Y - p2.Y);

            if (p2.X < p1.X)
                x = p2.X;
            if (p2.Y < p1.Y)
                y = p2.Y;
            return new Rectangle(x, y, w, h);
        }

        private double CalcDist(int x, int y, Point p)
        {

            return Math.Sqrt(Math.Pow(x - p.X, 2) + Math.Pow(y - p.Y, 2));
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _bMouseDown = false;
        }

        public PiPSelector()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 3, 3);
            _configs = new List<Pipconfig>();
            BackgroundImageLayout = ImageLayout.Stretch;
        }
        public string Areas
        {
            get
            {
                string r = _configs.Aggregate("", (current, rect) => current + (rect.CameraID.ToString(CultureInfo.InvariantCulture) + "," + rect.Rect.Left.ToString(CultureInfo.InvariantCulture) + "," + rect.Rect.Top.ToString(CultureInfo.InvariantCulture) + "," + rect.Rect.Width.ToString(CultureInfo.InvariantCulture) + "," + rect.Rect.Height.ToString(CultureInfo.InvariantCulture) + "|"));
                return r.Trim('|');
            }
            set
            {
                _configs = new List<Pipconfig>();
                var cfg = value.Split('|');
                foreach (var s in cfg)
                {
                    if (s != "")
                    {
                        var t = s.Split(',');
                        if (t.Length != 5) continue;
                        int cid,x, y, w, h;
                        if (int.TryParse(t[0], out cid) && int.TryParse(t[1], out x) && int.TryParse(t[2], out y) && int.TryParse(t[3], out w) &&
                            int.TryParse(t[4], out h))
                        {
                            _configs.Add(new Pipconfig {CameraID = cid, Rect = new Rectangle(x, y, w, h)});
                        }
                    }
                }
            }
        }

        private class Pipconfig
        {
            public int CameraID;
            public Rectangle Rect;

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // lock           
            var g = pe.Graphics;
            var c = Color.FromArgb(128, 255,255,255);
            var h = new SolidBrush(c);
            var p = new Pen(Color.DarkGray);
            try
            {
                var bmp = LastFrame;
                if (bmp != null)
                    g.DrawImage(_lastFrame, 0, 0, Width, Height);

                double wmulti = Convert.ToDouble(Width) / Convert.ToDouble(100);
                double hmulti = Convert.ToDouble(Height) / Convert.ToDouble(100);
                if (!Enabled)
                {
                    var d = new SolidBrush(Color.FromArgb(160,255,255,255));
                    g.FillRectangle(d, ClientRectangle);
                    d.Dispose();
                }
                else
                {
                    if (_configs.Count > 0)
                    {
                        foreach (var r in _configs)
                        {
                            var rMod = new Rectangle(Convert.ToInt32(r.Rect.X * wmulti), Convert.ToInt32(r.Rect.Y * hmulti), Convert.ToInt32(r.Rect.Width * wmulti), Convert.ToInt32(r.Rect.Height * hmulti));
                            g.FillRectangle(h, rMod);
                            g.DrawRectangle(p, rMod);

                            var n = MainForm.Cameras.FirstOrDefault(q => q.id == r.CameraID);
                            if (n != null)
                                g.DrawString(n.name, MainForm.Drawfont, MainForm.OverlayBrush, Convert.ToInt32(r.Rect.X * wmulti) + 2,
                                 Convert.ToInt32(r.Rect.Y * hmulti) + 2);
                        }
                    }

                    if (_rectIndex == -1 && _bMouseDown)
                    {
                        var p1 = new Point(Convert.ToInt32(_rectStart.X * wmulti), Convert.ToInt32(_rectStart.Y * hmulti));
                        var p2 = new Point(Convert.ToInt32(_rectStop.X * wmulti), Convert.ToInt32(_rectStop.Y * hmulti));

                        var ps = new[] { p1, new Point(p1.X, p2.Y), p2, new Point(p2.X, p1.Y), p1 };
                        g.FillPolygon(h, ps);
                        g.DrawPolygon(p, ps);

                    
                        var n = MainForm.Cameras.FirstOrDefault(q => q.id == CurrentCameraID);
                        if (n != null)
                            g.DrawString(n.name, MainForm.Drawfont, MainForm.OverlayBrush,
                                Convert.ToInt32(_rectStart.X*wmulti) + 2,
                                Convert.ToInt32(_rectStart.Y*hmulti) + 2);
                    
                    }

                    if (_hoverPoint!=Point.Empty)
                        g.FillEllipse(Brushes.DeepSkyBlue,Convert.ToInt32(_hoverPoint.X * wmulti)-5,Convert.ToInt32(_hoverPoint.Y*hmulti)-5,10,10);
                }
            }
            catch
            {
                // ignored
            }
            p.Dispose();
            h.Dispose();
            g.DrawRectangle(Pens.DarkGray,0,0,Width-1,Height-1);
            base.OnPaint(pe);
        }
        public event EventHandler BoundsChanged;

        private void OnBoundsChanged()
        {
            BoundsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}