using System;
using System.Drawing;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class ConfigureTimestamp : Form
    {
        public int TimeStampLocation = 0;
        public decimal Offset = 0;
        public Color TimestampForeColor;
        public Color TimestampBackColor;
        public string TagsNV;
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
            button5.Text = LocRm.GetString("Edit");
            chkTimestampBack.Text = LocRm.GetString("ShowBackground");
            label5.Text = LocRm.GetString("Tags");
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

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (TagConfigure tc = new TagConfigure {TagsNV = TagsNV, Owner = this})
            {
                if (tc.ShowDialog() == DialogResult.OK)
                {
                    TagsNV = tc.TagsNV;
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            using (TagEditor te = new TagEditor())
            {
                te.ShowDialog(this);
            }
        }
    }
}
