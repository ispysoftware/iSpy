using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class PTZCommandButtons : Form
    {
        private bool _loaded;
        private CameraWindow _cameraControl;

        private bool _shouldDisplay = false;
        public bool ShouldDisplay
        {
            get { return _shouldDisplay; }
            
        }

        public CameraWindow CameraControl
        {
            get { return _cameraControl; }
            set { 
                _cameraControl = value;
                _loaded = false;

                this.Size = new System.Drawing.Size(20, 110);
                this.pnlController.Size = new System.Drawing.Size(20, 110);
                this.pnlController.Controls.Clear();
                


                pnlController.Enabled = false;
                if (value != null && value.IsEnabled)
                {
                    if (CameraControl.Camobject.ptz > -1)
                    {
                        
                        PTZSettings2Camera ptz = MainForm.PTZs.Single(p => p.id == CameraControl.Camobject.ptz);
                        pnlController.Enabled = true;
                    }
                    else
                    {
                        switch (CameraControl.Camobject.ptz)    
                        {
                            case -1:
                                //ddlExtended.Items.Add(new ListItem(LocRm.GetString("DigitalPTZonly"), ""));
                                pnlController.Enabled = true;
                                break;
                            case -2:
                                //ddlExtended.Items.Add(new ListItem("IAM-Control", ""));
                                pnlController.Enabled = true;
                                break;
                            case -3:
                            case -4:
                                //Pelco extended
                                //ddlExtended.Items.Add(new ListItem(LocRm.GetString("SelectCommand"), ""));
                                //foreach(string cmd in PTZController.PelcoCommands)
                                //{
                                //    ddlExtended.Items.Add(new ListItem(cmd, cmd));
                                //}
                                pnlController.Enabled = true;
                                break;
                            case -5:
                                //ONVIF
                                //ddlExtended.Items.Add(new ListItem(LocRm.GetString("SelectCommand"), ""));
                                this.Size = new System.Drawing.Size(25, 110);
                                this.pnlController.Size = new System.Drawing.Size(25, 110);
                                int basex = this.pnlController.Location.X + 5;
                                int basey = this.pnlController.Location.Y;
                                foreach (var cmd in CameraControl.PTZ.ONVIFPresets)
                                {
                                    //ddlExtended.Items.Add(new ListItem(cmd.Name, cmd.token));

                                    this.Size = new System.Drawing.Size(this.pnlController.Size.Width + 133, 110);
                                    this.pnlController.Size = new System.Drawing.Size(this.pnlController.Size.Width + 133, 110);

                                    Button b = new Button();
                                    b.Left = basex;
                                    b.Top = basey;
                                    b.Size = new System.Drawing.Size(100, 50); 
                                    b.Name = cmd.token;
                                    b.Text = cmd.Name;
                                    b.Click += new EventHandler(b_dynamicButton_Click);
                                    this.pnlController.Controls.Add(b);
                                    
                                    basex += 110;
                                    
                                }
                                if (CameraControl.PTZ.ONVIFPresets.Count() > 0)
                                { 
                                    pnlController.Enabled = true;
                                    _shouldDisplay = true;
                                }
                                else
                                {
                                    pnlController.Enabled = false;
                                    _shouldDisplay = false;
                                }
                                break;
                            case -6:
                                //ddlExtended.Items.Add(new ListItem(LocRm.GetString("None"), ""));
                                pnlController.Enabled = false;
                                break;
                        }
                    }
                    Text = "PTZ: "+CameraControl.Camobject.name;
                }
                _loaded = true;
            }

        }

        public PTZCommandButtons()
        {
            InitializeComponent();
            
        }

        private void b_dynamicButton_Click(object sender, EventArgs e)
        {
            if (_loaded && CameraControl != null)
            {
                Button presetButton = (Button)sender;
                CameraControl.PTZ.SendPTZCommand(presetButton.Name);
                this.pnlController.Focus(); //return the focus to the panel, so the onlostfocus and on gotfocus events shall be fired
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

        private void PTZCommandButtons_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            Text = LocRm.GetString("PTZCommandButtons");
            this.ControlBox = false;
            int basex = this.Owner.Location.X;
            int basey = this.Owner.Location.Y;
            int height = this.Owner.Height;

            this.Location = new System.Drawing.Point(basex + 10, basey + height - this.Height/2);

            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.ControlBox = false;
            //this.Text = String.Empty;
        }

        private void PTZCommandButtons_FormClosing(object sender, FormClosingEventArgs e)
        {
            

        }
    }
}
