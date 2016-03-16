using System.ComponentModel;
using System.Windows.Forms;

namespace Waddu.UI.Forms
{
    partial class ChangeLogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.wbChangeLog = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wbChangeLog
            // 
            this.wbChangeLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wbChangeLog.Location = new System.Drawing.Point(12, 12);
            this.wbChangeLog.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChangeLog.Name = "wbChangeLog";
            this.wbChangeLog.Size = new System.Drawing.Size(660, 418);
            this.wbChangeLog.TabIndex = 1;
            // 
            // ChangeLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 442);
            this.Controls.Add(this.wbChangeLog);
            this.Name = "ChangeLogForm";
            this.Text = "Change Log";
            this.ResumeLayout(false);

        }

        #endregion

        private WebBrowser wbChangeLog;
    }
}