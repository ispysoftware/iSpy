namespace iSpyApplication
{
    partial class PTZConfig
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
            this.label18 = new System.Windows.Forms.Label();
            this.txtPTZURL = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtPTZUsername = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.txtPTZPassword = new System.Windows.Forms.TextBox();
            this.label91 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPTZChannel = new System.Windows.Forms.TextBox();
            this.chkPTZFlipX = new System.Windows.Forms.CheckBox();
            this.chkPTZFlipY = new System.Windows.Forms.CheckBox();
            this.chkPTZRotate90 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(2, 6);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 13);
            this.label18.TabIndex = 76;
            this.label18.Text = "PTZ URL";
            // 
            // txtPTZURL
            // 
            this.txtPTZURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPTZURL.Location = new System.Drawing.Point(77, 2);
            this.txtPTZURL.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPTZURL.Name = "txtPTZURL";
            this.txtPTZURL.Size = new System.Drawing.Size(232, 20);
            this.txtPTZURL.TabIndex = 77;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(2, 98);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(55, 13);
            this.label22.TabIndex = 78;
            this.label22.Text = "Username";
            // 
            // txtPTZUsername
            // 
            this.txtPTZUsername.Location = new System.Drawing.Point(77, 94);
            this.txtPTZUsername.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPTZUsername.Name = "txtPTZUsername";
            this.txtPTZUsername.Size = new System.Drawing.Size(88, 20);
            this.txtPTZUsername.TabIndex = 79;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(2, 122);
            this.label42.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(53, 13);
            this.label42.TabIndex = 81;
            this.label42.Text = "Password";
            // 
            // txtPTZPassword
            // 
            this.txtPTZPassword.Location = new System.Drawing.Point(77, 118);
            this.txtPTZPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPTZPassword.Name = "txtPTZPassword";
            this.txtPTZPassword.PasswordChar = '*';
            this.txtPTZPassword.Size = new System.Drawing.Size(88, 20);
            this.txtPTZPassword.TabIndex = 80;
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Location = new System.Drawing.Point(2, 74);
            this.label91.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(46, 13);
            this.label91.TabIndex = 82;
            this.label91.Text = "Channel";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtPTZPassword, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label42, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtPTZUsername, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label22, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtPTZURL, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label91, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPTZChannel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkPTZFlipX, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.chkPTZFlipY, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.chkPTZRotate90, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.numPort, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(311, 288);
            this.tableLayoutPanel1.TabIndex = 85;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 84;
            this.label1.Text = "Port";
            // 
            // txtPTZChannel
            // 
            this.txtPTZChannel.Location = new System.Drawing.Point(77, 70);
            this.txtPTZChannel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPTZChannel.Name = "txtPTZChannel";
            this.txtPTZChannel.Size = new System.Drawing.Size(30, 20);
            this.txtPTZChannel.TabIndex = 83;
            // 
            // chkPTZFlipX
            // 
            this.chkPTZFlipX.AutoSize = true;
            this.chkPTZFlipX.Location = new System.Drawing.Point(79, 168);
            this.chkPTZFlipX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkPTZFlipX.Name = "chkPTZFlipX";
            this.chkPTZFlipX.Size = new System.Drawing.Size(52, 17);
            this.chkPTZFlipX.TabIndex = 86;
            this.chkPTZFlipX.Text = "Flip-X";
            this.chkPTZFlipX.UseVisualStyleBackColor = true;
            this.chkPTZFlipX.CheckedChanged += new System.EventHandler(this.chkPTZFlipX_CheckedChanged);
            // 
            // chkPTZFlipY
            // 
            this.chkPTZFlipY.AutoSize = true;
            this.chkPTZFlipY.Location = new System.Drawing.Point(79, 195);
            this.chkPTZFlipY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkPTZFlipY.Name = "chkPTZFlipY";
            this.chkPTZFlipY.Size = new System.Drawing.Size(52, 17);
            this.chkPTZFlipY.TabIndex = 87;
            this.chkPTZFlipY.Text = "Flip-Y";
            this.chkPTZFlipY.UseVisualStyleBackColor = true;
            this.chkPTZFlipY.CheckedChanged += new System.EventHandler(this.chkPTZFlipY_CheckedChanged);
            // 
            // chkPTZRotate90
            // 
            this.chkPTZRotate90.AutoSize = true;
            this.chkPTZRotate90.Location = new System.Drawing.Point(79, 222);
            this.chkPTZRotate90.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkPTZRotate90.Name = "chkPTZRotate90";
            this.chkPTZRotate90.Size = new System.Drawing.Size(73, 17);
            this.chkPTZRotate90.TabIndex = 88;
            this.chkPTZRotate90.Text = "Rotate 90";
            this.chkPTZRotate90.UseVisualStyleBackColor = true;
            this.chkPTZRotate90.CheckedChanged += new System.EventHandler(this.chkPTZRotate90_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 146);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 89;
            this.label2.Text = "Pelco";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(77, 142);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 19);
            this.button1.TabIndex = 90;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(77, 46);
            this.numPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(87, 20);
            this.numPort.TabIndex = 91;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(77, 246);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(232, 40);
            this.flowLayoutPanel1.TabIndex = 92;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(2, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 19);
            this.button2.TabIndex = 0;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(62, 2);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 19);
            this.button3.TabIndex = 1;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 13);
            this.label3.TabIndex = 93;
            this.label3.Text = "(override url - eg: http://someurl.com/)";
            // 
            // PTZConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 288);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PTZConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PTZConfig";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PTZConfig_FormClosing);
            this.Load += new System.EventHandler(this.PTZConfig_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtPTZURL;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtPTZUsername;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox txtPTZPassword;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPTZFlipX;
        private System.Windows.Forms.CheckBox chkPTZFlipY;
        private System.Windows.Forms.CheckBox chkPTZRotate90;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPTZChannel;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
    }
}