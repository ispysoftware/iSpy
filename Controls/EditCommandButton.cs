using System;
using System.Drawing;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class EditCommandButton : Form
    {
        public objectsCommand CMD;
        public SerializableFont CustomFont;
        public Color color, backColor;

        public EditCommandButton()
        {
            InitializeComponent();
            RenderResources();

        }

        private void RenderResources()
        {
            Text = LocRm.GetString("Configure");
            label7.Text = LocRm.GetString("Font");
            label2.Text = LocRm.GetString("ForeColor");
            label6.Text = LocRm.GetString("BackColor");

            button1.Text = LocRm.GetString("OK");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var fd = new FontDialog { ShowColor = false, Font = CustomFont.FontValue, Color = color, ShowEffects = true };

            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                CustomFont = new SerializableFont(fd.Font);
                color = fd.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cd = new ColorDialog { Color = color, AllowFullOpen = true, SolidColorOnly = false };
            if (cd.ShowDialog(this) == DialogResult.OK)
            {
                color = cd.Color;
            }
            cd.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var cd = new ColorDialog { Color = backColor, AllowFullOpen = true, SolidColorOnly = false };
            if (cd.ShowDialog(this) == DialogResult.OK)
            {
                backColor = cd.Color;
            }
            cd.Dispose();
        }

        

        private void EditCommandButton_Load(object sender, EventArgs e)
        {
            
            CustomFont = FontXmlConverter.ConvertToFont(CMD.font);
            color = CMD.color.ToColor();
            backColor = CMD.backcolor.ToColor();
            var s = CMD.size.Split('x');
            numW.Value = Convert.ToInt32(s[0]);
            numH.Value = Convert.ToInt32(s[1]);
            var l = CMD.location.Split(',');
            locX.Value = Convert.ToInt32(l[0]);
            locY.Value = Convert.ToInt32(l[1]);

            var n = CMD.name;
            if (n.StartsWith("cmd_"))
                n = LocRm.GetString(n);
            lblCommand.Text = n;

        }

        private void EditCommandButton_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CMD.font = CustomFont.SerializeFontAttribute;
            CMD.color = color.ToRGBString();
            CMD.backcolor = backColor.ToRGBString();
            CMD.size = numW.Value + "x" + numH.Value;
            CMD.location = locX.Value + "," + locY.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CMD.inwindow = false;
            Close();
        }
    }
}
