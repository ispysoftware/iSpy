using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class ScheduleEntryEditor : Form
    {
        public objectsScheduleEntry ose;

        public ScheduleEntryEditor()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("ScheduleEditor");
            label1.Text = LocRm.GetString("When");
            label2.Text = LocRm.GetString("Days");
            label3.Text = LocRm.GetString("Action");
            btnOK.Text = LocRm.GetString("OK");
            chkMon.Text = LocRm.GetString("DOWMon");
            chkTue.Text = LocRm.GetString("DOWTue");
            chkWed.Text = LocRm.GetString("DOWWed");
            chkThu.Text = LocRm.GetString("DOWThu");
            chkFri.Text = LocRm.GetString("DOWFri");
            chkSat.Text = LocRm.GetString("DOWSat");
            chkSun.Text = LocRm.GetString("DOWSun");
        }

        private void ScheduleEntryEditor_Load(object sender, EventArgs e)
        {
            if (ose != null)
            {
                string[] days = ose.daysofweek.Split(',');
                foreach (string d in days)
                {
                    switch (d)
                    {
                        case "1":
                            chkMon.Checked = true;
                            break;
                        case "2":
                            chkTue.Checked = true;
                            break;
                        case "3":
                            chkWed.Checked = true;
                            break;
                        case "4":
                            chkThu.Checked = true;
                            break;
                        case "5":
                            chkFri.Checked = true;
                            break;
                        case "6":
                            chkSat.Checked = true;
                            break;
                        case "0":
                            chkSun.Checked = true;
                            break;
                    }
                }

                dtWhen.Value = DateTime.Now.Date + TimeSpan.FromMinutes(ose.time);

                var actions = Helper.Actions.ToList();
                switch (ose.objecttypeid)
                {
                    case 1:
                        actions = actions.Where(p => p.TypeID != Helper.ScheduleAction.ActionTypeID.CameraOnly).ToList();
                        break;
                    case 4:
                        actions = actions.Where(p => p.TypeID == Helper.ScheduleAction.ActionTypeID.All).ToList();
                        break;
                }

                foreach (var aa in MainForm.Actions.Where(p => p.objectid == ose.objectid && p.objecttypeid == ose.objecttypeid))
                {
                    var ae = new ActionEditor.ActionEntry(aa);
                    ddlAlertAction.Items.Add(new ParamDisplay(aa.mode + ": " + ae.Summary, aa.ident));
                    if (aa.ident == ose.parameter)
                    {
                        ddlAlertAction.SelectedIndex = ddlAlertAction.Items.Count - 1;
                    }
                }
                if (ddlAlertAction.Items.Count > 0 && ddlAlertAction.SelectedIndex == -1)
                    ddlAlertAction.SelectedIndex = 0;

                var a = actions.FirstOrDefault(p => p.ID == ose.typeid);

                foreach (var act in actions)
                {
                    ddlAction.Items.Add(act);
                    if (a != null && act.ID == a.ID)
                        ddlAction.SelectedIndex = ddlAction.Items.Count - 1;
                }

                if (ddlAction.SelectedIndex == -1)
                    ddlAction.SelectedIndex = 0;
            }
        }

        public class ParamDisplay
        {
            public string Ident;
            public string Text;
            public ParamDisplay(string text, string ident)
            {
                Ident = ident;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            DateTime dt = Convert.ToDateTime(dtWhen.Value);
            ose.time = Convert.ToInt32(dt.TimeOfDay.TotalMinutes);
            
            string days = "";
            if (chkMon.Checked)
            {
                days += "1,";
            }
            if (chkTue.Checked)
            {
                days += "2,";
            }
            if (chkWed.Checked)
            {
                days += "3,";
            }
            if (chkThu.Checked)
            {
                days += "4,";
            }
            if (chkFri.Checked)
            {
                days += "5,";
            }
            if (chkSat.Checked)
            {
                days += "6,";
            }
            if (chkSun.Checked)
            {
                days += "0,";
            }

            ose.daysofweek = days.Trim(',');
            var sa = ((Helper.ScheduleAction)ddlAction.SelectedItem);
            ose.typeid = sa.ID;
            
            if (ddlAlertAction.Visible)
            {
                if (sa.ParameterName == "Action")
                {
                    if (ddlAlertAction.SelectedIndex > -1)
                    {
                        var aa = (ParamDisplay)ddlAlertAction.SelectedItem;
                        ose.parameter = aa.Ident;
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAlertAction.Hide();
            if (ddlAction.SelectedIndex > -1)
            {
                var a = ddlAction.SelectedItem as Helper.ScheduleAction;
                if (a != null)
                {
                    if (a.ParameterName == "Action")
                        ddlAlertAction.Show();
                }
            }
        }
    }
}
