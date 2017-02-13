namespace iSpyApplication
{
    partial class GettingStarted
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GettingStarted));
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this._btnOk = new System.Windows.Forms.Button();
            this._ddlLanguage = new System.Windows.Forms.ComboBox();
            this.pnlWBContainer = new System.Windows.Forms.Panel();
            this.chkShowGettingStarted = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this._btnOk);
            this.flowLayoutPanel2.Controls.Add(this._ddlLanguage);
            this.flowLayoutPanel2.Controls.Add(this.chkShowGettingStarted);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 429);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel2.Size = new System.Drawing.Size(581, 35);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // _btnOk
            // 
            this._btnOk.AutoSize = true;
            this._btnOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnOk.Location = new System.Drawing.Point(546, 3);
            this._btnOk.Name = "_btnOk";
            this._btnOk.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._btnOk.Size = new System.Drawing.Size(32, 23);
            this._btnOk.TabIndex = 19;
            this._btnOk.Text = "OK";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _ddlLanguage
            // 
            this._ddlLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ddlLanguage.FormattingEnabled = true;
            this._ddlLanguage.Location = new System.Drawing.Point(410, 3);
            this._ddlLanguage.Name = "_ddlLanguage";
            this._ddlLanguage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ddlLanguage.Size = new System.Drawing.Size(130, 21);
            this._ddlLanguage.TabIndex = 52;
            this._ddlLanguage.SelectedIndexChanged += new System.EventHandler(this._ddlLanguage_SelectedIndexChanged);
            // 
            // pnlWBContainer
            // 
            this.pnlWBContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWBContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlWBContainer.Name = "pnlWBContainer";
            this.pnlWBContainer.Size = new System.Drawing.Size(581, 429);
            this.pnlWBContainer.TabIndex = 3;
            // 
            // chkShowGettingStarted
            // 
            this.chkShowGettingStarted.AutoSize = true;
            this.chkShowGettingStarted.Location = new System.Drawing.Point(274, 6);
            this.chkShowGettingStarted.Margin = new System.Windows.Forms.Padding(6);
            this.chkShowGettingStarted.Name = "chkShowGettingStarted";
            this.chkShowGettingStarted.Size = new System.Drawing.Size(127, 17);
            this.chkShowGettingStarted.TabIndex = 53;
            this.chkShowGettingStarted.Text = "Show Getting Started";
            this.chkShowGettingStarted.UseVisualStyleBackColor = true;
            // 
            // GettingStarted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 464);
            this.Controls.Add(this.pnlWBContainer);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GettingStarted";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Getting Started";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GettingStarted_FormClosing);
            this.Load += new System.EventHandler(this.GettingStarted_Load);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.ComboBox _ddlLanguage;
        private System.Windows.Forms.Panel pnlWBContainer;
        private System.Windows.Forms.CheckBox chkShowGettingStarted;
    }
}