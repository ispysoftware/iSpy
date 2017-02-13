namespace iSpyApplication.Controls
{
    partial class PTZTracking
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
            this.pnlTrack = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numAutoHomeDelay = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numTTH = new System.Windows.Forms.NumericUpDown();
            this.label87 = new System.Windows.Forms.Label();
            this.ddlHomeCommand = new System.Windows.Forms.ComboBox();
            this.label59 = new System.Windows.Forms.Label();
            this.chkAutoHome = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel21 = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoAny = new System.Windows.Forms.RadioButton();
            this.rdoVert = new System.Windows.Forms.RadioButton();
            this.rdoHor = new System.Windows.Forms.RadioButton();
            this.chkReverseTracking = new System.Windows.Forms.CheckBox();
            this.chkTrack = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlTrack.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoHomeDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTTH)).BeginInit();
            this.flowLayoutPanel21.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTrack
            // 
            this.pnlTrack.ColumnCount = 2;
            this.pnlTrack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.pnlTrack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlTrack.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.pnlTrack.Controls.Add(this.chkAutoHome, 0, 1);
            this.pnlTrack.Controls.Add(this.flowLayoutPanel21, 0, 0);
            this.pnlTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrack.Location = new System.Drawing.Point(0, 47);
            this.pnlTrack.Name = "pnlTrack";
            this.pnlTrack.RowCount = 2;
            this.pnlTrack.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.pnlTrack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlTrack.Size = new System.Drawing.Size(452, 149);
            this.pnlTrack.TabIndex = 82;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.06897F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.93103F));
            this.tableLayoutPanel1.Controls.Add(this.numAutoHomeDelay, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numTTH, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label87, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ddlHomeCommand, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label59, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(147, 36);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(302, 110);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // numAutoHomeDelay
            // 
            this.numAutoHomeDelay.Location = new System.Drawing.Point(117, 68);
            this.numAutoHomeDelay.Margin = new System.Windows.Forms.Padding(6);
            this.numAutoHomeDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numAutoHomeDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAutoHomeDelay.Name = "numAutoHomeDelay";
            this.numAutoHomeDelay.Size = new System.Drawing.Size(39, 20);
            this.numAutoHomeDelay.TabIndex = 6;
            this.numAutoHomeDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Home Delay";
            // 
            // numTTH
            // 
            this.numTTH.Location = new System.Drawing.Point(117, 37);
            this.numTTH.Margin = new System.Windows.Forms.Padding(6);
            this.numTTH.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTTH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTTH.Name = "numTTH";
            this.numTTH.Size = new System.Drawing.Size(40, 20);
            this.numTTH.TabIndex = 4;
            this.numTTH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.Location = new System.Drawing.Point(6, 39);
            this.label87.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(77, 13);
            this.label87.TabIndex = 5;
            this.label87.Text = "Time To Home";
            // 
            // ddlHomeCommand
            // 
            this.ddlHomeCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlHomeCommand.FormattingEnabled = true;
            this.ddlHomeCommand.Location = new System.Drawing.Point(117, 6);
            this.ddlHomeCommand.Margin = new System.Windows.Forms.Padding(6);
            this.ddlHomeCommand.Name = "ddlHomeCommand";
            this.ddlHomeCommand.Size = new System.Drawing.Size(110, 21);
            this.ddlHomeCommand.TabIndex = 9;
            this.ddlHomeCommand.SelectedIndexChanged += new System.EventHandler(this.ddlHomeCommand_SelectedIndexChanged);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(6, 8);
            this.label59.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(54, 13);
            this.label59.TabIndex = 8;
            this.label59.Text = "Command";
            // 
            // chkAutoHome
            // 
            this.chkAutoHome.AutoSize = true;
            this.chkAutoHome.Location = new System.Drawing.Point(6, 39);
            this.chkAutoHome.Margin = new System.Windows.Forms.Padding(6);
            this.chkAutoHome.Name = "chkAutoHome";
            this.chkAutoHome.Size = new System.Drawing.Size(79, 17);
            this.chkAutoHome.TabIndex = 3;
            this.chkAutoHome.Text = "Auto Home";
            this.chkAutoHome.UseVisualStyleBackColor = true;
            this.chkAutoHome.CheckedChanged += new System.EventHandler(this.chkAutoHome_CheckedChanged);
            // 
            // flowLayoutPanel21
            // 
            this.pnlTrack.SetColumnSpan(this.flowLayoutPanel21, 2);
            this.flowLayoutPanel21.Controls.Add(this.rdoAny);
            this.flowLayoutPanel21.Controls.Add(this.rdoVert);
            this.flowLayoutPanel21.Controls.Add(this.rdoHor);
            this.flowLayoutPanel21.Controls.Add(this.chkReverseTracking);
            this.flowLayoutPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel21.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel21.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel21.Name = "flowLayoutPanel21";
            this.flowLayoutPanel21.Size = new System.Drawing.Size(452, 33);
            this.flowLayoutPanel21.TabIndex = 8;
            // 
            // rdoAny
            // 
            this.rdoAny.AutoSize = true;
            this.rdoAny.Location = new System.Drawing.Point(6, 6);
            this.rdoAny.Margin = new System.Windows.Forms.Padding(6);
            this.rdoAny.Name = "rdoAny";
            this.rdoAny.Size = new System.Drawing.Size(88, 17);
            this.rdoAny.TabIndex = 0;
            this.rdoAny.TabStop = true;
            this.rdoAny.Text = "Any Direction";
            this.rdoAny.UseVisualStyleBackColor = true;
            // 
            // rdoVert
            // 
            this.rdoVert.AutoSize = true;
            this.rdoVert.Location = new System.Drawing.Point(106, 6);
            this.rdoVert.Margin = new System.Windows.Forms.Padding(6);
            this.rdoVert.Name = "rdoVert";
            this.rdoVert.Size = new System.Drawing.Size(84, 17);
            this.rdoVert.TabIndex = 2;
            this.rdoVert.TabStop = true;
            this.rdoVert.Text = "Vertical Only";
            this.rdoVert.UseVisualStyleBackColor = true;
            // 
            // rdoHor
            // 
            this.rdoHor.AutoSize = true;
            this.rdoHor.Location = new System.Drawing.Point(202, 6);
            this.rdoHor.Margin = new System.Windows.Forms.Padding(6);
            this.rdoHor.Name = "rdoHor";
            this.rdoHor.Size = new System.Drawing.Size(96, 17);
            this.rdoHor.TabIndex = 1;
            this.rdoHor.TabStop = true;
            this.rdoHor.Text = "Horizontal Only";
            this.rdoHor.UseVisualStyleBackColor = true;
            // 
            // chkReverseTracking
            // 
            this.chkReverseTracking.AutoSize = true;
            this.chkReverseTracking.Location = new System.Drawing.Point(310, 6);
            this.chkReverseTracking.Margin = new System.Windows.Forms.Padding(6);
            this.chkReverseTracking.Name = "chkReverseTracking";
            this.chkReverseTracking.Size = new System.Drawing.Size(66, 17);
            this.chkReverseTracking.TabIndex = 3;
            this.chkReverseTracking.Text = "Reverse";
            this.chkReverseTracking.UseVisualStyleBackColor = true;
            // 
            // chkTrack
            // 
            this.chkTrack.AutoSize = true;
            this.chkTrack.Location = new System.Drawing.Point(6, 15);
            this.chkTrack.Margin = new System.Windows.Forms.Padding(6);
            this.chkTrack.Name = "chkTrack";
            this.chkTrack.Size = new System.Drawing.Size(396, 17);
            this.chkTrack.TabIndex = 78;
            this.chkTrack.Text = "Track Objects (Requires Object Tracking display style on motion detection tab)";
            this.chkTrack.UseVisualStyleBackColor = true;
            this.chkTrack.CheckedChanged += new System.EventHandler(this.chkTrack_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkTrack);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 47);
            this.panel1.TabIndex = 83;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 196);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(452, 41);
            this.panel2.TabIndex = 84;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(365, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // PTZTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 237);
            this.Controls.Add(this.pnlTrack);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PTZTracking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PTZTracking";
            this.Load += new System.EventHandler(this.PTZTracking_Load);
            this.pnlTrack.ResumeLayout(false);
            this.pnlTrack.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoHomeDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTTH)).EndInit();
            this.flowLayoutPanel21.ResumeLayout(false);
            this.flowLayoutPanel21.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlTrack;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel21;
        private System.Windows.Forms.RadioButton rdoAny;
        private System.Windows.Forms.RadioButton rdoVert;
        private System.Windows.Forms.RadioButton rdoHor;
        private System.Windows.Forms.CheckBox chkReverseTracking;
        private System.Windows.Forms.CheckBox chkAutoHome;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.ComboBox ddlHomeCommand;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.NumericUpDown numTTH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numAutoHomeDelay;
        private System.Windows.Forms.CheckBox chkTrack;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
    }
}