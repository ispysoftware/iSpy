using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace iSpyApplication.Controls
{
    public sealed partial class AreaSelector : Panel
    {
        private Point _rectStart = Point.Empty;
        private Point _rectStop = Point.Empty;
        private Point _hoverPoint = Point.Empty;

        private bool _bMouseDown;
        private List<Rectangle> _motionZonesRectangles = new List<Rectangle>();

        private Bitmap _lastFrame;
        private readonly object _lockobject = new object();
        public Bitmap LastFrame
        {
            get
            {
                lock (_lockobject)
                {
                    if (_lastFrame == null)
                    {
                        return null;
                    }
                    return (Bitmap)_lastFrame.Clone();
                }
            }
            set
            {
                lock (_lockobject)
                {
                    if (_lastFrame != null)
                        _lastFrame.Dispose();
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

        public void ClearRectangles()
        {
            _motionZonesRectangles = new List<Rectangle>();
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
            foreach(var r in _motionZonesRectangles)
            {
                if (startX>r.X && startX<r.X+r.Width && startY>r.Y && startY<r.Y+r.Height)
                {
                    _rectIndex = i;
                    _rectOrig = new Rectangle(r.X,r.Y,r.Width,r.Height);
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
                    _motionZonesRectangles.RemoveAt(_rectIndex);
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
                _rectStart = new Point(0, 0);
                _rectStop = new Point(100, 100);
            }
            var start = new Point();
            var stop = new Point();
            
            start.X = _rectStart.X;
            if (_rectStop.X<_rectStart.X)
                start.X = _rectStop.X;
            start.Y = _rectStart.Y;
            if (_rectStop.Y<_rectStart.Y)
                start.Y = _rectStop.Y;

            stop.X = _rectStop.X;
            if (_rectStop.X<_rectStart.X)
                stop.X = _rectStart.X;
            stop.Y = _rectStop.Y;
            if (_rectStop.Y<_rectStart.Y)
                stop.Y = _rectStart.Y;

            var size = new Size(stop.X-start.X,stop.Y-start.Y);
            _motionZonesRectangles.Add(new Rectangle(start, size));
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
                    var r = _motionZonesRectangles[_rectIndex];

                    Rectangle rnew = Rectangle.Empty;
                    if (_hoverPoint.X==r.Left)
                    {
                        if (_hoverPoint.Y == r.Top)
                        {
                            rnew = NormRect(new Point(p.X, p.Y), new Point(r.Right, r.Bottom));
                        }
                        else
                        {
                            rnew = NormRect(new Point(p.X, r.Top), new Point(r.Right, p.Y));
                        }
                    }
                    if (_hoverPoint.X==r.Right)
                    {
                        if (_hoverPoint.Y == r.Top)
                        {
                            rnew = NormRect(new Point(r.X, p.Y), new Point(p.X, r.Bottom));
                        }
                        else
                        {
                            rnew = NormRect(new Point(r.X, r.Y), new Point(p.X, p.Y));
                        }                        
                    }
                    _hoverPoint = new Point(p.X,p.Y);
                    _motionZonesRectangles[_rectIndex] = rnew;
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
                    var mz = _motionZonesRectangles[_rectIndex];
                    mz.X = _rectOrig.X + (_rectStop.X - _rectStart.X);
                    mz.Y = _rectOrig.Y + (_rectStop.Y - _rectStart.Y);
                    _motionZonesRectangles[_rectIndex] = mz;
                }
            }
            else
            {
                _hoverPoint = Point.Empty;               

                for(int i=0;i<_motionZonesRectangles.Count;i++)
                {
                    var r = _motionZonesRectangles[i];
                    if (CalcDist(r.Left,r.Top, p)<5)
                    {
                        _hoverPoint = new Point(r.Left,r.Top);
                        _rectIndex = i;
                        break;
                    }
                    if (CalcDist( r.Right,r.Top, p) < 5)
                    {
                        _hoverPoint = new Point(r.Right,r.Top);
                        _rectIndex = i;
                        break;
                    }
                    if (CalcDist(r.Right,r.Bottom,  p) < 5)
                    {
                        _hoverPoint = new Point(r.Right, r.Bottom);
                        _rectIndex = i;
                        break;
                    }
                    if (CalcDist(r.Left,r.Bottom,  p) < 5)
                    {
                        _hoverPoint = new Point(r.Left, r.Bottom );
                        _rectIndex = i;
                        break;
                    }
                }
            }
            Invalidate();
        }

        internal Rectangle NormRect(Point p1, Point p2)
        {
            int x = p1.X, y = p1.Y, w, h;
            w = Math.Abs(p1.X - p2.X);
            h = Math.Abs(p1.Y - p2.Y);

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

        public AreaSelector()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 3, 3);
            _motionZonesRectangles = new List<Rectangle>();
            BackgroundImageLayout = ImageLayout.Stretch;
        }
        public objectsCameraDetectorZone[] MotionZones
        {
            get
            {
                var ocdzs = new List<objectsCameraDetectorZone>();
                for (int index = 0; index < _motionZonesRectangles.Count; index++)
                {
                    Rectangle r = _motionZonesRectangles[index];
                    var ocdz = new objectsCameraDetectorZone
                                   {
                                       left = r.Left,
                                       top = r.Top,
                                       width = r.Width,
                                       height = r.Height
                                   };
                    ocdzs.Add(ocdz);
                }
                return ocdzs.ToArray();
            }
            set
            {
                _motionZonesRectangles = new List<Rectangle>();
                if (value == null) return;
                foreach (var r in value)
                    _motionZonesRectangles.Add(new Rectangle(r.left, r.top, r.width, r.height));
            }
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

                if (_motionZonesRectangles.Count > 0)
                {
                    foreach (var r in _motionZonesRectangles)
                    {
                        var rMod = new Rectangle(Convert.ToInt32(r.X * wmulti), Convert.ToInt32(r.Y * hmulti), Convert.ToInt32(r.Width * wmulti), Convert.ToInt32(r.Height * hmulti));
                        g.FillRectangle(h, rMod);
                        g.DrawRectangle(p, rMod);
                    }
                }

                if (_rectIndex == -1)
                {
                    var p1 = new Point(Convert.ToInt32(_rectStart.X*wmulti), Convert.ToInt32(_rectStart.Y*hmulti));
                    var p2 = new Point(Convert.ToInt32(_rectStop.X*wmulti), Convert.ToInt32(_rectStop.Y*hmulti));

                    var ps = new[] {p1, new Point(p1.X, p2.Y), p2, new Point(p2.X, p1.Y), p1};
                    g.FillPolygon(h, ps);
                    g.DrawPolygon(p, ps);
                }
                if (_hoverPoint!=Point.Empty)
                    g.FillEllipse(Brushes.DeepSkyBlue,Convert.ToInt32(_hoverPoint.X * wmulti)-5,Convert.ToInt32(_hoverPoint.Y*hmulti)-5,10,10);

            }
            catch
            {
            }
            p.Dispose();
            h.Dispose();
            g.DrawRectangle(Pens.DarkGray,0,0,Width-1,Height-1);
            base.OnPaint(pe);
        }
        public event EventHandler BoundsChanged;

        private void OnBoundsChanged()
        {
            if (BoundsChanged!=null)
                BoundsChanged(this, EventArgs.Empty);
        }
    }
}