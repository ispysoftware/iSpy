using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RestSharp.Extensions;

namespace iSpyApplication.Controls
{
    public partial class TagEntry : UserControl
    {
        private string _tagName;
        private string _displayName;

        public string TagName
        {
            get
            {
                return _tagName;
            }
            set
            {
                _tagName = value;
                _displayName = value.Trim().Trim('{', '}').Replace("_", " ").ToLowerInvariant();
            }
        }

        public string TagValue;
        

        public TagEntry()
        {
            InitializeComponent();
            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void Commit()
        {
            TagValue = textBox1.Text;
        }

        private void TagEntry_Load(object sender, EventArgs e)
        {
            lblTag.Text = _displayName;
            textBox1.Text = TagValue;
        }
    }
}
