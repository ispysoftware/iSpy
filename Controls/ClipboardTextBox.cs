using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class ClipboardTextBox : TextBox
    {
        private const int WmCut = 0x0300;
        private const int WmCopy = 0x0301;
        private const int WmPaste = 0x0302;

        public delegate void ClipboardEventHandler(object sender, ClipboardEventArgs e);

        [Category("Clipboard")]
        public event ClipboardEventHandler CutText;
        [Category("Clipboard")]
        public event ClipboardEventHandler CopiedText;
        [Category("Clipboard")]
        public event ClipboardEventHandler PastedText;

        public ClipboardTextBox()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmCut)
            {
                CutText?.Invoke(this, new ClipboardEventArgs(SelectedText));
            }
            else if (m.Msg == WmCopy)
            {
                CopiedText?.Invoke(this, new ClipboardEventArgs(SelectedText));
            }
            else if (m.Msg == WmPaste)
            {
                PastedText?.Invoke(this, new ClipboardEventArgs(Clipboard.GetText()));
            }

            base.WndProc(ref m);
        }
    }

    public class ClipboardEventArgs : EventArgs
    {
        public string ClipboardText { get; set; }

        public ClipboardEventArgs(string clipboardText)
        {
            ClipboardText = clipboardText;
        }
    }
}
