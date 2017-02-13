using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class ActionEditor : UserControl
    {
        public event EventHandler LoginRequested;
        public string Mode = "alert";
        public int Oid = -1;
        public int Otid = -1;

        public string[] Actions =
            {
                "Exe|ExecuteFile",
                "URL|CallURL",
                "NM|NetworkMessage",
                "S|PlaySound",
                "ATC|SoundThroughCamera",
                "SW|ShowWindow",
                "B|Beep",
                "M|Maximise",
                "SOO|SwitchObjectOn",
                "SOF|SwitchObjectOff",
                "MO|SwitchMonitorOn",
                "TA|TriggerAlertOn",
                "E|SendEmail[1]",
                "SMS|SendSMS[SUBSCRIBER]",
                "TM|SendTwitterMessage[SUBSCRIBER]"
            };

        public ActionEditor()
        {
            InitializeComponent();
        }
        
        public void Init(string mode, int oid, int otid)
        {
            Oid = oid;
            Otid = otid;
            Mode = mode;
            Init();

        }

        private void Init() {
            ddlAction.Items.Clear();
            ddlAction.Items.Add(new ListItem {Name = LocRm.GetString("SelectAction"), Restricted = false, Value = ""});
            for(int i=0;i < Actions.Length;i++)
            {
                Actions[i] = Actions[i].Replace("[1]", MainForm.Conf.UseSMTP ? "" : "[SUBSCRIBER]");
            }
            foreach (string s in Actions)
            {
                var oc = s.Split('|');
                var li = new ListItem();
                bool restricted = oc[1].IndexOf("[SUBSCRIBER]", StringComparison.Ordinal) != -1 && !MainForm.Conf.Subscribed;
                oc[1] = oc[1].Replace("[SUBSCRIBER]", "");
                oc[1] = LocRm.GetString(oc[1].Trim());
                if (restricted)
                    oc[1] += " " + LocRm.GetString("SubscribersOnly");
                
                li.Name = oc[1];
                li.Value = oc[0];
                li.Restricted = restricted;
                ddlAction.Items.Add(li);
            }
            ddlAction.SelectedIndex = 0;
            flpActions.VerticalScroll.Visible = true;
            flpActions.HorizontalScroll.Visible = false;
            RenderEventList();

            button1.Text = LocRm.GetString("Add");
        }

        void RenderEventList()
        {
            flpActions.Controls.Clear();
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

            var w = flpActions.Width - 2;

            var oae = MainForm.Actions.Where(p => p.mode == Mode && p.objectid == Oid && p.objecttypeid == Otid).ToList();

            if (oae.Count * AlertEventRow.Height >= flpActions.Height)
                w = flpActions.Width - vertScrollWidth - 2;
            foreach (var e in oae)
            {
                var c = new AlertEventRow(e) {Width = w};
                c.AlertEntryDelete += CAlertEntryDelete;
                c.AlertEntryEdit += CAlertEntryEdit;
                c.MouseOver += CMouseOver;
                flpActions.Controls.Add(c);
                flpActions.SetFlowBreak(c, true);
            }
            
            flpActions.PerformLayout();
            flpActions.HorizontalScroll.Visible = flpActions.HorizontalScroll.Enabled = false;
            
        }

        void CMouseOver(object sender, EventArgs e)
        {
            foreach (var c in flpActions.Controls)
            {
                var o = (AlertEventRow) c;
                if (o!=sender)
                {
                    o.RevertBackground();

                }
            }
        }

        void CAlertEntryEdit(object sender, EventArgs e)
        {
            var oe = ((AlertEventRow)sender).Oae;
            string t = oe.type;
            string param1Val = oe.param1;
            string param2Val = oe.param2;
            string param3Val = oe.param3;
            string param4Val = oe.param4;


            bool cancel;
            var config = GetConfig(param2Val, param3Val, param4Val, param1Val, t, out cancel);

            if (cancel)
                return;


            oe = ((AlertEventRow)sender).Oae;

            if (config.Length > 0)
            {
                oe.param1 = config[0];
            }
            if (config.Length > 1)
            {
                oe.param2 = config[1];
            }
            if (config.Length > 2)
            {
                oe.param3 = config[2];
            }
            if (config.Length > 3)
            {
                oe.param4 = config[3];
            }
            
            RenderEventList();
        }

        private string[] GetConfig(string param2Val, string param3Val, string param4Val, string param1Val, string t,
                                   out bool cancel)
        {
            cancel = false;
            var config = new string[] {};
            switch (t)
            {
                case "Exe":
                    config = GetParamConfig(GetName(t), out cancel, LocRm.GetString("File")+"|FBD:*.*", param1Val, LocRm.GetString("Arguments"), param2Val);
                    break;
                case "URL":
                    if (param1Val == "")
                        param1Val = "http://";
                    if (param2Val == "")
                        param2Val = "True";
                    config = GetParamConfig(GetName(t), out cancel, "URL", param1Val, LocRm.GetString("UploadImage")+"|Checkbox:True", param2Val);
                    break;
                case "NM":
                    if (param3Val=="")
                       param3Val = "1010";
                    config = GetParamConfig(GetName(t), out cancel, LocRm.GetString("Type")+"|DDL:TCP,UDP", param1Val, "IP Address",
                                            param2Val, LocRm.GetString("Port")+"|Numeric:0,65535", param3Val, LocRm.GetString("Message"), param4Val);
                    break;
                case "S":
                case "ATC":
                    config = GetParamConfig(GetName(t), out cancel, LocRm.GetString("File") + "|FBD:*.wav", param1Val);
                    break;
                case "SW":
                case "B":
                case "M":
                case "MO":
                    config = new [] {"","","",""};
                    break;
                case "TA":
                case "SOF":
                case "SOO":
                    config = GetParamConfig(GetName(t), out cancel, LocRm.GetString("Object")+"|Object", param1Val);
                    break;
                case "E":
                    if (param2Val == "")
                        param2Val = "True";
                    config = GetParamConfig(GetName(t), out cancel, LocRm.GetString("EmailAddress"), param1Val, LocRm.GetString("IncludeImage") + "|Checkbox:True", param2Val);
                    break;
                case "SMS":
                    config = GetParamConfig(GetName(t), out cancel, LocRm.GetString("SMSNumber")+"|SMS", param1Val);
                    break;
                case "TM":
                    config = GetParamConfig(GetName(t), out cancel, "|Link:"+LocRm.GetString("AuthoriseTwitter"), MainForm.Webserver + "/account.aspx?task=twitter-auth");
                    break;
            }
            
            return config;
        }

        string GetName(string id)
        {
            foreach (ListItem li in ddlAction.Items)
            {
                if (li.Value == id)
                    return li.Name;
            }
            return "";
        }

        void CAlertEntryDelete(object sender, EventArgs e)
        {
            var oe = ((AlertEventRow)sender).Oae;
            MainForm.Actions.Remove(oe);
            RenderEventList();
        }


        private struct ListItem
        {
            public string Name, Value;
            public bool Restricted;

            public override string ToString()
            {
                return Name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ddlAction.SelectedIndex < 1)
            {
                MessageBox.Show(this, LocRm.GetString("SelectAnActionToAdd"));
                return;
            }
            var oa = (ListItem) ddlAction.SelectedItem;
            
            if (!MainForm.Conf.Subscribed && oa.Restricted)
            {
                LoginRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            bool cancel;
            string[] config = GetConfig("", "", "", "", oa.Value, out cancel);
            if (cancel)
                return;

            
            var oae = new objectsActionsEntry
                      {
                                mode=Mode,
                                objectid = Oid,
                                objecttypeid = Otid,
                                type = oa.Value,
                                param1 = config[0],
                                param2 = config[1],
                                param3 = config[2],
                                param4 = config[3],
                                ident = Guid.NewGuid().ToString()
            };
            MainForm.Actions.Add(oae);
            RenderEventList();
        }

        public class ActionEntry
        {
            public ActionEntry(objectsActionsEntry e)
            {
                OAE = e;
            }

            public objectsActionsEntry OAE { get; }

            public bool Checked
            {
                get { return OAE.active; }
                set
                {
                    OAE.active = value;
                }
            }

            public string EditVisiblity
            {
                get
                {
                    switch (OAE.type)
                    {
                        case "SW":
                        case "B":
                        case "M":
                        case "TM":
                            return "Collapsed";
                    }
                    return "Visible";
                }
            }

            public string Summary
            {
                get
                {
                    string t = "Unknown";
                    switch (OAE.type)
                    {
                        case "Exe":
                            t = LocRm.GetString("ExecuteFile") + ": " + OAE.param1;
                            break;
                        case "URL":
                            t = LocRm.GetString("CallURL") + ": " + OAE.param1;
                            if (Convert.ToBoolean(OAE.param2))
                                t += " (POST grab)";
                            break;
                        case "NM":
                            t = OAE.param1 + " " + OAE.param2 + ":" + OAE.param3 + " (" + OAE.param4 + ")";
                            break;
                        case "S":
                            t = LocRm.GetString("PlaySound") + ": " + OAE.param1;
                            break;
                        case "ATC":
                            t = LocRm.GetString("SoundThroughCamera") + ": " + OAE.param1;
                            break;
                        case "SW":
                            t = LocRm.GetString("ShowWindow");
                            break;
                        case "B":
                            t = LocRm.GetString("Beep");
                            break;
                        case "M":
                            t = LocRm.GetString("Maximise");
                            break;
                        case "TA":
                            {
                                string[] op = OAE.param1.Split(',');
                                string n = "[removed]";
                                int id = Convert.ToInt32(op[1]);
                                switch (op[0])
                                {
                                    case "1":
                                        objectsMicrophone om = MainForm.Microphones.FirstOrDefault(p => p.id == id);
                                        if (om != null)
                                            n = om.name;
                                        break;
                                    case "2":
                                        objectsCamera oc = MainForm.Cameras.FirstOrDefault(p => p.id == id);
                                        if (oc != null)
                                            n = oc.name;
                                        break;
                                }
                                t = LocRm.GetString("TriggerAlertOn") + " " + n;
                            }
                            break;
                        case "SOO":
                            {
                                string[] op = OAE.param1.Split(',');
                                string n = "[removed]";
                                int id = Convert.ToInt32(op[1]);
                                switch (op[0])
                                {
                                    case "1":
                                        objectsMicrophone om = MainForm.Microphones.FirstOrDefault(p => p.id == id);
                                        if (om != null)
                                            n = om.name;
                                        break;
                                    case "2":
                                        objectsCamera oc = MainForm.Cameras.FirstOrDefault(p => p.id == id);
                                        if (oc != null)
                                            n = oc.name;
                                        break;
                                }
                                t = LocRm.GetString("SwitchObjectOn") + " " + n;
                            }
                            break;
                        case "SOF":
                            {
                                string[] op = OAE.param1.Split(',');
                                string n = "[removed]";
                                int id = Convert.ToInt32(op[1]);
                                switch (op[0])
                                {
                                    case "1":
                                        objectsMicrophone om = MainForm.Microphones.FirstOrDefault(p => p.id == id);
                                        if (om != null)
                                            n = om.name;
                                        break;
                                    case "2":
                                        objectsCamera oc = MainForm.Cameras.FirstOrDefault(p => p.id == id);
                                        if (oc != null)
                                            n = oc.name;
                                        break;
                                }
                                t = LocRm.GetString("SwitchObjectOff") + " " + n;
                            }
                            break;
                        case "E":
                            t = LocRm.GetString("SendEmail") + ": " + OAE.param1;
                            if (OAE.param2 != "" && Convert.ToBoolean(OAE.param2))
                                t += " (include grab)";
                            break;
                        case "SMS":
                            t = LocRm.GetString("SendSMS") + ": " + OAE.param1;
                            break;
                        case "TM":
                            t = LocRm.GetString("SendTwitterMessage");
                            break;
                        case "MO":
                            {
                                t = LocRm.GetString("SwitchMonitorOn");
                            }
                            break;
                    }

                    return t;
                }
            }
        }

        private string[] GetParamConfig(string typeName, out bool cancel, 
            string param1 = "", string param1Value = "",
            string param2 = "", string param2Value = "",
            string param3 = "", string param3Value = "",
            string param4 = "", string param4Value = "")
        {
            cancel = false;
            var pc = new ParamConfig {TypeName = typeName, 
                Param1 = param1, Param1Value = param1Value,
                Param2 = param2, Param2Value = param2Value,
                Param3 = param3, Param3Value = param3Value,
                Param4 = param4, Param4Value = param4Value};
            if (pc.ShowDialog(this)!=DialogResult.OK)
            {
                cancel = true;
            }
            var cfg = new [] { pc.Param1Value, pc.Param2Value, pc.Param3Value, pc.Param4Value };
            pc.Dispose();
            return cfg;
        }
    }
}
