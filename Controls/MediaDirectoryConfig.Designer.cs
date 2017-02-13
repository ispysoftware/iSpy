namespace iSpyApplication.Controls
{
    partial class MediaDirectoryConfig
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
            this.gbStorage = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDaysDelete = new System.Windows.Forms.NumericUpDown();
            this.txtMaxMediaSize = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chkStopRecording = new System.Windows.Forms.CheckBox();
            this.chkArchive = new System.Windows.Forms.CheckBox();
            this.chkStorage = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtMediaDirectory = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.gbStorage.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDaysDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxMediaSize)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbStorage
            // 
            this.gbStorage.Controls.Add(this.tableLayoutPanel3);
            this.gbStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbStorage.Location = new System.Drawing.Point(67, 66);
            this.gbStorage.Margin = new System.Windows.Forms.Padding(6);
            this.gbStorage.Name = "gbStorage";
            this.gbStorage.Padding = new System.Windows.Forms.Padding(6);
            this.gbStorage.Size = new System.Drawing.Size(477, 186);
            this.gbStorage.TabIndex = 38;
            this.gbStorage.TabStop = false;
            this.gbStorage.Text = "Storage Management";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label12, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtDaysDelete, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtMaxMediaSize, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label9, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.chkStopRecording, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.chkArchive, 1, 4);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(465, 161);
            this.tableLayoutPanel3.TabIndex = 39;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(6, 8);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 23);
            this.label10.TabIndex = 32;
            this.label10.Text = "Max. Media Folder Size";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(235, 101);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(153, 18);
            this.label12.TabIndex = 36;
            this.label12.Text = "days old (0 for no deletions)";
            // 
            // txtDaysDelete
            // 
            this.txtDaysDelete.Location = new System.Drawing.Point(135, 99);
            this.txtDaysDelete.Margin = new System.Windows.Forms.Padding(6);
            this.txtDaysDelete.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtDaysDelete.Name = "txtDaysDelete";
            this.txtDaysDelete.Size = new System.Drawing.Size(88, 20);
            this.txtDaysDelete.TabIndex = 38;
            // 
            // txtMaxMediaSize
            // 
            this.txtMaxMediaSize.Location = new System.Drawing.Point(135, 6);
            this.txtMaxMediaSize.Margin = new System.Windows.Forms.Padding(6);
            this.txtMaxMediaSize.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.txtMaxMediaSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtMaxMediaSize.Name = "txtMaxMediaSize";
            this.txtMaxMediaSize.Size = new System.Drawing.Size(88, 20);
            this.txtMaxMediaSize.TabIndex = 37;
            this.txtMaxMediaSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(235, 8);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "MB";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label11, 3);
            this.label11.Location = new System.Drawing.Point(6, 70);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(179, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "When over 70% full, delete files over";
            // 
            // chkStopRecording
            // 
            this.chkStopRecording.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.chkStopRecording, 2);
            this.chkStopRecording.Location = new System.Drawing.Point(135, 37);
            this.chkStopRecording.Margin = new System.Windows.Forms.Padding(6);
            this.chkStopRecording.Name = "chkStopRecording";
            this.chkStopRecording.Size = new System.Drawing.Size(194, 17);
            this.chkStopRecording.TabIndex = 39;
            this.chkStopRecording.Text = "Stop recording when limit exceeded";
            this.chkStopRecording.UseVisualStyleBackColor = true;
            // 
            // chkArchive
            // 
            this.chkArchive.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.chkArchive, 2);
            this.chkArchive.Location = new System.Drawing.Point(135, 130);
            this.chkArchive.Margin = new System.Windows.Forms.Padding(6);
            this.chkArchive.Name = "chkArchive";
            this.chkArchive.Size = new System.Drawing.Size(143, 17);
            this.chkArchive.TabIndex = 40;
            this.chkArchive.Text = "Archive instead of delete";
            this.chkArchive.UseVisualStyleBackColor = true;
            // 
            // chkStorage
            // 
            this.chkStorage.AutoSize = true;
            this.chkStorage.Location = new System.Drawing.Point(67, 37);
            this.chkStorage.Margin = new System.Windows.Forms.Padding(6);
            this.chkStorage.Name = "chkStorage";
            this.chkStorage.Size = new System.Drawing.Size(161, 17);
            this.chkStorage.TabIndex = 39;
            this.chkStorage.Text = "Enable storage management";
            this.chkStorage.UseVisualStyleBackColor = true;
            this.chkStorage.CheckedChanged += new System.EventHandler(this.chkStorage_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbStorage, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkStorage, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(550, 295);
            this.tableLayoutPanel1.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Directory";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.txtMediaDirectory);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(61, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(489, 31);
            this.flowLayoutPanel1.TabIndex = 40;
            // 
            // txtMediaDirectory
            // 
            this.txtMediaDirectory.Enabled = false;
            this.txtMediaDirectory.Location = new System.Drawing.Point(6, 6);
            this.txtMediaDirectory.Name = "txtMediaDirectory";
            this.txtMediaDirectory.Size = new System.Drawing.Size(229, 20);
            this.txtMediaDirectory.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(241, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(472, 261);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 41;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MediaDirectoryConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 295);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MediaDirectoryConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Media Directory Configuration";
            this.Load += new System.EventHandler(this.MediaDirectoryConfig_Load);
            this.gbStorage.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDaysDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxMediaSize)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbStorage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown txtDaysDelete;
        private System.Windows.Forms.NumericUpDown txtMaxMediaSize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkStopRecording;
        private System.Windows.Forms.CheckBox chkStorage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox txtMediaDirectory;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkArchive;
    }
}