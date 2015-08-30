using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    class VideoPlayback:PictureBox
    {
        public string Time, Duration;

        private bool _navTimeline;
        private readonly TrackBarGdi _tbVolume;
        //private readonly TrackBarGdi _tbSpeed;

        private FilesFile _ff;
        public event SeekEventHandler Seek;
        public event VolumeEventHandler VolumeChanged;
        public event PlayPauseEventHandler PlayPause;
        public delegate void SeekEventHandler(object sender, float percent);
        public delegate void SpeedEventHandler(object sender, int percent);
        public delegate void VolumeEventHandler(object sender, int percent);
        public delegate void PlayPauseEventHandler(object sender);

        readonly SolidBrush _bControl = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
        readonly SolidBrush _bTimeLine = new SolidBrush(Color.FromArgb(200, 255, 255, 255));

        private readonly ToolTip _toolTip;

        public bool RequestFrame;
        public Timer TmrRefresh;

        private DateTime _lastMove = Helper.Now;

        private Bitmap _lastFrame;
        public Bitmap LastFrame
        {
            get { return _lastFrame; }
            set
            {
                lock (this)
                {
                    _lastFrame?.Dispose();
                    _lastFrame = value;
                    if (value != null)
                    {
                        if (!_inited)
                        {
                            SetSize();
                            _inited = true;
                        }
                        Invalidate();
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TmrRefresh.Stop();
                TmrRefresh.Dispose();
                _bControl.Dispose();
                _bTimeLine.Dispose();
            }
            base.Dispose(disposing);
        }

        public VideoPlayback()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 0, 0);
            BorderStyle = BorderStyle.None;

            ResizeRedraw = true;
            TmrRefresh = new Timer();
            TmrRefresh.Tick += TmrRefreshTick;
            TmrRefresh.Interval = 100;
            TmrRefresh.Start();
            Time = Duration="00:00:00";
            _tbVolume = new TrackBarGdi(80,25,0,0, 50,0);
            //_tbSpeed = new TrackBarGdi(80, 25, 0, 0, 50,10);

            _toolTip = new ToolTip { AutomaticDelay = 500, AutoPopDelay = 1500 };
        }

        void TmrRefreshTick(object sender, EventArgs e)
        {
            if (RequestFrame)
            {
                RequestFrame = false;
                Invalidate();
            }
        }

        private bool _inited;
        private int _w, _h,_x,_y;
        int _fw, _fh;
        private const int TimelineHeight = 30, ControlHeight = 30;

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetSize();
        }
        private void SetSize()
        {
            if (_lastFrame != null)
            {
                _fw = _lastFrame.Width;
                _fh = _lastFrame.Height;
                if (Height > 0 && Width > 0)
                {
                    double arw = Convert.ToDouble(Width) / Convert.ToDouble(_fw);
                    double arh = Convert.ToDouble(Height) / Convert.ToDouble(_fh);
                    if (arh <= arw)
                    {
                        _w = Convert.ToInt32(((Convert.ToDouble(Width) * arh) / arw));
                        _h = Height;
                    }
                    else
                    {
                        _w = Width;
                        _h = Convert.ToInt32((Convert.ToDouble(Height) * arw) / arh);
                    }
                    _x = ((Width - _w) / 2);
                    _y = ((Height - _h) / 2);

                }
            }
        }
        private readonly object _frameLock = new object();
        protected override void OnPaint(PaintEventArgs pe)
        {
            lock (_frameLock)
            {
                Graphics gCam = pe.Graphics;

                if (LastFrame != null)
                {
                    try
                    {
                        gCam.DrawImage(LastFrame, _x, _y, _w, _h);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                
                if (_lastMove>Helper.Now.AddSeconds(-3))
                {
                    //draw scrolling graph
                    var pxCursor = ((float)Value / 100) * Width;
                    if (_navTimeline)
                        pxCursor = _mouseX;

                    int w = Width;
                    int xoff = 0;
                    if (ActivityGraph.Width>w)
                    {
                        w = ActivityGraph.Width;
                        double val = ((float)Value / 100d); 
                        if (_navTimeline)
                            val = Convert.ToDouble(pxCursor) / Width;
                        xoff = 0 - Convert.ToInt32(Convert.ToDouble(ActivityGraph.Width - Width)*val);
                    }

                    gCam.DrawImage(ActivityGraph, xoff, Height - TimelineHeight, w, TimelineHeight);

                    
                    Brush bPosition = new SolidBrush(Color.Black);
                    

                    int x2 = (int)pxCursor - 4;
                    int y2 = Height - TimelineHeight/2;
                    var navPoints = new[]
                    {
                        new Point(x2-4,y2-6), 
                        new Point(x2+4,y2),
                        new Point(x2-4,y2+6)
                    };

                    gCam.FillPolygon(Brushes.White, navPoints);
                    gCam.DrawPolygon(Pens.Black, navPoints);

                        
                    bPosition.Dispose();

                    gCam.FillRectangle(_bTimeLine, 0, Height - TimelineHeight - ControlHeight, Width, ControlHeight);
                    gCam.DrawString(Time + " / " + Duration, MainForm.DrawfontMed, Brushes.Black, 3, Height - TimelineHeight - ControlHeight + 2);


                    
                    int x = Width - 30;
                    int y = Height - TimelineHeight - ControlHeight + 2;
                    gCam.FillRectangle(_bControl, x - 5, y, 28, 25);
                    string c = ">";
                    if (CurrentState==PlaybackState.Playing)
                    {
                        c = "||";
                    }
                    gCam.DrawString(c, MainForm.DrawfontMed, Brushes.White, x, y-2);

                    _tbVolume.X = x - 100;
                    _tbVolume.Y = y;

                    //_tbSpeed.X = x - 200;
                    //_tbSpeed.Y = y;

                    DrawTrackBar(gCam, _tbVolume);
                    //DrawTrackBar(gCam, _tbSpeed);
                }
            }
        }

        

        private Bitmap _activityGraph;
        private Bitmap ActivityGraph
        {
            get
            {
                if (_activityGraph == null)
                {
                    if (_datapoints==null)
                    {
                        _activityGraph = new Bitmap(320, TimelineHeight);
                    }
                    else
                    {                   
                        _activityGraph = new Bitmap(_datapoints.Length, TimelineHeight);
                        Graphics gGraph = Graphics.FromImage(_activityGraph);
                        gGraph.FillRectangle(_bTimeLine, 0, 0, _activityGraph.Width, _activityGraph.Height);

                        if (_ff != null && _datapoints.Length > 0)
                        {
                            var pAlarm = new Pen(Color.Red);
                            var pOk = new Pen(Color.Black);
                            var trigger = (float)_ff.TriggerLevel;
                            var triggermax = (float)100;
                            if (_ff.TriggerLevelMaxSpecified)
                            {
                                triggermax = (float)_ff.TriggerLevelMax;
                            }


                            for (int i = 0; i < _datapoints.Length; i++)
                            {
                                float d;
                                if (float.TryParse(_datapoints[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d))
                                {
                                    if (d >= trigger && d <= triggermax)
                                    {
                                        gGraph.DrawLine(pAlarm, i, TimelineHeight, i,
                                                      TimelineHeight - Convert.ToInt32(d * (TimelineHeight / 100d)));
                                    }
                                    else
                                        gGraph.DrawLine(pOk, i, TimelineHeight, i, TimelineHeight - Convert.ToInt32(d * (TimelineHeight / 100d)));
                                }
                            
                            }
                            gGraph.DrawLine(pOk, 0, TimelineHeight / 2, _activityGraph.Width, TimelineHeight / 2);
                            pOk.Dispose();
                            pAlarm.Dispose();
                        }
                        gGraph.Dispose();
                    }
                }
                return _activityGraph;
            }
        }

        public void ResetActivtyGraph()
        {
            if (_activityGraph!=null)
            {
                _activityGraph.Dispose();
                _activityGraph = null;
            }
        }

        private void DrawTrackBar(Graphics g, TrackBarGdi tb)
        {
            var p = new Pen(_bControl);
            var b = new SolidBrush(Color.Black);
            g.DrawLine(p, tb.X, tb.Y, tb.X, tb.Y + tb.H);
            g.DrawLine(p, tb.X + tb.W, tb.Y, tb.X + tb.W, tb.Y + tb.H);
            g.DrawLine(p, tb.X, tb.Y + tb.H / 2, tb.X + tb.W, tb.Y + tb.H / 2);
            int gx = 1 + Convert.ToInt32((Convert.ToDouble(tb.W -tb.Wgrab - 1) / 100d) * tb.Val);
            g.FillRectangle(b, tb.X + gx, tb.Y + 2, tb.Wgrab, tb.H - 4);
            g.DrawRectangle(p, tb.X + gx, tb.Y + 2, tb.Wgrab, tb.H - 4);
            b.Dispose();
            p.Dispose();
            tb.Grab.X = tb.X + gx + tb.Wgrab / 2;
            tb.Grab.Y = tb.Y + tb.H / 2;
        }

        public enum PlaybackState {Stopped,Playing,Paused}


        private PlaybackState _currentState = PlaybackState.Stopped;

        public PlaybackState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RequestFrame = true;
            }
        }


        private string[] _datapoints;
        private double _value;
        private int _mouseX;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Location.Y>Height-TimelineHeight)
                _navTimeline = true;

            //if (distance(e.Location, _tbSpeed.Grab) < 6)
             //   _tbSpeed.Nav = true;
            if (distance(e.Location, _tbVolume.Grab) < 6)
                _tbVolume.Nav = true;

            if (distance(e.Location, new Point(Width - 20, Height - TimelineHeight - 20))<20)
            {
                PlayPause?.Invoke(this);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                if (_navTimeline)
                {
                    _navTimeline = false;
                    var v = (float) e.Location.X;
                    var val = (v/Width)*100;
                    Seek?.Invoke(this, val);
                }
                //if (_tbSpeed.Nav)
                //{
                //    if (_tbSpeed.Val>40 && _tbSpeed.Val<60)
                //    {
                //        _tbSpeed.Val = 50;
                //    }
                //}
            }
            //_tbSpeed.Nav = false;
            _tbVolume.Nav = false;
        }
        int _ttind = -1;
        protected override void  OnMouseMove(MouseEventArgs e)
        {
 	        base.OnMouseMove(e);
            _lastMove = Helper.Now;
            Cursor = Cursors.Default;
            string m = "";
            int newttind = -1;
            if (e.Location.Y>Height-TimelineHeight)
            {
                Cursor = Cursors.Hand;
                m = "Seek";
                newttind = 0;
            }
                
            _mouseX = e.Location.X;
            if (_mouseX < 0)
                _mouseX = 0;
            if (_mouseX > Width)
                _mouseX = Width;

            //if (distance(e.Location, _tbSpeed.Grab) < 6)
            //{
            //    Cursor = Cursors.Hand;
            //    m = "Speed";
            //    newttind = 1;
            //}
            if (distance(e.Location, _tbVolume.Grab) < 6)
            {
                Cursor = Cursors.Hand;
                m = "Volume";
                newttind = 2;
            }
            if (distance(e.Location, new Point(Width - 20, Height - TimelineHeight - 20)) < 20)
            {
                Cursor = Cursors.Hand;
                m = "Play/Pause";
                newttind = 3;
            }
            
            //if (_tbSpeed.Nav)
            //{
            //    _tbSpeed.CalcVal(e.Location);
            //    if (SpeedChanged != null)
            //        SpeedChanged(this, _tbSpeed.Val);

            //}
            if (_tbVolume.Nav)
            {
                _tbVolume.CalcVal(e.Location);
                VolumeChanged?.Invoke(this, _tbVolume.Val);
            }
            
            if (m != "" && newttind != _ttind)
            {
                _ttind = newttind;
                _toolTip.Show(m, this, e.Location, 1000);
            }
                
            RequestFrame = true;
        }

        public int VolumePercent => _tbVolume.Val;
        //public int SpeedPercent
        //{
        //    get { return _tbSpeed.Val; }
        //}

        private double distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        
        public void Init(FilesFile fileData)
        {
            _ff = fileData ?? new FilesFile {AlertData = "0"};

            _datapoints = _ff.AlertData.Split(',');
            
            Invalidate();
        }
    }

    public class TrackBarGdi
    {
        public bool Nav;
        public int W, H, X, Y, Val, InitVal;
        public Point Grab;
        public int Wgrab;
        public int SnapRange;
        public TrackBarGdi(int w, int h, int x, int y, int val, int snapRange)
        {
            W = w;
            H = h;
            X = x;
            Y = y;
            Val = val;
            InitVal = val;
            Grab = new Point(0,0);
            Nav = false;
            Wgrab = 7;
            SnapRange = snapRange;
        }

        public void CalcVal(Point mouseLocation)
        {
            int ox = mouseLocation.X - X;
            if (ox < 0)
                ox = 0;
            if (ox > W - Wgrab)
                ox = W - Wgrab;
            Val = Convert.ToInt32((Convert.ToDouble(ox) / Convert.ToDouble(W - Wgrab)) * 100);
            if (Val>InitVal-SnapRange && Val<InitVal+SnapRange)
                Reset();
        }

        private void Reset()
        {
            Val = InitVal;
        }
    }
}
