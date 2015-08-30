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
            ((System.ComponentModel.ISupportInitialize)(this.numGain)).BeginInit();
            this.SuspendLayout();
            // 
            // txtVal1
            // 
            this.txtVal1.Location = new System.Drawing.Point(3, 34);
            this.txtVal1.Name = "txtVal1";
            this.txtVal1.Size = new System.Drawing.Size(63, 20);
            this.txtVal1.TabIndex = 0;
            this.txtVal1.TextChanged += new System.EventHandler(this.txtVal1_TextChanged);
            this.txtVal1.Leave += new System.EventHandler(this.txtVal1_Leave);
            // 
            // txtVal2
            // 
            this.txtVal2.Location = new System.Drawing.Point(97, 34);
            this.txtVal2.Name = "txtVal2";
            this.txtVal2.Size = new System.Drawing.Size(63, 20);
            this.txtVal2.TabIndex = 1;
            this.txtVal2.TextChanged += new System.EventHandler(this.txtVal2_TextChanged);
            this.txtVal2.Leave += new System.EventHandler(this.txtVal2_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "-->";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Gain";
            // 
            // numGain
            // 
            this.numGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numGain.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.numGain.DecimalPlaces = 5;
            this.numGain.Location = new System.Drawing.Point(266, 34);
            this.numGain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numGain.Name = "numGain";
            this.numGain.Size = new System.Drawing.Size(62, 20);
            this.numGain.TabIndex = 4;
            this.numGain.Value = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numGain.ValueChanged += new System.EventHandler(this.numGain_ValueChanged);
            // 
            // Ranger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numGain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtVal2);
            this.Controls.Add(this.txtVal1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "Ranger";
            this.Size = new System.Drawing.Size(331, 58);
            this.Load += new System.EventHandler(this.Ranger_Load);
            this.SizeChanged += new System.EventHandler(this.Ranger_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RangerMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RangerMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RangerMouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.numGain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtVal1;
        private System.Windows.Forms.TextBox txtVal2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numGain;
    }
}
