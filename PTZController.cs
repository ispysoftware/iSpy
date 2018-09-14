using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iSpyApplication.Controls;
using iSpyApplication.OnvifServices;
using iSpyApplication.Pelco;
using iSpyApplication.Sources.Video;
using iSpyApplication.Utilities;
using iSpyPRO.DirectShow;
using DateTime = System.DateTime;
using Rectangle = System.Drawing.Rectangle;
using SerialPort = System.IO.Ports.SerialPort;

namespace iSpyApplication
{
    public class PTZController
    {
        readonly ConnectionFactory _connectionFactory = new ConnectionFactory();
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
        private Enums.PtzCommand _lastCommand = Enums.PtzCommand.Stop;

        public void CheckSendStop()
        {
            if (IsContinuous || !string.IsNullOrEmpty(PTZSettings?.Commands.Stop))
                SendPTZCommand(Enums.PtzCommand.Stop);
        }
        public bool IsContinuous
        {
            get
            {
                if (_cameraControl?.Camobject.ptz < -2)
                {
                    //onvif/pelco-p/pelco-d
                    return true;
                }
                return false;
            }
        }

        private uint _addr;

        public void ConfigurePelco()
        {
            if (_serialPort != null)
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
                { WriteTimeout = 2000, ReadTimeout = 2000 };
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            _addr = Convert.ToUInt16(cfg[5]);
        }

        //private HttpWebRequest _request;
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
            set
            {
                _ptzSettings = value;
                _ptzNull = _ptzSettings == null;
            }
        }

        internal bool DigitalPTZ => PTZSettings == null;


        public PTZController(CameraWindow cameraControl)
        {
            _cameraControl = cameraControl;
        }

