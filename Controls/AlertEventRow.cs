using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed partial class AlertEventRow : UserControl
    {
        public new static int Height = 31;
        public objectsActionsEntry Oae;

        public event EventHandler AlertEntryDelete;
        public event EventHandler AlertEntryEdit;
        public event EventHandler MouseOver;


        public AlertEventRow(objectsActionsEntry oae)
        {
            Oae = oae;
            InitializeComponent();
            chkSummary.Text = GetSummary(Oae.type, Oae.param1, Oae.param2, Oae.param3, Oae.param4);
            chkSummary.Checked = Oae.active;
            BackColor = DefaultBackColor;
        }

        private string GetSummary(string type, string param1, string param2, string param3, string param4)
        {
            string t= "Unknown";
            switch (type)
            {
                case "Exe":
                    t = LocRm.GetString("ExecuteFile") + ": " + param1;
                    break;
                case "URL":
                    t = LocRm.GetString("CallURL")+": " + param1;
                    if (Convert.ToBoolean(param2))
                        t += " (POST grab)";
                    break;
                case "NM":
                    t = param1 + " " + param2 + ":" + param3 + " (" + param4 + ")";
                    break;
                case "S":
                    t = LocRm.GetString("PlaySound")+": " + param1;
                    break;
                case "ATC":
                    t = LocRm.GetString("SoundThroughCamera") + ": " + param1;
                    break;
                case "SW":
                    t = LocRm.GetString("ShowWindow");
                    pbEdit.Visible = false;
                    break;
                case "B":
                    t = LocRm.GetString("Beep");
                    pbEdit.Visible = false;
                    break;
                case "M":
                    t = LocRm.GetString("Maximise");
                    pbEdit.Visible = false;
                    break;
                case "MO":
                    t = LocRm.GetString("SwitchMonitorOn");
                    pbEdit.Visible = false;
                    break;
                case "TA":
                    {
                        string[] op = param1.Split(',');
                        string n = "[removed]";
                        int id = Convert.ToInt32(op[1]);
                        switch (op[0])
                        {
                            case "1":
                                var om = MainForm.Microphones.FirstOrDefault(p => p.id == id);
                                if (om != null)
                                    n = om.name;
                                break;
                            case "2":
                                var oc = MainForm.Cameras.FirstOrDefault(p => p.id == id);
                                if (oc != null)
                                    n = oc.name;
                                break;
                        }
                        t = LocRm.GetString("TriggerAlertOn")+" " + n;
                    }
                    break;
                case "SOO":
                    {
                        string[] op = param1.Split(',');
                        string n = "[removed]";
                        int id = Convert.ToInt32(op[1]);
                        switch (op[0])
                        {
                            case "1":
                                var om = MainForm.Microphones.FirstOrDefault(p => p.id == id);
                                if (om != null)
                                    n = om.name;
                                break;
                            case "2":
                                var oc = MainForm.Cameras.FirstOrDefault(p => p.id == id);
                                if (oc != null)
                                    n = oc.name;
                                break;
                        }
                        t = LocRm.GetString("SwitchObjectOn")+" " + n;
                    }
                    break;
                case "SOF":
                    {
                        string[] op = param1.Split(',');
                        string n = "[removed]";
                        int id = Convert.ToInt32(op[1]);
                        switch (op[0])
                        {
                            case "1":
                                var om = MainForm.Microphones.FirstOrDefault(p => p.id == id);
                                if (om != null)
                                    n = om.name;
                                break;
                            case "2":
                                var oc = MainForm.Cameras.FirstOrDefault(p => p.id == id);
                                if (oc != null)
                                    n = oc.name;
                                break;
                        }
                        t = LocRm.GetString("SwitchObjectOff") + " " + n;
                    }
                    break;
                case "E":
                    t = LocRm.GetString("SendEmail")+": " + param1;
                    if (param2!="" && Convert.ToBoolean(param2))
                        t += " (include grab)";
                    break;
                case "SMS":
                    t = LocRm.GetString("SendSMS")+": " + param1;
                    break;
                case "TM":
                    t = LocRm.GetString("SendTwitterMessage");
                    pbEdit.Visible = false;
                    break;
            }
            //if (t.Length > 50)
            //    t = t.Substring(0, 47) + "...";
            return t;
        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (AlertEntryEdit != null)
                AlertEntryEdit(this, EventArgs.Empty);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (AlertEntryDelete != null)
                AlertEntryDelete(this, EventArgs.Empty);
        }

        private void tableLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(255, 221, 221, 221);
            if (MouseOver != null)
                MouseOver(this, EventArgs.Empty);
        }

        public void RevertBackground()
        {
            tableLayoutPanel1.BackColor = DefaultBackColor;
            Invalidate();
        }

        private void chkSummary_CheckedChanged(object sender, EventArgs e)
        {
            Oae.active = chkSummary.Checked;
        }
    }
}
