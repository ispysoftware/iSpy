using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class AddCommandButton : Form
    {
        public objectsCommand CMD;
        public SerializableFont CustomFont;
        public Color color, backColor;
        public Point location;

        public AddCommandButton()
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
                ValidateSize();
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
            foreach (var c in MainForm.RemoteCommands.Where(p=>!p.inwindow))
            {
                var n = c.name;
                if (n.StartsWith("cmd_"))
                    n = LocRm.GetString(n);
                ddlCommand.Items.Add(new MainForm.ListItem(n, c.id));
            }
            if (ddlCommand.Items.Count == 0)
            {
                MessageBox.Show(this, "No commands available. See remote commands in iSpy");
                Close();
            }

            if (location != Point.Empty)
            {
                locX.Value = location.X;
                locY.Value = location.Y;
            }

            color = Color.Black;
            backColor = Color.White;
            CustomFont = MainForm.Drawfont;
            ddlCommand.SelectedIndex = 0;

        }

        private void EditCommandButton_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CMD = MainForm.RemoteCommands.FirstOrDefault(p => p.id == (int)((MainForm.ListItem)ddlCommand.SelectedItem).Value);
            if (CMD != null)
            {
                CMD.font = CustomFont.SerializeFontAttribute;
                CMD.color = color.ToRGBString();
                CMD.backcolor = backColor.ToRGBString();
                CMD.size = numW.Value + "x" + numH.Value;
                CMD.location = locX.Value + "," + locY.Value;
                CMD.inwindow = true;
                DialogResult = DialogResult.OK;
            }
            Close();

        }

        private void ddlCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateSize();
        }

        private void ValidateSize()
        {
            var cmd =
               MainForm.RemoteCommands.FirstOrDefault(p => p.id == (int)((MainForm.ListItem)ddlCommand.SelectedItem).Value);

            if (cmd != null)
            {
                using (var g = CreateGraphics())
                {
                    var ts = g.MeasureString(cmd.name, CustomFont);
                    int w = Convert.ToInt32(ts.Width * 1.5);
                    int h = Convert.ToInt32(ts.Height * 1.5);
                    if (numW.Value < w) numW.Value = w;
                    if (numH.Value < h) numH.Value = h;
                }
            }
        }
    }
}
