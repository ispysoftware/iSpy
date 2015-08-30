using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class ConfigureRepeat : Form
    {
        public int Interval = 10;
        public DateTime Until = Helper.Now;

        public ConfigureRepeat()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            label48.Text = LocRm.GetString("Seconds");
            button1.Text = LocRm.GetString("OK");
            label1.Text = LocRm.GetString("Every");
            label2.Text = LocRm.GetString("Until");
            Text = LocRm.GetString("Repeat");

        }

        private void ForSecondsLoad(object sender, EventArgs e)
        {
            txtSeconds.Value = Interval;
            dtpSchedulePTZ.Value = Until;
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Interval = Convert.ToInt32(txtSeconds.Value);
            Until = dtpSchedulePTZ.Value;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
