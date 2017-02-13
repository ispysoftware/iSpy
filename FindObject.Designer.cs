namespace iSpyApplication
{
    partial class FindObject
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
            this.ddlObject = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ddlObject
            // 
            this.ddlObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlObject.FormattingEnabled = true;
            this.ddlObject.Location = new System.Drawing.Point(3, 3);
            this.ddlObject.Name = "ddlObject";
            this.ddlObject.Size = new System.Drawing.Size(173, 21);
            this.ddlObject.TabIndex = 0;
            this.ddlObject.SelectedIndexChanged += new System.EventHandler(this.ddlObject_SelectedIndexChanged);
            // 
            // FindObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 28);
            this.Controls.Add(this.ddlObject);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindObject";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Object";
            this.Activated += new System.EventHandler(this.FindObject_Activated);
            this.Load += new System.EventHandler(this.FindObject_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlObject;
    }
}