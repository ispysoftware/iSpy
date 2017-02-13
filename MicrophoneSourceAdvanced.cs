using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class MicrophoneSourceAdvanced : Form
    {
        public objectsMicrophone Micobject;
        public MicrophoneSourceAdvanced()
        {
            InitializeComponent();
            RenderResources();
        }

        private void MicrophoneSourceAdvanced_Load(object sender, EventArgs e)
        {
            
            txtReconnect.Value = Micobject.settings.reconnectinterval;
            numTimeout.Value = Micobject.settings.timeout;

        }

        private void RenderResources()
        {
            label4.Text = LocRm.GetString("Seconds");
            label3.Text = LocRm.GetString("ReconnectEvery");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var iReconnect = (int)txtReconnect.Value;
            if (iReconnect < 30 && iReconnect != 0)
            {
                MessageBox.Show(LocRm.GetString("Validate_ReconnectInterval"), LocRm.GetString("Note"));
                return;
            }

            Micobject.settings.reconnectinterval = iReconnect;
            Micobject.settings.timeout = (int) numTimeout.Value;
            Close();
        }
    }
}
