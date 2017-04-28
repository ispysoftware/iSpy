using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;

namespace iSpyApplication.Kinect
{
    public class TripWireEditor : Panel
    {
        private Point _pointStart;
        private Point _pointEnd;
        private bool _bMouseDown;
        private DepthLine _hoverDepthLine;
        public static List<DepthLine> TripWires = new List<DepthLine>();
        public string Config;

        internal static Pen TripWirePen = new Pen(Color.Red);
        internal static Pen LivePen = new Pen(Color.White);
        internal static Pen DepthPen = new Pen(Color.Black);
        internal static Brush LiveBrush = new SolidBrush(Color.White);
        internal static Brush DepthBrush = new SolidBrush(Color.FromArgb(200, Color.White));
        internal static Brush HoverBrush = new SolidBrush(Color.FromArgb(100, Color.White));
        internal static Brush DepthFontBrush = new SolidBrush(Color.Black);
        internal static Brush TripWireBrush = new SolidBrush(Color.FromArgb(100, 255, 0, 0));

        internal static Font DepthFont = new Font(FontFamily.GenericSansSerif, 12, GraphicsUnit.Pixel);

        private static readonly object SyncRoot = new object();

        public Bitmap BmpBack;
        public Bitmap LastFrame
        {
            set
            {
                lock (SyncRoot)
                {
                    if (BmpBack != null)
                    {
                        BmpBack.Dispose();
                    }
                    BmpBack = value;
                }
                Invalidate();
            }
        }


