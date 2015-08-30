using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class VideoNavigator : UserControl
    {
        private bool _navTimeline;
        private FilesFile _ff;
        public event SeekEventHandler Seek;
        public delegate void SeekEventHandler(object sender, float percent);

        public VideoNavigator()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 5, 5);
            BorderStyle = BorderStyle.None;

            ResizeRedraw = true;
        }

        private int _value;
        private int _mouseX;
        public int Value
        {
            get { return _value; }
            set { _value = value;
                Invalidate();
            }
        }

        private void VideoNavigator_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (_ff != null)
            { 
                //draw scrolling graph
                var pxCursor = ((float)Value / 100) * Width;
                if (_navTimeline)
                    pxCursor = _mouseX;

                Graphics g = pe.Graphics;
                int w = Width;
                int xoff = 0;
                if (ActivityGraph.Width > w)
                {
                    w = ActivityGraph.Width;
                    double val = (Value / 100d);
                    if (_navTimeline)
                        val = Convert.ToDouble(pxCursor) / Width;
                    xoff = 0 - Convert.ToInt32(Convert.ToDouble(ActivityGraph.Width - Width) * val);
                }
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(ActivityGraph, xoff, 0, w, _timelineHeight);


                Brush bPosition = new SolidBrush(Color.Black);


                int x2 = (int)pxCursor - 4;
                int y2 = Height - _timelineHeight / 2;
                var navPoints = new[]
                    {
                        new Point(x2-4,y2-6), 
                        new Point(x2+4,y2),
                        new Point(x2-4,y2+6)
                    };

                g.FillPolygon(Brushes.White, navPoints);
                g.DrawPolygon(Pens.Black, navPoints);


                bPosition.Dispose();
            }

        }
        
        public void Init(FilesFile fileData)
        {
            _ff = fileData ?? new FilesFile { AlertData = "0" };
            if (_activityGraph != null)
            {
                lock (_activityGraph)
                {
                    _activityGraph.Dispose();
                    _activityGraph = null;
                }
            }

            _datapoints = _ff.AlertData.Split(',');
            _timelineHeight = Height;
            Invalidate();
        }

        private readonly object _aglock = new object();
        public void ReleaseGraph()
        {
            if (_activityGraph != null)
            {
                lock (_aglock)
                {
                    _activityGraph.Dispose();
                    _activityGraph = null;
                }
            }
        }

        public bool IsAudio;

        private string[] _datapoints;
        private int _timelineHeight = 30;
        //readonly SolidBrush _bTimeLine = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
        
        private Bitmap _activityGraph;
        private Bitmap ActivityGraph
        {
            get
            {
                if (_activityGraph == null)
                {
                    if (_datapoints == null)
                    {
                        lock (_aglock)
                        {
                            _activityGraph = new Bitmap(320, _timelineHeight);
                        }
                    }
                    else
                    {
                        
                        lock (_aglock)
                        {
                            _activityGraph = new Bitmap(_datapoints.Length, _timelineHeight);
                        }
                        Graphics gGraph = Graphics.FromImage(_activityGraph);
                        gGraph.Clear(ColorTranslator.FromHtml("#05AEE2"));
                        //gGraph.FillRectangle(_bTimeLine, 0, 0, _activityGraph.Width, _activityGraph.Height);
                        float dFact = 1;
                        if (IsAudio)
                            dFact = 100;

                        if (_ff != null && _datapoints.Length > 0)
                        {
                            var pAlarm = new Pen(Color.Red);
                            var pOk = new Pen(Color.Black);
                            var trigger = (float)_ff.TriggerLevel;
                            var triggermax = (float)100;
                            if (_ff.TriggerLevelMaxSpecified)
                            {
                                triggermax = ((float)_ff.TriggerLevelMax) * dFact;
                            }


                            for (int i = 0; i < _datapoints.Length; i++)
                            {
                                float d;
                                if (float.TryParse(_datapoints[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d))
                                {
                                    d = d * dFact;
                                    if (d >= trigger && d <= triggermax)
                                    {
                                        gGraph.DrawLine(pAlarm, i, _timelineHeight, i,
                                            _timelineHeight - Convert.ToInt32(d * (_timelineHeight / 100d)));
                                    }
                                    else
                                        gGraph.DrawLine(pOk, i, _timelineHeight, i, _timelineHeight - Convert.ToInt32(d * (_timelineHeight / 100d)));
                                }

                            }
                            gGraph.DrawLine(pOk, 0, _timelineHeight / 2, _activityGraph.Width, _timelineHeight / 2);
                            pOk.Dispose();
                            pAlarm.Dispose();
                        }
                        gGraph.Dispose();
                    }
                }
                return _activityGraph;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = Cursors.Default;
            if (e.Location.Y > Height - _timelineHeight)
            {
                Cursor = Cursors.Hand;
            }

            _mouseX = e.Location.X;
            if (_mouseX < 0)
                _mouseX = 0;
            if (_mouseX > Width)
                _mouseX = Width;
            Invalidate();
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            _navTimeline = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _navTimeline = false;
            if (e.Button == MouseButtons.Left)
            {
                var v = (float)e.Location.X;
                var val = (v / Width) * 100;
                Seek?.Invoke(this, val);
            }
        }

    }
}
