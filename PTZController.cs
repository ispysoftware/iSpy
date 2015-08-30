using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using iSpyApplication.Controls;
using iSpyApplication.Pelco;
using iSpyApplication.Sources.Video;
using iSpyPRO.DirectShow;
using odm.core;
using onvif.services;
using utils;
using Rectangle = System.Drawing.Rectangle;

namespace iSpyApplication
{
    public class PTZController
    {
        public static string[] PelcoCommands =
                                             {
                                                 "Focus Near", "Focus Far", "Open Iris", "Close Iris", "Switch On",
                                                 "Switch Off", "Clear Screen", "Flip", "Pattern Stop", "Pattern Run",
                                                 "Pattern Start", "Go Preset 1", "Set Preset 1", "Clear Preset 1",
                                                 "Go Preset 2", "Set Preset 2", "Clear Preset 2", "Go Preset 3",
                                                 "Set Preset 3", "Clear Preset 3", "Go Preset 4", "Set Preset 4",
                                                 "Clear Preset 4", "Go Preset 5", "Set Preset 5", "Clear Preset 5",
                                                 "Go Preset 6", "Set Preset 6", "Clear Preset 6", "Remote Reset",
                                                 "Zero Pan Position", "Start Zone 1", "Stop Zone 1", "Start Zone 2",
                                                 "Stop Zone 2", "Start Zone 3", "Stop Zone 3", "Start Zone 4",
                                                 "Stop Zone 4", "Start Zone 5", "Stop Zone 5", "Start Zone 6",
                                                 "Stop Zone 6", "Start Zone Scan", "Stop Zone Scan"
                                             };

        private readonly CameraWindow _cameraControl;
        private Enums.PtzCommand _previousCommand;
        private SerialPort _serialPort;

        public bool IsContinuous
        {
            get
            {
                if (_cameraControl?.Camobject.ptz<-2)
                {
                    //onvif/pelco-p/pelco-d
                    return true;
                }
                return false;
            }
        }

        #region ONVIF
        private Profile _ptzProfile;
        private INvtSession _nvtSession;
        #endregion

        private uint _addr;

