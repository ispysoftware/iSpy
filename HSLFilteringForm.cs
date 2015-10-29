// Image Processing Lab
// http://www.aforgenet.com/projects/iplab/
//
// Copyright © Andrew Kirillov, 2005-2009
// andrew.kirillov@aforgenet.com
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using iSpyApplication.Controls;
using PictureBox = iSpyApplication.Controls.PictureBox;

namespace iSpyApplication
{
    /// <summary>
    /// Summary description for HSLFilteringForm.
    /// </summary>
    public class HSLFilteringForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private readonly HSLFiltering _filter = new HSLFiltering();
        private Button _cancelButton;
        private int _fillH;
        private TextBox _fillHBox;
        private float _fillL;
        private TextBox _fillLBox;
        private float _fillS;
        private TextBox _fillSBox;
        private ComboBox _fillTypeCombo;
        private PictureBox _filterPreview;

        private GroupBox _groupBox1;
        private GroupBox _groupBox2;
        private GroupBox _groupBox3;
        private GroupBox _groupBox4;
        private GroupBox _groupBox5;
        private IntRange _hue = new IntRange(0, 359);
        private Label _label1;
        private Label _label10;
        private Label _label2;
        private Label _label3;
        private Label _label4;
        private Label _label5;
        private Label _label6;
        private Label _label7;
        private Label _label8;
        private Label _label9;
        private Range _luminance = new Range(0, 1);
        private ColorSlider _luminanceSlider;
        private TextBox _maxHBox;
        private TextBox _maxLBox;
        private TextBox _maxSBox;
        private TextBox _minHBox;
        private TextBox _minLBox;
        private TextBox _minSBox;
        private Button _okButton;
        private Range _saturation = new Range(0, 1);
        private ColorSlider _saturationSlider;
        private CheckBox _updateHCheck;
        private CheckBox _updateLCheck;
        private CheckBox _updateSCheck;
        private HuePicker _huePicker;
        private LinkLabel llblHelp;
    
        public string Configuration
        {
            get { 
                string ret = "";
                ret += _hue.Min + "|" + _hue.Max + "|" + _fillH+"|";
                ret += String.Format(CultureInfo.InvariantCulture,"{0:0.000}", _saturation.Min)+"|";
                ret += String.Format(CultureInfo.InvariantCulture,"{0:0.000}", _saturation.Max) + "|";
                ret += String.Format(CultureInfo.InvariantCulture,"{0:0.000}", _fillS) + "|";
                ret += String.Format(CultureInfo.InvariantCulture,"{0:0.000}", _luminance.Min) + "|";
                ret += String.Format(CultureInfo.InvariantCulture,"{0:0.000}", +_luminance.Max) + "|";
                ret +=String.Format(CultureInfo.InvariantCulture,"{0:0.000}", _fillL) + "|";
                ret += _fillTypeCombo.SelectedIndex + "|" + _filter.UpdateHue.ToString().ToLower() + "|";

                ret += _filter.UpdateSaturation.ToString().ToLower() + "|" + _filter.UpdateLuminance.ToString().ToLower();
                return ret;
            }
        }

        private T ParseValue<T>(string valueString)
        {
            return (T)Convert.ChangeType(
            valueString.Replace(',', '.'),
            typeof(T),
            CultureInfo.InvariantCulture);
        }


        private Bitmap _imageprocess;
        private static readonly object SyncLock = new object();

        public Bitmap ImageProcess
        {
            get
            {
                return _imageprocess;
            }
            set
            {
                if (value!=null)
                {
                    lock(SyncLock)
                    {
                        var rz = new ResizeBilinear(_filterPreview.Width, _filterPreview.Height);
                        _imageprocess = rz.Apply(value);  
                    }
                    UpdateFilter();
                }
            }
        }

