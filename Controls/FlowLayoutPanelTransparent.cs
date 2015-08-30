using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    

    class FlowLayoutPanelTransparent : FlowLayoutPanel
    {
        public FlowLayoutPanelTransparent()
        {
            this.SetStyle(ControlStyles.Opaque, true);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                // Turn on the WS_EX_TRANSPARENT style
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }
    }
}
