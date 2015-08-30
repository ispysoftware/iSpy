using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ClipboardTextBoxExample
{
    public partial class ClipboardTextBox : TextBox
    {
        private const int WM_CUT = 0x0300;
        private const int WM_COPY = 0x0301;
        private const int WM_PASTE = 0x0302;

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
            if (m.Msg == WM_CUT)
            {
                if (CutText != null)
                    CutText(this, new ClipboardEventArgs(this.SelectedText));
            }
            else if (m.Msg == WM_COPY)
            {
                if (CopiedText != null)
                    CopiedText(this, new ClipboardEventArgs(this.SelectedText));
            }
            else if (m.Msg == WM_PASTE)
            {
                if (PastedText != null)
                    PastedText(this, new ClipboardEventArgs(Clipboard.GetText()));
            }

            base.WndProc(ref m);
        }
    }

    public class ClipboardEventArgs : EventArgs
    {
        private string clipboardText;
        public string ClipboardText
        {
            get
            {
                return clipboardText;
            }

            set
            {
                clipboardText = value;
            }
        }

        public ClipboardEventArgs(string clipboardText)
        {
            this.clipboardText = clipboardText;
        }
    }
}
