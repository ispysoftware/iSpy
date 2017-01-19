using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class SelectObjects : Form
    {
        public List<object> SelectedObjects = new List<object>();
 
        public SelectObjects()
        {
            InitializeComponent();

            Text = LocRm.GetString("PleaseSelect");
            button1.Text = LocRm.GetString("OK");

        }

        private void CopyTo_Load(object sender, EventArgs e)
        {
          
            var lc = MainForm.Cameras.OrderBy(p => p.name).ToList();
            foreach (var c in lc)
            {
                clbObjects.Items.Add(new MainForm.ListItem(c.name, "2|"+c.id));                    
            }
            
            var lc3 = MainForm.FloorPlans.OrderBy(p => p.name).ToList();
            foreach (var c in lc3)
            {
                clbObjects.Items.Add(new MainForm.ListItem(c.name, "3|" + c.id));
            }
          
            var lc2 = MainForm.Microphones.OrderBy(p => p.name).ToList();
            foreach (var c in lc2)
            {
                clbObjects.Items.Add(new MainForm.ListItem(c.name, "1|"+c.id));
            }

           
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (MainForm.ListItem i in clbObjects.CheckedItems)
            {
                var c = i.Value.ToString().Split('|');
                switch (c[0])
                {
                    case "1":
                    SelectedObjects.Add(MainForm.Microphones.First(p=>p.id==Convert.ToInt32(c[1])));
                        break;
                    case "2":
                        SelectedObjects.Add(MainForm.Cameras.First(p => p.id == Convert.ToInt32(c[1])));
                        break;
                    case "3":
                        SelectedObjects.Add(MainForm.FloorPlans.First(p => p.id == Convert.ToInt32(c[1])));
                        break;
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
