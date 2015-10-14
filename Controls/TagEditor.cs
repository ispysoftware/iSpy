using System;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class TagEditor : Form
    {
        public TagEditor()
        {
            InitializeComponent();
            Text = LocRm.GetString("Tags");
        }

        private void TagEditor_Load(object sender, EventArgs e)
        {
            textBox1.Text = MainForm.Conf.Tags;
        }

        private void TagEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.Conf.Tags = textBox1.Text;
            MainForm.Tags = null;//reloads them
        }
    }
}
