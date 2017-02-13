using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed partial class DirectoryEventRow : UserControl
    {
        public new static int Height = 31;
        public configurationDirectory Directory;
        public int Index;

        public event EventHandler DirectoryEntryDelete;
        public event EventHandler DirectoryEntryEdit;
        public event EventHandler MouseOver;


        public DirectoryEventRow(configurationDirectory directory, int index)
        {
            Directory = directory;
            Index = index;
            InitializeComponent();
            lblSummary.Text = directory.Entry;
            BackColor = DefaultBackColor;
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (DirectoryEntryEdit != null)
                DirectoryEntryEdit(this, EventArgs.Empty);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (DirectoryEntryDelete != null)
                DirectoryEntryDelete(this, EventArgs.Empty);
        }

        private void tableLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(255, 221, 221, 221);
            if (MouseOver != null)
                MouseOver(this, EventArgs.Empty);
        }

        public void RevertBackground()
        {
            tableLayoutPanel1.BackColor = DefaultBackColor;
            Invalidate();
        }

    }
}
