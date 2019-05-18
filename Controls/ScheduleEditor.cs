using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class ScheduleEditor : UserControl
    {
        
        private ISpyControl io;

        public ISpyControl Io
        {
            get { return io; }
            set
            {
                if (value != null)
                {
                    io = value;
                    io.ReloadSchedule();
                    RenderSchedule();
                }
            }
        }

        public ScheduleEditor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ose = new objectsScheduleEntry
            {
                objectid = Io.ObjectID,
                objecttypeid = Io.ObjectTypeID,
                daysofweek = "",
                parameter = "",
                time = (int) DateTime.Now.TimeOfDay.TotalMinutes,
                active = true
            };

            using (var see = new ScheduleEntryEditor())
            {
                see.ose = ose;
                if (see.ShowDialog(this) == DialogResult.OK)
                {
                    MainForm.Schedule.Add(see.ose);
                    RenderSchedule();
                }
            }            
        }

        public void RenderSchedule()
        {
            if (Io == null)
                return;
            var se = MainForm.Schedule.Where(p => p.objectid == Io.ObjectID && p.objecttypeid == Io.ObjectTypeID).ToList();

            flpSchedule.SuspendLayout();
            flpSchedule.Controls.Clear();
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

            var w = flpSchedule.Width - 2;
            if (se.Count*AlertEventRow.Height >= flpSchedule.Height)
                w = w - vertScrollWidth;

            foreach (var e in se)
            {
                var c = new ScheduleRow(e);
                c.ScheduleEntryDelete += CScheduleEntryDelete;
                c.ScheduleEntryEdit += CScheduleEntryEdit;
                c.MouseOver += CMouseOver;
                c.Width = w;
                flpSchedule.Controls.Add(c);
                flpSchedule.SetFlowBreak(c, true);
            }

            flpSchedule.ResumeLayout(true);
            flpSchedule.HorizontalScroll.Visible = flpSchedule.HorizontalScroll.Enabled = false;


        }

        void CMouseOver(object sender, EventArgs e)
        {
            foreach (var c in flpSchedule.Controls)
            {
                var o = (ScheduleRow)c;
                if (o != sender)
                {
                    o.RevertBackground();

                }
            }
        }

        void CScheduleEntryDelete(object sender, EventArgs e)
        {
            var sr = (ScheduleRow) sender;
            var oe = sr.Ose;
            MainForm.Schedule.Remove(oe);
            flpSchedule.Controls.Remove(sr);
            flpSchedule.Invalidate();
        }

        void CScheduleEntryEdit(object sender, EventArgs e)
        {
            var sr = (ScheduleRow)sender;
            var ose = sr.Ose;
            using (var see = new ScheduleEntryEditor())
            {
                see.ose = ose;
                if (see.ShowDialog(this) == DialogResult.OK)
                {
                    sr.Ose = see.ose;
                    sr.Invalidate();
                }
            }
        }


        public class ScheduleItem
        {
            public string Name { get; set; }

            public objectsScheduleEntry Value { get; set; }

            public int Index { get; set; }

            public bool Checked
            {
                get { return Value.active; }
                set { Value.active = value; }
            }

            public override string ToString()
            {
                return Name;
            }

            public ScheduleItem(objectsScheduleEntry se, int index = 0)
            {
                Name = FormatTime(se.time) + "\t" + FormatDays(se.daysofweek.Split(',')) + " \t" +
                       Helper.ScheduleDescription(se.typeid);
                Value = se;
                Index = index;
            }

            private static string FormatTime(int t)
            {
                var ts = TimeSpan.FromMinutes(t);
                string h = ts.Hours.ToString(CultureInfo.InvariantCulture);
                string m = ts.Minutes.ToString(CultureInfo.InvariantCulture);
                if (h.Length == 1)
                    h = "0" + h;
                if (m.Length == 1)
                    m = "0" + m;
                return h + ":" + m;
            }

            private static string FormatDays(IEnumerable<string> d)
            {
                string r = "";
                foreach (var day in d)
                {
                    switch (day)
                    {
                        case "1":
                            r += LocRm.GetString("DOWMon")+",";
                            break;
                        case "2":
                            r += LocRm.GetString("DOWTue") + ",";
                            break;
                        case "3":
                            r += LocRm.GetString("DOWWed") + ",";
                            break;
                        case "4":
                            r += LocRm.GetString("DOWThu") + ",";
                            break;
                        case "5":
                            r += LocRm.GetString("DOWFri") + ",";
                            break;
                        case "6":
                            r += LocRm.GetString("DOWSat") + ",";
                            break;
                        case "0":
                            r += LocRm.GetString("DOWSun") + ",";
                            break;
                    }
                }
                return r.Trim(',');
            }

        }

        private void ScheduleEditor_Load(object sender, EventArgs e)
        {
            


            button1.Text = LocRm.GetString("Add");
        }

        private void flpSchedule_Resize(object sender, EventArgs e)
        {
            //RenderSchedule();
        }

        private void flpSchedule_ClientSizeChanged(object sender, EventArgs e)
        {
            RenderSchedule();
        }

        private void flpSchedule_Layout(object sender, LayoutEventArgs e)
        {

        }
    }
}
