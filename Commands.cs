using System.Linq;
using iSpyApplication.Objects;

namespace iSpyApplication
{
    public static class Commands
    {

        public static void SwitchObjects(bool on)
        {
            foreach (var ctrl in Statics.Controls)
            {
                if (on)
                {
                    ctrl.Enable();
                }
                else
                {
                    ctrl.Disable();
                }
            }

        }

        public static void ApplySchedule()
        {
            foreach (var ctrl in Statics.Controls)
            {
                ctrl.ApplySchedule();
            }

        }


        public static void SetFlag(string flag, int otid, int oid)
        {
            var lc =
                Statics.Controls.Where(p => (otid == 0 || p.ObjectTypeID == otid) && (oid == 0 || p.ObjectID == oid))
                    .ToList();


            foreach (var c in lc)
            {
                var vl = c as Microphone;
                var cw = c as Camera;
                switch (flag.ToLowerInvariant())
                {
                    case "recordondetecton":
                        if (vl != null)
                        {
                            vl.MicrophoneObject.detector.recordondetect = true;
                            vl.MicrophoneObject.detector.recordonalert = false;
                        }
                        if (cw != null)
                        {
                            cw.CameraObject.detector.recordondetect = true;
                            cw.CameraObject.detector.recordonalert = false;
                        }
                        break;
                    case "recordonalerton":
                        if (vl != null)
                        {
                            vl.MicrophoneObject.detector.recordondetect = false;
                            vl.MicrophoneObject.detector.recordonalert = true;
                        }
                        if (cw != null)
                        {
                            cw.CameraObject.detector.recordondetect = false;
                            cw.CameraObject.detector.recordonalert = true;
                        }
                        break;
                    case "recordingoff":
                        if (vl != null)
                        {
                            vl.MicrophoneObject.detector.recordondetect = false;
                            vl.MicrophoneObject.detector.recordonalert = false;
                        }
                        if (cw != null)
                        {
                            cw.CameraObject.detector.recordondetect = false;
                            cw.CameraObject.detector.recordonalert = false;
                        }
                        break;
                    case "record":
                        c.RecordSwitch(true);
                        break;
                    case "recordstop":
                        c.RecordSwitch(false);
                        break;
                    case "alerton":
                        if (vl != null)
                        {
                            vl.MicrophoneObject.alerts.active = true;
                        }
                        if (cw != null)
                        {
                            cw.CameraObject.alerts.active = true;
                        }
                        break;
                    case "alertoff":
                        if (vl != null)
                        {
                            vl.MicrophoneObject.alerts.active = false;
                        }
                        if (cw != null)
                        {
                            cw.CameraObject.alerts.active = false;
                        }
                        break;
                    case "snapshot":
                        cw?.SaveFrame();
                        break;
                }
            }
        }
    }
}
