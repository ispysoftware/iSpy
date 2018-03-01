using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class PTZUI : UserControl
    {
        public CameraWindow CameraControl;
        private bool _mousedown;
        private Point _location = Point.Empty;

        public PTZUI()
        {
            InitializeComponent();
        }

        private void pnlPTZ_MouseMove(object sender, MouseEventArgs e)
        {
            _location = e.Location;
        }

        private void pnlPTZ_MouseEnter(object sender, EventArgs e)
        {

        }

        private void ProcessPtzInput(Point p)
        {
            if (CameraControl?.Camera == null)
                return;

            var comm = Enums.PtzCommand.Center;
            bool cmd = false;

            if (p.X > 170 && p.Y < 45)
            {
                comm = Enums.PtzCommand.ZoomIn;
                cmd = true;
            }
            if (p.X > 170 && p.Y > 45 && p.Y < 90)
            {
                comm = Enums.PtzCommand.ZoomOut;
                cmd = true;
            }

            if (cmd)
            {
                CameraControl.Calibrating = true;
                CameraControl.PTZ.SendPTZCommand(comm);
            }
            else
            {
                double angle = Math.Atan2(86 - p.Y, 86 - p.X);
                CameraControl.Calibrating = true;
                CameraControl.PTZ.SendPTZDirection(angle);
            }


        }

        private void pnlPTZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (CameraControl == null)
                return;
            _mousedown = true;
            _location = e.Location;
            tmrRepeater.Start();
            ProcessPtzInput(_location);
        }

        private void pnlPTZ_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pnlPTZ_MouseUp(object sender, MouseEventArgs e)
        {
            _mousedown = false;
            tmrRepeater.Stop();

            CameraControl?.PTZ.CheckSendStop();
        }

        private void tmrRepeater_Tick(object sender, EventArgs e)
        {
            if (_mousedown)
                ProcessPtzInput(_location);
        }
    }
}