        public TripWireEditor()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 3, 3);
        }

        public void Init(string cfg)
        {
            TripWires.Clear();
            if (!string.IsNullOrEmpty(cfg))
            {
                try
                {
                    var tw = cfg.Trim().Split(';');
                    for (int i = 0; i < tw.Length; i++)
                    {
                        var twe = tw[i].Split(',');
                        if (!string.IsNullOrEmpty(twe[0]))
                        {
                            var sp = new Point(Convert.ToInt32(twe[0]), Convert.ToInt32(twe[1]));
                            var ep = new Point(Convert.ToInt32(twe[2]), Convert.ToInt32(twe[3]));
                            int dmin = Convert.ToInt32(twe[4]);
                            int dmax = Convert.ToInt32(twe[5]);
                            TripWires.Add(new DepthLine(sp, ep, dmin, dmax));
                        }
                    }
                    Config = cfg;
                }
                catch (Exception)
                {
                    Config = "";
                    TripWires.Clear();
                    MessageBox.Show(this, LocRm.GetString("TripWiresReset"));
                }
            }
        }

        public void ClearTripWires()
        {
            TripWires = new List<DepthLine>();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_hoverDepthLine != null)
            {
                var edl = new EditDepthLine(_hoverDepthLine);
                edl.ShowDialog(this);
                SaveTripWires();
                return;
            }
            int startX = e.X;
            int startY = e.Y;

            _pointStart = new Point(startX, startY);
            _pointEnd = new Point(startX, startY);
            OnBoundsChanged();
            _bMouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _bMouseDown = false;

            SaveTripWires();

            if (_hoverDepthLine != null)
            {
                _pointStart = Point.Empty;
                _pointEnd = Point.Empty;
                OnBoundsChanged();
                return;
            }
            if (_moveLine != null)
            {
                _moveLine.RecalculateSummaryPoint();
                _pointStart = Point.Empty;
                _pointEnd = Point.Empty;
                _moveLine = null;
                OnBoundsChanged();
                return;
            }

            int endX = e.X;
            int endY = e.Y;

            _pointEnd = new Point(endX, endY);

            if (Math.Sqrt(Math.Pow(endX - _pointStart.X, 2) + Math.Pow(endY - _pointStart.Y, 2)) < 5)
            {
                return;
            }
            var start = new Point();
            var stop = new Point();

            start.X = _pointStart.X;
            if (_pointEnd.X < _pointStart.X)
                start.X = _pointEnd.X;
            start.Y = _pointStart.Y;
            if (_pointEnd.Y < _pointStart.Y)
                start.Y = _pointEnd.Y;

            stop.X = _pointEnd.X;
            if (_pointEnd.X < _pointStart.X)
                stop.X = _pointStart.X;
            stop.Y = _pointEnd.Y;
            if (_pointEnd.Y < _pointStart.Y)
                stop.Y = _pointStart.Y;


            TripWires.Add(new DepthLine(_pointStart, _pointEnd));
            SaveTripWires();

            _pointStart = Point.Empty;
            _pointEnd = Point.Empty;
            OnBoundsChanged();
        }

        private void SaveTripWires()
        {
            string tw = TripWires.Aggregate("",
                                                        (current, dl) =>
                                                        current +
                                                        (dl.StartPoint.X + "," + dl.StartPoint.Y + "," + dl.EndPoint.X +
                                                        "," + dl.EndPoint.Y + "," + dl.DepthMin + "," + dl.DepthMax +
                                                        ";"));
            Config = tw.Trim(';');

        }

        
        private DepthLine _moveLine;
        private bool _moveLineStart;
        private Point _hoverPoint = Point.Empty;


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _hoverPoint = Point.Empty;
            if (_bMouseDown)
            {
                if (_moveLine != null)
                {
                    if (_moveLineStart)
                    {
                        _moveLine.StartPoint.X = e.X;
                        _moveLine.StartPoint.Y = e.Y;
                    }
                    else
                    {
                        _moveLine.EndPoint.X = e.X;
                        _moveLine.EndPoint.Y = e.Y;
                    }
                    _moveLine.RecalculateSummaryPoint();
                    return;
                }
                _pointEnd = new Point(e.X, e.Y);
            }
            else
            {
                Cursor = Cursors.Arrow;
               
                _hoverDepthLine =
                    TripWires.FirstOrDefault(
                        d =>
                        e.X > d.SummaryPoint.X && e.X < d.SummaryPoint.X + d.SummaryWidth &&
                        e.Y > d.SummaryPoint.Y &&
                        e.Y < d.SummaryPoint.Y + 15);
                if (_hoverDepthLine != null)
                {
                    Cursor = Cursors.Hand;
                }
                else
                {
                    double minD = 10;
                    _moveLine = null;
                    foreach (var dl in TripWires)
                    {
                        var d =
                            Math.Sqrt(Math.Pow(dl.StartPoint.X - e.X, 2) + Math.Pow(dl.StartPoint.Y - e.Y, 2));
                        if (d < minD)
                        {
                            minD = d;
                            _moveLine = dl;
                            _moveLineStart = true;
                            Cursor = Cursors.Hand;
                            _hoverPoint = dl.StartPoint;
                        }
                        d = Math.Sqrt(Math.Pow(dl.EndPoint.X - e.X, 2) + Math.Pow(dl.EndPoint.Y - e.Y, 2));
                        if (d < minD)
                        {
                            minD = d;
                            _moveLine = dl;
                            _moveLineStart = false;
                            Cursor = Cursors.Hand;
                            _hoverPoint = dl.EndPoint;
                        }
                    }
                }
            }
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _bMouseDown = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;
            g.CompositingMode = CompositingMode.SourceCopy;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.Default;
            try
            {
                lock (SyncRoot)
                {
                    if (BmpBack != null)
                    {

                        g.DrawImage(BmpBack, 0, 0, Width, Height);
                        g.CompositingMode = CompositingMode.SourceOver;

                        {
                            var p1 = new Point(_pointStart.X, _pointStart.Y);
                            var p2 = new Point(_pointEnd.X, _pointEnd.Y);

                                    g.DrawLine(LivePen, p1, p2);
                            
                            if (TripWires.Count > 0)
                            {
                                for (int i = 0; i < TripWires.Count; i++)
                                {
                                    var dl = TripWires[i];
                                    g.DrawLine(TripWirePen, dl.StartPoint, dl.EndPoint);
                                    var m = dl.SummaryText;
                                    if (dl.WidthChanged)
                                    {
                                        var s = g.MeasureString(m, DepthFont);
                                        dl.SummaryWidth = Convert.ToInt32(s.Width);
                                    }

                                    var r = new Rectangle(dl.SummaryPoint, new Size(dl.SummaryWidth, 15));
                                    g.FillRectangle(DepthBrush, r);
                                    g.DrawRectangle(DepthPen, r);
                                    g.DrawString(m, DepthFont, DepthFontBrush, r);
                                }

                                if (_hoverPoint != Point.Empty)
                                {
                                    var rh = new Rectangle(_hoverPoint.X - 4, _hoverPoint.Y - 4, 8, 8);
                                    g.FillEllipse(HoverBrush, rh);
                                }
                            }
                            
                        }
                    }

                }
                base.OnPaint(pe);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

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

        public event EventHandler BoundsChanged;

        private void OnBoundsChanged()
        {
            if (BoundsChanged != null)
                BoundsChanged(this, EventArgs.Empty);
        }

    }



}