        public void ConfigurePelco()
        {
            if (_serialPort!=null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }

            string[] cfg = _cameraControl.Camobject.settings.ptzpelcoconfig.Split('|');
            Parity p;
            Enum.TryParse(cfg[4], out p);
            StopBits sb;
            Enum.TryParse(cfg[3], out sb);
            try
            {
                _serialPort = new SerialPort(cfg[0], Convert.ToInt32(cfg[1]), p, Convert.ToInt32(cfg[2]), sb)
                                 {WriteTimeout = 2000, ReadTimeout = 2000};
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            _addr = Convert.ToUInt16(cfg[5]);
        }

        private HttpWebRequest _request;
        const double Arc = Math.PI / 8;
        private string _nextcommand = "";

        private PTZSettings2Camera _ptzSettings;
        private bool _ptzNull;
        internal PTZSettings2Camera PTZSettings
        {
            get
            {
                if (_ptzSettings != null || _ptzNull)
                    return _ptzSettings;
                _ptzSettings = MainForm.PTZs.SingleOrDefault(q => q.id == _cameraControl.Camobject.ptz);
                _ptzNull = _ptzSettings == null;
                return _ptzSettings;
            }
            set { 
                _ptzSettings = value;
                _ptzNull = _ptzSettings == null;
            }
        }

        internal bool DigitalPTZ => PTZSettings == null;


        public PTZController(CameraWindow cameraControl)
        {
            _cameraControl = cameraControl;
        }

        public void SendPTZDirection(double angle, int repeat)
        {
            for (int i = 0; i < repeat; i++)
            {
                SendPTZDirection(angle);
            }
        }

        public void AddPreset(string name)
        {
            if (PTZProfile!=null)
            {
                try
                {
                    PTZSession.SetPreset(PTZProfile.token, name,null).RunSynchronously();
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
            }
        }

        public void DeletePreset(string name)
        {
            if (PTZProfile != null)
            {
                try
                {
                    var l = PTZSession.GetPresets(PTZProfile.token).RunSynchronously();
                    string t = "";
                    foreach(var p in l)
                    {
                        if (p.name==name)
                        {
                            t = p.token;
                            break;
                        }
                    }

                    if (t!="")
                        PTZSession.RemovePreset(PTZProfile.token, t).RunSynchronously();
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
            }
        }

        public void SendPTZDirection(double angle)
        {
            if (_cameraControl.Camobject.settings.ptzrotate90)
            {
                angle -= (Math.PI/2);
                if (angle < -Math.PI)
                {
                    angle += (2*Math.PI);
                }
            }
            if (_cameraControl.Camobject.ptz!=-1)
            {
                //don't flip digital controls
                if (_cameraControl.Camobject.settings.ptzflipx)
                {
                    if (angle <= 0)
                        angle = -Math.PI - angle;
                    else
                        angle = Math.PI - angle;
                }
                if (_cameraControl.Camobject.settings.ptzflipy)
                {
                    angle = angle * -1;
                }
            }
            
           

            if (PTZSettings == null)
            {
                var cmd = Enums.PtzCommand.Stop;
                if (angle < Arc && angle > -Arc)
                {
                    cmd = Enums.PtzCommand.Left;
                }
                if (angle >= Arc && angle < 3 * Arc)
                {
                    cmd = Enums.PtzCommand.Upleft;
                }
                if (angle >= 3 * Arc && angle < 5 * Arc)
                {
                    cmd = Enums.PtzCommand.Up;
                }
                if (angle >= 5 * Arc && angle < 7 * Arc)
                {
                    cmd = Enums.PtzCommand.UpRight;
                }
                if (angle >= 7 * Arc || angle < -7 * Arc)
                {
                    cmd = Enums.PtzCommand.Right;
                }
                if (angle <= -5 * Arc && angle > -7 * Arc)
                {
                    cmd = Enums.PtzCommand.DownRight;
                }
                if (angle <= -3 * Arc && angle > -5 * Arc)
                {
                    cmd = Enums.PtzCommand.Down;
                }
                if (angle <= -Arc && angle > -3 * Arc)
                {
                    cmd = Enums.PtzCommand.DownLeft;
                }

                switch (_cameraControl.Camobject.ptz)
                {
                    default:
                        var p = _cameraControl.Camera.ZPoint;
                        p.X -= Convert.ToInt32(15 * Math.Cos(angle));
                        p.Y -= Convert.ToInt32(15 * Math.Sin(angle));
                        _cameraControl.Camera.ZPoint = p;
                        break;
                    case -2://IAM
                        ProcessIAM(cmd);
                        return;
                    case -3://PELCO-P
                        ProcessPelco(cmd, true);
                        return;
                    case -4://PELCO-D
                        ProcessPelco(cmd, false);
                        return;
                    case -5://ONVIF
                        ProcessOnvif(cmd);
                        break;
                    case -6:
                        //none - ignore
                        break;
                }               
                return;
            }

            

            string command = PTZSettings.Commands.Center;
            string diag = "";

            if (angle < Arc && angle > -Arc)
            {
                command = PTZSettings.Commands.Left;

            }
            if (angle >= Arc && angle < 3 * Arc)
            {
                command = PTZSettings.Commands.LeftUp;
                diag = "leftup";
            }
            if (angle >= 3 * Arc && angle < 5 * Arc)
            {
                command = PTZSettings.Commands.Up;
            }
            if (angle >= 5 * Arc && angle < 7 * Arc)
            {
                command = PTZSettings.Commands.RightUp;
                diag = "rightup";
            }
            if (angle >= 7 * Arc || angle < -7 * Arc)
            {
                command = PTZSettings.Commands.Right;
            }
            if (angle <= -5 * Arc && angle > -7 * Arc)
            {
                command = PTZSettings.Commands.RightDown;
                diag = "rightdown";
            }
            if (angle <= -3 * Arc && angle > -5 * Arc)
            {
                command = PTZSettings.Commands.Down;
            }
            if (angle <= -Arc && angle > -3 * Arc)
            {
                command = PTZSettings.Commands.LeftDown;
                diag = "leftdown";
            }

            if (String.IsNullOrEmpty(command)) //some PTZ cameras don't have diagonal controls, this fixes that
            {
                switch (diag)
                {
                    case "leftup":
                        _nextcommand = PTZSettings.Commands.Up;
                        SendPTZCommand(PTZSettings.Commands.Left);
                        break;
                    case "rightup":
                        _nextcommand = PTZSettings.Commands.Up;
                        SendPTZCommand(PTZSettings.Commands.Right);
                        break;
                    case "rightdown":
                        _nextcommand = PTZSettings.Commands.Down;
                        SendPTZCommand(PTZSettings.Commands.Right);
                        break;
                    case "leftdown":
                        _nextcommand = PTZSettings.Commands.Down;
                        SendPTZCommand(PTZSettings.Commands.Left);
                        break;
                }
            }
            else
                SendPTZCommand(command);

        }

        public void SendPTZCommand(Enums.PtzCommand command)
        {
            SendPTZCommand(command,false);
        }

        internal bool DigitalZoom
        {
            get
            {
                if (_cameraControl.Camera == null)
                    return false;

                switch (_cameraControl.Camobject.ptz)
                {
                    case -1: //digital only
                        return true;
                    case -2:
                    case -3:
                    case -4:
                    case -5:
                    case -6:
                        return false;
                    default:
                    {
                        PTZSettings2Camera ptz = MainForm.PTZs.SingleOrDefault(q => q.id == _cameraControl.Camobject.ptz);
                        bool d = (ptz?.Commands == null);

                        if (!d)
                        {
                           
                            if (String.IsNullOrEmpty(ptz.Commands.ZoomIn))
                                d = true;
                            if (String.IsNullOrEmpty(ptz.Commands.ZoomOut))
                                d = true;
                        }
                        return d;
                    }
                }
            }
        }

        
        public void SendPTZCommand(Enums.PtzCommand command, bool wait)
        {
            if (_cameraControl.Camera == null)
                return;


            PTZSettings2Camera ptz = null;
            switch (_cameraControl.Camobject.ptz)
            {
                case -1://digital only
                    break;
                case -2://IAM
                    _cameraControl.Calibrating = true;
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            SendPTZDirection(0d);
                            break;
                        case Enums.PtzCommand.Upleft:
                            SendPTZDirection(Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Up:
                            SendPTZDirection(Math.PI / 2);
                            break;
                        case Enums.PtzCommand.UpRight:
                            SendPTZDirection(3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Right:
                            SendPTZDirection(Math.PI);
                            break;
                        case Enums.PtzCommand.DownRight:
                            SendPTZDirection(-3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Down:
                            SendPTZDirection(-Math.PI / 2);
                            break;
                        case Enums.PtzCommand.DownLeft:
                            SendPTZDirection(-Math.PI / 4);
                            break;
                        default:
                            ProcessIAM(command);
                            break;
                    }
                    return;
                case -3://PELCO-P
                    _cameraControl.Calibrating = true;
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            SendPTZDirection(0d);
                            break;
                        case Enums.PtzCommand.Upleft:
                            SendPTZDirection(Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Up:
                            SendPTZDirection(Math.PI / 2);
                            break;
                        case Enums.PtzCommand.UpRight:
                            SendPTZDirection(3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Right:
                            SendPTZDirection(Math.PI);
                            break;
                        case Enums.PtzCommand.DownRight:
                            SendPTZDirection(-3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Down:
                            SendPTZDirection(-Math.PI / 2);
                            break;
                        case Enums.PtzCommand.DownLeft:
                            SendPTZDirection(-Math.PI / 4);
                            break;
                        default:
                            ProcessPelco(command, true);
                            break;
                    }
                    return;
                case -4://PELCO-D
                    _cameraControl.Calibrating = true;
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            SendPTZDirection(0d);
                            break;
                        case Enums.PtzCommand.Upleft:
                            SendPTZDirection(Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Up:
                            SendPTZDirection(Math.PI / 2);
                            break;
                        case Enums.PtzCommand.UpRight:
                            SendPTZDirection(3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Right:
                            SendPTZDirection(Math.PI);
                            break;
                        case Enums.PtzCommand.DownRight:
                            SendPTZDirection(-3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Down:
                            SendPTZDirection(-Math.PI / 2);
                            break;
                        case Enums.PtzCommand.DownLeft:
                            SendPTZDirection(-Math.PI / 4);
                            break;
                        default:
                            ProcessPelco(command, false);
                            break;
                    }
                    return;
                case -5://ONVIF
                    _cameraControl.Calibrating = true;
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            SendPTZDirection(0d);
                            break;
                        case Enums.PtzCommand.Upleft:
                            SendPTZDirection(Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Up:
                            SendPTZDirection(Math.PI / 2);
                            break;
                        case Enums.PtzCommand.UpRight:
                            SendPTZDirection(3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Right:
                            SendPTZDirection(Math.PI);
                            break;
                        case Enums.PtzCommand.DownRight:
                            SendPTZDirection(-3 * Math.PI / 4);
                            break;
                        case Enums.PtzCommand.Down:
                            SendPTZDirection(-Math.PI / 2);
                            break;
                        case Enums.PtzCommand.DownLeft:
                            SendPTZDirection(-Math.PI / 4);
                            break;
                        default:
                            ProcessOnvif(command);
                            break;
                    }
                    return;
                case -6:
                    return;
                default: //IP CAMERA
                    ptz = MainForm.PTZs.SingleOrDefault(q => q.id == _cameraControl.Camobject.ptz);
                    break;
            }

            bool d = (ptz?.Commands == null);

            if (!d)
            {
                if (command == Enums.PtzCommand.ZoomIn)
                {
                    if (String.IsNullOrEmpty(ptz.Commands.ZoomIn))
                        d = true;
                }
                if (command == Enums.PtzCommand.ZoomOut)
                {
                    if (String.IsNullOrEmpty(ptz.Commands.ZoomOut))
                        d = true;
                }
            }

            if (!d)
            {
                _cameraControl.Calibrating = true;
                switch (command)
                {
                    case Enums.PtzCommand.Left:
                        SendPTZDirection(0d);
                        break;
                    case Enums.PtzCommand.Upleft:
                        SendPTZDirection(Math.PI/4);
                        break;
                    case Enums.PtzCommand.Up:
                        SendPTZDirection(Math.PI / 2);
                        break;
                    case Enums.PtzCommand.UpRight:
                        SendPTZDirection(3 * Math.PI / 4);
                        break;
                    case Enums.PtzCommand.Right:
                        SendPTZDirection(Math.PI);
                        break;
                    case Enums.PtzCommand.DownRight:
                        SendPTZDirection(-3*Math.PI / 4);
                        break;
                    case Enums.PtzCommand.Down:
                        SendPTZDirection(-Math.PI / 2);
                        break;
                    case Enums.PtzCommand.DownLeft:
                        SendPTZDirection(-Math.PI / 4);
                        break;
                    case Enums.PtzCommand.ZoomIn:
                        SendPTZCommand(ptz.Commands.ZoomIn, wait);
                        break;
                    case Enums.PtzCommand.ZoomOut:
                        SendPTZCommand(ptz.Commands.ZoomOut, wait);
                        break;
                    case Enums.PtzCommand.Center:
                        SendPTZCommand(ptz.Commands.Center, wait);
                        break;
                    case Enums.PtzCommand.Stop:
                        if (_previousCommand == Enums.PtzCommand.ZoomIn)
                        {
                            if (!String.IsNullOrEmpty(ptz.Commands.ZoomInStop))
                            {
                                SendPTZCommand(ptz.Commands.ZoomInStop, wait);
                                break;
                            }
                        }
                        if (_previousCommand == Enums.PtzCommand.ZoomOut)
                        {
                            if (!String.IsNullOrEmpty(ptz.Commands.ZoomOutStop))
                            {
                                SendPTZCommand(ptz.Commands.ZoomOutStop, wait);
                                break;
                            }
                        }
                        SendPTZCommand(ptz.Commands.Stop, wait);
                        break;
                }
                _previousCommand = command;
            }
            else
            {
                Rectangle r = _cameraControl.Camera.ViewRectangle;
                if (r != Rectangle.Empty)
                {
                    double angle = 0;
                    bool isangle = true;
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            angle = 0;
                            break;
                        case Enums.PtzCommand.Upleft:
                            angle = Math.PI / 4;
                            break;
                        case Enums.PtzCommand.Up:
                            angle = Math.PI / 2;
                            break;
                        case Enums.PtzCommand.UpRight:
                            angle = 3 * Math.PI / 4;
                            break;
                        case Enums.PtzCommand.Right:
                            angle = Math.PI;
                            break;
                        case Enums.PtzCommand.DownRight:
                            angle = -3 * Math.PI / 4;
                            break;
                        case Enums.PtzCommand.Down:
                            angle = -Math.PI / 2;
                            break;
                        case Enums.PtzCommand.DownLeft:
                            angle = -Math.PI / 4;
                            break;
                        case Enums.PtzCommand.ZoomIn:
                            isangle = false;
                            _cameraControl.Camera.ZFactor += 0.2f;
                            break;
                        case Enums.PtzCommand.ZoomOut:
                            isangle = false;                        
                            var f = _cameraControl.Camera.ZFactor;
                            f -= 0.2f;
                            if (f < 1)
                                f = 1;
                            _cameraControl.Camera.ZFactor = f;
                            break;
                        case Enums.PtzCommand.Center:
                            isangle = false;
                            _cameraControl.Camera.ZFactor = 1;
                            break;
                        case Enums.PtzCommand.Stop:
                            isangle = false;
                            break;

                    }
                    if (isangle)
                    {
                        var p = _cameraControl.Camera.ZPoint;
                        p.X -= Convert.ToInt32(15 * Math.Cos(angle));
                        p.Y -= Convert.ToInt32(15 * Math.Sin(angle));
                        _cameraControl.Camera.ZPoint = p;
                    }   

                }
            }
        }

        void ProcessIAM(Enums.PtzCommand command)
        {
            var d = _cameraControl.Camera?.VideoSource as VideoCaptureDevice;
            if (d != null)
            {
                try
                {
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            IAMMove(d, CameraControlProperty.Pan, -1);
                            break;
                        case Enums.PtzCommand.Upleft:
                            IAMMove(d, CameraControlProperty.Pan, -1);
                            IAMMove(d, CameraControlProperty.Tilt, 1);
                            break;
                        case Enums.PtzCommand.Up:
                            IAMMove(d, CameraControlProperty.Tilt, 1);
                            break;
                        case Enums.PtzCommand.UpRight:
                            IAMMove(d, CameraControlProperty.Pan, 1);
                            IAMMove(d, CameraControlProperty.Tilt, 1);
                            break;
                        case Enums.PtzCommand.Right:
                            IAMMove(d, CameraControlProperty.Pan, 1);
                            break;
                        case Enums.PtzCommand.DownRight:
                            IAMMove(d, CameraControlProperty.Pan, 1);
                            IAMMove(d, CameraControlProperty.Tilt, -1);
                            break;
                        case Enums.PtzCommand.Down:
                            IAMMove(d, CameraControlProperty.Tilt, -1);
                            break;
                        case Enums.PtzCommand.DownLeft:
                            IAMMove(d, CameraControlProperty.Tilt, -1);
                            IAMMove(d, CameraControlProperty.Pan, -1);
                            break;
                        case Enums.PtzCommand.ZoomIn:
                            IAMMove(d, CameraControlProperty.Zoom, 1);
                            break;
                        case Enums.PtzCommand.ZoomOut:
                            IAMMove(d, CameraControlProperty.Zoom, -1);
                            break;
                        case Enums.PtzCommand.Center:
                            IAMMove(d, CameraControlProperty.Pan, 0);
                            IAMMove(d, CameraControlProperty.Tilt, 0);
                            IAMMove(d, CameraControlProperty.Zoom, 0);
                            break;
                        case Enums.PtzCommand.Stop:
                            //not implemented
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
            }
        }

        void IAMMove(VideoCaptureDevice d, CameraControlProperty p, int i)
        {
            int v, minv, maxv, stepSize, defVal;
            CameraControlFlags f, cf;
            d.GetCameraProperty(p, out v, out f);
            d.GetCameraPropertyRange(p, out minv, out maxv, out stepSize, out defVal, out cf);

            int newv = v + i*stepSize;
            if (newv<minv)
                newv = minv;
            if (newv>maxv)
                newv = maxv;

            if (i == 0)
                newv = defVal;

            if (cf== CameraControlFlags.Manual)
            {
                d.SetCameraProperty(p, newv, CameraControlFlags.Manual);
            }
            else
            {
                MainForm.LogMessageToFile("Camera control flags are not manual");
            }
            
        }

        private Profile PTZProfile
        {
            get
            {
                if (PTZSession == null)
                    return null;

                if (_ptzProfile!=null)
                    return _ptzProfile;

                string[] cfg = _cameraControl.Camobject.settings.onvifident.Split('|');
                if (cfg.Length != 2)
                    return null;

                int profileid = Convert.ToInt32(cfg[1]);

                Profile[] profiles;
                try
                {
                    profiles = PTZSession.GetProfiles().RunSynchronously();
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                    return null;
                }
                if (profiles.Length>profileid)
                    _ptzProfile = profiles[profileid];
                return _ptzProfile;
            }
        }
        private INvtSession PTZSession
        {
            get
            {
                if (_nvtSession != null)
                    return _nvtSession;

                string[] cfg = _cameraControl.Camobject.settings.onvifident.Split('|');
                if (cfg.Length != 2)
                    return null;

                string addr = cfg[0];
                DeviceDescriptionHolder ddh = MainForm.ONVIFDevices.FirstOrDefault(p => p.URL == addr);
                if (ddh == null)
                {
                    Uri u;
                    if (!Uri.TryCreate(addr, UriKind.Absolute, out u))
                    {
                        return null;
                    }
                    ddh = new DeviceDescriptionHolder { Uris = new[] { u }, Address = "" };
                    ddh.Address += u.DnsSafeHost + "; ";
                    ddh.Address = ddh.Address.TrimEnd(';', ' ');
                    if (ddh.Address == "")
                    {
                        ddh.IsInvalidUris = true;
                        ddh.Address = "Invalid Uri";
                    }
                    ddh.Name = u.AbsoluteUri;
                    ddh.Location = "Unknown";
                    ddh.DeviceIconUri = null;
                    MainForm.ONVIFDevices.Add(ddh);
                }
                    

                ddh.Account = new NetworkCredential { UserName = _cameraControl.Camobject.settings.login, Password = _cameraControl.Camobject.settings.password };
                var sessionFactory = new NvtSessionFactory(ddh.Account);


                try {_nvtSession = sessionFactory.CreateSession(ddh.Uris[0]);}
                catch(Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                    return null;
                }
                //ddh.Name = _nvtSession
                return _nvtSession;
            }
        }

        public void ResetONVIF()
        {
            _ptzProfile = null;
            _ptzSettings = null;
        }

        void ProcessOnvif(Enums.PtzCommand command)
        {
            if (PTZProfile!=null)
            {
                //var speed = PTZProfile.ptzConfiguration.defaultPTZSpeed;
                //string spacePT = PTZProfile.ptzConfiguration.defaultContinuousPanTiltVelocitySpace;
                //string spaceZ = PTZProfile.ptzConfiguration.defaultContinuousZoomVelocitySpace;

                Vector2D panTilt = null;
                Vector1D zoom = null;
                try
                {
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            panTilt = new Vector2D {space = null, x = -0.5f, y = 0};
                            break;
                        case Enums.PtzCommand.Upleft:
                            panTilt = new Vector2D {space = null, x = -0.5f, y = 0.5f};
                            break;
                        case Enums.PtzCommand.Up:
                            panTilt = new Vector2D {space = null, x = 0, y = 0.5f};
                            break;
                        case Enums.PtzCommand.UpRight:
                            panTilt = new Vector2D {space = null, x = 0.5f, y = 0.5f};
                            break;
                        case Enums.PtzCommand.Right:
                            panTilt = new Vector2D {space = null, x = 0.5f, y = 0};
                            break;
                        case Enums.PtzCommand.DownRight:
                            panTilt = new Vector2D {space = null, x = 0.5f, y = -0.5f};
                            break;
                        case Enums.PtzCommand.Down:
                            panTilt = new Vector2D {space = null, x = 0, y = -0.5f};
                            break;
                        case Enums.PtzCommand.DownLeft:
                            panTilt = new Vector2D {space = null, x = -0.5f, y = -0.5f};
                            break;
                        case Enums.PtzCommand.ZoomIn:
                            zoom = new Vector1D {space = null, x = 0.5f};
                            break;
                        case Enums.PtzCommand.ZoomOut:
                            zoom = new Vector1D {space = null, x = -0.5f};
                            break;
                        case Enums.PtzCommand.Center:
                            ProcessOnvifCommand(_cameraControl.Camobject.settings.ptzautohomecommand);
                            return;
                        case Enums.PtzCommand.Stop:
                            PTZSession.Stop(PTZProfile.token, true, true).RunSynchronously();
                            return;
                    }
                    PTZSession.ContinuousMove(PTZProfile.token, new PTZSpeed() {panTilt = panTilt, zoom = zoom},
                                              null).RunSynchronously();
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
            }
        }

        void ProcessOnvifCommand(string name)
        {
            if (PTZProfile != null)
            {
                try
                {
                    var l = PTZSession.GetPresets(PTZProfile.token).RunSynchronously();
                    string t = "";
                    foreach (var p in l)
                    {
                        if (p.name == name)
                        {
                            t = p.token;
                            break;
                        }
                    }

                    if (t != "")
                        PTZSession.GotoPreset(PTZProfile.token, t, null).RunSynchronously();


                    
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
                
            }
        }

        public string[] ONVIFPresets
        {
            get
            {
                var pl = new List<string>();
                try
                {
                    if (PTZProfile != null && PTZSession!=null)
                    {
                    
                            var presets = PTZSession.GetPresets(PTZProfile.token).RunSynchronously();
                            pl.AddRange(presets.Select(p => p.name));
                    
                    }
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
                return pl.ToArray();
            }
        }


        void ProcessPelco(Enums.PtzCommand command, bool usePelcoP)
        {
            //PELCO
            if (_serialPort == null)
            {
                ConfigurePelco();
            }
            if (_serialPort != null)
            {
                if (!_serialPort.IsOpen)
                {
                    try
                    {
                        _serialPort.Open();
                    }
                    catch (Exception ex)
                    {
                        _serialPort.Dispose();
                        _serialPort = null;
                        MainForm.LogExceptionToFile(ex);
                        return;
                    }
                    //SerialPort.DataReceived += SerialPort_DataReceived;
                }
            }
            if (_serialPort == null || !_serialPort.IsOpen)
                return;

            if (usePelcoP)
            {
                var pelcoP = new P();
                switch (command)
                {
                    case Enums.PtzCommand.Left:
                        SendPelco(pelcoP.CameraPan(_addr, P.Pan.Left, 1));
                        break;
                    case Enums.PtzCommand.Upleft:
                        SendPelco(pelcoP.CameraTilt(_addr, P.Tilt.Up, 1));
                        SendPelco(pelcoP.CameraPan(_addr, P.Pan.Left, 1));
                        break;
                    case Enums.PtzCommand.Up:
                        SendPelco(pelcoP.CameraTilt(_addr, P.Tilt.Up, 1));
                        break;
                    case Enums.PtzCommand.UpRight:
                        SendPelco(pelcoP.CameraTilt(_addr, P.Tilt.Up, 1));
                        SendPelco(pelcoP.CameraPan(_addr, P.Pan.Right, 1));
                        break;
                    case Enums.PtzCommand.Right:
                        SendPelco(pelcoP.CameraPan(_addr, P.Pan.Right, 1));
                        break;
                    case Enums.PtzCommand.DownRight:
                        SendPelco(pelcoP.CameraTilt(_addr, P.Tilt.Down, 1));
                        SendPelco(pelcoP.CameraPan(_addr, P.Pan.Right, 1));
                        break;
                    case Enums.PtzCommand.Down:
                        SendPelco(pelcoP.CameraTilt(_addr, P.Tilt.Down, 1));
                        break;
                    case Enums.PtzCommand.DownLeft:
                        SendPelco(pelcoP.CameraTilt(_addr, P.Tilt.Down, 1));
                        SendPelco(pelcoP.CameraPan(_addr, P.Pan.Left, 1));
                        break;
                    case Enums.PtzCommand.ZoomIn:
                        SendPelco(pelcoP.CameraZoom(_addr, P.Zoom.Tele));
                        break;
                    case Enums.PtzCommand.ZoomOut:
                        SendPelco(pelcoP.CameraZoom(_addr, P.Zoom.Wide));
                        break;
                    case Enums.PtzCommand.Center:
                        SendPelco(pelcoP.ZeroPanPosition(_addr));
                        break;
                    case Enums.PtzCommand.Stop:
                        SendPelco(pelcoP.CameraStop(_addr));
                        break;
                }
            }
            else
            {
                var pelcoD = new D();
                switch (command)
                {
                    case Enums.PtzCommand.Left:
                        SendPelco(pelcoD.CameraPan(_addr, D.Pan.Left, 1));
                        break;
                    case Enums.PtzCommand.Upleft:
                        SendPelco(pelcoD.CameraTilt(_addr, D.Tilt.Up, 1));
                        SendPelco(pelcoD.CameraPan(_addr, D.Pan.Left, 1));
                        break;
                    case Enums.PtzCommand.Up:
                        SendPelco(pelcoD.CameraTilt(_addr, D.Tilt.Up, 1));
                        break;
                    case Enums.PtzCommand.UpRight:
                        SendPelco(pelcoD.CameraTilt(_addr, D.Tilt.Up, 1));
                        SendPelco(pelcoD.CameraPan(_addr, D.Pan.Right, 1));
                        break;
                    case Enums.PtzCommand.Right:
                        SendPelco(pelcoD.CameraPan(_addr, D.Pan.Right, 1));
                        break;
                    case Enums.PtzCommand.DownRight:
                        SendPelco(pelcoD.CameraTilt(_addr, D.Tilt.Down, 1));
                        SendPelco(pelcoD.CameraPan(_addr, D.Pan.Right, 1));
                        break;
                    case Enums.PtzCommand.Down:
                        SendPelco(pelcoD.CameraTilt(_addr, D.Tilt.Down, 1));
                        break;
                    case Enums.PtzCommand.DownLeft:
                        SendPelco(pelcoD.CameraTilt(_addr, D.Tilt.Down, 1));
                        SendPelco(pelcoD.CameraPan(_addr, D.Pan.Left, 1));
                        break;
                    case Enums.PtzCommand.ZoomIn:
                        SendPelco(pelcoD.CameraZoom(_addr, D.Zoom.Tele));
                        break;
                    case Enums.PtzCommand.ZoomOut:
                        SendPelco(pelcoD.CameraZoom(_addr, D.Zoom.Wide));
                        break;
                    case Enums.PtzCommand.Center:
                        SendPelco(pelcoD.ZeroPanPosition(_addr));
                        break;
                    case Enums.PtzCommand.Stop:
                        SendPelco(pelcoD.CameraStop(_addr));
                        break;
                }
            }

            _serialPort.Close();
            _serialPort.Dispose();
            _serialPort = null;
        }

        void ProcessPelcoCommand(string command, bool usePelcoP)
        {
            //PELCO
            if (_serialPort == null)
            {
                ConfigurePelco();
            }
            if (_serialPort != null)
            {
                if (!_serialPort.IsOpen)
                {
                    try
                    {
                        _serialPort.Open();
                    }
                    catch (Exception ex)
                    {
                        _serialPort.Dispose();
                        _serialPort = null;
                        MainForm.LogExceptionToFile(ex);
                        return;
                    }
                    //SerialPort.DataReceived += SerialPort_DataReceived;
                }
            }
            if (_serialPort == null || !_serialPort.IsOpen)
                return;
            if (usePelcoP)
            {
                var pelcoP = new P();
                switch (command)
                {
                    case "Focus Near":
                        SendPelco(pelcoP.CameraFocus(_addr, P.Focus.Near));
                        break;
                    case "Focus Far":
                        SendPelco(pelcoP.CameraFocus(_addr, P.Focus.Far));
                        break;
                    case "Open Iris":
                        SendPelco(pelcoP.CameraIrisSwitch(_addr, P.Iris.Open));
                        break;
                    case "Close Iris":
                        SendPelco(pelcoP.CameraIrisSwitch(_addr, P.Iris.Close));
                        break;
                    case "Switch On":
                        SendPelco(pelcoP.CameraSwitch(_addr, P.Switch.On));
                        break;
                    case "Switch Off":
                        SendPelco(pelcoP.CameraSwitch(_addr, P.Switch.Off));
                        break;
                    case "Clear Screen":
                        SendPelco(pelcoP.ClearScreen(_addr));
                        break;
                    case "Flip":
                        SendPelco(pelcoP.Flip(_addr));
                        break;
                    case "Pattern Stop":
                        SendPelco(pelcoP.Pattern(_addr,P.PatternAction.Stop));
                        break;
                    case "Pattern Run":
                        SendPelco(pelcoP.Pattern(_addr, P.PatternAction.Run));
                        break;
                    case "Pattern Start":
                        SendPelco(pelcoP.Pattern(_addr, P.PatternAction.Start));
                        break;
                    case "Go Preset 1":
                        SendPelco(pelcoP.Preset(_addr, 1, P.PresetAction.Goto));
                        break;
                    case "Set Preset 1":
                        SendPelco(pelcoP.Preset(_addr, 1, P.PresetAction.Set));
                        break;
                    case "Clear Preset 1":
                        SendPelco(pelcoP.Preset(_addr, 1, P.PresetAction.Clear));
                        break;
                    case "Go Preset 2":
                        SendPelco(pelcoP.Preset(_addr, 2, P.PresetAction.Goto));
                        break;
                    case "Set Preset 2":
                        SendPelco(pelcoP.Preset(_addr, 2, P.PresetAction.Set));
                        break;
                    case "Clear Preset 2":
                        SendPelco(pelcoP.Preset(_addr, 2, P.PresetAction.Clear));
                        break;
                    case "Go Preset 3":
                        SendPelco(pelcoP.Preset(_addr, 3, P.PresetAction.Goto));
                        break;
                    case "Set Preset 3":
                        SendPelco(pelcoP.Preset(_addr, 3, P.PresetAction.Set));
                        break;
                    case "Clear Preset 3":
                        SendPelco(pelcoP.Preset(_addr, 3, P.PresetAction.Clear));
                        break;
                    case "Go Preset 4":
                        SendPelco(pelcoP.Preset(_addr, 4, P.PresetAction.Goto));
                        break;
                    case "Set Preset 4":
                        SendPelco(pelcoP.Preset(_addr, 4, P.PresetAction.Set));
                        break;
                    case "Clear Preset 4":
                        SendPelco(pelcoP.Preset(_addr, 4, P.PresetAction.Clear));
                        break;
                    case "Go Preset 5":
                        SendPelco(pelcoP.Preset(_addr, 5, P.PresetAction.Goto));
                        break;
                    case "Set Preset 5":
                        SendPelco(pelcoP.Preset(_addr, 5, P.PresetAction.Set));
                        break;
                    case "Clear Preset 5":
                        SendPelco(pelcoP.Preset(_addr, 5, P.PresetAction.Clear));
                        break;
                    case "Go Preset 6":
                        SendPelco(pelcoP.Preset(_addr, 6, P.PresetAction.Goto));
                        break;
                    case "Set Preset 6":
                        SendPelco(pelcoP.Preset(_addr, 6, P.PresetAction.Set));
                        break;
                    case "Clear Preset 6":
                        SendPelco(pelcoP.Preset(_addr, 6, P.PresetAction.Clear));
                        break;
                    case "Remote Reset":
                        SendPelco(pelcoP.RemoteReset(_addr));
                        break;
                    case "Zero Pan Position":
                        SendPelco(pelcoP.ZeroPanPosition(_addr));
                        break;
                    case "Start Zone 1":
                        SendPelco(pelcoP.Zone(_addr,1,P.Action.Start));
                        break;
                    case "Stop Zone 1":
                        SendPelco(pelcoP.Zone(_addr, 1, P.Action.Stop));
                        break;
                    case "Start Zone 2":
                        SendPelco(pelcoP.Zone(_addr, 2, P.Action.Start));
                        break;
                    case "Stop Zone 2":
                        SendPelco(pelcoP.Zone(_addr, 2, P.Action.Stop));
                        break;
                    case "Start Zone 3":
                        SendPelco(pelcoP.Zone(_addr, 3, P.Action.Start));
                        break;
                    case "Stop Zone 3":
                        SendPelco(pelcoP.Zone(_addr, 3, P.Action.Stop));
                        break;
                    case "Start Zone 4":
                        SendPelco(pelcoP.Zone(_addr, 4, P.Action.Start));
                        break;
                    case "Stop Zone 4":
                        SendPelco(pelcoP.Zone(_addr, 4, P.Action.Stop));
                        break;
                    case "Start Zone 5":
                        SendPelco(pelcoP.Zone(_addr, 5, P.Action.Start));
                        break;
                    case "Stop Zone 5":
                        SendPelco(pelcoP.Zone(_addr, 5, P.Action.Stop));
                        break;
                    case "Start Zone 6":
                        SendPelco(pelcoP.Zone(_addr, 6, P.Action.Start));
                        break;
                    case "Stop Zone 6":
                        SendPelco(pelcoP.Zone(_addr, 6, P.Action.Stop));
                        break;
                    case "Start Zone Scan":
                        SendPelco(pelcoP.ZoneScan(_addr,P.Action.Start));
                        break;
                    case "Stop Zone Scan":
                        SendPelco(pelcoP.ZoneScan(_addr, P.Action.Stop));
                        break;
                }
            }
            else
            {
                var pelcoD = new D();
                switch (command)
                {
                    case "Focus Near":
                        SendPelco(pelcoD.CameraFocus(_addr, D.Focus.Near));
                        break;
                    case "Focus Far":
                        SendPelco(pelcoD.CameraFocus(_addr, D.Focus.Far));
                        break;
                    case "Open Iris":
                        SendPelco(pelcoD.CameraIrisSwitch(_addr, D.Iris.Open));
                        break;
                    case "Close Iris":
                        SendPelco(pelcoD.CameraIrisSwitch(_addr, D.Iris.Close));
                        break;
                    case "Switch On":
                        SendPelco(pelcoD.CameraSwitch(_addr, D.Switch.On));
                        break;
                    case "Switch Off":
                        SendPelco(pelcoD.CameraSwitch(_addr, D.Switch.Off));
                        break;
                    case "Clear Screen":
                        SendPelco(pelcoD.ClearScreen(_addr));
                        break;
                    case "Flip":
                        SendPelco(pelcoD.Flip(_addr));
                        break;
                    case "Pattern Stop":
                        SendPelco(pelcoD.Pattern(_addr, D.PatternAction.Stop));
                        break;
                    case "Pattern Run":
                        SendPelco(pelcoD.Pattern(_addr, D.PatternAction.Run));
                        break;
                    case "Pattern Start":
                        SendPelco(pelcoD.Pattern(_addr, D.PatternAction.Start));
                        break;
                    case "Go Preset 1":
                        SendPelco(pelcoD.Preset(_addr, 1, D.PresetAction.Goto));
                        break;
                    case "Set Preset 1":
                        SendPelco(pelcoD.Preset(_addr, 1, D.PresetAction.Set));
                        break;
                    case "Clear Preset 1":
                        SendPelco(pelcoD.Preset(_addr, 1, D.PresetAction.Clear));
                        break;
                    case "Go Preset 2":
                        SendPelco(pelcoD.Preset(_addr, 2, D.PresetAction.Goto));
                        break;
                    case "Set Preset 2":
                        SendPelco(pelcoD.Preset(_addr, 2, D.PresetAction.Set));
                        break;
                    case "Clear Preset 2":
                        SendPelco(pelcoD.Preset(_addr, 2, D.PresetAction.Clear));
                        break;
                    case "Go Preset 3":
                        SendPelco(pelcoD.Preset(_addr, 3, D.PresetAction.Goto));
                        break;
                    case "Set Preset 3":
                        SendPelco(pelcoD.Preset(_addr, 3, D.PresetAction.Set));
                        break;
                    case "Clear Preset 3":
                        SendPelco(pelcoD.Preset(_addr, 3, D.PresetAction.Clear));
                        break;
                    case "Go Preset 4":
                        SendPelco(pelcoD.Preset(_addr, 4, D.PresetAction.Goto));
                        break;
                    case "Set Preset 4":
                        SendPelco(pelcoD.Preset(_addr, 4, D.PresetAction.Set));
                        break;
                    case "Clear Preset 4":
                        SendPelco(pelcoD.Preset(_addr, 4, D.PresetAction.Clear));
                        break;
                    case "Go Preset 5":
                        SendPelco(pelcoD.Preset(_addr, 5, D.PresetAction.Goto));
                        break;
                    case "Set Preset 5":
                        SendPelco(pelcoD.Preset(_addr, 5, D.PresetAction.Set));
                        break;
                    case "Clear Preset 5":
                        SendPelco(pelcoD.Preset(_addr, 5, D.PresetAction.Clear));
                        break;
                    case "Go Preset 6":
                        SendPelco(pelcoD.Preset(_addr, 6, D.PresetAction.Goto));
                        break;
                    case "Set Preset 6":
                        SendPelco(pelcoD.Preset(_addr, 6, D.PresetAction.Set));
                        break;
                    case "Clear Preset 6":
                        SendPelco(pelcoD.Preset(_addr, 6, D.PresetAction.Clear));
                        break;
                    case "Remote Reset":
                        SendPelco(pelcoD.RemoteReset(_addr));
                        break;
                    case "Zero Pan Position":
                        SendPelco(pelcoD.ZeroPanPosition(_addr));
                        break;
                    case "Start Zone 1":
                        SendPelco(pelcoD.Zone(_addr, 1, D.Action.Start));
                        break;
                    case "Stop Zone 1":
                        SendPelco(pelcoD.Zone(_addr, 1, D.Action.Stop));
                        break;
                    case "Start Zone 2":
                        SendPelco(pelcoD.Zone(_addr, 2, D.Action.Start));
                        break;
                    case "Stop Zone 2":
                        SendPelco(pelcoD.Zone(_addr, 2, D.Action.Stop));
                        break;
                    case "Start Zone 3":
                        SendPelco(pelcoD.Zone(_addr, 3, D.Action.Start));
                        break;
                    case "Stop Zone 3":
                        SendPelco(pelcoD.Zone(_addr, 3, D.Action.Stop));
                        break;
                    case "Start Zone 4":
                        SendPelco(pelcoD.Zone(_addr, 4, D.Action.Start));
                        break;
                    case "Stop Zone 4":
                        SendPelco(pelcoD.Zone(_addr, 4, D.Action.Stop));
                        break;
                    case "Start Zone 5":
                        SendPelco(pelcoD.Zone(_addr, 5, D.Action.Start));
                        break;
                    case "Stop Zone 5":
                        SendPelco(pelcoD.Zone(_addr, 5, D.Action.Stop));
                        break;
                    case "Start Zone 6":
                        SendPelco(pelcoD.Zone(_addr, 6, D.Action.Start));
                        break;
                    case "Stop Zone 6":
                        SendPelco(pelcoD.Zone(_addr, 6, D.Action.Stop));
                        break;
                    case "Start Zone Scan":
                        SendPelco(pelcoD.ZoneScan(_addr, D.Action.Start));
                        break;
                    case "Stop Zone Scan":
                        SendPelco(pelcoD.ZoneScan(_addr, D.Action.Stop));
                        break;
                }
            }

            _serialPort.Close();
            _serialPort.Dispose();
            _serialPort = null;
        }

        void SendPelco(byte[] arr)
        {
            _serialPort.Write(arr,0,arr.Length);
        }

        //void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    string data = _serialPort.ReadLine();
        //    Debug.WriteLine(" <- " + data);
        //}

        public void SendPTZCommand(string cmd)
        {
            SendPTZCommand(cmd,false);
        }

        public void SendPTZCommand(string cmd, bool wait)
        {
            if (String.IsNullOrEmpty(cmd))
                return;

            if (_request != null)
            {
                if (!wait)
                    return;
                _request.Abort();
            }

            PTZSettings2Camera ptz;
            switch (_cameraControl.Camobject.ptz)
            {
                case -1://digital only
                    return;
                case -2://IAM
                    return;
                case -3://PELCO-P
                    ProcessPelcoCommand(cmd, true);
                    return;
                case -4://PELCO-D
                    ProcessPelcoCommand(cmd, false);
                    return;
                case -5://ONVIF
                    ProcessOnvifCommand(cmd);
                    return;
                case -6://NONE
                    return;
                default: //IP CAMERA
                    ptz = MainForm.PTZs.SingleOrDefault(q => q.id == _cameraControl.Camobject.ptz);
                    break;
            }
            if (ptz == null)
                return;

            Uri uri;
            bool absURL = false;
            
            string url = _cameraControl.Camobject.settings.videosourcestring;

            if (_cameraControl.Camobject.settings.ptzurlbase.Contains("://"))
            {
                url = _cameraControl.Camobject.settings.ptzurlbase;
                absURL = true;
            }

            if (cmd.Contains("://"))
            {
                Uri uriTemp;
                if (Uri.TryCreate(cmd, UriKind.RelativeOrAbsolute, out uriTemp))
                {
                    absURL = uriTemp.IsAbsoluteUri;
                    if (absURL)
                        url = cmd;
                }
            }

            try
            {
                uri = new Uri(url);
            }
            catch (Exception e)
            {
                MainForm.LogExceptionToFile(e);
                return;
            }
            if (uri.Scheme == Uri.UriSchemeFile)
                return;

            if (!absURL)
            {
                url = uri.AbsoluteUri.Replace(uri.PathAndQuery, "/");

                if (ptz.portSpecified && ptz.port > 0)
                {
                    url = url.ReplaceFirst(":" + uri.Port + "/", ":" + ptz.port + "/");
                }

                if (!uri.Scheme.ToLower().StartsWith("http")) //allow http and https
                {
                    url = url.ReplaceFirst(uri.Scheme + "://", "http://");
                }

                url = url.Trim('/');

                if (!cmd.StartsWith("/"))
                {
                    url += _cameraControl.Camobject.settings.ptzurlbase;

                    if (cmd != "")
                    {
                        if (!url.EndsWith("/"))
                        {
                            string ext = "?";
                            if (url.IndexOf("?", StringComparison.Ordinal) != -1)
                                ext = "&";
                            url += ext + cmd;
                        }
                        else
                        {
                            url += cmd;
                        }

                    }
                }
                else
                {
                    url += cmd;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(cmd))
                {
                    if (!cmd.Contains("://"))
                    {
                        if (!url.EndsWith("/"))
                        {
                            string ext = "?";
                            if (url.IndexOf("?", StringComparison.Ordinal) != -1)
                                ext = "&";
                            url += ext + cmd;
                        }
                        else
                        {
                            url += cmd;
                        }
                    }
                    else
                    {
                        url = cmd;
                    }

                }
            }


            string un = _cameraControl.Camobject.settings.login;
            string pwd = _cameraControl.Camobject.settings.password;

            if (!String.IsNullOrEmpty(_cameraControl.Camobject.settings.ptzusername))
            {
                un = _cameraControl.Camobject.settings.ptzusername;
                pwd = _cameraControl.Camobject.settings.ptzpassword;
            }
            else
            {
                if (_cameraControl.Camobject.settings.login == string.Empty)
                {

                    //get from url
                    if (!String.IsNullOrEmpty(uri.UserInfo))
                    {
                        string[] creds = uri.UserInfo.Split(':');
                        if (creds.Length >= 2)
                        {
                            un = creds[0];
                            pwd = creds[1];
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(ptz.AppendAuth))
            {
                if (url.IndexOf("?", StringComparison.Ordinal) == -1)
                    url += "?" + ptz.AppendAuth;
                else
                    url += "&" + ptz.AppendAuth;

            }

            url = url.Replace("[USERNAME]", Uri.EscapeDataString(un));
            url = url.Replace("[PASSWORD]", Uri.EscapeDataString(pwd));
            url = url.Replace("[CHANNEL]", _cameraControl.Camobject.settings.ptzchannel);

            _request = (HttpWebRequest) WebRequest.Create(url);
            _request.Timeout = 5000;
            _request.AllowAutoRedirect = true;
            _request.KeepAlive = true;
            _request.SendChunked = false;
            _request.AllowWriteStreamBuffering = true;
            _request.UserAgent = _cameraControl.Camobject.settings.useragent;
            if (_cameraControl.Camobject.settings.usehttp10)
                _request.ProtocolVersion = HttpVersion.Version10;
            //
            
            //get credentials
            
            // set login and password

            string authInfo = "";
            if (!String.IsNullOrEmpty(un))
            {
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(Uri.EscapeDataString(un) + ":" + Uri.EscapeDataString(pwd)));
                _request.Headers["Authorization"] = "Basic " + authInfo;
            }
            
            string ckies = _cameraControl.Camobject.settings.cookies ?? "";
            if (!String.IsNullOrEmpty(ckies))
            {
                if (!ckies.EndsWith(";"))
                    ckies += ";";
            }
            if (!String.IsNullOrEmpty(ptz.Cookies))
                ckies += ptz.Cookies;

            if (!String.IsNullOrEmpty(ckies))
            {
                ckies = ckies.Replace("[USERNAME]", un);
                ckies = ckies.Replace("[PASSWORD]", pwd);
                ckies = ckies.Replace("[CHANNEL]", _cameraControl.Camobject.settings.ptzchannel);
                ckies = ckies.Replace("[AUTH]", authInfo);
                var myContainer = new CookieContainer();
                string[] coll = ckies.Split(';');
                foreach (var ckie in coll)
                {
                    if (!String.IsNullOrEmpty(ckie))
                    {
                        string[] nv = ckie.Split('=');
                        if (nv.Length == 2)
                        {
                            var cookie = new Cookie(nv[0].Trim(), nv[1].Trim());
                            myContainer.Add(new Uri(_request.RequestUri.ToString()), cookie);
                        }
                    }
                }
                _request.CookieContainer = myContainer;
            }

            if (ptz.POST)
            {
               
                var i = url.IndexOf("?", StringComparison.Ordinal);
                if (i>-1 && i<url.Length)
                {
                    var encoding = new ASCIIEncoding();
                    string postData = url.Substring(i + 1);
                    byte[] data = encoding.GetBytes(postData);

                    _request.Method = "POST";
                    _request.ContentType = "application/x-www-form-urlencoded";
                    _request.ContentLength = data.Length;

                    using (Stream stream = _request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }    
                }
            }


            var myRequestState = new RequestState {Request = _request};
            _request.BeginGetResponse(FinishPTZRequest, myRequestState);
        }

        private void FinishPTZRequest(IAsyncResult result)
        {
            var myRequestState = (RequestState) result.AsyncState;
            WebRequest myWebRequest = myRequestState.Request;
            // End the Asynchronous request.
            try
            {
                myRequestState.Response = myWebRequest.EndGetResponse(result);

#if DEBUG
                using (Stream data = myRequestState.Response.GetResponseStream())
                {
                    if (data != null)
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();
                            //Debug.WriteLine("PTZ Response: "+text);
                        }
                }
#endif

                myRequestState.Response.Close();
            }
            catch(Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            myRequestState.Response = null;
                myRequestState.Request = null;
            
            _request = null;
            if (_nextcommand!="")
            {
                string nc = _nextcommand;
                _nextcommand = "";
                SendPTZCommand(nc);
            }
        }

        #region Nested type: RequestState

        public class RequestState
        {
            // This class stores the request state of the request.
            public WebRequest Request;
            public WebResponse Response;

            public RequestState()
            {
                Request = null;
                Response = null;
            }
        }

        #endregion
    }
}