using System;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class PTZTool : Form
    {
        private bool _loaded;
        private CameraWindow _cameraControl;
        public CameraWindow CameraControl
        {
            get { return _cameraControl; }
            set { 
                _cameraControl = value;
                _loaded = false;
                ddlExtended.Items.Clear();
                if (_cameraControl == null)
                {
                    ddlExtended.Items.Add(new ListItem(LocRm.GetString("ClickCamera"),""));
                    ddlExtended.SelectedIndex = 0;
                }

                pnlController.Enabled = false;
                if (value != null && value.IsEnabled)
                {
                    if (CameraControl.Camobject.ptz > -1)
                    {
                        ddlExtended.Items.Add(new ListItem(LocRm.GetString("SelectCommand"), ""));
                        PTZSettings2Camera ptz = MainForm.PTZs.Single(p => p.id == CameraControl.Camobject.ptz);
                        if (ptz.ExtendedCommands != null && ptz.ExtendedCommands.Command!=null)
                        {
                            foreach (var extcmd in ptz.ExtendedCommands.Command)
                            {
                                ddlExtended.Items.Add(new ListItem(extcmd.Name, extcmd.Value));
                            }
                        }
                        pnlController.Enabled = true;
                    }
                    else
                    {
                        switch (CameraControl.Camobject.ptz)    
                        {
                            case -1:
                                ddlExtended.Items.Add(new ListItem(LocRm.GetString("DigitalPTZonly"), ""));
                                pnlController.Enabled = true;
                                break;
                            case -2:
                                ddlExtended.Items.Add(new ListItem("IAM-Control", ""));
                                pnlController.Enabled = true;
                                break;
                            case -3:
                            case -4:
                                //Pelco extended
                                ddlExtended.Items.Add(new ListItem(LocRm.GetString("SelectCommand"), ""));
                                foreach(string cmd in PTZController.PelcoCommands)
                                {
                                    ddlExtended.Items.Add(new ListItem(cmd, cmd));
                                }
                                pnlController.Enabled = true;
                                break;
                            case -5:
                                //ONVIF
                                ddlExtended.Items.Add(new ListItem(LocRm.GetString("SelectCommand"), ""));
                                foreach(var cmd in CameraControl.PTZ.ONVIFPresets)
                                {
                                    ddlExtended.Items.Add(new ListItem(cmd.Name, cmd.token ));
                                }
                                pnlController.Enabled = true;
                                break;
                            case -6:
                                ddlExtended.Items.Add(new ListItem(LocRm.GetString("None"), ""));
                                pnlController.Enabled = false;
                                break;
                        }
                    }
                    Text = "PTZ: "+CameraControl.Camobject.name;
                    ptzui1.CameraControl = value;
                    if (ddlExtended.Items.Count>0)
                        ddlExtended.SelectedIndex = 0;
                }
                _loaded = true;
            }

        }

        public PTZTool()
        {
            InitializeComponent();
            
        }

        private void ddlExtended_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loaded && CameraControl!=null)
            {
                if (ddlExtended.SelectedIndex > 0)
                {
                    var li = ((ListItem) ddlExtended.SelectedItem);
                    CameraControl.PTZ.SendPTZCommand(li.Value);
                    ddlExtended.SelectedIndex = 0;
                }
            }
        }

        private struct ListItem
        {
            private readonly string _name;
            internal readonly string Value;

            public ListItem(string name, string value)
            {
                _name = name;
                Value = value;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        private void PTZTool_Load(object sender, EventArgs e)
        {
            Text = LocRm.GetString("PTZTool");
        }

        private void PTZTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            

        }
    }
}
