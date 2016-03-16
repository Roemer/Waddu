﻿using System.ComponentModel;
using System.Windows.Forms;

namespace Waddu.UI.Forms
{
    partial class SettingsForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblWoWPath = new System.Windows.Forms.Label();
            this.btnBrowseWoW = new System.Windows.Forms.Button();
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
            this.btnBrowse7z = new System.Windows.Forms.Button();
            this.lbPriority = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnBrowseMapping = new System.Windows.Forms.Button();
            this.chkUseCustomMapping = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkUseOlderNoLib = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnUnignore = new System.Windows.Forms.Button();
            this.lvIgnored = new System.Windows.Forms.ListView();
            this.chIgnoredName = new System.Windows.Forms.ColumnHeader();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnRemovePreferred = new System.Windows.Forms.Button();
            this.lvPreferred = new System.Windows.Forms.ListView();
            this.chPreferredName = new System.Windows.Forms.ColumnHeader();
            this.chPreferredMapping = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(301, 239);
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
            this.btnCancel.Location = new System.Drawing.Point(382, 239);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblWoWPath
            // 
            this.lblWoWPath.AutoSize = true;
            this.lblWoWPath.Location = new System.Drawing.Point(6, 11);
            this.lblWoWPath.Name = "lblWoWPath";
            this.lblWoWPath.Size = new System.Drawing.Size(60, 13);
            this.lblWoWPath.TabIndex = 2;
            this.lblWoWPath.Text = "WoW Path";
            // 
            // btnBrowseWoW
            // 
            this.btnBrowseWoW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseWoW.Location = new System.Drawing.Point(356, 6);
            this.btnBrowseWoW.Name = "btnBrowseWoW";
            this.btnBrowseWoW.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseWoW.TabIndex = 3;
            this.btnBrowseWoW.Text = "Browse";
            this.btnBrowseWoW.UseVisualStyleBackColor = true;
            this.btnBrowseWoW.Click += new System.EventHandler(this.btnBrowseWoW_Click);
            // 
            // txtWoWPath
            // 
            this.txtWoWPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWoWPath.Location = new System.Drawing.Point(80, 8);
            this.txtWoWPath.Name = "txtWoWPath";
            this.txtWoWPath.Size = new System.Drawing.Size(270, 20);
            this.txtWoWPath.TabIndex = 4;
            // 
            // chkDelete
            // 
            this.chkDelete.AutoSize = true;
            this.chkDelete.Location = new System.Drawing.Point(6, 6);
            this.chkDelete.Name = "chkDelete";
            this.chkDelete.Size = new System.Drawing.Size(136, 17);
            this.chkDelete.TabIndex = 5;
            this.chkDelete.Text = "Delete before Updating";
            this.chkDelete.UseVisualStyleBackColor = true;
            this.chkDelete.CheckedChanged += new System.EventHandler(this.chkDelete_CheckedChanged);
            // 
            // chkMoveToTrash
            // 
            this.chkMoveToTrash.AutoSize = true;
            this.chkMoveToTrash.Location = new System.Drawing.Point(148, 6);
            this.chkMoveToTrash.Name = "chkMoveToTrash";
            this.chkMoveToTrash.Size = new System.Drawing.Size(95, 17);
            this.chkMoveToTrash.TabIndex = 6;
            this.chkMoveToTrash.Text = "Move to Trash";
            this.chkMoveToTrash.UseVisualStyleBackColor = true;
            // 
            // lblNumOfThreads
            // 
            this.lblNumOfThreads.AutoSize = true;
            this.lblNumOfThreads.Location = new System.Drawing.Point(6, 114);
            this.lblNumOfThreads.Name = "lblNumOfThreads";
            this.lblNumOfThreads.Size = new System.Drawing.Size(68, 13);
            this.lblNumOfThreads.TabIndex = 7;
            this.lblNumOfThreads.Text = "# of Threads";
            // 
            // numThreads
            // 
            this.numThreads.Location = new System.Drawing.Point(80, 112);
            this.numThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreads.Name = "numThreads";
            this.numThreads.Size = new System.Drawing.Size(75, 20);
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
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
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
            this.label3.Location = new System.Drawing.Point(30, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Mapping File";
            // 
            // txtMappingFile
            // 
            this.txtMappingFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMappingFile.Location = new System.Drawing.Point(103, 86);
            this.txtMappingFile.Name = "txtMappingFile";
            this.txtMappingFile.Size = new System.Drawing.Size(247, 20);
            this.txtMappingFile.TabIndex = 12;
            // 
            // chkNoLib
            // 
            this.chkNoLib.AutoSize = true;
            this.chkNoLib.Location = new System.Drawing.Point(6, 29);
            this.chkNoLib.Name = "chkNoLib";
            this.chkNoLib.Size = new System.Drawing.Size(85, 17);
            this.chkNoLib.TabIndex = 13;
            this.chkNoLib.Text = "Prefer NoLib";
            this.chkNoLib.UseVisualStyleBackColor = true;
            this.chkNoLib.CheckedChanged += new System.EventHandler(this.chkNoLib_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Path to 7z";
            // 
            // txt7zPath
            // 
            this.txt7zPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt7zPath.Location = new System.Drawing.Point(80, 37);
            this.txt7zPath.Name = "txt7zPath";
            this.txt7zPath.Size = new System.Drawing.Size(270, 20);
            this.txt7zPath.TabIndex = 15;
            // 
            // btnBrowse7z
            // 
            this.btnBrowse7z.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse7z.Location = new System.Drawing.Point(356, 35);
            this.btnBrowse7z.Name = "btnBrowse7z";
            this.btnBrowse7z.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse7z.TabIndex = 16;
            this.btnBrowse7z.Text = "Browse";
            this.btnBrowse7z.UseVisualStyleBackColor = true;
            this.btnBrowse7z.Click += new System.EventHandler(this.btnBrowse7z_Click);
            // 
            // lbPriority
            // 
            this.lbPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPriority.FormattingEnabled = true;
            this.lbPriority.IntegralHeight = false;
            this.lbPriority.Location = new System.Drawing.Point(6, 19);
            this.lbPriority.Name = "lbPriority";
            this.lbPriority.Size = new System.Drawing.Size(151, 108);
            this.lbPriority.TabIndex = 17;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(445, 221);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.btnBrowseMapping);
            this.tabPage1.Controls.Add(this.chkUseCustomMapping);
            this.tabPage1.Controls.Add(this.txtWoWPath);
            this.tabPage1.Controls.Add(this.lblWoWPath);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.numThreads);
            this.tabPage1.Controls.Add(this.txtMappingFile);
            this.tabPage1.Controls.Add(this.lblNumOfThreads);
            this.tabPage1.Controls.Add(this.btnBrowse7z);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnBrowseWoW);
            this.tabPage1.Controls.Add(this.txt7zPath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(437, 195);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnBrowseMapping
            // 
            this.btnBrowseMapping.Location = new System.Drawing.Point(356, 84);
            this.btnBrowseMapping.Name = "btnBrowseMapping";
            this.btnBrowseMapping.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseMapping.TabIndex = 18;
            this.btnBrowseMapping.Text = "Browse";
            this.btnBrowseMapping.UseVisualStyleBackColor = true;
            this.btnBrowseMapping.Click += new System.EventHandler(this.btnBrowseMapping_Click);
            // 
            // chkUseCustomMapping
            // 
            this.chkUseCustomMapping.AutoSize = true;
            this.chkUseCustomMapping.Location = new System.Drawing.Point(9, 63);
            this.chkUseCustomMapping.Name = "chkUseCustomMapping";
            this.chkUseCustomMapping.Size = new System.Drawing.Size(127, 17);
            this.chkUseCustomMapping.TabIndex = 17;
            this.chkUseCustomMapping.Text = "Use Custom Mapping";
            this.chkUseCustomMapping.UseVisualStyleBackColor = true;
            this.chkUseCustomMapping.CheckedChanged += new System.EventHandler(this.chkUseCustomMapping_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.chkUseOlderNoLib);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.chkDelete);
            this.tabPage2.Controls.Add(this.chkMoveToTrash);
            this.tabPage2.Controls.Add(this.chkNoLib);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(437, 195);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Updating";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkUseOlderNoLib
            // 
            this.chkUseOlderNoLib.AutoSize = true;
            this.chkUseOlderNoLib.Location = new System.Drawing.Point(148, 29);
            this.chkUseOlderNoLib.Name = "chkUseOlderNoLib";
            this.chkUseOlderNoLib.Size = new System.Drawing.Size(71, 17);
            this.chkUseOlderNoLib.TabIndex = 20;
            this.chkUseOlderNoLib.Text = "Use older";
            this.chkUseOlderNoLib.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbPriority);
            this.groupBox2.Location = new System.Drawing.Point(6, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 133);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Site Priorities";
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(437, 195);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Network";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnUnignore);
            this.tabPage4.Controls.Add(this.lvIgnored);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(437, 195);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Ignored";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnUnignore
            // 
            this.btnUnignore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnignore.Location = new System.Drawing.Point(356, 6);
            this.btnUnignore.Name = "btnUnignore";
            this.btnUnignore.Size = new System.Drawing.Size(75, 23);
            this.btnUnignore.TabIndex = 1;
            this.btnUnignore.Text = "Unignore";
            this.btnUnignore.UseVisualStyleBackColor = true;
            this.btnUnignore.Click += new System.EventHandler(this.btnUnignore_Click);
            // 
            // lvIgnored
            // 
            this.lvIgnored.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvIgnored.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chIgnoredName});
            this.lvIgnored.FullRowSelect = true;
            this.lvIgnored.GridLines = true;
            this.lvIgnored.Location = new System.Drawing.Point(6, 6);
            this.lvIgnored.Name = "lvIgnored";
            this.lvIgnored.Size = new System.Drawing.Size(344, 183);
            this.lvIgnored.TabIndex = 0;
            this.lvIgnored.UseCompatibleStateImageBehavior = false;
            this.lvIgnored.View = System.Windows.Forms.View.Details;
            // 
            // chIgnoredName
            // 
            this.chIgnoredName.Text = "Name";
            this.chIgnoredName.Width = 200;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnRemovePreferred);
            this.tabPage5.Controls.Add(this.lvPreferred);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(437, 195);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Preferred";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnRemovePreferred
            // 
            this.btnRemovePreferred.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePreferred.Location = new System.Drawing.Point(356, 6);
            this.btnRemovePreferred.Name = "btnRemovePreferred";
            this.btnRemovePreferred.Size = new System.Drawing.Size(75, 23);
            this.btnRemovePreferred.TabIndex = 1;
            this.btnRemovePreferred.Text = "Remove";
            this.btnRemovePreferred.UseVisualStyleBackColor = true;
            this.btnRemovePreferred.Click += new System.EventHandler(this.btnRemovePreferred_Click);
            // 
            // lvPreferred
            // 
            this.lvPreferred.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPreferred.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPreferredName,
            this.chPreferredMapping});
            this.lvPreferred.FullRowSelect = true;
            this.lvPreferred.GridLines = true;
            this.lvPreferred.Location = new System.Drawing.Point(6, 6);
            this.lvPreferred.Name = "lvPreferred";
            this.lvPreferred.Size = new System.Drawing.Size(344, 183);
            this.lvPreferred.TabIndex = 0;
            this.lvPreferred.UseCompatibleStateImageBehavior = false;
            this.lvPreferred.View = System.Windows.Forms.View.Details;
            // 
            // chPreferredName
            // 
            this.chPreferredName.Text = "Name";
            this.chPreferredName.Width = 139;
            // 
            // chPreferredMapping
            // 
            this.chPreferredMapping.Text = "Mapping";
            this.chPreferredMapping.Width = 127;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(469, 274);
            this.Controls.Add(this.tabControl1);
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
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Label lblWoWPath;
        private Button btnBrowseWoW;
        private TextBox txtWoWPath;
        private CheckBox chkDelete;
        private CheckBox chkMoveToTrash;
        private Label lblNumOfThreads;
        private NumericUpDown numThreads;
        private Label label1;
        private GroupBox groupBox1;
        private CheckBox chkSavePassword;
        private TextBox txtCursePassword;
        private TextBox txtCurseLogin;
        private Label label2;
        private Label label3;
        private TextBox txtMappingFile;
        private CheckBox chkNoLib;
        private Label label4;
        private TextBox txt7zPath;
        private Button btnBrowse7z;
        private ListBox lbPriority;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private GroupBox groupBox2;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private CheckBox chkUseCustomMapping;
        private Button btnBrowseMapping;
        private ListView lvIgnored;
        private Button btnUnignore;
        private Button btnRemovePreferred;
        private ListView lvPreferred;
        private ColumnHeader chIgnoredName;
        private ColumnHeader chPreferredName;
        private ColumnHeader chPreferredMapping;
        private CheckBox chkUseOlderNoLib;
    }
}