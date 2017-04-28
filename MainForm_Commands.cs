using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using iSpyApplication.Controls;
using iSpyApplication.Joystick;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    partial class MainForm
    {
        private JoystickDevice _jst;
        private readonly bool[] _buttonsLast = new bool[128];
        private bool _needstop, _sentdirection;

        void TmrJoystickElapsed(object sender, ElapsedEventArgs e)
        {
            if (_shuttingDown)
                return;
            _tmrJoystick.Stop();
            Invoke(new Delegates.RunCheckJoystick(CheckJoystick));
            _tmrJoystick.Start();
        }

        private void CheckJoystick()    {

            if (_jst != null)
            {
                _jst.UpdateStatus();

                CameraWindow cw=null;
                VolumeLevel vl=null;

                foreach (Control c in _pnlCameras.Controls)
                {
                    if (c.Focused)
                    {
                        cw = c as CameraWindow;
                        vl = c as VolumeLevel;
                        break;
                    }
                }

                for (int i = 0; i < _jst.Buttons.Length; i++)
                {

                    if (_jst.Buttons[i] != _buttonsLast[i] && _jst.Buttons[i])
                    {
                        int j = i + 1;

                        if (j == Conf.Joystick.Listen)
                        {
                            if (cw?.VolumeControl != null)
                            {
                                cw.VolumeControl.Listening = !cw.VolumeControl.Listening;
                            }
                            if (vl!=null)
                            {
                                vl.Listening = !vl.Listening;
                            }
                        }

                        if (j == Conf.Joystick.Talk)
                        {
                            if (cw != null)
                            {
                                cw.Talking = !cw.Talking;
                                TalkTo(cw, cw.Talking);
                            }
                        }
                        
                        if (j == Conf.Joystick.Previous)
                        {
                            ProcessKey("previous_control");
                        }

                        if (j == Conf.Joystick.Next)
                        {
                            ProcessKey("next_control");
                        }

                        if (j == Conf.Joystick.Play)
                        {
                            ProcessKey("play");
                        }

                        if (j == Conf.Joystick.Stop)
                        {
                            ProcessKey("stop");
                        }

                        if (j == Conf.Joystick.Record)
                        {
                            ProcessKey("record");
                        }

                        if (j == Conf.Joystick.Snapshot)
                        {
                            cw?.SaveFrame();
                        }

                        if (j == Conf.Joystick.MaxMin)
                        {
                            ProcessKey("maxmin");
                        }

                    }

                    _buttonsLast[i] = _jst.Buttons[i];

                }

                if (cw != null)
                {
                    _sentdirection = false;
                    int x = 0, y = 0;

                    double angle = -1000;

                    if (Conf.Joystick.XAxis < 0)
                    {
                        //dpad - handles x and y
                        int dpad = _jst.Dpads[(0 - Conf.Joystick.XAxis) - 1];
                        switch (dpad)
                        {
                            case 27000:
                                angle = 0;
                                break;
                            case 31500:
                                angle = Math.PI/4;
                                break;
                            case 0:
                                angle = Math.PI/2;
                                break;
                            case 4500:
                                angle = 3*Math.PI/4;
                                break;
                            case 9000:
                                angle = Math.PI;
                                break;
                            case 13500:
                                angle = -3*Math.PI/4;
                                break;
                            case 18000:
                                angle = -Math.PI/2;
                                break;
                            case 22500:
                                angle = -Math.PI/4;
                                break;
                        }
                    }
                    else
                    {
                        if (Conf.Joystick.XAxis > 0)
                        {
                            x = _jst.Axis[Conf.Joystick.XAxis - 1] - Conf.Joystick.CenterXAxis;
                        }

                        if (Conf.Joystick.YAxis > 0)
                        {
                            y = _jst.Axis[Conf.Joystick.YAxis - 1] - Conf.Joystick.CenterYAxis;
                        }

                        var d = Math.Sqrt((x*x) + (y*y));
                        if (d > 20)
                        {
                            angle = Math.Atan2(y, x);
                        }
                    }

                    if (angle > -1000)
                    {
                        if (Conf.Joystick.InvertYAxis)
                        {
                            angle = 0 - angle;
                        }
                        if (Conf.Joystick.InvertXAxis)
                        {
                            if (angle >= 0)
                                angle = Math.PI - angle;
                            else
                                angle = (0 - Math.PI) - angle;
                        }

                        cw.Calibrating = true;
                        cw.PTZ.SendPTZDirection(angle);
                        if (!cw.PTZ.DigitalPTZ)
                            _needstop = _sentdirection = true;
                    }

                    if (Conf.Joystick.ZAxis > 0)
                    {
                        var z = _jst.Axis[Conf.Joystick.ZAxis - 1] - Conf.Joystick.CenterZAxis;

                        if (Math.Abs(z) > 20)
                        {
                            if (Conf.Joystick.InvertZAxis)
                                z = 0-z;
                            cw.Calibrating = true;
                            cw.PTZ.SendPTZCommand(z > 0 ? Enums.PtzCommand.ZoomIn : Enums.PtzCommand.ZoomOut);

                            if (!cw.PTZ.DigitalZoom)
                                _needstop = _sentdirection = true;
                        }
                    }

                    if (!_sentdirection && _needstop)
                    {
                        cw.PTZ.SendPTZCommand(Enums.PtzCommand.Stop);
                        _needstop = false;
                    }
                }



            }
        }

        public void SwitchObjects(bool scheduledOnly, bool on)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    if (on && !cameraControl.IsEnabled)
                    {
                        if (!scheduledOnly)
                            cameraControl.Enable();
                    }

                    if (!on && cameraControl.IsEnabled)
                    {
                        if (!scheduledOnly)
                            cameraControl.Disable();
                    }
                }
            }
            foreach (Control c in _pnlCameras.Controls)
            {
                var level = c as VolumeLevel;
                if (level != null)
                {
                    var volumeControl = level;

                    if (on && !volumeControl.IsEnabled)
                    {
                        if (!scheduledOnly)
                            volumeControl.Enable();
                    }

                    if (!on && volumeControl.IsEnabled)
                    {
                        if (!scheduledOnly)
                            volumeControl.Disable();
                    }
                }
            }
        }

        public void RecordOnDetect(bool on)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    cameraControl.Camobject.detector.recordondetect = on;
                    if (on && cameraControl.Camobject.detector.recordonalert)
                        cameraControl.Camobject.detector.recordonalert = false;
                    continue;
                }
                var level = c as VolumeLevel;
                if (level == null) continue;
                var volumeControl = level;
                volumeControl.Micobject.detector.recordondetect = @on;
                if (@on && volumeControl.Micobject.detector.recordonalert)
                    volumeControl.Micobject.detector.recordonalert = false;
            }
        }

        public void SnapshotAll()
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    if (cameraControl.Camobject.settings.active)
                        cameraControl.SaveFrame();
                }
            }
        }

        public void RecordOnAlert(bool on)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    cameraControl.Camobject.detector.recordonalert = on;
                    if (on && cameraControl.Camobject.detector.recordondetect)
                        cameraControl.Camobject.detector.recordondetect = false;
                    continue;
                }
                var level = c as VolumeLevel;
                if (level == null) continue;
                var volumeControl = level;
                volumeControl.Micobject.detector.recordonalert = @on;
                if (@on && volumeControl.Micobject.detector.recordondetect)
                    volumeControl.Micobject.detector.recordondetect = false;
            }
        }

        public void AlertsActive(bool on)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    cameraControl.Camobject.alerts.active = on;
                    continue;
                }
                var level = c as VolumeLevel;
                if (level == null) continue;
                var volumeControl = level;
                volumeControl.Micobject.alerts.active = @on;
            }
        }

        public void RecordAll(bool record)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    if (cameraControl.IsEnabled)
                        cameraControl.RecordSwitch(record);
                    continue;
                }
                var level = c as VolumeLevel;
                if (level == null) continue;
                var volumeControl = level;
                if (volumeControl.IsEnabled)
                    volumeControl.RecordSwitch(record);
            }
        }

        private void ShowRemoteCommands()
        {
            var ma = new RemoteCommands { Owner = this };
            ma.ShowDialog(this);
            ma.Dispose();
            LoadCommands();
        }

        public static objectsCommand[] GenerateRemoteCommands()
        {
            //copy over 
            var lcom = new List<objectsCommand>();
            var cmd = new objectsCommand
            {
                command = "ispy ALLON",
                id = 0,
                name = "cmd_SwitchAllOn",
            };

            lcom.Add(cmd);

            cmd = new objectsCommand
            {
                command = "ispy ALLOFF",
                id = 1,
                name = "cmd_SwitchAllOff",
            };
            lcom.Add(cmd);

            cmd = new objectsCommand
            {
                command = "ispy APPLYSCHEDULE",
                id = 2,
                name = "cmd_ApplySchedule",
            };
            lcom.Add(cmd);

            if (Helper.HasFeature(Enums.Features.Recording))
            {
                cmd = new objectsCommand
                      {
                          command = "ispy RECORDONDETECTON",
                          id = 3,
                          name = "cmd_RecordOnDetectAll",
                      };
                lcom.Add(cmd);

                cmd = new objectsCommand
                      {
                          command = "ispy RECORDONALERTON",
                          id = 4,
                          name = "cmd_RecordOnAlertAll",
                      };
                lcom.Add(cmd);

                cmd = new objectsCommand
                      {
                          command = "ispy RECORDINGOFF",
                          id = 5,
                          name = "cmd_RecordOffAll",
                      };
                lcom.Add(cmd);

                cmd = new objectsCommand
                {
                    command = "ispy RECORD",
                    id = 8,
                    name = "cmd_RecordAll",
                };
                lcom.Add(cmd);

                cmd = new objectsCommand
                {
                    command = "ispy RECORDSTOP",
                    id = 9,
                    name = "cmd_RecordAllStop",
                };
                lcom.Add(cmd);
            }

            cmd = new objectsCommand
            {
                command = "ispy ALERTON",
                id = 6,
                name = "cmd_AlertsOnAll",
            };
            lcom.Add(cmd);

            cmd = new objectsCommand
            {
                command = "ispy ALERTOFF",
                id = 7,
                name = "cmd_AlertsOffAll",
            };
            lcom.Add(cmd);

            if (Helper.HasFeature(Enums.Features.Save_Frames))
            {

                cmd = new objectsCommand
                      {
                          command = "ispy SNAPSHOT",
                          id = 10,
                          name = "cmd_SnapshotAll",
                      };
                lcom.Add(cmd);
            }
            return lcom.ToArray();
        }

        public void RunCommand(int commandIndex)
        {
            objectsCommand oc = RemoteCommands.FirstOrDefault(p => p.id == commandIndex);

            if (oc != null)
            {
                if (!string.IsNullOrEmpty(oc.command))
                    RunCommand(oc.command);
                if (!string.IsNullOrEmpty(oc.emitshortcut))
                {
                    var converter = new KeysConverter();
                    var keys = converter.ConvertFromString(oc.emitshortcut);
                    if (keys != null)
                    {
                        var shortcutKeys = (Keys)keys;
                        MainForm_KeyDown(this, new KeyEventArgs(shortcutKeys));
                    }
                }
            }
        }

        internal void RunCommand(string command)
        {
            try
            {
                if (command.ToLower().StartsWith("ispy ") || command.ToLower().StartsWith("ispy.exe "))
                {
                    string cmd2 = command.Substring(command.IndexOf(" ", StringComparison.Ordinal) + 1).ToLower().Trim();
                    if (cmd2.StartsWith("commands "))
                        cmd2 = cmd2.Substring(cmd2.IndexOf(" ", StringComparison.Ordinal) + 1).Trim();

                    string cmd = cmd2.Trim('"');
                    string[] commands = cmd.Split('|');
                    foreach (string command2 in commands)
                    {
                        if (command2 != "")
                        {
                            if (InvokeRequired)
                                Invoke(new Delegates.ExternalCommandDelegate(ProcessCommandInternal), command2.Trim('"'));
                            else
                                ProcessCommandInternal(command2.Trim('"'));
                        }
                    }
                }
                else
                    Process.Start(command);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }

        private Control GetActiveControl(out int index)
        {
            int i = 0;
            foreach (Control c in _pnlCameras.Controls)
            {
                if (c.Equals(LastFocussedControl))
                {
                    index = i;
                    return c;
                }
                i++;
            }
            if (_pnlCameras.Controls.Count > 0)
            {
                _pnlCameras.Controls[0].Focus();
                index = 0;
                return _pnlCameras.Controls[0];
            }
            index = -1;
            return null;

        }

        public void ProcessKey(string keycommand)
        {
            //non-specific commands
            switch (keycommand.ToLower())
            {
                case "standby":
                case "back":
                case "power":
                    Close();
                    return;
                case "import":
                    using (var imp = new Importer())
                    {
                        imp.ShowDialog(this);
                    }
                    return;
            }
            int i;
            var c = GetActiveControl(out i);
            if (i == -1)
                return;
            

            var cw = c as CameraWindow;
            var vl = c as VolumeLevel;
            var fp = c as FloorPlanControl;

            switch (keycommand.ToLower())
            {
                case "channelup":
                case "nexttrack":
                case "next_control":
                    i++;
                    if (i == _pnlCameras.Controls.Count)
                        i = 0;
                    _pnlCameras.Controls[i].Focus();
                    break;
                case "channeldown":
                case "previoustrack":
                case "previous_control":
                    i--;
                    if (i == -1)
                        i = _pnlCameras.Controls.Count - 1;
                    _pnlCameras.Controls[i].Focus();
                    break;
                case "play":
                case "pause":
                    if (cw != null)
                    {
                        if (cw.Camobject.settings.active)
                        {
                            _pnlCameras.Maximise(cw);
                        }
                        else
                            cw.Enable();
                    }
                    if (vl != null)
                    {
                        if (vl.Micobject.settings.active)
                        {
                            _pnlCameras.Maximise(vl);
                        }
                        else
                            vl.Enable();
                    }
                    break;
                case "stop":
                    cw?.Disable();
                    vl?.Disable();
                    break;
                case "record":
                    cw?.RecordSwitch(!((CameraWindow)c).Recording);
                    vl?.RecordSwitch(!((VolumeLevel)c).Recording);
                    break;
                case "maxmin":
                case "zoom":
                    if (c is CameraWindow || c is VolumeLevel || c is FloorPlanControl)
                    {
                        _pnlCameras.Maximise(c);
                    }
                    break;
                case "delete":
                    if (cw != null)
                    {
                        RemoveCamera(cw,true);
                    }
                    if (vl != null)
                    {
                        RemoveMicrophone(vl, true);
                    }
                    if (fp != null)
                    {
                        RemoveFloorplan(fp, true);
                    }
                    break;
                case "talk":
                    
                    if (cw!=null)
                    {
                        cw.Talking = !cw.Talking;
                        TalkTo(cw, cw.Talking);
                    }
                    break;
                case "listen":
                    cw?.Listen();
                    vl?.Listen();
                    break;
                case "grab":
                    cw?.Snapshot();
                    break;
                case "edit":
                    if (cw != null)
                        EditCamera(cw.Camobject);
                    if (vl!=null)
                        EditMicrophone(vl.Micobject);
                    if (fp!=null)
                        EditFloorplan(fp.Fpobject);

                    break;
                case "tags":
                    if (cw != null)
                    {
                        using (TagConfigure tc = new TagConfigure { TagsNV = cw.Camobject.settings.tagsnv, Owner = this })
                        {
                            if (tc.ShowDialog() == DialogResult.OK)
                            {
                                cw.Camobject.settings.tagsnv = tc.TagsNV;
                                if (cw.Camera!=null)
                                    cw.Camera.Tags = null;
                            }
                        }
                    }
                    break;
            }
        }
    }
}
