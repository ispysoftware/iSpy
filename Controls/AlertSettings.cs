using System;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class AlertSettings : Form
    {
        public objectsCameraAlerts CamalertSettings;
        public objectsMicrophoneAlerts MicalertSettings;
        public AlertSettings()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("Settings");
            button1.Text = LocRm.GetString("OK");
            label3.Text = LocRm.GetString("AlertGroup");
            label2.Text = LocRm.GetString("ResetAlertInterval");
            label1.Text = LocRm.GetString("DistinctAlertInterval");
            linkLabel1.Text = LocRm.GetString("Help");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CamalertSettings != null)
            {
                CamalertSettings.minimuminterval = (int) numDistinctInterval.Value;
                CamalertSettings.resetinterval = (int)numResetInterval.Value;
                CamalertSettings.groupname = cmbGroup.Text.Trim().ToLower();
            }
            if (MicalertSettings != null)
            {
                MicalertSettings.minimuminterval = (int)numDistinctInterval.Value;
                MicalertSettings.resetinterval = (int)numResetInterval.Value;
                MicalertSettings.groupname = cmbGroup.Text.Trim().ToLower();
            }
            Close();
        }

        private void AlertSettings_Load(object sender, EventArgs e)
        {
            if (CamalertSettings != null)
            {
                numDistinctInterval.Value = CamalertSettings.minimuminterval;
                numResetInterval.Value = CamalertSettings.resetinterval;
                cmbGroup.Text = CamalertSettings.groupname;
            }
            if (MicalertSettings != null)
            {
                numDistinctInterval.Value = MicalertSettings.minimuminterval;
                numResetInterval.Value = MicalertSettings.resetinterval;
                cmbGroup.Text = MicalertSettings.groupname;
            }

            foreach (var cam in MainForm.Cameras)
            {
                string gn = cam.alerts.groupname.Trim().ToLower();
                if (!string.IsNullOrEmpty(gn))
                {
                    if (!cmbGroup.Items.Contains(gn))
                        cmbGroup.Items.Add(gn);
                }

            }
            foreach (var mic in MainForm.Microphones)
            {
                string gn = mic.alerts.groupname.Trim().ToLower();
                if (!string.IsNullOrEmpty(gn))
                {
                    if (!cmbGroup.Items.Contains(gn))
                        cmbGroup.Items.Add(gn);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Webserver + "/userguide-alert-intervals.aspx");
        }
    }
}
