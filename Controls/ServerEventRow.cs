using System;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed partial class ServerEventRow : UserControl
    {
        public new static int Height = 31;
        public configurationServer Server;
        public int Index;

        public event EventHandler EntryDelete;
        public event EventHandler EntryEdit;
        public event EventHandler MouseOver;


        public ServerEventRow(configurationServer server, int index)
        {
            Server = server;
            Index = index;
            InitializeComponent();
            if (server.sftp)
                lblSummary.Text = "SFTP: "+server.name;
            else
                lblSummary.Text = server.name;
            BackColor = DefaultBackColor;
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (EntryEdit != null)
                EntryEdit(this, EventArgs.Empty);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (EntryDelete != null)
                EntryDelete(this, EventArgs.Empty);
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
