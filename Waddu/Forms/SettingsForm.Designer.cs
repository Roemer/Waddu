namespace Waddu.Forms
{
    partial class SettingsForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblWoWPath = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtWoWPath = new System.Windows.Forms.TextBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.chkMoveToTrash = new System.Windows.Forms.CheckBox();
            this.lblNumOfThreads = new System.Windows.Forms.Label();
            this.numThreads = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.txtCursePassword = new System.Windows.Forms.TextBox();
            this.txtCurseLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMappingFile = new System.Windows.Forms.TextBox();
            this.chkNoLib = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt7zPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(311, 278);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(392, 278);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblWoWPath
            // 
            this.lblWoWPath.AutoSize = true;
            this.lblWoWPath.Location = new System.Drawing.Point(12, 17);
            this.lblWoWPath.Name = "lblWoWPath";
            this.lblWoWPath.Size = new System.Drawing.Size(60, 13);
            this.lblWoWPath.TabIndex = 2;
            this.lblWoWPath.Text = "WoW Path";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(392, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtWoWPath
            // 
            this.txtWoWPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWoWPath.Location = new System.Drawing.Point(87, 14);
            this.txtWoWPath.Name = "txtWoWPath";
            this.txtWoWPath.Size = new System.Drawing.Size(299, 20);
            this.txtWoWPath.TabIndex = 4;
            // 
            // chkDelete
            // 
            this.chkDelete.AutoSize = true;
            this.chkDelete.Location = new System.Drawing.Point(12, 42);
            this.chkDelete.Name = "chkDelete";
            this.chkDelete.Size = new System.Drawing.Size(136, 17);
            this.chkDelete.TabIndex = 5;
            this.chkDelete.Text = "Delete before Updating";
            this.chkDelete.UseVisualStyleBackColor = true;
            // 
            // chkMoveToTrash
            // 
            this.chkMoveToTrash.AutoSize = true;
            this.chkMoveToTrash.Location = new System.Drawing.Point(26, 65);
            this.chkMoveToTrash.Name = "chkMoveToTrash";
            this.chkMoveToTrash.Size = new System.Drawing.Size(95, 17);
            this.chkMoveToTrash.TabIndex = 6;
            this.chkMoveToTrash.Text = "Move to Trash";
            this.chkMoveToTrash.UseVisualStyleBackColor = true;
            // 
            // lblNumOfThreads
            // 
            this.lblNumOfThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNumOfThreads.AutoSize = true;
            this.lblNumOfThreads.Location = new System.Drawing.Point(297, 57);
            this.lblNumOfThreads.Name = "lblNumOfThreads";
            this.lblNumOfThreads.Size = new System.Drawing.Size(68, 13);
            this.lblNumOfThreads.TabIndex = 7;
            this.lblNumOfThreads.Text = "# of Threads";
            // 
            // numThreads
            // 
            this.numThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numThreads.Location = new System.Drawing.Point(371, 53);
            this.numThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreads.Name = "numThreads";
            this.numThreads.Size = new System.Drawing.Size(96, 20);
            this.numThreads.TabIndex = 8;
            this.numThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Login";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSavePassword);
            this.groupBox1.Controls.Add(this.txtCursePassword);
            this.groupBox1.Controls.Add(this.txtCurseLogin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 98);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WoWAce/Curse Credentials";
            // 
            // chkSavePassword
            // 
            this.chkSavePassword.AutoSize = true;
            this.chkSavePassword.Location = new System.Drawing.Point(65, 72);
            this.chkSavePassword.Name = "chkSavePassword";
            this.chkSavePassword.Size = new System.Drawing.Size(100, 17);
            this.chkSavePassword.TabIndex = 13;
            this.chkSavePassword.Text = "Save Password";
            this.chkSavePassword.UseVisualStyleBackColor = true;
            // 
            // txtCursePassword
            // 
            this.txtCursePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCursePassword.Location = new System.Drawing.Point(65, 45);
            this.txtCursePassword.Name = "txtCursePassword";
            this.txtCursePassword.PasswordChar = '*';
            this.txtCursePassword.Size = new System.Drawing.Size(194, 20);
            this.txtCursePassword.TabIndex = 12;
            // 
            // txtCurseLogin
            // 
            this.txtCurseLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurseLogin.Location = new System.Drawing.Point(65, 19);
            this.txtCurseLogin.Name = "txtCurseLogin";
            this.txtCurseLogin.Size = new System.Drawing.Size(194, 20);
            this.txtCurseLogin.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Mapping File";
            // 
            // txtMappingFile
            // 
            this.txtMappingFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMappingFile.Location = new System.Drawing.Point(85, 92);
            this.txtMappingFile.Name = "txtMappingFile";
            this.txtMappingFile.Size = new System.Drawing.Size(382, 20);
            this.txtMappingFile.TabIndex = 12;
            // 
            // chkNoLib
            // 
            this.chkNoLib.AutoSize = true;
            this.chkNoLib.Location = new System.Drawing.Point(12, 225);
            this.chkNoLib.Name = "chkNoLib";
            this.chkNoLib.Size = new System.Drawing.Size(85, 17);
            this.chkNoLib.TabIndex = 13;
            this.chkNoLib.Text = "Prefer NoLib";
            this.chkNoLib.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Path to 7z.exe";
            // 
            // txt7zPath
            // 
            this.txt7zPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt7zPath.Location = new System.Drawing.Point(93, 249);
            this.txt7zPath.Name = "txt7zPath";
            this.txt7zPath.Size = new System.Drawing.Size(374, 20);
            this.txt7zPath.TabIndex = 15;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(479, 313);
            this.Controls.Add(this.txt7zPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkNoLib);
            this.Controls.Add(this.txtMappingFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numThreads);
            this.Controls.Add(this.lblNumOfThreads);
            this.Controls.Add(this.chkMoveToTrash);
            this.Controls.Add(this.chkDelete);
            this.Controls.Add(this.txtWoWPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblWoWPath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblWoWPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtWoWPath;
        private System.Windows.Forms.CheckBox chkDelete;
        private System.Windows.Forms.CheckBox chkMoveToTrash;
        private System.Windows.Forms.Label lblNumOfThreads;
        private System.Windows.Forms.NumericUpDown numThreads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSavePassword;
        private System.Windows.Forms.TextBox txtCursePassword;
        private System.Windows.Forms.TextBox txtCurseLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMappingFile;
        private System.Windows.Forms.CheckBox chkNoLib;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt7zPath;
    }
}