namespace Waddu.Forms
{
    partial class UpdateAvailableForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.llInfoSite = new System.Windows.Forms.LinkLabel();
            this.lblLocalVersion = new System.Windows.Forms.Label();
            this.lblNewestVersion = new System.Windows.Forms.Label();
            this.llInfoSiteMirror = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Your Version";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Newest Version";
            // 
            // llInfoSite
            // 
            this.llInfoSite.AutoSize = true;
            this.llInfoSite.Location = new System.Drawing.Point(12, 66);
            this.llInfoSite.Name = "llInfoSite";
            this.llInfoSite.Size = new System.Drawing.Size(183, 13);
            this.llInfoSite.TabIndex = 2;
            this.llInfoSite.TabStop = true;
            this.llInfoSite.Text = "Click here to go to the Download Site";
            this.llInfoSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // lblLocalVersion
            // 
            this.lblLocalVersion.AutoSize = true;
            this.lblLocalVersion.Location = new System.Drawing.Point(124, 9);
            this.lblLocalVersion.Name = "lblLocalVersion";
            this.lblLocalVersion.Size = new System.Drawing.Size(35, 13);
            this.lblLocalVersion.TabIndex = 3;
            this.lblLocalVersion.Text = "label3";
            // 
            // lblNewestVersion
            // 
            this.lblNewestVersion.AutoSize = true;
            this.lblNewestVersion.Location = new System.Drawing.Point(124, 37);
            this.lblNewestVersion.Name = "lblNewestVersion";
            this.lblNewestVersion.Size = new System.Drawing.Size(35, 13);
            this.lblNewestVersion.TabIndex = 4;
            this.lblNewestVersion.Text = "label3";
            // 
            // llInfoSiteMirror
            // 
            this.llInfoSiteMirror.AutoSize = true;
            this.llInfoSiteMirror.Location = new System.Drawing.Point(201, 66);
            this.llInfoSiteMirror.Name = "llInfoSiteMirror";
            this.llInfoSiteMirror.Size = new System.Drawing.Size(39, 13);
            this.llInfoSiteMirror.TabIndex = 5;
            this.llInfoSiteMirror.TabStop = true;
            this.llInfoSiteMirror.Text = "(Mirror)";
            this.llInfoSiteMirror.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // UpdateAvailableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 95);
            this.Controls.Add(this.llInfoSiteMirror);
            this.Controls.Add(this.lblNewestVersion);
            this.Controls.Add(this.lblLocalVersion);
            this.Controls.Add(this.llInfoSite);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UpdateAvailableForm";
            this.Text = "Update Available";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel llInfoSite;
        private System.Windows.Forms.Label lblLocalVersion;
        private System.Windows.Forms.Label lblNewestVersion;
        private System.Windows.Forms.LinkLabel llInfoSiteMirror;
    }
}