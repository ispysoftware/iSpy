using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed partial class MediaPanel : FlowLayoutPanel
    {
        public bool Loading = false;
        public Point SelectStart = Point.Empty;
        public Point SelectEnd = Point.Empty;
        

        public MediaPanel()
        {
            InitializeComponent();
            KeyDown += MediaPanelKeyDown;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        void MediaPanelKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            Invalidate();
            base.OnScroll(se);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_CLIPCHILDREN
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var g = pe.Graphics;
            if (Loading)
            {
                var txt = LocRm.GetString("Loading");
                var s = g.MeasureString(txt, MainForm.Drawfont);
                g.DrawString(txt, MainForm.Drawfont, MainForm.OverlayBrush, Convert.ToInt32(Width / 2) - s.Width / 2, Convert.ToInt32(Height / 2) - s.Height / 2);
            }

            if (SelectStart != Point.Empty && SelectEnd != Point.Empty)
            {
                var b = new SolidBrush(Color.White);
                var p = new Pen(b, 1) { DashStyle = DashStyle.Dash };
                g.DrawLine(p, SelectStart.X, SelectStart.Y, SelectStart.X, SelectEnd.Y);
                g.DrawLine(p, SelectStart.X, SelectEnd.Y, SelectEnd.X, SelectEnd.Y);
                g.DrawLine(p, SelectEnd.X, SelectEnd.Y, SelectEnd.X, SelectStart.Y);
                g.DrawLine(p, SelectEnd.X, SelectStart.Y, SelectStart.X, SelectStart.Y);

                b.Dispose();
                p.Dispose();
            }
            
            

        }

        private void MediaPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectStart = SelectEnd = e.Location;
            }
        }

        private void MediaPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Math.Sqrt(Math.Pow(SelectStart.X - SelectEnd.X, 2) + Math.Pow(SelectStart.Y - SelectEnd.Y, 2)) > 5)
                {
                    var r = NormRect(SelectStart, SelectEnd);
                    foreach (Control c in Controls)
                    {
                        var pb = c as PreviewBox;
                        if (pb?.Location.X < r.X + r.Width && pb.Location.X + pb.Width > r.X &&
                            pb.Location.Y < r.Y + r.Height && pb.Location.Y + pb.Height > r.Y)
                        {
                            pb.Selected = true;
                            pb.Invalidate();
                        }
                    }

                }
                SelectStart = Point.Empty;
                Invalidate();
            }
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

        private void MediaPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectStart != Point.Empty && e.Button== MouseButtons.Left)
            {
                SelectEnd = e.Location;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {

            if (ClientRectangle.Contains(PointToClient(MousePosition)))
                return;
            base.OnMouseLeave(e);
        }
    }
}
