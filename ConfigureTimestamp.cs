using System;
using System.Drawing;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class ConfigureTimestamp : Form
    {
        public int TimeStampLocation = 0;
        public decimal Offset = 0;
        public Color TimestampForeColor;
        public Color TimestampBackColor;
        public bool TimestampShowBack;
        public SerializableFont CustomFont;

        public ConfigureTimestamp()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("Configure");
            label1.Text = LocRm.GetString("Location");
            label3.Text = LocRm.GetString("Offset");
            label7.Text = LocRm.GetString("Font");
            label2.Text = LocRm.GetString("ForeColor");
            label6.Text = LocRm.GetString("BackColor");
            chkTimestampBack.Text = LocRm.GetString("ShowBackground");

            button1.Text = LocRm.GetString("OK");
        }

        private void ConfigureTimestamp_Load(object sender, EventArgs e)
        {
            ddlTimestampLocation.Items.AddRange(LocRm.GetString("TimeStampLocations").Split(','));
            ddlTimestampLocation.SelectedIndex = TimeStampLocation;
            numOffset.Value = Offset;
            chkTimestampBack.Checked = TimestampShowBack;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeStampLocation = ddlTimestampLocation.SelectedIndex;
            Offset = numOffset.Value;
            TimestampShowBack = chkTimestampBack.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var cd = new ColorDialog { Color = TimestampBackColor, AllowFullOpen = true, SolidColorOnly = false };
            if (cd.ShowDialog(this) == DialogResult.OK)
            {
                TimestampBackColor = cd.Color;
            }
            cd.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var fd = new FontDialog {ShowColor = false, Font = CustomFont.FontValue,Color = TimestampForeColor, ShowEffects = true};

            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                CustomFont = new SerializableFont(fd.Font);
                TimestampForeColor = fd.Color;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cd = new ColorDialog { Color = TimestampForeColor, AllowFullOpen = true, SolidColorOnly = false };
            if (cd.ShowDialog(this) == DialogResult.OK)
            {
                TimestampForeColor = cd.Color;
            }
            cd.Dispose();
        }
    }
}