        public HSLFilteringForm(string Config)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            if (!string.IsNullOrEmpty(Config))
            {
                string[] config = Config.Split(Config.IndexOf("|", StringComparison.Ordinal)!=-1 ? '|' : ',');

                _hue.Min = Convert.ToInt32(config[0]);
                _hue.Max = Convert.ToInt32(config[1]);
                _fillH = Convert.ToInt32(config[2]);

                _saturation.Min = ParseValue<float>(config[3]);
                _saturation.Max = ParseValue<float>(config[4]);
                _fillS = ParseValue<float>(config[5]);

                _luminance.Min = ParseValue<float>(config[6]);
                _luminance.Max = ParseValue<float>(config[7]);
                _fillL = ParseValue<float>(config[8]);

                _fillTypeCombo.SelectedIndex = Convert.ToInt32(config[9]);
                _filter.UpdateHue = Convert.ToBoolean(config[10]);
                _filter.UpdateSaturation = Convert.ToBoolean(config[11]);
                _filter.UpdateLuminance = Convert.ToBoolean(config[12]);
            }
            else
                _fillTypeCombo.SelectedIndex = 0;


            //
            _minHBox.Text = _hue.Min.ToString(CultureInfo.InvariantCulture);
            _maxHBox.Text = _hue.Max.ToString(CultureInfo.InvariantCulture);
            _fillHBox.Text = _fillH.ToString(CultureInfo.InvariantCulture);

            _minSBox.Text = _saturation.Min.ToString("F3");
            _maxSBox.Text = _saturation.Max.ToString("F3");
            _fillSBox.Text = _fillS.ToString("F3");

            _minLBox.Text = _luminance.Min.ToString("F3");
            _maxLBox.Text = _luminance.Max.ToString("F3");
            _fillLBox.Text = _fillL.ToString("F3");

            

            _updateHCheck.Checked = _filter.UpdateHue;
            _updateSCheck.Checked = _filter.UpdateSaturation;
            _updateLCheck.Checked = _filter.UpdateLuminance;

