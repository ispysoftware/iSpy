using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed partial class ScheduleRow : UserControl
    {

        public new static int Height = 31;
        public objectsScheduleEntry Ose;

        public event EventHandler ScheduleEntryDelete;
        public event EventHandler ScheduleEntryEdit;
        public event EventHandler MouseOver;


        public ScheduleRow(objectsScheduleEntry ose)
        {
            Ose = ose;
            InitializeComponent();
            chkSummary.Text = GetSummary(ose);
            chkSummary.Checked = Ose.active;
            BackColor = DefaultBackColor;
        }

        private string GetSummary(objectsScheduleEntry se)
        {
            return FormatTime(se.time) + " \t" + FormatDays(se.daysofweek.Split(',')) + " \t" +
                       Helper.ScheduleDescription(se.typeid);
        }


        private static string FormatTime(int t)
        {
            var ts = TimeSpan.FromMinutes(t);
            string h = ts.Hours.ToString(CultureInfo.InvariantCulture);
            string m = ts.Minutes.ToString(CultureInfo.InvariantCulture);
            if (h.Length == 1)
                h = "0" + h;
            if (m.Length == 1)
                m = "0" + m;
            return h + ":" + m;
        }

        private static string FormatDays(IEnumerable<string> d)
        {
            string r = "";
            foreach (var day in d)
            {
                switch (day)
                {
                    case "1":
                        r += "Mon,";
                        break;
                    case "2":
                        r += "Tue,";
                        break;
                    case "3":
                        r += "Wed,";
                        break;
                    case "4":
                        r += "Thu,";
                        break;
                    case "5":
                        r += "Fri,";
                        break;
                    case "6":
                        r += "Sat,";
                        break;
                    case "0":
                        r += "Sun,";
                        break;
                }
            }
            return r.Trim(',');
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ScheduleEntryEdit?.Invoke(this, EventArgs.Empty);
            chkSummary.Text = GetSummary(Ose);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ScheduleEntryDelete?.Invoke(this, EventArgs.Empty);
        }

        private void tableLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(255, 221, 221, 221);
            MouseOver?.Invoke(this, EventArgs.Empty);
        }

        public void RevertBackground()
        {
            tableLayoutPanel1.BackColor = DefaultBackColor;
            Invalidate();
        }

        private void chkSummary_CheckedChanged(object sender, EventArgs e)
        {
            Ose.active = chkSummary.Checked;
        }

        private void ScheduleRow_Layout(object sender, LayoutEventArgs e)
        {
        }
    }
}
