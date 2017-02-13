namespace iSpyApplication
{
    partial class AddFloorPlan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddFloorPlan));
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbObjects = new System.Windows.Forms.ListBox();
            this.ttObject = new System.Windows.Forms.ToolTip(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.llblHelp = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkOriginalSize = new System.Windows.Forms.CheckBox();
            this.pnlFloorPlan = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAccessGroups = new System.Windows.Forms.Label();
            this.txtAccessGroups = new System.Windows.Forms.TextBox();
            this.linkLabel14 = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            this.txtName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtNameKeyUp);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnChooseFile
            // 
            resources.ApplyResources(this.btnChooseFile, "btnChooseFile");
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.BtnChooseFileClick);
            // 
            // btnFinish
            // 
            resources.ApplyResources(this.btnFinish, "btnFinish");
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.BtnFinishClick);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lbObjects
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.lbObjects, 2);
            resources.ApplyResources(this.lbObjects, "lbObjects");
            this.lbObjects.FormattingEnabled = true;
            this.lbObjects.Name = "lbObjects";
            this.lbObjects.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.LbObjectsQueryContinueDrag);
            this.lbObjects.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LbObjectsMouseDown);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // llblHelp
            // 
            resources.ApplyResources(this.llblHelp, "llblHelp");
            this.llblHelp.Name = "llblHelp";
            this.llblHelp.TabStop = true;
            this.llblHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblHelp_LinkClicked);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.txtName);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.btnChooseFile);
            this.flowLayoutPanel1.Controls.Add(this.chkOriginalSize);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // chkOriginalSize
            // 
            resources.ApplyResources(this.chkOriginalSize, "chkOriginalSize");
            this.chkOriginalSize.Name = "chkOriginalSize";
            this.chkOriginalSize.UseVisualStyleBackColor = true;
            this.chkOriginalSize.CheckedChanged += new System.EventHandler(this.chkOriginalSize_CheckedChanged);
            // 
            // pnlFloorPlan
            // 
            resources.ApplyResources(this.pnlFloorPlan, "pnlFloorPlan");
            this.pnlFloorPlan.Name = "pnlFloorPlan";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbObjects, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblAccessGroups, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtAccessGroups, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel14, 2, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // lblAccessGroups
            // 
            resources.ApplyResources(this.lblAccessGroups, "lblAccessGroups");
            this.lblAccessGroups.Name = "lblAccessGroups";
            // 
            // txtAccessGroups
            // 
            resources.ApplyResources(this.txtAccessGroups, "txtAccessGroups");
            this.txtAccessGroups.Name = "txtAccessGroups";
            // 
            // linkLabel14
            // 
            resources.ApplyResources(this.linkLabel14, "linkLabel14");
            this.linkLabel14.Name = "linkLabel14";
            this.linkLabel14.TabStop = true;
            this.linkLabel14.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel14_LinkClicked);
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnFinish);
            this.flowLayoutPanel2.Controls.Add(this.llblHelp);
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.panel2);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // AddFloorPlan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlFloorPlan);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "AddFloorPlan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddFloorPlan_FormClosing);
            this.Load += new System.EventHandler(this.AddFloorPlanLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AddFloorPlanPaint);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbObjects;
        private System.Windows.Forms.ToolTip ttObject;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel llblHelp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel pnlFloorPlan;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkOriginalSize;
        private System.Windows.Forms.Label lblAccessGroups;
        private System.Windows.Forms.TextBox txtAccessGroups;
        private System.Windows.Forms.LinkLabel linkLabel14;
    }
}