            RenderResources();
            //filterPreview.Filter = filter;
        }

        // Image property
        public Bitmap Image
        {
            set { _filterPreview.Image = value; }
        }

        // Filter property
        public IFilter Filter => _filter;

        // Constructor

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        // Update filter
        private void UpdateFilter()
        {
            _filter.Hue = _hue;
            _filter.Saturation = _saturation;
            _filter.Luminance = _luminance;

            lock (SyncLock)
            {
                if (ImageProcess != null)
                {
                    _filterPreview.Image?.Dispose();

                    _filterPreview.Image = _filter.Apply(ImageProcess);
                    _filterPreview.Invalidate();
                }
            }
        }

        // Min hue changed
        private void minHBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _huePicker.Min = _hue.Min = Math.Max(0, Math.Min(359, int.Parse(_minHBox.Text)));
                UpdateFilter();
            }
            catch (Exception)
            {
            }
        }

        // Max hue changed
        private void maxHBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _huePicker.Max = _hue.Max = Math.Max(0, Math.Min(359, int.Parse(_maxHBox.Text)));
                UpdateFilter();
            }
            catch (Exception)
            {
            }
        }

        // Min saturation changed
        private void minSBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _saturation.Min = ParseValue<float>(_minSBox.Text);
                _saturationSlider.Min = (int) (_saturation.Min*255);
                UpdateFilter();
            }
            catch (Exception)
            {
            }
        }

        // Max saturation changed
        private void maxSBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _saturation.Max = ParseValue<float>(_maxSBox.Text);
                _saturationSlider.Max = (int) (_saturation.Max*255);
                UpdateFilter();
            }
            catch (Exception)
            {
            }
        }

        // Min luminance changed
        private void minLBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _luminance.Min = ParseValue<float>(_minLBox.Text);
                _luminanceSlider.Min = (int) (_luminance.Min*255);
                UpdateFilter();
            }
            catch (Exception)
            {
            }
        }

        // Max luminance changed
        private void maxLBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _luminance.Max = ParseValue<float>(_maxLBox.Text);
                _luminanceSlider.Max = (int) (_luminance.Max*255);
                UpdateFilter();
            }
            catch (Exception)
            {
            }
        }

        // Hue picker changed
        private void huePicker_ValuesChanged(object sender, EventArgs e)
        {
            _minHBox.Text = _huePicker.Min.ToString();
            _maxHBox.Text = _huePicker.Max.ToString();
        }

        // Saturation slider changed
        private void saturationSlider_ValuesChanged(object sender, EventArgs e)
        {
            _minSBox.Text = ((double) _saturationSlider.Min/255).ToString("F3");
            _maxSBox.Text = ((double) _saturationSlider.Max/255).ToString("F3");
        }

        // Luminance slider changed
        private void luminanceSlider_ValuesChanged(object sender, EventArgs e)
        {
            _minLBox.Text = ((double) _luminanceSlider.Min/255).ToString("F3");
            _maxLBox.Text = ((double) _luminanceSlider.Max/255).ToString("F3");
        }

        // Fill hue changed
        private void fillHBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _fillH = int.Parse(_fillHBox.Text);
                UpdateFillColor();
            }
            catch (Exception)
            {
            }
        }

        // Fill saturation changed
        private void fillSBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _fillS = ParseValue<float>(_fillSBox.Text);
                UpdateFillColor();
            }
            catch (Exception)
            {
            }
        }

        // Fill luminance changed
        private void fillLBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _fillL = ParseValue<float>(_fillLBox.Text);
                UpdateFillColor();
            }
            catch (Exception)
            {
            }
        }

        // Update fill color
        private void UpdateFillColor()
        {
            var v = (int) (_fillS*255);
            _saturationSlider.FillColor = Color.FromArgb(v, v, v);
            v = (int) (_fillL*255);
            _luminanceSlider.FillColor = Color.FromArgb(v, v, v);


            _filter.FillColor = new HSL(_fillH, _fillS, _fillL);
            UpdateFilter();
        }

        // Update Hue check clicked
        private void updateHCheck_CheckedChanged(object sender, EventArgs e)
        {
            _filter.UpdateHue = _updateHCheck.Checked;
            UpdateFilter();
        }

        // Update Saturation check clicked
        private void updateSCheck_CheckedChanged(object sender, EventArgs e)
        {
            _filter.UpdateSaturation = _updateSCheck.Checked;
            UpdateFilter();
        }

        // Update Luminance check clicked
        private void updateLCheck_CheckedChanged(object sender, EventArgs e)
        {
            _filter.UpdateLuminance = _updateLCheck.Checked;
            UpdateFilter();
        }

        // Fill type changed
        private void fillTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var types =
                new[]
                    {
                        ColorSlider.ColorSliderType.InnerGradient,
                        ColorSlider.ColorSliderType.OuterGradient
                    };
            ColorSlider.ColorSliderType type = types[_fillTypeCombo.SelectedIndex];

            _saturationSlider.Type = type;
            _luminanceSlider.Type = type;

            _filter.FillOutsideRange = (_fillTypeCombo.SelectedIndex == 0);
            UpdateFilter();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._groupBox1 = new System.Windows.Forms.GroupBox();
            this._maxHBox = new System.Windows.Forms.TextBox();
            this._label2 = new System.Windows.Forms.Label();
            this._minHBox = new System.Windows.Forms.TextBox();
            this._label1 = new System.Windows.Forms.Label();
            this._huePicker = new HuePicker();
            this._groupBox2 = new System.Windows.Forms.GroupBox();
            this._saturationSlider = new ColorSlider();
            this._maxSBox = new System.Windows.Forms.TextBox();
            this._minSBox = new System.Windows.Forms.TextBox();
            this._label4 = new System.Windows.Forms.Label();
            this._label3 = new System.Windows.Forms.Label();
            this._groupBox3 = new System.Windows.Forms.GroupBox();
            this._luminanceSlider = new ColorSlider();
            this._maxLBox = new System.Windows.Forms.TextBox();
            this._minLBox = new System.Windows.Forms.TextBox();
            this._label5 = new System.Windows.Forms.Label();
            this._label6 = new System.Windows.Forms.Label();
            this._groupBox5 = new System.Windows.Forms.GroupBox();
            this._filterPreview = new PictureBox();
            this._groupBox4 = new System.Windows.Forms.GroupBox();
            this._updateLCheck = new System.Windows.Forms.CheckBox();
            this._fillLBox = new System.Windows.Forms.TextBox();
            this._label9 = new System.Windows.Forms.Label();
            this._updateSCheck = new System.Windows.Forms.CheckBox();
            this._fillSBox = new System.Windows.Forms.TextBox();
            this._label8 = new System.Windows.Forms.Label();
            this._updateHCheck = new System.Windows.Forms.CheckBox();
            this._fillHBox = new System.Windows.Forms.TextBox();
            this._label7 = new System.Windows.Forms.Label();
            this._fillTypeCombo = new System.Windows.Forms.ComboBox();
            this._label10 = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this.llblHelp = new System.Windows.Forms.LinkLabel();
            this._groupBox1.SuspendLayout();
            this._groupBox2.SuspendLayout();
            this._groupBox3.SuspendLayout();
            this._groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._filterPreview)).BeginInit();
            this._groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupBox1
            // 
            this._groupBox1.Controls.Add(this._maxHBox);
            this._groupBox1.Controls.Add(this._label2);
            this._groupBox1.Controls.Add(this._minHBox);
            this._groupBox1.Controls.Add(this._label1);
            this._groupBox1.Controls.Add(this._huePicker);
            this._groupBox1.Location = new System.Drawing.Point(10, 10);
            this._groupBox1.Name = "_groupBox1";
            this._groupBox1.Size = new System.Drawing.Size(280, 230);
            this._groupBox1.TabIndex = 1;
            this._groupBox1.TabStop = false;
            this._groupBox1.Text = "Hue";
            // 
            // _maxHBox
            // 
            this._maxHBox.Location = new System.Drawing.Point(218, 20);
            this._maxHBox.Name = "_maxHBox";
            this._maxHBox.Size = new System.Drawing.Size(50, 20);
            this._maxHBox.TabIndex = 4;
            this._maxHBox.TextChanged += new System.EventHandler(this.maxHBox_TextChanged);
            // 
            // _label2
            // 
            this._label2.Location = new System.Drawing.Point(186, 23);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(39, 15);
            this._label2.TabIndex = 3;
            this._label2.Text = "Max:";
            // 
            // _minHBox
            // 
            this._minHBox.Location = new System.Drawing.Point(40, 20);
            this._minHBox.Name = "_minHBox";
            this._minHBox.Size = new System.Drawing.Size(50, 20);
            this._minHBox.TabIndex = 2;
            this._minHBox.TextChanged += new System.EventHandler(this.minHBox_TextChanged);
            // 
            // _label1
            // 
            this._label1.Location = new System.Drawing.Point(10, 23);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(31, 17);
            this._label1.TabIndex = 1;
            this._label1.Text = "Min:";
            // 
            // _huePicker
            // 
            this._huePicker.Location = new System.Drawing.Point(53, 50);
            this._huePicker.Name = "_huePicker";
            this._huePicker.Size = new System.Drawing.Size(170, 170);
            this._huePicker.TabIndex = 0;
            this._huePicker.Type = HuePicker.HuePickerType.Range;
            this._huePicker.ValuesChanged += new System.EventHandler(this.huePicker_ValuesChanged);
            this._huePicker.Click += new System.EventHandler(this._huePicker_Click);
            // 
            // _groupBox2
            // 
            this._groupBox2.Controls.Add(this._saturationSlider);
            this._groupBox2.Controls.Add(this._maxSBox);
            this._groupBox2.Controls.Add(this._minSBox);
            this._groupBox2.Controls.Add(this._label4);
            this._groupBox2.Controls.Add(this._label3);
            this._groupBox2.Location = new System.Drawing.Point(10, 245);
            this._groupBox2.Name = "_groupBox2";
            this._groupBox2.Size = new System.Drawing.Size(280, 75);
            this._groupBox2.TabIndex = 2;
            this._groupBox2.TabStop = false;
            this._groupBox2.Text = "Saturation";
            // 
            // _saturationSlider
            // 
            this._saturationSlider.Location = new System.Drawing.Point(8, 45);
            this._saturationSlider.Name = "_saturationSlider";
            this._saturationSlider.Size = new System.Drawing.Size(262, 23);
            this._saturationSlider.TabIndex = 4;
            this._saturationSlider.Type = ColorSlider.ColorSliderType.InnerGradient;
            this._saturationSlider.ValuesChanged += new System.EventHandler(this.saturationSlider_ValuesChanged);
            // 
            // _maxSBox
            // 
            this._maxSBox.Location = new System.Drawing.Point(218, 20);
            this._maxSBox.Name = "_maxSBox";
            this._maxSBox.Size = new System.Drawing.Size(50, 20);
            this._maxSBox.TabIndex = 3;
            this._maxSBox.TextChanged += new System.EventHandler(this.maxSBox_TextChanged);
            // 
            // _minSBox
            // 
            this._minSBox.Location = new System.Drawing.Point(40, 20);
            this._minSBox.Name = "_minSBox";
            this._minSBox.Size = new System.Drawing.Size(50, 20);
            this._minSBox.TabIndex = 2;
            this._minSBox.TextChanged += new System.EventHandler(this.minSBox_TextChanged);
            // 
            // _label4
            // 
            this._label4.Location = new System.Drawing.Point(186, 23);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(30, 17);
            this._label4.TabIndex = 1;
            this._label4.Text = "Max:";
            // 
            // _label3
            // 
            this._label3.Location = new System.Drawing.Point(10, 23);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(30, 16);
            this._label3.TabIndex = 0;
            this._label3.Text = "Min:";
            // 
            // _groupBox3
            // 
            this._groupBox3.Controls.Add(this._luminanceSlider);
            this._groupBox3.Controls.Add(this._maxLBox);
            this._groupBox3.Controls.Add(this._minLBox);
            this._groupBox3.Controls.Add(this._label5);
            this._groupBox3.Controls.Add(this._label6);
            this._groupBox3.Location = new System.Drawing.Point(10, 325);
            this._groupBox3.Name = "_groupBox3";
            this._groupBox3.Size = new System.Drawing.Size(280, 75);
            this._groupBox3.TabIndex = 3;
            this._groupBox3.TabStop = false;
            this._groupBox3.Text = "Luminance";
            // 
            // _luminanceSlider
            // 
            this._luminanceSlider.Location = new System.Drawing.Point(8, 45);
            this._luminanceSlider.Name = "_luminanceSlider";
            this._luminanceSlider.Size = new System.Drawing.Size(262, 23);
            this._luminanceSlider.TabIndex = 9;
            this._luminanceSlider.Type = ColorSlider.ColorSliderType.InnerGradient;
            this._luminanceSlider.ValuesChanged += new System.EventHandler(this.luminanceSlider_ValuesChanged);
            // 
            // _maxLBox
            // 
            this._maxLBox.Location = new System.Drawing.Point(218, 20);
            this._maxLBox.Name = "_maxLBox";
            this._maxLBox.Size = new System.Drawing.Size(50, 20);
            this._maxLBox.TabIndex = 8;
            this._maxLBox.TextChanged += new System.EventHandler(this.maxLBox_TextChanged);
            // 
            // _minLBox
            // 
            this._minLBox.Location = new System.Drawing.Point(40, 20);
            this._minLBox.Name = "_minLBox";
            this._minLBox.Size = new System.Drawing.Size(50, 20);
            this._minLBox.TabIndex = 7;
            this._minLBox.TextChanged += new System.EventHandler(this.minLBox_TextChanged);
            // 
            // _label5
            // 
            this._label5.Location = new System.Drawing.Point(186, 23);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(30, 17);
            this._label5.TabIndex = 6;
            this._label5.Text = "Max:";
            // 
            // _label6
            // 
            this._label6.Location = new System.Drawing.Point(10, 23);
            this._label6.Name = "_label6";
            this._label6.Size = new System.Drawing.Size(30, 16);
            this._label6.TabIndex = 5;
            this._label6.Text = "Min:";
            // 
            // _groupBox5
            // 
            this._groupBox5.Controls.Add(this._filterPreview);
            this._groupBox5.Location = new System.Drawing.Point(300, 10);
            this._groupBox5.Name = "_groupBox5";
            this._groupBox5.Size = new System.Drawing.Size(322, 230);
            this._groupBox5.TabIndex = 4;
            this._groupBox5.TabStop = false;
            this._groupBox5.Text = "Detector View";
            // 
            // _filterPreview
            // 
            this._filterPreview.Image = null;
            this._filterPreview.Location = new System.Drawing.Point(10, 15);
            this._filterPreview.Name = "_filterPreview";
            this._filterPreview.Size = new System.Drawing.Size(306, 205);
            this._filterPreview.TabIndex = 0;
            this._filterPreview.TabStop = false;
            this._filterPreview.Click += new System.EventHandler(this._filterPreview_Click);
            // 
            // _groupBox4
            // 
            this._groupBox4.Controls.Add(this._updateLCheck);
            this._groupBox4.Controls.Add(this._fillLBox);
            this._groupBox4.Controls.Add(this._label9);
            this._groupBox4.Controls.Add(this._updateSCheck);
            this._groupBox4.Controls.Add(this._fillSBox);
            this._groupBox4.Controls.Add(this._label8);
            this._groupBox4.Controls.Add(this._updateHCheck);
            this._groupBox4.Controls.Add(this._fillHBox);
            this._groupBox4.Controls.Add(this._label7);
            this._groupBox4.Location = new System.Drawing.Point(300, 245);
            this._groupBox4.Name = "_groupBox4";
            this._groupBox4.Size = new System.Drawing.Size(170, 100);
            this._groupBox4.TabIndex = 5;
            this._groupBox4.TabStop = false;
            this._groupBox4.Text = "Fill Color";
            // 
            // _updateLCheck
            // 
            this._updateLCheck.Checked = true;
            this._updateLCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this._updateLCheck.Location = new System.Drawing.Point(125, 70);
            this._updateLCheck.Name = "_updateLCheck";
            this._updateLCheck.Size = new System.Drawing.Size(14, 24);
            this._updateLCheck.TabIndex = 8;
            this._updateLCheck.CheckedChanged += new System.EventHandler(this.updateLCheck_CheckedChanged);
            // 
            // _fillLBox
            // 
            this._fillLBox.Location = new System.Drawing.Point(40, 70);
            this._fillLBox.Name = "_fillLBox";
            this._fillLBox.Size = new System.Drawing.Size(50, 20);
            this._fillLBox.TabIndex = 7;
            this._fillLBox.TextChanged += new System.EventHandler(this.fillLBox_TextChanged);
            // 
            // _label9
            // 
            this._label9.Location = new System.Drawing.Point(10, 73);
            this._label9.Name = "_label9";
            this._label9.Size = new System.Drawing.Size(20, 16);
            this._label9.TabIndex = 6;
            this._label9.Text = "L:";
            // 
            // _updateSCheck
            // 
            this._updateSCheck.Checked = true;
            this._updateSCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this._updateSCheck.Location = new System.Drawing.Point(125, 45);
            this._updateSCheck.Name = "_updateSCheck";
            this._updateSCheck.Size = new System.Drawing.Size(14, 24);
            this._updateSCheck.TabIndex = 5;
            this._updateSCheck.CheckedChanged += new System.EventHandler(this.updateSCheck_CheckedChanged);
            // 
            // _fillSBox
            // 
            this._fillSBox.Location = new System.Drawing.Point(40, 45);
            this._fillSBox.Name = "_fillSBox";
            this._fillSBox.Size = new System.Drawing.Size(50, 20);
            this._fillSBox.TabIndex = 4;
            this._fillSBox.TextChanged += new System.EventHandler(this.fillSBox_TextChanged);
            // 
            // _label8
            // 
            this._label8.Location = new System.Drawing.Point(10, 48);
            this._label8.Name = "_label8";
            this._label8.Size = new System.Drawing.Size(20, 16);
            this._label8.TabIndex = 3;
            this._label8.Text = "S:";
            // 
            // _updateHCheck
            // 
            this._updateHCheck.Checked = true;
            this._updateHCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this._updateHCheck.Location = new System.Drawing.Point(125, 20);
            this._updateHCheck.Name = "_updateHCheck";
            this._updateHCheck.Size = new System.Drawing.Size(14, 24);
            this._updateHCheck.TabIndex = 2;
            this._updateHCheck.CheckedChanged += new System.EventHandler(this.updateHCheck_CheckedChanged);
            // 
            // _fillHBox
            // 
            this._fillHBox.Location = new System.Drawing.Point(40, 20);
            this._fillHBox.Name = "_fillHBox";
            this._fillHBox.Size = new System.Drawing.Size(50, 20);
            this._fillHBox.TabIndex = 1;
            this._fillHBox.TextChanged += new System.EventHandler(this.fillHBox_TextChanged);
            // 
            // _label7
            // 
            this._label7.Location = new System.Drawing.Point(10, 23);
            this._label7.Name = "_label7";
            this._label7.Size = new System.Drawing.Size(20, 16);
            this._label7.TabIndex = 0;
            this._label7.Text = "H:";
            // 
            // _fillTypeCombo
            // 
            this._fillTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._fillTypeCombo.Items.AddRange(new object[] {
            "Outside",
            "Inside"});
            this._fillTypeCombo.Location = new System.Drawing.Point(300, 379);
            this._fillTypeCombo.Name = "_fillTypeCombo";
            this._fillTypeCombo.Size = new System.Drawing.Size(170, 21);
            this._fillTypeCombo.TabIndex = 10;
            this._fillTypeCombo.SelectedIndexChanged += new System.EventHandler(this.fillTypeCombo_SelectedIndexChanged);
            // 
            // _label10
            // 
            this._label10.AutoSize = true;
            this._label10.Location = new System.Drawing.Point(297, 352);
            this._label10.Name = "_label10";
            this._label10.Size = new System.Drawing.Size(45, 13);
            this._label10.TabIndex = 13;
            this._label10.Text = "Fill type:";
            // 
            // _cancelButton
            // 
            this._cancelButton.AutoSize = true;
            this._cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cancelButton.Location = new System.Drawing.Point(570, 407);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(52, 25);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            // 
            // _okButton
            // 
            this._okButton.AutoSize = true;
            this._okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._okButton.Location = new System.Drawing.Point(531, 407);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(33, 25);
            this._okButton.TabIndex = 11;
            this._okButton.Text = "Ok";
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // llblHelp
            // 
            this.llblHelp.AutoSize = true;
            this.llblHelp.Location = new System.Drawing.Point(476, 413);
            this.llblHelp.Name = "llblHelp";
            this.llblHelp.Size = new System.Drawing.Size(29, 13);
            this.llblHelp.TabIndex = 64;
            this.llblHelp.TabStop = true;
            this.llblHelp.Text = "Help";
            this.llblHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblHelp_LinkClicked);
            // 
            // HSLFilteringForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(634, 439);
            this.Controls.Add(this.llblHelp);
            this.Controls.Add(this._fillTypeCombo);
            this.Controls.Add(this._label10);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._groupBox4);
            this.Controls.Add(this._groupBox5);
            this.Controls.Add(this._groupBox3);
            this.Controls.Add(this._groupBox2);
            this.Controls.Add(this._groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSLFilteringForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HSL Filtering";
            this.Load += new System.EventHandler(this.HSLFilteringForm_Load);
            this._groupBox1.ResumeLayout(false);
            this._groupBox1.PerformLayout();
            this._groupBox2.ResumeLayout(false);
            this._groupBox2.PerformLayout();
            this._groupBox3.ResumeLayout(false);
            this._groupBox3.PerformLayout();
            this._groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._filterPreview)).EndInit();
            this._groupBox4.ResumeLayout(false);
            this._groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void HSLFilteringForm_Load(object sender, EventArgs e)
        {
            
            UpdateFilter();
        }

        private void RenderResources()
        {
            _groupBox5.Text = LocRm.GetString("DetectorView");
            _groupBox4.Text = LocRm.GetString("FillColor");
            _groupBox5.Text = LocRm.GetString("DetectorView");
            _label10.Text  = LocRm.GetString("FillType");
            _okButton.Text = LocRm.GetString("OK");
            _cancelButton.Text = LocRm.GetString("Cancel");
            llblHelp.Text = LocRm.GetString("help");
        }

        private void _okButton_Click(object sender, EventArgs e)
        {

        }

        private void _huePicker_Click(object sender, EventArgs e)
        {

        }

        private void llblHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/userguide-motion-detection.aspx#4");
        }

        private void _filterPreview_Click(object sender, EventArgs e)
        {

        }
    }
}