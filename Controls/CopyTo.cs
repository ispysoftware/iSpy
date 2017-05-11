using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class CopyTo : Form
    {
        public objectsCamera OC;
        public objectsMicrophone OM;
        private object[] cameraSettings = {
                                        "Groups", 
                                        "Transform",
                                        "Timestamp",
                                        "Mask Image",
                                        "Frame Rates",
                                        "Motion Detector Settings",
                                        "Motion Detector Zones",
                                        "Alert Intervals",
                                        "Actions: Alert",
                                        "Actions: Connection Lost",
                                        "Actions: Reconnect",
                                        "Recording Mode",
                                        "Recording Settings (excluding triggers)",
                                        "Timelapse Settings",
                                        "Image Settings",
                                        "FTP Upload Settings",
                                        "Schedule",
                                        "Storage Settings (excluding directory)"
                                    };
        private object[] micSettings = {
                                        "Groups", 
                                        "Sound Detector Settings",
                                        "Alert Intervals",
                                        "Actions: Alert",
                                        "Actions: Connection Lost",
                                        "Actions: Reconnect",
                                        "Recording Mode",
                                        "Recording Settings (excluding triggers)",
                                        "Schedule",
                                        "Storage Settings (excluding directory)"
                                    };
        public CopyTo()
        {
            InitializeComponent();

            Text = LocRm.GetString("Copy");
            label1.Text = LocRm.GetString("Settings");
            label2.Text = LocRm.GetString("To");
            button1.Text = LocRm.GetString("OK");

        }

        private void CopyTo_Load(object sender, EventArgs e)
        {
            var o = cameraSettings;
            if (OC == null)
                o = micSettings;
            clbSettings.Items.AddRange(o);

            if (OC != null)
            {
                var lc = MainForm.Cameras.OrderBy(p => p.name).ToList();
                foreach (var c in lc)
                {
                    if (c.id != OC.id)
                    {
                        clbObjects.Items.Add(new MainForm.ListItem(c.name, c.id));
                    }
                }
            
            }
            if (OM != null)
            {
                var lc = MainForm.Microphones.OrderBy(p => p.name).ToList();
                foreach (var c in lc)
                {
                    if (c.id != OM.id)
                    {
                        clbObjects.Items.Add(new MainForm.ListItem(c.name, c.id));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var i = clbObjects.CheckedItems.Count;
            var j = clbSettings.CheckedItems.Count;
            if (i == 0 || j == 0)
            {
                MessageBox.Show(this, LocRm.GetString("PleaseSelect"));
                return;
            }

            foreach (MainForm.ListItem li in clbObjects.CheckedItems)
            {
                if (OC != null)
                {
                    var oc = MainForm.Cameras.FirstOrDefault(p => p.id == (int)li.Value);
                    if (oc != null)
                    {
                        foreach (string s in clbSettings.CheckedItems)
                        {
                            SetCam(s, oc);
                        }
                    }
                }
                if (OM != null)
                {
                    var om = MainForm.Microphones.FirstOrDefault(p => p.id == (int)li.Value);
                    if (om != null)
                    {
                        foreach (string s in clbSettings.CheckedItems)
                        {
                            SetMic(s, om);
                        }
                    }
                }

            }
            MessageBox.Show(this, LocRm.GetString("OK"));
        }

        private void CopyActions(int OID, int oid, int otid, string mode)
        {
            MainForm.Actions.RemoveAll(p => p.objectid == oid && p.objecttypeid == otid && p.mode==mode);
            var l = MainForm.Actions.Where(p => p.objectid == OID && p.objecttypeid == otid && p.mode==mode).ToList();
            foreach (var oa in l)
            {
                var oae = new objectsActionsEntry
                {
                    mode = mode,
                    objectid = oid,
                    objecttypeid = otid,
                    type = oa.type,
                    param1 = oa.param1,
                    param2 = oa.param2,
                    param3 = oa.param3,
                    param4 = oa.param4,
                    ident = Guid.NewGuid().ToString()
                };
                MainForm.Actions.Add(oae);
            }
        }

        private void SetCam(string s, objectsCamera oc)
        {
            switch (s)
            {
                case "Groups":
                    oc.settings.accessgroups = OC.settings.accessgroups;
                    break;
                case "Transform":
                    oc.rotateMode = OC.rotateMode;
                    break;
                case "Timestamp":
                    oc.settings.timestampbackcolor = OC.settings.timestampbackcolor;
                    oc.settings.timestampfont = OC.settings.timestampfont;
                    oc.settings.timestampfontsize = OC.settings.timestampfontsize;
                    oc.settings.timestampforecolor = OC.settings.timestampforecolor;
                    oc.settings.timestampformatter = OC.settings.timestampformatter;
                    oc.settings.timestamplocation = OC.settings.timestamplocation;
                    oc.settings.timestampoffset = OC.settings.timestampoffset;
                    oc.settings.timestampshowback = OC.settings.timestampshowback;
                    break;
                case "Mask Image":
                    oc.settings.maskimage = OC.settings.maskimage;
                    break;
                case "Frame Rates":
                    oc.settings.maxframerate = OC.settings.maxframerate;
                    oc.settings.framerate = OC.settings.framerate;
                    break;
                case "Motion Detector Settings":
                    oc.detector.autooff = OC.detector.autooff;
                    oc.detector.calibrationdelay = OC.detector.autooff;
                    oc.detector.color = OC.detector.color;
                    oc.detector.colourprocessing = OC.detector.colourprocessing;
                    oc.detector.colourprocessingenabled = OC.detector.colourprocessingenabled;
                    oc.detector.gain = OC.detector.gain;
                    oc.detector.highlight = OC.detector.highlight;
                    oc.detector.keepobjectedges = OC.detector.keepobjectedges;
                    oc.detector.maxsensitivity = OC.detector.maxsensitivity;
                    oc.detector.minheight = OC.detector.minheight;
                    oc.detector.minsensitivity = OC.detector.minsensitivity;
                    oc.detector.minwidth = OC.detector.minwidth;
                    oc.detector.movementintervalnew = OC.detector.movementintervalnew;
                    oc.detector.nomovementintervalnew = OC.detector.nomovementintervalnew;
                    oc.detector.postprocessor = OC.detector.postprocessor;
                    oc.detector.processframeinterval = OC.detector.processframeinterval;
                    oc.detector.type = OC.detector.type;
                    oc.detector.sensitivity = OC.detector.sensitivity;
                    oc.detector.postprocessor = OC.detector.postprocessor;
                    oc.settings.suppressnoise = OC.settings.suppressnoise;
                    break;
                case "Motion Detector Zones":
                    oc.detector.motionzones = OC.detector.motionzones.ToArray();
                    break;
                case "Alert Settings":
                    oc.alerts.active = OC.alerts.active;
                    oc.alerts.mode = OC.alerts.mode;
                    oc.alerts.groupname = OC.alerts.groupname;
                    oc.alerts.resetinterval = OC.alerts.resetinterval;
                    oc.alerts.minimuminterval = OC.alerts.minimuminterval;
                    break;
                case "Actions: Alert":
                {
                    CopyActions(OC.id, oc.id, 2, "alert");
                }
                    break;
                case "Actions: Connection Lost":
                {
                    CopyActions(OC.id, oc.id, 2, "disconnect");
                }
                    break;
                case "Actions: Reconnect":
                {
                    CopyActions(OC.id, oc.id, 2, "reconnect");
                }
                    break;
                case "Recording Mode":
                    oc.detector.recordonalert = OC.detector.recordonalert;
                    oc.detector.recordondetect = OC.detector.recordondetect;
                    break;
                case "Recording Settings (excluding triggers)":
                    oc.recorder.bufferseconds = OC.recorder.bufferseconds;
                    oc.recorder.crf = OC.recorder.crf;
                    oc.recorder.inactiverecord = OC.recorder.inactiverecord;
                    oc.recorder.maxrecordtime = OC.recorder.maxrecordtime;
                    oc.recorder.minrecordtime = OC.recorder.minrecordtime;
                    oc.recorder.profile = OC.recorder.profile;
                    oc.recorder.quality = OC.recorder.quality;
                    break;
                case "Timelapse Settings":
                    oc.recorder.timelapse = OC.recorder.timelapse;
                    oc.recorder.timelapseenabled = OC.recorder.timelapseenabled;
                    oc.recorder.timelapseframerate = OC.recorder.timelapseframerate;
                    oc.recorder.timelapseframes = OC.recorder.timelapseframes;
                    oc.recorder.timelapsesave = OC.recorder.timelapsesave;
                    break;
                case "Image Settings":
                    oc.savelocal.enabled = OC.savelocal.enabled;
                    oc.savelocal.filename = OC.savelocal.filename;
                    oc.savelocal.countermax = OC.savelocal.countermax;
                    oc.savelocal.intervalnew = OC.savelocal.intervalnew;
                    oc.savelocal.minimumdelay = OC.savelocal.minimumdelay;
                    oc.savelocal.mode = OC.savelocal.mode;
                    oc.savelocal.quality = OC.savelocal.quality;
                    oc.savelocal.text = OC.savelocal.text;
                    break;
                case "FTP Upload Settings":
                    oc.ftp.server = OC.ftp.server;
                    oc.ftp.countermax = OC.ftp.countermax;
                    oc.ftp.enabled = OC.ftp.enabled;
                    oc.ftp.filename = OC.ftp.filename;
                    oc.ftp.intervalnew = OC.ftp.intervalnew;
                    oc.ftp.minimumdelay = OC.ftp.minimumdelay;
                    oc.ftp.quality = OC.ftp.quality;
                    oc.ftp.text = OC.ftp.text;
                    break;
                case "Schedule":
                {
                    oc.schedule.entries = OC.schedule.entries.ToArray();
                    oc.schedule.active = OC.schedule.active;
                }
                    break;
                case "Storage Settings (excluding directory)":
                    oc.settings.storagemanagement.archive = OC.settings.storagemanagement.archive;
                    oc.settings.storagemanagement.enabled = OC.settings.storagemanagement.enabled;
                    oc.settings.storagemanagement.maxage = OC.settings.storagemanagement.maxage;
                    oc.settings.storagemanagement.maxsize = OC.settings.storagemanagement.maxsize;
                    break;
            }
        }

        private void SetMic(string s, objectsMicrophone om)
        {
            switch (s)
            {
                case "Groups":
                    om.settings.accessgroups = OM.settings.accessgroups;
                    break;
                case "Sound Detector Settings":
                    om.detector.gain = OM.detector.gain;
                    om.detector.maxsensitivity = OM.detector.maxsensitivity;
                    om.detector.minsensitivity = OM.detector.minsensitivity;
                    om.detector.sensitivity = OM.detector.sensitivity;
                    break;
                case "Alert Settings":
                    om.alerts.active = OM.alerts.active;
                    om.alerts.mode = OM.alerts.mode;
                    om.alerts.groupname = OM.alerts.groupname;
                    om.alerts.resetinterval = OM.alerts.resetinterval;
                    om.alerts.minimuminterval = OM.alerts.minimuminterval;
                    break;
                case "Actions: Alert":
                    {
                        CopyActions(OM.id, om.id, 1, "alert");
                    }
                    break;
                case "Actions: Connection Lost":
                    {
                        CopyActions(OM.id, om.id, 1, "disconnect");
                    }
                    break;
                case "Actions: Reconnect":
                    {
                        CopyActions(OM.id, om.id, 1, "reconnect");
                    }
                    break;
                case "Recording Mode":
                    om.detector.recordonalert = OM.detector.recordonalert;
                    om.detector.recordondetect = OM.detector.recordondetect;
                    break;
                case "Recording Settings (excluding triggers)":
                    om.settings.buffer = OM.settings.buffer;
                    om.detector.recordondetect = OM.detector.recordondetect;
                    om.detector.recordonalert = OM.detector.recordonalert;
                    om.recorder.inactiverecord = OM.recorder.inactiverecord;
                    om.recorder.maxrecordtime = OM.recorder.maxrecordtime;
                    om.recorder.minrecordtime = OM.recorder.minrecordtime;
                    break;
                case "Schedule":
                    {
                        om.schedule.entries = OM.schedule.entries.ToArray();
                        om.schedule.active = OM.schedule.active;
                    }
                    break;
                case "Storage Settings (excluding directory)":
                    om.settings.storagemanagement.archive = OM.settings.storagemanagement.archive;
                    om.settings.storagemanagement.enabled = OM.settings.storagemanagement.enabled;
                    om.settings.storagemanagement.maxage = OM.settings.storagemanagement.maxage;
                    om.settings.storagemanagement.maxsize = OM.settings.storagemanagement.maxsize;
                    break;
            }
        }
    }
}
