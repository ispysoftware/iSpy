using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class FTPEditor : UserControl
    {
        public configurationServer[] Servers;

        public FTPEditor()
        {
            InitializeComponent();
        }

        public void Init(configurationServer[] ae)
        {
            Servers = ae;
            Init();

        }

        private void Init() {
           
            flpServers.VerticalScroll.Visible = true;
            flpServers.HorizontalScroll.Visible = false;
            button1.Text = LocRm.GetString("Add");
            RenderServerList();
        }

        void RenderServerList()
        {
            flpServers.Controls.Clear();
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

            var w = flpServers.Width - 2;


            if (Servers != null)
            {
                if (Servers.Length * ServerEventRow.Height >= flpServers.Height)
                    w = flpServers.Width - vertScrollWidth - 2;
                int i = 0;
                foreach (var e in Servers)
                {
                    var c = new ServerEventRow(e, i) {Width = w};
                    c.EntryDelete += CDirectoryEntryDelete;
                    c.EntryEdit += CDirectoryEntryEdit;
                    c.MouseOver += CMouseOver;
                    flpServers.Controls.Add(c);
                    flpServers.SetFlowBreak(c, true);
                    i++;
                }
            }
            
            flpServers.PerformLayout();
            flpServers.HorizontalScroll.Visible = flpServers.HorizontalScroll.Enabled = false;
            
        }

        void CMouseOver(object sender, EventArgs e)
        {
            foreach (var c in flpServers.Controls)
            {
                var o = c as ServerEventRow;
                if (o!=sender)
                {
                    if (o != null) o.RevertBackground();
                }
            }
        }

        void CDirectoryEntryEdit(object sender, EventArgs e)
        {       
            var oe = ((ServerEventRow)sender);
            var c = new FTPConfig {FTP = oe.Server};
            if (c.ShowDialog(this)==DialogResult.OK)
            {
                Servers[oe.Index] = c.FTP;
            }
            c.Dispose();


            RenderServerList();
        }


        void CDirectoryEntryDelete(object sender, EventArgs e)
        {
            var oe = ((ServerEventRow)sender);
            var l = Servers.ToList();
            l.RemoveAt(oe.Index);
            Servers = l.ToArray();
            RenderServerList();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var f = new FTPConfig { FTP = new configurationServer() };
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var l = Servers.ToList();
                l.Add(f.FTP);
                Servers = l.ToArray();
                f.Dispose();
                RenderServerList();
            } 
        }

        private void flpDirectories_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
