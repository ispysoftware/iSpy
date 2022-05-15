using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class PTZScheduler : Form
    {
        internal CameraWindow CameraControl;
        public PTZScheduler()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            LocRm.SetString(button8, "Delete All");
            LocRm.SetString(button7, "Repeat");
            LocRm.SetString(chkSuspendOnMovement, "SuspendOnMovement");
            button6.Text = LocRm.GetString("Add");
            btnDeletePTZ.Text = LocRm.GetString("Delete");
            chkSchedulePTZ.Text = LocRm.GetString("Scheduler");
            Text = LocRm.GetString("Scheduler");
        }

        private void chkSchedulePTZ_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanel20.Enabled = chkSchedulePTZ.Checked;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (ddlScheduleCommand.SelectedIndex > -1)
            {
                if (ddlScheduleCommand.SelectedIndex > -1)
                {
                    var commandItem = (MainForm.ListItem)ddlScheduleCommand.SelectedItem;

                    var cmd = commandItem.ToString();
                    var tkn = commandItem.Value.ToString();
                    var time = dtpSchedulePTZ.Value;
                    var s = new objectsCameraPtzscheduleEntry { command = cmd, token = tkn,  time = time };
                    List<objectsCameraPtzscheduleEntry> scheds = CameraControl.Camobject.ptzschedule.entries.ToList();
                    scheds.Add(s);
                    CameraControl.Camobject.ptzschedule.entries = scheds.ToArray();
                    ShowPTZSchedule();
                }
            }
        }

        private void ShowPTZSchedule()
        {
            tableLayoutPanel20.Enabled = chkSchedulePTZ.Checked;

            lbPTZSchedule.Items.Clear();
            var s = CameraControl.Camobject.ptzschedule.entries.ToList().OrderBy(p => p.time).ToList();
            foreach (var ptzs in s)
            {
                lbPTZSchedule.Items.Add(ptzs.time.ToString("HH:mm:ss tt") + " " + ptzs.command);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = lbPTZSchedule.SelectedIndex;
            if (i > -1)
            {
                var s = CameraControl.Camobject.ptzschedule.entries.ToList().OrderBy(p => p.time).ToList();
                var si = s[i];
                var cr = new ConfigureRepeat { Interval = 60, Until = si.time };
                if (cr.ShowDialog(this) == DialogResult.OK)
                {
                    var dtUntil = cr.Until;
                    var dtCurrent = si.time.AddSeconds(cr.Interval);
                    while (dtCurrent.TimeOfDay < dtUntil.TimeOfDay)
                    {
                        s.Add(new objectsCameraPtzscheduleEntry { command = si.command, token = si.token, time = dtCurrent });
                        dtCurrent = dtCurrent.AddSeconds(cr.Interval);
                    }
                }
                cr.Dispose();
                CameraControl.Camobject.ptzschedule.entries = s.ToArray();
                ShowPTZSchedule();
            }
            else
            {
                MessageBox.Show(this, LocRm.GetString("SelectPTZRepeat"));
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CameraControl.Camobject.ptzschedule.entries = new objectsCameraPtzscheduleEntry[0];
            ShowPTZSchedule();
        }

        private void btnDeletePTZ_Click(object sender, EventArgs e)
        {
            int i = lbPTZSchedule.SelectedIndex;
            if (i > -1)
            {
                var s = CameraControl.Camobject.ptzschedule.entries.ToList().OrderBy(p => p.time).ToList();
                s.RemoveAt(i);
                CameraControl.Camobject.ptzschedule.entries = s.ToArray();
                ShowPTZSchedule();
            }
        }

        private void PTZScheduler_Load(object sender, EventArgs e)
        {
            chkSchedulePTZ.Checked = CameraControl.Camobject.ptzschedule.active;
            chkSuspendOnMovement.Checked = CameraControl.Camobject.ptzschedule.suspend;
            ShowPTZSchedule();
            dtpSchedulePTZ.Value = new DateTime(2012, 1, 1, 0, 0, 0, 0);

            if (CameraControl.Camobject.ptz > -1)
            {
                PTZSettings2Camera ptz = MainForm.PTZs.Single(p => p.id == CameraControl.Camobject.ptz);
                CameraControl.PTZ.PTZSettings = ptz;
                if (ptz.ExtendedCommands != null && ptz.ExtendedCommands.Command != null)
                {
                    foreach (var extcmd in ptz.ExtendedCommands.Command)
                    {
                        ddlScheduleCommand.Items.Add(new MainForm.ListItem(extcmd.Name, extcmd.Value));
                    }
                }
            }
            if (CameraControl.Camobject.ptz == -3 || CameraControl.Camobject.ptz == -4)
            {
                foreach (string cmd in PTZController.PelcoCommands)
                {
                    ddlScheduleCommand.Items.Add(new MainForm.ListItem(cmd, cmd));
                }

            }

            if (CameraControl.Camobject.ptz == -5)
            {
                foreach (var preset in CameraControl.PTZ.ONVIFPresets)
                    ddlScheduleCommand.Items.Add(new MainForm.ListItem(preset.Name, preset.token));
            }
            if (ddlScheduleCommand.Items.Count > 0)
                ddlScheduleCommand.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CameraControl.Camobject.ptzschedule.active = chkSchedulePTZ.Checked;
            CameraControl.Camobject.ptzschedule.suspend = chkSuspendOnMovement.Checked;
            Close();
        }
    }
}
