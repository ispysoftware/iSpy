using System;

using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class TagConfigure : Form
    {
        public string TagsNV;
        public TagConfigure()
        {
            InitializeComponent();
            Text = LocRm.GetString("Tags");
        }

        private void TagConfigure_Load(object sender, EventArgs e)
        {
            var d = Helper.GetDictionary(TagsNV, ';');

            foreach (var t in MainForm.Tags)
            {
                string v = "";
                if (d.ContainsKey(t))
                    v = d[t];
                var tc = new TagEntry
                            {
                                TagName = t,
                                TagValue = v
                            };
                flowLayoutPanel1.Controls.Add(tc);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var c = "";
            foreach (TagEntry te in flowLayoutPanel1.Controls)
            {
                te.Commit();
                c += te.TagName + "=" + te.TagValue+";";
            }
            TagsNV = c;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
