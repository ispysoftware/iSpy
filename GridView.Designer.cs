namespace iSpyApplication
{
    partial class GridView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridView));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchFillModeAltFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeGridViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.switchFillModeAltFToolStripMenuItem,
            this.quickSelectToolStripMenuItem,
            this.fullScreenToolStripMenuItem,
            this.alwaysOnTopToolStripMenuItem,
            this.closeGridViewToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(245, 160);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.contextMenuStrip1_MouseMove);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // switchFillModeAltFToolStripMenuItem
            // 
            this.switchFillModeAltFToolStripMenuItem.Name = "switchFillModeAltFToolStripMenuItem";
            this.switchFillModeAltFToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M)));
            this.switchFillModeAltFToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.switchFillModeAltFToolStripMenuItem.Text = "Switch Fill Mode";
            this.switchFillModeAltFToolStripMenuItem.Click += new System.EventHandler(this.switchFillModeAltFToolStripMenuItem_Click);
            // 
            // quickSelectToolStripMenuItem
            // 
            this.quickSelectToolStripMenuItem.Name = "quickSelectToolStripMenuItem";
            this.quickSelectToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.quickSelectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.quickSelectToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.quickSelectToolStripMenuItem.Text = "Quick Select";
            this.quickSelectToolStripMenuItem.DropDownOpening += new System.EventHandler(this.quickSelectToolStripMenuItem_DropDownOpening);
            this.quickSelectToolStripMenuItem.Click += new System.EventHandler(this.quickSelectToolStripMenuItem_Click);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.CheckOnClick = true;
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.ShortcutKeyDisplayString = "Alt+ Enter";
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.fullScreenToolStripMenuItem.Text = "Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.alwaysOnTopToolStripMenuItem.Text = "Always On Top";
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem_Click);
            // 
            // closeGridViewToolStripMenuItem
            // 
            this.closeGridViewToolStripMenuItem.Name = "closeGridViewToolStripMenuItem";
            this.closeGridViewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.closeGridViewToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.closeGridViewToolStripMenuItem.Text = "Close Grid View";
            this.closeGridViewToolStripMenuItem.Click += new System.EventHandler(this.closeGridViewToolStripMenuItem_Click);
            // 
            // GridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 789);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GridView";
            this.Text = "iSpy Grid View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GridView_FormClosing);
            this.Load += new System.EventHandler(this.GridView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridView_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridView_MouseMove);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeGridViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchFillModeAltFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quickSelectToolStripMenuItem;

    }
}