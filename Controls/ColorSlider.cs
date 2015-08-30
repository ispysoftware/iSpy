using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using iSpyApplication.Properties;

namespace iSpyApplication.Controls
{
    public class ColorSlider : Control
    {
        private readonly Pen _blackPen = new Pen(Color.Black, 1);
        private Color _startColor = Color.Black;
        private Color _endColor = Color.White;
        private Color _fillColor = Color.Black;
        private ColorSliderType _type = ColorSliderType.Gradient;
        private bool _doubleArrow = true;
        private readonly Bitmap _arrow;
        private int _min, _max = 255;
        private readonly int width = 256;
        private readonly int height = 10;
        private int _trackMode;
        private int _dx;

        /// <summary>
        /// An event, to notify about changes of <see cref="Min"/> or <see cref="Max"/> properties.
        /// </summary>
        /// 
        /// <remarks><para>The event is fired after changes of <see cref="Min"/> or <see cref="Max"/> property,
        /// which is caused by user dragging the corresponding control’s arrow (slider).</para>
        /// </remarks>
        /// 
        public event EventHandler ValuesChanged;

        public enum ColorSliderType
        {
            /// <summary>
            /// Gradient color slider type.
            /// </summary>
            Gradient,

            /// <summary>
            /// Inner gradient color slider type.
            /// </summary>
            InnerGradient,

            /// <summary>
            /// Outer gradient color slider type.
            /// </summary>
            OuterGradient,

            /// <summary>
            /// Threshold color slider type.
            /// </summary>
            Threshold
        }