        public void AddPreset(string name, string presetToken)
        {
            try
            {
                if (PTZToken != null)
                {
                    var ptz = _cameraControl.ONVIFDevice?.PTZ;
                    if (ptz != null)
                    {
                        var ptzSetRequest = new SetPresetRequest(PTZToken, name, presetToken);
                        var r = ptz?.SetPreset(ptzSetRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void DeletePreset(string presetToken)
        {
            try
            {
                if (PTZToken != null)
                {
                    var ptz = _cameraControl.ONVIFDevice?.PTZ;
                    if (ptz != null)
                    {
                        ptz?.RemovePreset(PTZToken, presetToken);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void ResetONVIF()
        {
            _ptzSettings = null;
        }

        public void SendPTZDirection(double angle)
        {
            if (_cameraControl.Camobject.settings.ptzrotate90)
            {
                angle -= (Math.PI / 2);
                if (angle < -Math.PI)
                {
                    angle += (2 * Math.PI);
                }
            }
            if (_cameraControl.Camobject.ptz != -1)
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

                if (IsContinuous)
                {
                    if (cmd != Enums.PtzCommand.Stop)
                    {
                        //ignore - continuous
                        if (_lastCommand == cmd)
                            return;
                    }
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

            if (string.IsNullOrEmpty(command)) //some PTZ cameras don't have diagonal controls, this fixes that
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

                                if (string.IsNullOrEmpty(ptz.Commands.ZoomIn))
                                    d = true;
                                if (string.IsNullOrEmpty(ptz.Commands.ZoomOut))
                                    d = true;
                            }
                            return d;
                        }
                }
            }
        }


        public void SendPTZCommand(Enums.PtzCommand command)
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
                    if (string.IsNullOrEmpty(ptz.Commands.ZoomIn))
                        d = true;
                }
                if (command == Enums.PtzCommand.ZoomOut)
                {
                    if (string.IsNullOrEmpty(ptz.Commands.ZoomOut))
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
                    case Enums.PtzCommand.ZoomIn:
                        SendPTZCommand(ptz.Commands.ZoomIn);
                        break;
                    case Enums.PtzCommand.ZoomOut:
                        SendPTZCommand(ptz.Commands.ZoomOut);
                        break;
                    case Enums.PtzCommand.Center:
                        SendPTZCommand(ptz.Commands.Center);
                        break;
                    case Enums.PtzCommand.Stop:
                        if (_previousCommand == Enums.PtzCommand.ZoomIn)
                        {
                            if (!string.IsNullOrEmpty(ptz.Commands.ZoomInStop))
                            {
                                SendPTZCommand(ptz.Commands.ZoomInStop);
                                break;
                            }
                        }
                        if (_previousCommand == Enums.PtzCommand.ZoomOut)
                        {
                            if (!string.IsNullOrEmpty(ptz.Commands.ZoomOutStop))
                            {
                                SendPTZCommand(ptz.Commands.ZoomOutStop);
                                break;
                            }
                        }
                        SendPTZCommand(ptz.Commands.Stop);
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
                    Logger.LogException(ex);
                }
            }
        }

        void IAMMove(VideoCaptureDevice d, CameraControlProperty p, int i)
        {
            int v, minv, maxv, stepSize, defVal;
            CameraControlFlags f, cf;
            d.GetCameraProperty(p, out v, out f);
            d.GetCameraPropertyRange(p, out minv, out maxv, out stepSize, out defVal, out cf);

            int newv = v + i * stepSize;
            if (newv < minv)
                newv = minv;
            if (newv > maxv)
                newv = maxv;

            if (i == 0)
                newv = defVal;

            if (cf == CameraControlFlags.Manual)
            {
                d.SetCameraProperty(p, newv, CameraControlFlags.Manual);
            }
            else
            {
                Logger.LogMessage("Camera control flags are not manual");
            }

        }

        private Enums.PtzCommand _lastOnvifCommand = Enums.PtzCommand.Center;
        private DateTime _lastOnvifCommandSent = DateTime.MinValue;

        void ProcessOnvif(Enums.PtzCommand command)
        {
            if (!_cameraControl.ONVIFConnected)
                return;

            if (command == _lastOnvifCommand && _lastOnvifCommandSent > DateTime.UtcNow.AddSeconds(-4))
                return;
            _lastOnvifCommand = command;
            _lastOnvifCommandSent = DateTime.UtcNow;
            
            var ptz = _cameraControl?.ONVIFDevice?.PTZ;
            if (ptz != null)
            {
                //var speed = PTZProfile.ptzConfiguration.defaultPTZSpeed;
                //string spacePT = PTZProfile.ptzConfiguration.defaultContinuousPanTiltVelocitySpace;
                //string spaceZ = PTZProfile.ptzConfiguration.defaultContinuousZoomVelocitySpace;

                Vector2D panTilt = null;
                Vector1D zoom = null;
                try
                {
                    _lastCommand = command;
                    switch (command)
                    {
                        case Enums.PtzCommand.Left:
                            panTilt = new Vector2D { space = null, x = -0.5f, y = 0 };
                            break;
                        case Enums.PtzCommand.Upleft:
                            panTilt = new Vector2D { space = null, x = -0.5f, y = 0.5f };
                            break;
                        case Enums.PtzCommand.Up:
                            panTilt = new Vector2D { space = null, x = 0, y = 0.5f };
                            break;
                        case Enums.PtzCommand.UpRight:
                            panTilt = new Vector2D { space = null, x = 0.5f, y = 0.5f };
                            break;
                        case Enums.PtzCommand.Right:
                            panTilt = new Vector2D { space = null, x = 0.5f, y = 0 };
                            break;
                        case Enums.PtzCommand.DownRight:
                            panTilt = new Vector2D { space = null, x = 0.5f, y = -0.5f };
                            break;
                        case Enums.PtzCommand.Down:
                            panTilt = new Vector2D { space = null, x = 0, y = -0.5f };
                            break;
                        case Enums.PtzCommand.DownLeft:
                            panTilt = new Vector2D { space = null, x = -0.5f, y = -0.5f };
                            break;
                        case Enums.PtzCommand.ZoomIn:
                            zoom = new Vector1D { space = null, x = 1f };
                            break;
                        case Enums.PtzCommand.ZoomOut:
                            zoom = new Vector1D { space = null, x = -1f };
                            break;
                        case Enums.PtzCommand.Center:
                            //ProcessOnvifCommand(_cameraControl.CameraObject.settings.ptzautohomecommand);
                            return;
                        case Enums.PtzCommand.Stop:
                            ptz.Stop(PTZToken, true, true);
                            return;
                    }
                    var ptzSpeed = new PTZSpeed { PanTilt = panTilt, Zoom = zoom };
                    var cmr = new ContinuousMoveRequest(PTZToken, ptzSpeed, "PT10S");
                    ptz.ContinuousMoveAsync(cmr);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        void ProcessOnvifCommand(string ptzToken)
        {
            if (PTZToken != null)
            {
                try
                {
                    var ptz = _cameraControl.ONVIFDevice?.PTZ;
                    if (ptz != null)
                    {
                        ptz.GotoPresetAsync(PTZToken, ptzToken, null);
                    }
                    
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }

            }
        }

        private string PTZToken
        {
            get {
                if (!_cameraControl.ONVIFConnected)
                    return null;
                return _cameraControl?.ONVIFDevice?.Profile?.token;
            }
        }

        public PTZPreset[] ONVIFPresets
        {
            get
            {
                try
                {
                    if (PTZToken != null)
                    {
                        var ptz = _cameraControl?.ONVIFDevice?.PTZ;
                        if (ptz != null)
                        {
                            var gpr = new GetPresetsRequest(PTZToken);
                            return ptz.GetPresets(gpr).Preset;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                return new PTZPreset[] { };
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
                        Logger.LogException(ex);
                        return;
                    }
                    //SerialPort.DataReceived += SerialPort_DataReceived;
                }
            }
            if (_serialPort == null || !_serialPort.IsOpen)
                return;

            _lastCommand = command;

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
                        Logger.LogException(ex);
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
                        SendPelco(pelcoP.Pattern(_addr, P.PatternAction.Stop));
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
                        SendPelco(pelcoP.Zone(_addr, 1, P.Action.Start));
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
                        SendPelco(pelcoP.ZoneScan(_addr, P.Action.Start));
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
            _serialPort.Write(arr, 0, arr.Length);
        }

        public void SendPTZCommand(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
                return;

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

            UriBuilder uri;
            string urltemp = _cameraControl.Camobject.settings.videosourcestring;

            string ub = _cameraControl.Camobject.settings.ptzurlbase;

            if (ub != null && ub.StartsWith("http", true, CultureInfo.InvariantCulture))
            {
                urltemp = ub.Trim();
            }

            bool absURL = false;
            if (cmd.StartsWith("http", true, CultureInfo.InvariantCulture))
            {
                urltemp = cmd;
                absURL = true;
            }


            try
            {
                uri = new UriBuilder(urltemp);
            }
            catch (Exception e)
            {
                Logger.LogException(e, "PTZ Controller");
                return;
            }

            if (uri.Scheme == Uri.UriSchemeFile)
                return;


            uri.Port = _cameraControl.Camobject.settings.ptzport;

            if (!uri.Scheme.ToLower().StartsWith("http")) //allow http and https
            {
                uri.Scheme = "http";
            }

            if (!absURL)
            {
                string pandq = ptz.CommandURL;
                if (cmd.StartsWith("/") || string.IsNullOrEmpty(pandq))
                    pandq = cmd;
                else
                {
                    if (!string.IsNullOrEmpty(cmd))
                    {
                        string ext = "";
                        if (!pandq.EndsWith("/"))
                        {
                            ext = pandq.IndexOf("?", StringComparison.Ordinal) != -1 ? "&" : "?";
                        }
                        pandq += ext + cmd;
                    }
                }
                int i = pandq.IndexOf("?", StringComparison.Ordinal);
                if (i == -1 || i == pandq.Length)
                    uri.Path = pandq;
                else
                {
                    uri.Path = pandq.Substring(0, i);
                    uri.Query = pandq.Substring(i + 1);
                }
            }

            string un = _cameraControl.Camobject.settings.login;
            string pwd = _cameraControl.Camobject.settings.password;

            if (!string.IsNullOrEmpty(_cameraControl.Camobject.settings.ptzusername))
            {
                un = _cameraControl.Camobject.settings.ptzusername;
                pwd = _cameraControl.Camobject.settings.ptzpassword;
            }
            else
            {
                if (string.IsNullOrEmpty(_cameraControl.Camobject.settings.login))
                {
                    //get from url
                    un = uri.UserName;
                    pwd = uri.Password;
                }
            }

            uri.UserName = "[USERNAME]";
            uri.Password = "[PASSWORD]";


            if (!string.IsNullOrEmpty(ptz.AppendAuth))
            {
                string aurl = ptz.AppendAuth.Replace("[USERNAME]", Uri.EscapeDataString(un));
                aurl = aurl.Replace("[PASSWORD]", Uri.EscapeDataString(pwd));

                if (uri.Query == "")
                    uri.Query = aurl;
                else
                    uri.Query = uri.Query.Trim('?') + "&" + aurl;
            }


            string url = uri.ToString().Replace("%5B", "[");
            url = url.Replace("%5D", "]");
            url = url.Replace("[USERNAME]", Uri.EscapeDataString(un));
            url = url.Replace("[PASSWORD]", Uri.EscapeDataString(pwd));
            url = url.Replace("[CHANNEL]", _cameraControl.Camobject.settings.ptzchannel);

            byte[] data = {};
            if (ptz.Method != "GET")
            {
                var j = url.IndexOf("?", StringComparison.Ordinal);
                if (j > -1 && j < url.Length)
                {
                    var pd = url.Substring(j + 1);
                    var encoding = new ASCIIEncoding();
                    data = encoding.GetBytes(pd);
                    url = url.Substring(0, j);
                }
            }
            string ckies = _cameraControl.Camobject.settings.cookies;
            if (!string.IsNullOrEmpty(ptz.Cookies))
                ckies = ptz.Cookies;

            ckies = ckies.Replace("[USERNAME]", un);
            ckies = ckies.Replace("[PASSWORD]", pwd);
            ckies = ckies.Replace("[CHANNEL]", _cameraControl.Camobject.settings.ptzchannel);


            var co = new ConnectionOptions
            {
                channel = _cameraControl.Camobject.settings.ptzchannel,
                cookies = ckies,
                headers = _cameraControl.Camobject.settings.headers,
                method = ptz.Method,
                password = pwd,
                username = un,
                proxy = null,
                requestTimeout = 5000,
                source = url,
                data = data,
                useHttp10 = _cameraControl.Camobject.settings.usehttp10,
                useSeparateConnectionGroup = false,
                userAgent = _cameraControl.Camobject.settings.useragent
            };


            Task.Run(() => _connectionFactory.BeginGetResponse(co, CoCallback));
        }

        public void CoCallback(object sender, EventArgs e)
        {
            if (_nextcommand != "")
            {
                string nc = _nextcommand;
                _nextcommand = "";
                Thread.Sleep(100);
                
                SendPTZCommand(nc);
            }
        }
    }
}