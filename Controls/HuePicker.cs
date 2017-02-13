using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AForge.Imaging;

namespace iSpyApplication.Controls
{
    /// <summary>
    /// Hue picker control.
    /// </summary>
    /// 
    /// <remarks><para>The control allows selecting hue value (or range) from HSL color space. Hue values
    /// are integer values in the [0, 359] range.</para>
    /// 
    /// <para>If control's type is set to <see cref="HuePickerType.Value"/>, then it allows selecting single
    /// hue value and looks like this:<br />
    /// <img src="img/controls/hue_picker1.png" width="220" height="220" />
    /// </para>
    /// 
    /// <para>If control's type is set to <see cref="HuePickerType.Range"/>, then it allows selecting range
    /// of hue values and looks like this:<br />
    /// <img src="img/controls/hue_picker2.png" width="220" height="220" />
    /// </para>
    /// </remarks>
    /// 
    public class HuePicker : Control
    {
        private HuePickerType _type = HuePickerType.Value;

        private readonly Pen _blackPen;
        private readonly Brush _blackBrush;
        private readonly Pen _whitePen;
        private readonly Brush _whiteBrush;

        private Point _ptCenter = new Point(0, 0);
        private Point _ptMin = new Point(0, 0);
        private Point _ptMax = new Point(0, 0);
        private int _trackMode;

        private int _min;
        private int _max = 359;

        /// <summary>
        /// An event, to notify about changes of <see cref="Min"/> or <see cref="Max"/> properties.
        /// </summary>
        /// 
        /// <remarks><para>The event is fired after changes of its <see cref="Value"/>, <see cref="Min"/> or
        /// <see cref="Max"/> properties, which is caused by user dragging the corresponding hue picker's bullets.</para>
        /// </remarks>
        /// 
        public event EventHandler ValuesChanged;

        /// <summary>
        /// Enumeration of hue picker types.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>The <see cref="HuePickerType.Value"/> type provides single bullet to drag, which allows
        /// selecting single hue value. The value is accessible through <see cref="Value"/> property.</para>
        /// 
        /// <para>The <see cref="HuePickerType.Range"/> type provides two bullets to drag, which correspond
        /// to minimum and maximum values of the hue range. These values are accessible through
        /// <see cref="Min"/> and <see cref="Max"/> properties.</para>
        /// </remarks>
        /// 
        public enum HuePickerType
        {
            /// <summary>
            /// Selecting single hue value.
            /// </summary>
            Value,

            /// <summary>
            /// Selecting hue values range.
            /// </summary>
            Range
        }

