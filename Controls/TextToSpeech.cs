using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Sources.Audio;

namespace iSpyApplication.Controls
{
    public partial class TextToSpeech : Form
    {
        public CameraWindow CW;

        public TextToSpeech(CameraWindow cw = null)
        {
            InitializeComponent();

            CW = cw;
            Text = LocRm.GetString("TextToSpeech");
            button1.Text = LocRm.GetString("OK");
            button3.Text = LocRm.GetString("OK");
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void TextToSpeech_Load(object sender, EventArgs e)
        {
            PopSentences();
            PopPaths();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Say();
        }

        private void Say()
        {
            var t = ddlSay.Text;
            if (!string.IsNullOrEmpty(t))
            {
                SpeechSynth.Say(t, CW);
                CW.LogToPlugin("Text: " + t);
                var p = new List<string> { t };
                foreach (var i in ddlSay.Items)
                {
                    if (!p.Contains(i) && !string.IsNullOrEmpty(i.ToString()))
                        p.Add(i.ToString());
                }

                var x = "";
                int j = 0;
                foreach (string s in p)
                {
                    if (j < 10)
                        x += s + "|";
                    else
                    {
                        break;
                    }
                    j++;
                }
                x = x.Trim('|');
                MainForm.Conf.TextSentences = x;
                PopSentences();
            }
        }

        private void PopSentences()
        {
            ddlSay.Items.Clear();
            var s = MainForm.Conf.TextSentences.Split('|');
            foreach (var t in s)
            {
                if (!string.IsNullOrEmpty(t))
                    ddlSay.Items.Add(t);
            }
        }

        private void ddlSay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                Say();
                e.Handled = true;
            }
        }

        private void ddlPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                Play();
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dlgOpen.ShowDialog(this) == DialogResult.OK)
            {
                ddlPath.Text = dlgOpen.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void Play()
        {
            var p = ddlPath.Text;
            if (!string.IsNullOrEmpty(p))
            {
                AudioSynth.Play(p, CW);
                CW.LogToPlugin("Play: " + p);
                var _p = new List<string> { p };
                foreach (var i in ddlPath.Items)
                {
                    if (!_p.Contains(i) && !string.IsNullOrEmpty(i.ToString()))
                        _p.Add(i.ToString());
                }

                var x = "";
                int j = 0;
                foreach (string s in _p)
                {
                    if (j < 10)
                        x += s + "|";
                    else
                    {
                        break;
                    }
                    j++;
                }
                x = x.Trim('|');
                MainForm.Conf.PlayPaths = x;
                PopPaths();
            }
        }

        private void PopPaths()
        {
            ddlPath.Items.Clear();
            var s = MainForm.Conf.PlayPaths.Split('|');
            foreach (var p in s)
            {
                if (!string.IsNullOrEmpty(p))
                    ddlPath.Items.Add(p);
            }
        }
    }
}
