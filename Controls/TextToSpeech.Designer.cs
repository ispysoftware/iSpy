namespace iSpyApplication.Controls
{
    partial class TextToSpeech
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
            this.tabTts = new System.Windows.Forms.TabControl();
            this.tabPageSpeak = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlSay = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPagePlay = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlPath = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.tabTts.SuspendLayout();
            this.tabPageSpeak.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPagePlay.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabTts
            // 
            this.tabTts.Controls.Add(this.tabPageSpeak);
            this.tabTts.Controls.Add(this.tabPagePlay);
            this.tabTts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTts.Location = new System.Drawing.Point(0, 0);
            this.tabTts.Name = "tabTts";
            this.tabTts.SelectedIndex = 0;
            this.tabTts.Size = new System.Drawing.Size(404, 70);
            this.tabTts.TabIndex = 0;
            // 
            // tabPageSpeak
            // 
            this.tabPageSpeak.Controls.Add(this.flowLayoutPanel1);
            this.tabPageSpeak.Location = new System.Drawing.Point(4, 22);
            this.tabPageSpeak.Name = "tabPageSpeak";
            this.tabPageSpeak.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSpeak.Size = new System.Drawing.Size(396, 44);
            this.tabPageSpeak.TabIndex = 0;
            this.tabPageSpeak.Text = "Speak";
            this.tabPageSpeak.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.ddlSay);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(390, 38);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Say:";
            // 
            // ddlSay
            // 
            this.ddlSay.FormattingEnabled = true;
            this.ddlSay.Location = new System.Drawing.Point(46, 6);
            this.ddlSay.Margin = new System.Windows.Forms.Padding(6);
            this.ddlSay.Name = "ddlSay";
            this.ddlSay.Size = new System.Drawing.Size(269, 21);
            this.ddlSay.TabIndex = 1;
            this.ddlSay.KeyPress += ddlSay_KeyPress;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(327, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(53, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Go";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPagePlay
            // 
            this.tabPagePlay.Controls.Add(this.flowLayoutPanel2);
            this.tabPagePlay.Location = new System.Drawing.Point(4, 22);
            this.tabPagePlay.Name = "tabPagePlay";
            this.tabPagePlay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePlay.Size = new System.Drawing.Size(396, 44);
            this.tabPagePlay.TabIndex = 1;
            this.tabPagePlay.Text = "Play";
            this.tabPagePlay.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.ddlPath);
            this.flowLayoutPanel2.Controls.Add(this.button2);
            this.flowLayoutPanel2.Controls.Add(this.button3);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(390, 38);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Path:";
            // 
            // ddlPath
            // 
            this.ddlPath.FormattingEnabled = true;
            this.ddlPath.Location = new System.Drawing.Point(50, 6);
            this.ddlPath.Margin = new System.Windows.Forms.Padding(6);
            this.ddlPath.Name = "ddlPath";
            this.ddlPath.Size = new System.Drawing.Size(220, 21);
            this.ddlPath.TabIndex = 1;
            this.ddlPath.KeyPress += ddlPath_KeyPress;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(282, 6);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(32, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(326, 6);
            this.button3.Margin = new System.Windows.Forms.Padding(6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(53, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Go";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "All Files (*.*)|*.*";
            this.dlgOpen.Title = "Choose Audio File";
            // 
            // TextToSpeech
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 70);
            this.Controls.Add(this.tabTts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TextToSpeech";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TextToSpeech";
            this.Load += new System.EventHandler(this.TextToSpeech_Load);
            this.tabTts.ResumeLayout(false);
            this.tabPageSpeak.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabPagePlay.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        
        #endregion

        private System.Windows.Forms.TabControl tabTts;
        private System.Windows.Forms.TabPage tabPageSpeak;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlSay;
        private System.Windows.Forms.TabPage tabPagePlay;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
    }
}