        /// <summary>
        /// Selected value of the hue picker control in <see cref="HuePickerType.Value"/> mode.
        /// </summary>
        [DefaultValue(0)]
        public int Value
        {
            get { return _min; }
            set
            {
                if (_type == HuePickerType.Value)
                {
                    _min = Math.Max(0, Math.Min(359, value));
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Minimum selected value of the hue picker control in <see cref="HuePickerType.Range"/> mode.
        /// </summary>
        [DefaultValue(0)]
        public int Min
        {
            get { return _min; }
            set
            {
                if (_type == HuePickerType.Range)
                {
                    _min = Math.Max(0, Math.Min(359, value));
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Maximum selected value of the hue picker control in <see cref="HuePickerType.Range"/> mode.
        /// </summary>
        [DefaultValue(359)]
        public int Max
        {
            get { return _max; }
            set
            {
                if (_type == HuePickerType.Range)
                {
                    _max = Math.Max(0, Math.Min(359, value));
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Current type of the hue picker control.
        /// </summary>
        /// 
        /// <remarks><para>See <see cref="HuePickerType"/> enumeration for description of the available types.</para></remarks>
        /// 
        [DefaultValue(HuePickerType.Value)]
        public HuePickerType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HuePicker"/> class.
        /// </summary>
        /// 
        public HuePicker()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);

            _blackPen = new Pen(Color.Black, 1);
            _blackBrush = new SolidBrush(Color.Black);
            _whitePen = new Pen(Color.White, 1);
            _whiteBrush = new SolidBrush(Color.White);
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// 
        /// <param name="disposing">Specifies if disposing was invoked by user's code.</param>
        /// 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _blackPen.Dispose();
                _blackBrush.Dispose();
                _whitePen.Dispose();
                _whiteBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        // Init component
        private void InitializeComponent()
        {
            // 
            // HSLPicker
            // 
            MouseUp += HSLPickerMouseUp;
            MouseMove += HSLPickerMouseMove;
            MouseDown += HSLPickerMouseDown;

        }

        /// <summary>
        /// Paint the controls.
        /// </summary>
        /// 
        /// <param name="pe">Paint event arguments.</param>
        /// 
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            Rectangle rc = ClientRectangle;
            Brush brush;
            RGB rgb = new RGB();
            HSL hsl = new HSL();

            // get pie rectangle
            var rcPie = new Rectangle(4, 4, Math.Min(rc.Right, rc.Bottom) - 8, Math.Min(rc.Right, rc.Bottom) - 8);

            // init HSL value
            hsl.Luminance = 0.5f;
            hsl.Saturation = 1.0f;

            if (_type == HuePickerType.Value)
            {
                // draw HSL pie
                for (int i = 0; i < 360; i++)
                {
                    hsl.Hue = i;
                    // convert from HSL to RGB
                    HSL.ToRGB(hsl, rgb);
                    // create brush
                    brush = new SolidBrush(rgb.Color);
                    // draw one hue value
                    g.FillPie(brush, rcPie, -i, -1);

                    brush.Dispose();
                }
            }
            else
            {
                // draw HSL pie
                for (var i = 0; i < 360; i++)
                {
                    if (
                        ((_min < _max) && (i >= _min) && (i <= _max)) ||
                        ((_min > _max) && ((i >= _min) || (i <= _max))))
                    {
                        hsl.Hue = i;
                        // convert from HSL to RGB
                        HSL.ToRGB(hsl, rgb);
                        // create brush
                        brush = new SolidBrush(rgb.Color);
                    }
                    else
                    {
                        brush = new SolidBrush(Color.FromArgb(128, 128, 128));
                    }

                    // draw one hue value
                    g.FillPie(brush, rcPie, -i, -1);

                    brush.Dispose();
                }
            }

            //
            double halfWidth = (double) rcPie.Width/2;
            double angleRad = -_min*Math.PI/180;
            double angleCos = Math.Cos(angleRad);
            double angleSin = Math.Sin(angleRad);

            double x = halfWidth*angleCos;
            double y = halfWidth*angleSin;

            _ptCenter.X = rcPie.Left + (int) (halfWidth);
            _ptCenter.Y = rcPie.Top + (int) (halfWidth);
            _ptMin.X = rcPie.Left + (int) (halfWidth + x);
            _ptMin.Y = rcPie.Top + (int) (halfWidth + y);

            // draw MIN pointer
            g.FillEllipse(_blackBrush,
                rcPie.Left + (int) (halfWidth + x) - 4,
                rcPie.Top + (int) (halfWidth + y) - 4,
                8, 8);
            g.DrawLine(_blackPen, _ptCenter, _ptMin);

            // check picker type
            if (_type == HuePickerType.Range)
            {
                angleRad = -_max*Math.PI/180;
                angleCos = Math.Cos(angleRad);
                angleSin = Math.Sin(angleRad);

                x = halfWidth*angleCos;
                y = halfWidth*angleSin;

                _ptMax.X = rcPie.Left + (int) (halfWidth + x);
                _ptMax.Y = rcPie.Top + (int) (halfWidth + y);

                // draw MAX pointer
                g.FillEllipse(_whiteBrush,
                    rcPie.Left + (int) (halfWidth + x) - 4,
                    rcPie.Top + (int) (halfWidth + y) - 4,
                    8, 8);
                g.DrawLine(_whitePen, _ptCenter, _ptMax);
            }

            base.OnPaint(pe);
        }

        // On mouse down
        private void HSLPickerMouseDown(object sender, MouseEventArgs e)
        {
            // check coordinates of MIN pointer
            if ((e.X >= _ptMin.X - 4) && (e.Y >= _ptMin.Y - 4) &&
                (e.X < _ptMin.X + 4) && (e.Y < _ptMin.Y + 4))
            {
                _trackMode = 1;
            }
            if (_type == HuePickerType.Range)
            {
                // check coordinates of MAX pointer
                if ((e.X >= _ptMax.X - 4) && (e.Y >= _ptMax.Y - 4) &&
                    (e.X < _ptMax.X + 4) && (e.Y < _ptMax.Y + 4))
                {
                    _trackMode = 2;
                }
            }

            if (_trackMode != 0)
                Capture = true;
        }

        // On mouse up
        private void HSLPickerMouseUp(object sender, MouseEventArgs e)
        {
            if (_trackMode == 0) return;
            // release capture
            Capture = false;
            _trackMode = 0;

            // notify client
            ValuesChanged?.Invoke(this, new EventArgs());
        }

        // On mouse move
        private void HSLPickerMouseMove(object sender, MouseEventArgs e)
        {
            Cursor cursor = Cursors.Default;

            if (_trackMode != 0)
            {
                cursor = Cursors.Hand;

                int dy = e.Y - _ptCenter.Y;
                int dx = e.X - _ptCenter.X;

                if (_trackMode == 1)
                {
                    // MIN pointer tracking
                    _min = (int) (Math.Atan2(-dy, dx)*180/Math.PI);
                    if (_min < 0)
                    {
                        _min = 360 + _min;
                    }
                }
                else
                {
                    // MAX pointer tracking
                    _max = (int) (Math.Atan2(-dy, dx)*180/Math.PI);
                    if (_max < 0)
                    {
                        _max = 360 + _max;
                    }
                }

                // repaint control
                Invalidate();
            }
            else
            {
                // check coordinates of MIN pointer
                if ((e.X >= _ptMin.X - 4) && (e.Y >= _ptMin.Y - 4) &&
                    (e.X < _ptMin.X + 4) && (e.Y < _ptMin.Y + 4))
                {
                    cursor = Cursors.Hand;
                }
                if (_type == HuePickerType.Range)
                {
                    // check coordinates of MAX pointer
                    if ((e.X >= _ptMax.X - 4) && (e.Y >= _ptMax.Y - 4) &&
                        (e.X < _ptMax.X + 4) && (e.Y < _ptMax.Y + 4))
                    {
                        cursor = Cursors.Hand;
                    }
                }

            }

            Cursor = cursor;
        }
    }
}