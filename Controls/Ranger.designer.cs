namespace iSpyApplication.Controls
{
    sealed partial class Ranger
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtVal1 = new System.Windows.Forms.TextBox();
            this.txtVal2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numGain = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numGain)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtVal1
            // 
            this.txtVal1.Location = new System.Drawing.Point(4, 4);
            this.txtVal1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVal1.Name = "txtVal1";
            this.txtVal1.Size = new System.Drawing.Size(83, 22);
            this.txtVal1.TabIndex = 0;
            this.txtVal1.TextChanged += new System.EventHandler(this.txtVal1_TextChanged);
            this.txtVal1.Leave += new System.EventHandler(this.txtVal1_Leave);
            // 
            // txtVal2
            // 
            this.txtVal2.Location = new System.Drawing.Point(129, 4);
            this.txtVal2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVal2.Name = "txtVal2";
            this.txtVal2.Size = new System.Drawing.Size(83, 22);
            this.txtVal2.TabIndex = 1;
            this.txtVal2.TextChanged += new System.EventHandler(this.txtVal2_TextChanged);
            this.txtVal2.Leave += new System.EventHandler(this.txtVal2_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "-->";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Gain";
            // 
            // numGain
            // 
            this.numGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numGain.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.numGain.DecimalPlaces = 5;
            this.numGain.Location = new System.Drawing.Point(354, 4);
            this.numGain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numGain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numGain.Name = "numGain";
            this.numGain.Size = new System.Drawing.Size(83, 22);
            this.numGain.TabIndex = 4;
            this.numGain.Value = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numGain.ValueChanged += new System.EventHandler(this.numGain_ValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtVal2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numGain, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtVal1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(441, 30);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // Ranger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Ranger";
            this.Size = new System.Drawing.Size(441, 71);
            this.Load += new System.EventHandler(this.Ranger_Load);
            this.SizeChanged += new System.EventHandler(this.Ranger_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RangerMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RangerMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RangerMouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.numGain)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtVal1;
        private System.Windows.Forms.TextBox txtVal2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numGain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
