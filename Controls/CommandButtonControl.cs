using System;
using System.Drawing;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class CommandButtonControl : Panel
    {
        private const decimal SnapGrid = 10;
        private objectsCommand _flashButton;
        private DateTime _flashTime;
        private int _dx, _dy;
        private objectsCommand _editing, _resizing;

        public CommandButtonControl()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            MouseMove += CommandButtonsMouseMove;
            _tmrRefresh.Interval = 200;
            _tmrRefresh.Tick += TmrRefreshTick;
            _tmrRefresh.Start();
        }

        void TmrRefreshTick(object sender, EventArgs e)
        {
            if (_flashButton != null)
            {
                if ((DateTime.UtcNow - _flashTime).TotalMilliseconds > 500)
                {
                    _flashButton = null;
                    Invalidate();
                }

            }
        }
        public objectsCommand CurButton = null;

        void CommandButtonsMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                bool over = false;
                foreach (var btn in MainForm.RemoteCommands)
                {
                    if (btn.inwindow)
                    {
                        var loc = btn.location.Split(',');
                        var sz = btn.size.Split('x');
                        var x = Convert.ToInt32(loc[0]);
                        var y = Convert.ToInt32(loc[1]);
                        var w = Convert.ToInt32(sz[0]);
                        var h = Convert.ToInt32(sz[1]);

                        if (e.X > x && e.Y > y && e.X < x + w && e.Y < y + h)
                        {
                            CurButton = btn;
                            over = true;
                        }
                    }
                }
                if (!over)
                    CurButton = null;
            }
            else
            {
                if (_resizing!=null)
                {
                    var loc = _resizing.location.Split(',');
                    var x = Convert.ToInt32(loc[0]);
                    var y = Convert.ToInt32(loc[1]);
                    _resizing.size = Math.Max(Math.Min(e.Location.X,Width) - x,10) + "x" + Math.Max((Math.Min(e.Location.Y,Height) - y),10);
                    Constrain(_resizing);
                }
                else
                {
                    if (_editing!=null)
                    {
                        var nx = (e.Location.X - _dx);
                        var ny = (e.Location.Y - _dy);
                        _editing.location = nx + "," + ny;
                        Constrain(_editing);
                        
                        
                    }
                }
                   
                Invalidate();

            }
   
        }

        private void Constrain(objectsCommand cmd)
        {
            var loc = cmd.location.Split(',');
            var sz = cmd.size.Split('x');
            var x = Convert.ToInt32(loc[0]);
            var y = Convert.ToInt32(loc[1]);
            var w = Convert.ToInt32(sz[0]);
            var h = Convert.ToInt32(sz[1]);

            w = Math.Min(Width, w);
            h = Math.Min(Height, h);
            var nx = Math.Max(x, 0);
            var ny = Math.Max(y, 0);
            nx = Math.Min(nx, Width - w);
            ny = Math.Min(ny, Height - h);


            //snap w/h to nearest grid

            w = Convert.ToInt32(((int)Math.Round(w/SnapGrid)) * SnapGrid);
            h = Convert.ToInt32(((int)Math.Round(h / SnapGrid)) * SnapGrid);
            nx = Convert.ToInt32(((int)Math.Round(nx / SnapGrid)) * SnapGrid);
            ny = Convert.ToInt32(((int)Math.Round(ny / SnapGrid)) * SnapGrid);
            

            cmd.location = nx + "," + ny;
            cmd.size = w + "x" + h;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // lock           
            var g = pe.Graphics;
            var bText = new SolidBrush(Color.DarkGray);
            int nb = 0;
            try
            {
                foreach (var btn in MainForm.RemoteCommands)
                {
                    if (btn.inwindow)
                    {
                        nb++;
                        if (btn!=CurButton)
                            DrawButton(btn, g);
                    }
                }
                if (CurButton != null)
                {
                    //draw on top
                    DrawButton(CurButton, g);
                }

                if (nb == 0)
                {
                    const string txt = "Right click to add buttons";
                    var f = new Font(FontFamily.GenericSansSerif,12);
                    var ts = g.MeasureString(txt, f);
                    var fx = Convert.ToInt32(Width/2d - ts.Width/2);
                    var fy = Convert.ToInt32(Height/2d - ts.Height/2);
                    g.DrawString(txt, f, bText, fx, fy);
                }
            }
            catch
            {
            }
            bText.Dispose();
            base.OnPaint(pe);
        }

        private void DrawButton(objectsCommand btn, Graphics g)
        {
            var loc = btn.location.Split(',');
            var sz = btn.size.Split('x');
            var x = Convert.ToInt32(loc[0]);
            var y = Convert.ToInt32(loc[1]);
            var w = Convert.ToInt32(sz[0]);
            var h = Convert.ToInt32(sz[1]);

            using (var b = new SolidBrush(btn.backcolor.ToColor()))
            {
                g.FillRectangle(b, x, y, w, h);
            }

            var p = btn == _flashButton ? new Pen(Color.Red, 2) : new Pen(Color.DarkGray);

            g.DrawRectangle(p, x, y, w, h);
            p.Dispose();

            var f = FontXmlConverter.ConvertToFont(btn.font);
            var n = btn.name;
            if (n.StartsWith("cmd_"))
                n = LocRm.GetString(n);
            var ts = g.MeasureString(n, f);
            var fx = Convert.ToInt32(w/2d - ts.Width/2);
            var fy = Convert.ToInt32(h/2d - ts.Height/2);

            using (var b = new SolidBrush(btn.color.ToColor()))
            {
                g.DrawString(n, f, b, x + fx, y + fy);
            }
            if (_editing == btn)
            {
                g.FillEllipse(Brushes.Blue, x + w - 10, y + h - 10, 20, 20);
            }
        }

        public void Reposition()
        {
            if (CurButton != null)
            {
                _editing = CurButton;
                Invalidate();
            }
        }

        private objectsCommand _cmdDown;


        private void CommandButtonsMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_editing != null)
                {
                    var loc = _editing.location.Split(',');
                    var sz = _editing.size.Split('x');
                    var x = Convert.ToInt32(loc[0]);
                    var y = Convert.ToInt32(loc[1]);
                    var w = Convert.ToInt32(sz[0]);
                    var h = Convert.ToInt32(sz[1]);

                    if (Math.Sqrt(Math.Pow((x + w) - e.Location.X, 2) + Math.Pow((y + h) - e.Location.Y, 2)) < 20)
                    {
                        _resizing = _editing;
                        return;
                    }
                    _resizing = null;                    
                }

                if (CurButton != _editing)
                {
                    _cmdDown = CurButton;
                    _editing = null;
                    _resizing = null;
                    Invalidate();
                }
                else
                {
                    if (_editing!=null && CurButton == _editing)
                    {
                        var loc = _editing.location.Split(',');
                        var x = Convert.ToInt32(loc[0]);
                        var y = Convert.ToInt32(loc[1]);
                        _dx = e.Location.X - x;
                        _dy = e.Location.Y - y;

                    }    
                }
            }
            
        }

        public void Add(Point p)
        {
            using (var ecb = new AddCommandButton { location = p })
            {
                if (ecb.ShowDialog(this) == DialogResult.OK)
                {
                    _editing = ecb.CMD;
                    Constrain(_editing);
                }
            }
            Invalidate();
        }

        public void Edit()
        {
            using (var ecb = new EditCommandButton { CMD = CurButton })
            {
                ecb.ShowDialog(this);
                Constrain(CurButton);
            }
            Invalidate();
        }

        public void Remove()
        {
            CurButton.inwindow = false;
            Invalidate();
        }

        private void CommandButtonControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_editing != null)
                return;
            if (e.Button == MouseButtons.Left)
            {
                bool over = false;
                foreach (var btn in MainForm.RemoteCommands)
                {
                    if (btn.inwindow)
                    {
                        var loc = btn.location.Split(',');
                        var sz = btn.size.Split('x');
                        var x = Convert.ToInt32(loc[0]);
                        var y = Convert.ToInt32(loc[1]);
                        var w = Convert.ToInt32(sz[0]);
                        var h = Convert.ToInt32(sz[1]);

                        if (e.X > x && e.Y > y && e.X < x + w && e.Y < y + h)
                        {
                            CurButton = btn;
                            over = true;
                        }
                    }
                }
                if (!over)
                    CurButton = null;

                if (CurButton == _cmdDown && CurButton!=null)
                {
                    _flashButton = CurButton;
                    _flashTime = DateTime.UtcNow;
                    Invalidate();
                    MainForm.InstanceReference.RunCommand(CurButton.id);
                }
            }
        }

    }
}
