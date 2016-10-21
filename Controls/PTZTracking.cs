﻿using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class PTZTracking : Form
    {
        public CameraWindow CameraControl;

        public PTZTracking()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            rdoAny.Text = LocRm.GetString("AnyDirection");
            rdoVert.Text = LocRm.GetString("VertOnly");
            rdoHor.Text = LocRm.GetString("HorOnly"); 
            label5.Text = LocRm.GetString("homedelay"); 
            chkAutoHome.Text = LocRm.GetString("AutoHome");
            label87.Text = LocRm.GetString("TimeToHome");
            chkTrack.Text = LocRm.GetString("TrackObjects"); 
            label59.Text = LocRm.GetString("Command");
            Text = LocRm.GetString("TrackObjects"); 
        }

        private void PTZTracking_Load(object sender, EventArgs e)
        {
            chkReverseTracking.Checked = CameraControl.Camobject.settings.ptzautotrackreverse;
            chkTrack.Checked = CameraControl.Camobject.settings.ptzautotrack;
            chkAutoHome.Checked = CameraControl.Camobject.settings.ptzautohome;
            //chkCRF.Checked = CameraControl.Camobject.recorder.crf;
            numTTH.Value = CameraControl.Camobject.settings.ptztimetohome;
            pnlTrack.Enabled = chkTrack.Checked;

            rdoAny.Checked = CameraControl.Camobject.settings.ptzautotrackmode == 0;
            rdoVert.Checked = CameraControl.Camobject.settings.ptzautotrackmode == 1;
            rdoHor.Checked = CameraControl.Camobject.settings.ptzautotrackmode == 2;
            numAutoHomeDelay.Value = CameraControl.Camobject.settings.ptzautohomedelay;

            ddlHomeCommand.Items.Add(new MainForm.ListItem3("Center", "Center"));

            if (CameraControl.Camobject.ptz > -1)
            {
                PTZSettings2Camera ptz = MainForm.PTZs.Single(p => p.id == CameraControl.Camobject.ptz);
                CameraControl.PTZ.PTZSettings = ptz;
                if (ptz.ExtendedCommands != null && ptz.ExtendedCommands.Command != null)
                {
                    string subMenu = "", PTZ_SUBMENU_START = "  ";
                    foreach (var extcmd in ptz.ExtendedCommands.Command)
                    {
                        if ((extcmd.Value ?? "") != "")
                        {
                            ddlHomeCommand.Items.Add(new MainForm.ListItem3(subMenu + extcmd.Name, extcmd.Value));
                            if (CameraControl.Camobject.settings.ptzautohomecommand == extcmd.Value)
                            {
                                ddlHomeCommand.SelectedIndex = ddlHomeCommand.Items.Count - 1;
                            }
                        }
                        else if ((extcmd.Name ?? MainForm.PTZ_SUBMENU_END) != MainForm.PTZ_SUBMENU_END)
                        {
                            ddlHomeCommand.Items.Add(new MainForm.ListItem3(subMenu + extcmd.Name + MainForm.PTZ_SUBMENU_NAME_SUFFIX, extcmd.Value));
                            subMenu = subMenu + PTZ_SUBMENU_START;
                        }
                        else
                        {
                            subMenu = subMenu.Substring(Math.Min(PTZ_SUBMENU_START.Length, subMenu.Length));
                        }
                    }
                }
            }
            if (CameraControl.Camobject.ptz == -3 || CameraControl.Camobject.ptz == -4)
            {
                foreach (string cmd in PTZController.PelcoCommands)
                {
                    ddlHomeCommand.Items.Add(new MainForm.ListItem3(cmd, cmd));
                    if (CameraControl.Camobject.settings.ptzautohomecommand == cmd)
                    {
                        ddlHomeCommand.SelectedIndex = ddlHomeCommand.Items.Count - 1;
                    }
                }

            }

            if (CameraControl.Camobject.ptz == -5)
            {
                ddlHomeCommand.Items.Clear();
                foreach (string cmd in CameraControl.PTZ.ONVIFPresets)
                {
                    ddlHomeCommand.Items.Add(new MainForm.ListItem3(cmd, cmd));
                    if (CameraControl.Camobject.settings.ptzautohomecommand == cmd)
                    {
                        ddlHomeCommand.SelectedIndex = ddlHomeCommand.Items.Count - 1;
                    }
                }        
            }

            if (ddlHomeCommand.SelectedIndex == -1 && ddlHomeCommand.Items.Count > 0)
            {
                ddlHomeCommand.SelectedIndex = 0;
            }
            tableLayoutPanel1.Enabled = chkAutoHome.Checked;
        }

        private void chkTrack_CheckedChanged(object sender, EventArgs e)
        {
            pnlTrack.Enabled = chkTrack.Checked;
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CameraControl.Camobject.settings.ptzautohomedelay = (int)numAutoHomeDelay.Value;
            CameraControl.Camobject.settings.ptzautotrack = chkTrack.Checked;
            CameraControl.Camobject.settings.ptzautohome = chkAutoHome.Checked;
            CameraControl.Camobject.settings.ptzautotrackmode = 0;

            if (rdoVert.Checked)
                CameraControl.Camobject.settings.ptzautotrackmode = 1;
            if (rdoHor.Checked)
                CameraControl.Camobject.settings.ptzautotrackmode = 2;

            CameraControl.Camobject.settings.ptztimetohome = Convert.ToInt32(numTTH.Value);

            if (ddlHomeCommand.SelectedIndex > -1)
            {
                var li = ((MainForm.ListItem3)ddlHomeCommand.SelectedItem);
                CameraControl.Camobject.settings.ptzautohomecommand = li.Value;
            }

            if (chkTrack.Checked)
            {
                CameraControl.Camobject.settings.ptzautotrack = true;
                CameraControl.Camobject.detector.highlight = false;

            }
            CameraControl.Camobject.settings.ptzautotrackreverse = chkReverseTracking.Checked;

            Close();
        }

        private void ddlHomeCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void chkAutoHome_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanel1.Enabled = chkAutoHome.Checked;
        }
    }
}
