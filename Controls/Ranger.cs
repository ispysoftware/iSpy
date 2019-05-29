using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed partial class Ranger : UserControl
    {
        public double Maximum = 100;
        public double Minimum = 0;
        

        private double _valueMin = 20;
        private double _valueMax = 100;
        private float _gain = 10;

        public double ValueMin
        {
            get { return _valueMin; }
            set {
                if (value < Minimum)
                    value = Minimum;
                _valueMin = value;
                if (ValueMinChanged != null)
                    ValueMinChanged();
            }
        }
        public double ValueMax
        {
            get { return _valueMax; }
            set
            {
                if (value > Maximum)
                    value = Maximum;
                _valueMax = value;
                if (ValueMaxChanged != null)
                    ValueMaxChanged();
            }
        }
        public float Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                if (GainChanged != null)
                    GainChanged();
            }
        }

        public void SetText()
        {
            txtVal1.Text = ValueMin.ToString("0.###");
            txtVal2.Text = ValueMax.ToString("0.###");
            numGain.Value = (decimal) Gain;
            Refresh();
        }


        public Ranger()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Ranger_Load(object sender, EventArgs e)
        {
            label2.Text = LocRm.GetString("GainCamera");
        }

        private void Ranger_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        public delegate void ValueMinChangedDelegate();
        public event ValueMinChangedDelegate ValueMinChanged;

        public delegate void ValueMaxChangedDelegate();
        public event ValueMaxChangedDelegate ValueMaxChanged;

        public delegate void GainChangedDelegate();
        public event GainChangedDelegate GainChanged;

        

       
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int barWidth = Width - 16;
            
            //setup drawing objects
            var baroutline = new Pen(Color.FromArgb(255,214,214,214), 1.0f);
            var greenfill = new SolidBrush(Color.FromArgb(255, 38,255,92));
            var graboutline = new Pen(Color.FromArgb(255, 172, 172, 172));
            var grabBrush = new LinearGradientBrush(new Rectangle(0,0,11,20),Color.FromArgb(255,240,240,240),Color.FromArgb(255,229,229,229),270);
            var fill = new SolidBrush(Color.FromArgb(255, 231, 234, 234));

            //Fill Bar - red if zone is too small
            if (ValueMax - ValueMin < 1)
                fill.Color = Color.Red;
            g.FillRectangle(fill, 8, 8, barWidth, 4);
            
            //Fill Green Zone
            int x1 = 8 + Convert.ToInt32((Convert.ToDouble(barWidth) / (Maximum - Minimum)) * ValueMin);
            int x2 = 8 + Convert.ToInt32((Convert.ToDouble(barWidth) / (Maximum - Minimum)) * ValueMax);
            g.FillRectangle(greenfill, x1,8,x2-x1,4);

            //Draw Bar Outline
            g.DrawRectangle(baroutline, 8, 8, barWidth, 4);
            
            //Draw Scale
            double xStep = Convert.ToDouble(barWidth-8) / 50d;
            for (int i = 0; i <= 50; i++ )
            {
                var h = 3;
                if (i == 0 || i == 50)
                    h = 4;
                g.DrawLine(baroutline, 12 + Convert.ToInt32(i * xStep), 22, 12 + Convert.ToInt32(i * xStep), 22 + h);
            }

            //Draw Grabbers
            var gp1 = new GraphicsPath(FillMode.Winding);
            gp1.AddLines(new [] {
                                 new Point(x1-5,3), 
                                 new Point(x1+5,3), 
                                 new Point(x1+5,14),
                                 new Point(x1,19), 
                                 new Point(x1-5,14), 
                                 new Point(x1-5,3)
                             });

            g.FillPath(grabBrush, gp1);
            g.DrawPath(graboutline, gp1);

            var gp2 = new GraphicsPath(FillMode.Winding);
            gp2.AddLines(new [] {
                                 new Point(x2-5,3), 
                                 new Point(x2+5,3), 
                                 new Point(x2+5,14),
                                 new Point(x2,19), 
                                 new Point(x2-5,14), 
                                 new Point(x2-5,3)
                             });

            g.FillPath(grabBrush, gp2);
            g.DrawPath(graboutline, gp2);

            gp1.Dispose();
            gp2.Dispose();
            grabBrush.Dispose();
            baroutline.Dispose();
            fill.Dispose();
            graboutline.Dispose();
            greenfill.Dispose();

            base.OnPaint(e);
        }

        private enum GrabSelected
        {
            MinGrab,MaxGrab,None
        }

        private GrabSelected _currentGrab = GrabSelected.None;

        private void RangerMouseDown(object sender, MouseEventArgs e)
        {
            _currentGrab = GrabSelected.None;
            int barWidth = Width - 16;
            int x1 = 8 + Convert.ToInt32((Convert.ToDouble(barWidth) / (Maximum - Minimum)) * ValueMin);
            int x2 = 8 + Convert.ToInt32((Convert.ToDouble(barWidth) / (Maximum - Minimum)) * ValueMax);

            if (e.X > x1 - 5 && e.X < x1 + 5 && e.Y > 3 && e.Y < 19)
                _currentGrab = GrabSelected.MinGrab;
            else
            {
                if (e.X > x2 - 5 && e.X < x2 + 5 && e.Y > 3 && e.Y < 19)
                    _currentGrab = GrabSelected.MaxGrab;
            }

        }

        private void RangerMouseUp(object sender, MouseEventArgs e)
        {
            _currentGrab = GrabSelected.None;
        }


        private void RangerMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int barWidth = Width - 16;
                switch(_currentGrab)
                {
                    case GrabSelected.MinGrab:
                        ValueMin = (e.X - 8)/((Convert.ToDouble(barWidth)/(Maximum - Minimum)));
                        if (ValueMin < Minimum)
                            ValueMin = Minimum;
                        if (ValueMin > Maximum)
                            ValueMin = Maximum;
                            
                        if (ValueMin>ValueMax)
                        {
                            ValueMax = ValueMin;
                        }
                        break;
                    case GrabSelected.MaxGrab:
                        ValueMax = (e.X - 8)/((Convert.ToDouble(barWidth)/(Maximum - Minimum)));
                        if (ValueMax < Minimum)
                            ValueMax = Minimum;
                        if (ValueMax > Maximum)
                            ValueMax = Maximum;

                        if (ValueMax < ValueMin)
                        {
                            ValueMin = ValueMax;
                        }
                        break;
                }
                
                txtVal1.Text = ValueMin.ToString("0.###");
                txtVal2.Text = ValueMax.ToString("0.###");
                Refresh();
            }
            else
            {
                Cursor = Cursors.Default;
                int barWidth = Width - 16;
                int x1 = 8 + Convert.ToInt32((Convert.ToDouble(barWidth) / (Maximum - Minimum)) * ValueMin);
                int x2 = 8 + Convert.ToInt32((Convert.ToDouble(barWidth) / (Maximum - Minimum)) * ValueMax);
                if (e.X > x1 - 5 && e.X < x1 + 5 && e.Y > 3 && e.Y < 19)
                    Cursor = Cursors.Hand;
                else
                {
                    if (e.X > x2 - 5 && e.X < x2 + 5 && e.Y > 3 && e.Y < 19)
                        Cursor = Cursors.Hand;
                }
            }
        }

        private void txtVal1_TextChanged(object sender, EventArgs e)
        {
            double d;
            if (double.TryParse(txtVal1.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d))
            {
                if (Math.Abs(d - ValueMin) > double.Epsilon)
                {
                    if (d <= ValueMax && d >= Minimum)
                    {
                        ValueMin = d;
                        Refresh();
                    }
                }
            }
        }

        private void txtVal2_TextChanged(object sender, EventArgs e)
        {
            double d;
            if (double.TryParse(txtVal2.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d))
            {
                if (Math.Abs(d - ValueMax) > double.Epsilon)
                {
                    if (d >= ValueMin && d<=Maximum)
                    {
                        ValueMax = d;
                        Refresh();
                    }
                }
            }
        }

        private void txtVal2_Leave(object sender, EventArgs e)
        {
            txtVal2.Text = ValueMax.ToString("0.###");
        }

        private void txtVal1_Leave(object sender, EventArgs e)
        {
            txtVal1.Text = ValueMin.ToString("0.###");
        }

        private void numGain_ValueChanged(object sender, EventArgs e)
        {
            Gain = (float) numGain.Value;
        }

    }
}
