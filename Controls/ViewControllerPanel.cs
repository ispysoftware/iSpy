using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    internal class ViewControllerPanel : PictureBox
    {
        public LayoutPanel LayoutTarget;
        private int _hScrollBound;
        private int _hscrolloffset;
        private Point _startPoint = Point.Empty;
        private int _vScrollBound;
        private int _vscrolloffset;
        private double _xRat, _yRat;

        public ViewControllerPanel(LayoutPanel layoutTarget)
        {
            LayoutTarget = layoutTarget;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics gLayout = pe.Graphics;

            var rects = new List<AlertRectangle>();
            double xMax = 0, yMax = 0;

            int xOff = LayoutTarget.HorizontalScroll.Value;
            int yOff = LayoutTarget.VerticalScroll.Value;

            foreach (Control c in LayoutTarget.Controls)
            {
                bool alert = false;
                var window = c as CameraWindow;
                if (window != null)
                {
                    alert = window.Alerted;
                }
                else
                {
                    var level = c as VolumeLevel;
                    if (level != null)
                    {
                        alert = level.Alerted;
                    }
                }


                rects.Add(new AlertRectangle(
                              new Rectangle(c.Location.X + xOff, c.Location.Y + yOff, c.Width, c.Height), alert,
                              LayoutTarget.Controls.GetChildIndex(c)));
                if (c.Height + c.Location.Y + yOff > yMax)
                    yMax = c.Height + c.Location.Y + yOff;
                if (c.Width + c.Location.X + xOff > xMax)
                    xMax = c.Width + c.Location.X + xOff;
            }
            rects = rects.OrderByDescending(p => p.ChildIndex).ToList();


            if (xMax > 0 && yMax > 0)
            {
                Rectangle rc = LayoutTarget.ClientRectangle;
                rc.X = xOff;
                rc.Y = yOff;

                gLayout.FillRectangle(Brushes.DarkGray, 0, 0, Width, Height);
                if (xMax < rc.Width + rc.X)
                    xMax = rc.Width + rc.X;
                if (yMax < rc.Height + rc.Y)
                    yMax = rc.Height + rc.Y;

                _hScrollBound = Convert.ToInt32(xMax - rc.Width);

                _vScrollBound = Convert.ToInt32(yMax - rc.Height);
                if (LayoutTarget.VerticalScroll.Visible && LayoutTarget.HorizontalScroll.Visible)
                {
                    _hScrollBound += SystemInformation.VerticalScrollBarWidth;
                    _vScrollBound += SystemInformation.HorizontalScrollBarHeight;
                }

                _xRat = Convert.ToDouble(Width)/xMax;
                _yRat = Convert.ToDouble(Height)/yMax;

                foreach (AlertRectangle r in rects)
                {
                    gLayout.DrawRectangle(!r.Alert ? Pens.White : Pens.Red, (float) _xRat*r.Rect.X,
                        (float) _yRat*r.Rect.Y,
                        (float) _xRat*r.Rect.Width,
                        (float) _yRat*r.Rect.Height);
                }

                gLayout.DrawRectangle(Pens.Orange, (float) _xRat*rc.X, (float) _yRat*rc.Y, (float) _xRat*rc.Width,
                                      (float) _yRat*rc.Height);
            }

            base.OnPaint(pe);
        }


        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            MainForm.InstanceReference.ShowIfUnlocked();
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left &&
                (LayoutTarget.VerticalScroll.Visible || LayoutTarget.HorizontalScroll.Visible))
            {
                int dx = e.Location.X - _startPoint.X;
                int dy = e.Location.Y - _startPoint.Y;
                double mx = dx/_xRat;
                double my = dy/_yRat;

                double hScroll = _hscrolloffset + mx;
                double vScroll = _vscrolloffset + my;

                if (hScroll > _hScrollBound) hScroll = _hScrollBound;
                if (vScroll > _vScrollBound) vScroll = _vScrollBound;

                if (hScroll < 0) hScroll = 0;
                if (vScroll < 0) vScroll = 0;


                LayoutTarget.HorizontalScroll.Value = Convert.ToInt32(hScroll);
                LayoutTarget.VerticalScroll.Value = Convert.ToInt32(vScroll);
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _startPoint = e.Location;
            _hscrolloffset = LayoutTarget.HorizontalScroll.Value;
            _vscrolloffset = LayoutTarget.VerticalScroll.Value;
            base.OnMouseDown(e);
        }

        #region Nested type: AlertRectangle

        private class AlertRectangle
        {
            public readonly bool Alert;
            public readonly int ChildIndex;
            public Rectangle Rect;

            public AlertRectangle(Rectangle rect, bool alert, int childIndex)
            {
                Rect = rect;
                Alert = alert;
                ChildIndex = childIndex;
            }
        }

        #endregion
    }
}