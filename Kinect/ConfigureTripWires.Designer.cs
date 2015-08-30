namespace iSpyApplication.Kinect
{
    partial class ConfigureTripWires
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
            this.TripWireEditor1 = new iSpyApplication.Kinect.TripWireEditor();
            this.SuspendLayout();
            // 
            // TripWireEditor1
            // 
            this.TripWireEditor1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TripWireEditor1.Location = new System.Drawing.Point(0, 0);
            this.TripWireEditor1.Margin = new System.Windows.Forms.Padding(0);
            this.TripWireEditor1.Name = "TripWireEditor1";
            this.TripWireEditor1.Size = new System.Drawing.Size(640, 480);
            this.TripWireEditor1.TabIndex = 0;
            // 
            // ConfigureTripWires
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(641, 481);
            this.Controls.Add(this.TripWireEditor1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfigureTripWires";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Trip Wires";
            this.Load += new System.EventHandler(this.ConfigureTripWires_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public TripWireEditor TripWireEditor1;

    }
}