        /// <summary>
        /// Start color for gradient filling.
        /// </summary>
        ///
        /// <remarks>See documentation to <see cref="ColorSliderType"/> enumeration for information about
        /// the usage of this property.</remarks>
        ///
        [DefaultValue(typeof(Color), "Black")]
        public Color StartColor
        {
            get { return _startColor; }
            set
            {
                _startColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// End color for gradient filling.
        /// </summary>
        ///
        /// <remarks>See documentation to <see cref="ColorSliderType"/> enumeration for information about
        /// the usage of this property.</remarks>
        ///
        [DefaultValue(typeof(Color), "White")]
        public Color EndColor
        {
            get { return _endColor; }
            set
            {
                _endColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Color to fill control's background in filtered zones.
        /// </summary>
        ///
        /// <remarks>See documentation to <see cref="ColorSliderType"/> enumeration for information about
        /// the usage of this property.</remarks>
        ///
        [DefaultValue(typeof(Color), "Black")]
        public Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Specifies control's type.
        /// </summary>
        /// 
        /// <remarks>See documentation to <see cref="ColorSliderType"/> enumeration for information about
        /// the usage of this property.</remarks>
        ///
        [DefaultValue(ColorSliderType.Gradient)]
        public ColorSliderType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if ((_type != ColorSliderType.Gradient) && (_type != ColorSliderType.Threshold))
                    DoubleArrow = true;
                Invalidate();
            }
        }

        /// <summary>
        /// Minimum selected value, [0, 255].
        /// </summary>
        /// 
        [DefaultValue(0)]
        public int Min
        {
            get { return _min; }
            set
            {
                _min = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Maximum selected value, [0, 255].
        /// </summary>
        /// 
        [DefaultValue(255)]
        public int Max
        {
            get { return _max; }
            set
            {
                _max = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Single or Double arrow slider control.
        /// </summary>
        /// 
        /// <remarks><para>The property specifies if the slider has one or two selection arrows (sliders).</para>
        /// 
        /// <para>The single arrow allows only to specify one value, which is set by <see cref="Min"/>
        /// property. The single arrow slider is useful for applications, where it is required to select
        /// color threshold, for example.</para>
        /// 
        /// <para>The double arrow allows to specify two values, which are set by <see cref="Min"/>
        /// and <see cref="Max"/> properties. The double arrow slider is useful for applications, where it is
        /// required to select filtering color range, for example.</para>
        /// </remarks>
        /// 
        [DefaultValue(true)]
        public bool DoubleArrow
        {
            get { return _doubleArrow; }
            set
            {
                _doubleArrow = value;
                if ((!_doubleArrow) && (_type != ColorSliderType.Threshold))
                {
                    Type = ColorSliderType.Gradient;
                }
                Invalidate();
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSlider"/> class.
        /// </summary>
        /// 
        public ColorSlider()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);

            _arrow = new Bitmap(Resources.arrow);
            _arrow.MakeTransparent(Color.FromArgb(255, 255, 255));
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
                _arrow.Dispose();
            }
            base.Dispose(disposing);
        }

        // Init component
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // ColorSlider
            // 
            Paint += ColorSliderPaint;
            MouseMove += ColorSliderMouseMove;
            MouseDown += ColorSliderMouseDown;
            MouseUp += ColorSliderMouseUp;
            ResumeLayout(false);

        }

        // Paint control
        private void ColorSliderPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = ClientRectangle;
            Brush brush;
            int x = (rc.Right - width) / 2;
            int y = 2;

            // draw rectangle around the control
            g.DrawRectangle(_blackPen, x - 1, y - 1, width + 1, height + 1);

            switch (_type)
            {
                case ColorSliderType.Gradient:
                case ColorSliderType.InnerGradient:
                case ColorSliderType.OuterGradient:

                    // create gradient brush
                    brush = new LinearGradientBrush(new Point(x, 0), new Point(x + width, 0), _startColor, _endColor);
                    g.FillRectangle(brush, x, y, width, height);
                    brush.Dispose();

                    // check type
                    if (_type == ColorSliderType.InnerGradient)
                    {
                        // inner gradient
                        brush = new SolidBrush(_fillColor);

                        if (_min != 0)
                        {
                            g.FillRectangle(brush, x, y, _min, height);
                        }
                        if (_max != 255)
                        {
                            g.FillRectangle(brush, x + _max + 1, y, 255 - _max, height);
                        }
                        brush.Dispose();
                    }
                    else if (_type == ColorSliderType.OuterGradient)
                    {
                        // outer gradient
                        brush = new SolidBrush(_fillColor);
                        // fill space between min & max with color 3
                        g.FillRectangle(brush, x + _min, y, _max - _min + 1, height);
                        brush.Dispose();
                    }
                    break;
                case ColorSliderType.Threshold:
                    // 1 - fill with color 1
                    brush = new SolidBrush(_startColor);
                    g.FillRectangle(brush, x, y, width, height);
                    brush.Dispose();
                    // 2 - fill space between min & max with color 2
                    brush = new SolidBrush(_endColor);
                    g.FillRectangle(brush, x + _min, y, _max - _min + 1, height);
                    brush.Dispose();
                    break;
            }


            // draw arrows
            x -= 4;
            y += 1 + height;

            g.DrawImage(_arrow, x + _min, y, 9, 6);
            if (_doubleArrow)
                g.DrawImage(_arrow, x + _max, y, 9, 6);
        }

        // On mouse down
        private void ColorSliderMouseDown(object sender, MouseEventArgs e)
        {
            int x = (ClientRectangle.Right - width) / 2 - 4;
            int y = 3 + height;

            // check Y coordinate
            if ((e.Y >= y) && (e.Y < y + 6))
            {
                // check X coordinate
                if ((e.X >= x + _min) && (e.X < x + _min + 9))
                {
                    // left arrow
                    _trackMode = 1;
                    _dx = e.X - _min;
                }
                if ((_doubleArrow) && (e.X >= x + _max) && (e.X < x + _max + 9))
                {
                    // right arrow
                    _trackMode = 2;
                    _dx = e.X - _max;
                }

                if (_trackMode != 0)
                    Capture = true;
            }
        }

        // On mouse up
        private void ColorSliderMouseUp(object sender, MouseEventArgs e)
        {
            if (_trackMode == 0) return;
            // release capture
            Capture = false;
            _trackMode = 0;

            // notify client
            ValuesChanged?.Invoke(this, new EventArgs());
        }

        // On mouse move
        private void ColorSliderMouseMove(object sender, MouseEventArgs e)
        {
            if (_trackMode != 0)
            {
                if (_trackMode == 1)
                {
                    // left arrow tracking
                    _min = e.X - _dx;
                    _min = Math.Max(_min, 0);
                    _min = Math.Min(_min, _max);
                }
                if (_trackMode == 2)
                {
                    // right arrow tracking
                    _max = e.X - _dx;
                    _max = Math.Max(_max, _min);
                    _max = Math.Min(_max, 255);
                }

                // repaint control
                Invalidate();
            }
        }
    }
}
