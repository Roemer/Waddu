using System.ComponentModel;
using System.Windows.Forms;
using Waddu.UI.Controls;

namespace Waddu.UI.Forms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            var listViewGroup7 = new System.Windows.Forms.ListViewGroup("Dependencies", System.Windows.Forms.HorizontalAlignment.Left);
            var listViewGroup8 = new System.Windows.Forms.ListViewGroup("Contains", System.Windows.Forms.HorizontalAlignment.Left);
            var listViewGroup9 = new System.Windows.Forms.ListViewGroup("Part Of", System.Windows.Forms.HorizontalAlignment.Left);
            var dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddons = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReloadLocalAddons = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWadduAllAddons = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCollectUnknownAddons = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxAddon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddonCheckForUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddonUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddonIgnore = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddonUnignore = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddonMappings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddonSetAsUpdated = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxThread = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAbortThread = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMapping = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMappingChangeLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMappingCheckVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMappingUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMappingSetAsPreferred = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMappingInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMappingDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckAndUpdatdAddons = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainerEx2 = new Waddu.UI.Controls.SplitContainerEx();
            this.splitContainerEx1 = new Waddu.UI.Controls.SplitContainerEx();
            this.gbAddons = new System.Windows.Forms.GroupBox();
            this.dgvAddons = new System.Windows.Forms.DataGridView();
            this.dgvColAddonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColAddonLastUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColAddonLocalVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColAddonPreferredMapping = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsAddon = new System.Windows.Forms.ToolStrip();
            this.tsddFilter = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiFilterInstalled = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFilterNotInstalled = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFilterSubAddons = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFilterBlizzard = new System.Windows.Forms.ToolStripMenuItem();
            this.txtAddonFilter = new System.Windows.Forms.ToolStripTextBox();
            this.gbAddonDetails = new System.Windows.Forms.GroupBox();
            this.gbRelations = new System.Windows.Forms.GroupBox();
            this.lvRelations = new System.Windows.Forms.ListView();
            this.chRelationsName = new System.Windows.Forms.ColumnHeader();
            this.txtLocalVersion = new System.Windows.Forms.TextBox();
            this.lblLocalVersion = new System.Windows.Forms.Label();
            this.gbMappings = new System.Windows.Forms.GroupBox();
            this.dgvMappings = new System.Windows.Forms.DataGridView();
            this.dgvColMappingSite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColMappingVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColMappingLastUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.splitContainerEx3 = new Waddu.UI.Controls.SplitContainerEx();
            this.dgvThreadActivity = new System.Windows.Forms.DataGridView();
            this.dgvColThreadID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColThreadState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColThreadInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.dgvColLogTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColLogType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColLogMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsLog = new System.Windows.Forms.ToolStrip();
            this.tsddLogLevel = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiLogDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogInformation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogWarning = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogError = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.ctxAddon.SuspendLayout();
            this.ctxLog.SuspendLayout();
            this.ctxThread.SuspendLayout();
            this.ctxMapping.SuspendLayout();
            this.splitContainerEx2.Panel1.SuspendLayout();
            this.splitContainerEx2.Panel2.SuspendLayout();
            this.splitContainerEx2.SuspendLayout();
            this.splitContainerEx1.Panel1.SuspendLayout();
            this.splitContainerEx1.Panel2.SuspendLayout();
            this.splitContainerEx1.SuspendLayout();
            this.gbAddons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddons)).BeginInit();
            this.tsAddon.SuspendLayout();
            this.gbAddonDetails.SuspendLayout();
            this.gbRelations.SuspendLayout();
            this.gbMappings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
            this.splitContainerEx3.Panel1.SuspendLayout();
            this.splitContainerEx3.Panel2.SuspendLayout();
            this.splitContainerEx3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThreadActivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.tsLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiAddons,
            this.tsmiHelp,
            this.tsmiAdmin});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(823, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSettings,
            this.toolStripMenuItem1,
            this.tsmiExit});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(37, 20);
            this.tsmiFile.Text = "&File";
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(116, 22);
            this.tsmiSettings.Text = "&Settings";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(113, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(116, 22);
            this.tsmiExit.Text = "E&xit";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiAddons
            // 
            this.tsmiAddons.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiReloadLocalAddons,
            this.tsmiCheckAndUpdatdAddons,
            this.toolStripMenuItem2,
            this.tsmiCheckForUpdates,
            this.tsmWadduAllAddons});
            this.tsmiAddons.Name = "tsmiAddons";
            this.tsmiAddons.Size = new System.Drawing.Size(60, 20);
            this.tsmiAddons.Text = "&Addons";
            // 
            // tsmiReloadLocalAddons
            // 
            this.tsmiReloadLocalAddons.Name = "tsmiReloadLocalAddons";
            this.tsmiReloadLocalAddons.Size = new System.Drawing.Size(219, 22);
            this.tsmiReloadLocalAddons.Text = "&Reload Local Addons";
            this.tsmiReloadLocalAddons.Click += new System.EventHandler(this.tsmiReloadLocalAddons_Click);
            // 
            // tsmiCheckForUpdates
            // 
            this.tsmiCheckForUpdates.Name = "tsmiCheckForUpdates";
            this.tsmiCheckForUpdates.Size = new System.Drawing.Size(219, 22);
            this.tsmiCheckForUpdates.Text = "Check for Updates";
            this.tsmiCheckForUpdates.Click += new System.EventHandler(this.tsmiCheckForUpdates_Click);
            // 
            // tsmWadduAllAddons
            // 
            this.tsmWadduAllAddons.Name = "tsmWadduAllAddons";
            this.tsmWadduAllAddons.Size = new System.Drawing.Size(219, 22);
            this.tsmWadduAllAddons.Text = "&Update Addons";
            this.tsmWadduAllAddons.Click += new System.EventHandler(this.tsmiUpdateAllAddons_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCollectUnknownAddons,
            this.tsmiHelpAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(44, 20);
            this.tsmiHelp.Text = "&Help";
            // 
            // tsmiCollectUnknownAddons
            // 
            this.tsmiCollectUnknownAddons.Name = "tsmiCollectUnknownAddons";
            this.tsmiCollectUnknownAddons.Size = new System.Drawing.Size(208, 22);
            this.tsmiCollectUnknownAddons.Text = "Collect unknown Addons";
            this.tsmiCollectUnknownAddons.Click += new System.EventHandler(this.tsmiCollectUnknownAddons_Click);
            // 
            // tsmiHelpAbout
            // 
            this.tsmiHelpAbout.Name = "tsmiHelpAbout";
            this.tsmiHelpAbout.Size = new System.Drawing.Size(208, 22);
            this.tsmiHelpAbout.Text = "&About";
            this.tsmiHelpAbout.Click += new System.EventHandler(this.tsmiHelpAbout_Click);
            // 
            // tsmiAdmin
            // 
            this.tsmiAdmin.Name = "tsmiAdmin";
            this.tsmiAdmin.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.tsmiAdmin.Size = new System.Drawing.Size(55, 20);
            this.tsmiAdmin.Text = "A&dmin";
            this.tsmiAdmin.Visible = false;
            this.tsmiAdmin.Click += new System.EventHandler(this.tsmiAdmin_Click);
            // 
            // ctxAddon
            // 
            this.ctxAddon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddonCheckForUpdate,
            this.tsmiAddonUpdate,
            this.tsmiAddonIgnore,
            this.tsmiAddonUnignore,
            this.tsmiAddonMappings,
            this.tsmiAddonSetAsUpdated});
            this.ctxAddon.Name = "ctxAddon";
            this.ctxAddon.Size = new System.Drawing.Size(167, 136);
            // 
            // tsmiAddonCheckForUpdate
            // 
            this.tsmiAddonCheckForUpdate.Name = "tsmiAddonCheckForUpdate";
            this.tsmiAddonCheckForUpdate.Size = new System.Drawing.Size(166, 22);
            this.tsmiAddonCheckForUpdate.Text = "&Check for Update";
            this.tsmiAddonCheckForUpdate.Click += new System.EventHandler(this.tsmiAddonCheckForUpdate_Click);
            // 
            // tsmiAddonUpdate
            // 
            this.tsmiAddonUpdate.Name = "tsmiAddonUpdate";
            this.tsmiAddonUpdate.Size = new System.Drawing.Size(166, 22);
            this.tsmiAddonUpdate.Text = "&Update";
            this.tsmiAddonUpdate.Click += new System.EventHandler(this.tsmiAddonUpdate_Click);
            // 
            // tsmiAddonIgnore
            // 
            this.tsmiAddonIgnore.Name = "tsmiAddonIgnore";
            this.tsmiAddonIgnore.Size = new System.Drawing.Size(166, 22);
            this.tsmiAddonIgnore.Text = "&Ignore";
            this.tsmiAddonIgnore.Click += new System.EventHandler(this.tsmiAddonIgnore_Click);
            // 
            // tsmiAddonUnignore
            // 
            this.tsmiAddonUnignore.Name = "tsmiAddonUnignore";
            this.tsmiAddonUnignore.Size = new System.Drawing.Size(166, 22);
            this.tsmiAddonUnignore.Text = "U&nignore";
            this.tsmiAddonUnignore.Click += new System.EventHandler(this.tsmiAddonUnignore_Click);
            // 
            // tsmiAddonMappings
            // 
            this.tsmiAddonMappings.Name = "tsmiAddonMappings";
            this.tsmiAddonMappings.Size = new System.Drawing.Size(166, 22);
            this.tsmiAddonMappings.Text = "&Mappings";
            // 
            // tsmiAddonSetAsUpdated
            // 
            this.tsmiAddonSetAsUpdated.Name = "tsmiAddonSetAsUpdated";
            this.tsmiAddonSetAsUpdated.Size = new System.Drawing.Size(166, 22);
            this.tsmiAddonSetAsUpdated.Text = "&Set as Updated";
            this.tsmiAddonSetAsUpdated.Click += new System.EventHandler(this.tsmiAddonSetAsUpdated_Click);
            // 
            // ctxLog
            // 
            this.ctxLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearLog});
            this.ctxLog.Name = "ctxLog";
            this.ctxLog.Size = new System.Drawing.Size(125, 26);
            // 
            // tsmiClearLog
            // 
            this.tsmiClearLog.Name = "tsmiClearLog";
            this.tsmiClearLog.Size = new System.Drawing.Size(124, 22);
            this.tsmiClearLog.Text = "&Clear Log";
            this.tsmiClearLog.Click += new System.EventHandler(this.tsmiClearLog_Click);
            // 
            // ctxThread
            // 
            this.ctxThread.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbortThread});
            this.ctxThread.Name = "ctxThread";
            this.ctxThread.Size = new System.Drawing.Size(105, 26);
            // 
            // tsmiAbortThread
            // 
            this.tsmiAbortThread.Name = "tsmiAbortThread";
            this.tsmiAbortThread.Size = new System.Drawing.Size(104, 22);
            this.tsmiAbortThread.Text = "&Abort";
            this.tsmiAbortThread.Click += new System.EventHandler(this.tsmiAbortThread_Click);
            // 
            // ctxMapping
            // 
            this.ctxMapping.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMappingChangeLog,
            this.tsmiMappingCheckVersion,
            this.tsmiMappingUpdate,
            this.tsmiMappingSetAsPreferred,
            this.tsmiMappingInfo,
            this.tsmiMappingDownload});
            this.ctxMapping.Name = "ctxMapping";
            this.ctxMapping.Size = new System.Drawing.Size(156, 136);
            // 
            // tsmiMappingChangeLog
            // 
            this.tsmiMappingChangeLog.Name = "tsmiMappingChangeLog";
            this.tsmiMappingChangeLog.Size = new System.Drawing.Size(155, 22);
            this.tsmiMappingChangeLog.Text = "&Change Log";
            this.tsmiMappingChangeLog.Click += new System.EventHandler(this.tsmiMappingChangeLog_Click);
            // 
            // tsmiMappingCheckVersion
            // 
            this.tsmiMappingCheckVersion.Name = "tsmiMappingCheckVersion";
            this.tsmiMappingCheckVersion.Size = new System.Drawing.Size(155, 22);
            this.tsmiMappingCheckVersion.Text = "Check Version";
            this.tsmiMappingCheckVersion.Click += new System.EventHandler(this.tsmiMappingCheckVersion_Click);
            // 
            // tsmiMappingUpdate
            // 
            this.tsmiMappingUpdate.Name = "tsmiMappingUpdate";
            this.tsmiMappingUpdate.Size = new System.Drawing.Size(155, 22);
            this.tsmiMappingUpdate.Text = "Update";
            this.tsmiMappingUpdate.Click += new System.EventHandler(this.tsmiMappingUpdate_Click);
            // 
            // tsmiMappingSetAsPreferred
            // 
            this.tsmiMappingSetAsPreferred.Name = "tsmiMappingSetAsPreferred";
            this.tsmiMappingSetAsPreferred.Size = new System.Drawing.Size(155, 22);
            this.tsmiMappingSetAsPreferred.Text = "Set as Preferred";
            this.tsmiMappingSetAsPreferred.Click += new System.EventHandler(this.tsmiMappingSetAsPreferred_Click);
            // 
            // tsmiMappingInfo
            // 
            this.tsmiMappingInfo.Name = "tsmiMappingInfo";
            this.tsmiMappingInfo.Size = new System.Drawing.Size(155, 22);
            this.tsmiMappingInfo.Text = "Info";
            this.tsmiMappingInfo.Click += new System.EventHandler(this.tsmiMappingInfo_Click);
            // 
            // tsmiMappingDownload
            // 
            this.tsmiMappingDownload.Name = "tsmiMappingDownload";
            this.tsmiMappingDownload.Size = new System.Drawing.Size(155, 22);
            this.tsmiMappingDownload.Text = "Download";
            this.tsmiMappingDownload.Click += new System.EventHandler(this.tsmiMappingDownload_Click);
            // 
            // tsmiCheckAndUpdatdAddons
            // 
            this.tsmiCheckAndUpdatdAddons.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiCheckAndUpdatdAddons.Name = "tsmiCheckAndUpdatdAddons";
            this.tsmiCheckAndUpdatdAddons.Size = new System.Drawing.Size(219, 22);
            this.tsmiCheckAndUpdatdAddons.Text = "Check and Update Addons";
            this.tsmiCheckAndUpdatdAddons.Click += new System.EventHandler(this.tsmiCheckAndUpdatdAddons_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(216, 6);
            // 
            // splitContainerEx2
            // 
            this.splitContainerEx2.AlternativeCollapseDefault = false;
            this.splitContainerEx2.AlternativeCollapsePanel = Waddu.UI.Controls.SplitContainerEx.Panels.Panel2;
            this.splitContainerEx2.BottomRightLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            this.splitContainerEx2.CenterLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            this.splitContainerEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerEx2.DragLineOffset = 0;
            this.splitContainerEx2.DragLines = Waddu.UI.Controls.SplitContainerEx.LineMode.Normal;
            this.splitContainerEx2.DragLineWidth = 40;
            this.splitContainerEx2.Location = new System.Drawing.Point(0, 24);
            this.splitContainerEx2.Name = "splitContainerEx2";
            this.splitContainerEx2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerEx2.Panel1
            // 
            this.splitContainerEx2.Panel1.Controls.Add(this.splitContainerEx1);
            this.splitContainerEx2.Panel1MaxSize = 0;
            // 
            // splitContainerEx2.Panel2
            // 
            this.splitContainerEx2.Panel2.Controls.Add(this.splitContainerEx3);
            this.splitContainerEx2.Panel2MaxSize = 0;
            this.splitContainerEx2.Size = new System.Drawing.Size(823, 557);
            this.splitContainerEx2.SplitterDistance = 350;
            this.splitContainerEx2.SplitterWidth = 20;
            this.splitContainerEx2.TabIndex = 13;
            this.splitContainerEx2.TopLeftLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            // 
            // splitContainerEx1
            // 
            this.splitContainerEx1.AlternativeCollapseDefault = false;
            this.splitContainerEx1.AlternativeCollapsePanel = Waddu.UI.Controls.SplitContainerEx.Panels.Panel2;
            this.splitContainerEx1.BottomRightLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            this.splitContainerEx1.CenterLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            this.splitContainerEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerEx1.DragLineOffset = 0;
            this.splitContainerEx1.DragLines = Waddu.UI.Controls.SplitContainerEx.LineMode.Normal;
            this.splitContainerEx1.DragLineWidth = 40;
            this.splitContainerEx1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerEx1.Name = "splitContainerEx1";
            // 
            // splitContainerEx1.Panel1
            // 
            this.splitContainerEx1.Panel1.Controls.Add(this.gbAddons);
            this.splitContainerEx1.Panel1MaxSize = 0;
            // 
            // splitContainerEx1.Panel2
            // 
            this.splitContainerEx1.Panel2.Controls.Add(this.gbAddonDetails);
            this.splitContainerEx1.Panel2MaxSize = 0;
            this.splitContainerEx1.Size = new System.Drawing.Size(823, 350);
            this.splitContainerEx1.SplitterDistance = 461;
            this.splitContainerEx1.SplitterWidth = 20;
            this.splitContainerEx1.TabIndex = 0;
            this.splitContainerEx1.TopLeftLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            // 
            // gbAddons
            // 
            this.gbAddons.Controls.Add(this.dgvAddons);
            this.gbAddons.Controls.Add(this.tsAddon);
            this.gbAddons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAddons.Location = new System.Drawing.Point(0, 0);
            this.gbAddons.Name = "gbAddons";
            this.gbAddons.Size = new System.Drawing.Size(461, 350);
            this.gbAddons.TabIndex = 10;
            this.gbAddons.TabStop = false;
            this.gbAddons.Text = "Addons";
            // 
            // dgvAddons
            // 
            this.dgvAddons.AllowUserToAddRows = false;
            this.dgvAddons.AllowUserToDeleteRows = false;
            this.dgvAddons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAddons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColAddonName,
            this.dgvColAddonLastUpdated,
            this.dgvColAddonLocalVersion,
            this.dgvColAddonPreferredMapping});
            this.dgvAddons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAddons.Location = new System.Drawing.Point(3, 41);
            this.dgvAddons.MultiSelect = false;
            this.dgvAddons.Name = "dgvAddons";
            this.dgvAddons.ReadOnly = true;
            this.dgvAddons.RowHeadersVisible = false;
            this.dgvAddons.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAddons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAddons.Size = new System.Drawing.Size(455, 306);
            this.dgvAddons.TabIndex = 4;
            this.dgvAddons.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvAddons_MouseClick);
            this.dgvAddons.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvAddons_CellPainting);
            this.dgvAddons.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgvAddons_RowStateChanged);
            // 
            // dgvColAddonName
            // 
            this.dgvColAddonName.HeaderText = "Name";
            this.dgvColAddonName.Name = "dgvColAddonName";
            this.dgvColAddonName.ReadOnly = true;
            // 
            // dgvColAddonLastUpdated
            // 
            this.dgvColAddonLastUpdated.HeaderText = "Last Updated";
            this.dgvColAddonLastUpdated.Name = "dgvColAddonLastUpdated";
            this.dgvColAddonLastUpdated.ReadOnly = true;
            // 
            // dgvColAddonLocalVersion
            // 
            this.dgvColAddonLocalVersion.HeaderText = "Local Version";
            this.dgvColAddonLocalVersion.Name = "dgvColAddonLocalVersion";
            this.dgvColAddonLocalVersion.ReadOnly = true;
            // 
            // dgvColAddonPreferredMapping
            // 
            this.dgvColAddonPreferredMapping.HeaderText = "Preferred Mapping";
            this.dgvColAddonPreferredMapping.Name = "dgvColAddonPreferredMapping";
            this.dgvColAddonPreferredMapping.ReadOnly = true;
            this.dgvColAddonPreferredMapping.Width = 120;
            // 
            // tsAddon
            // 
            this.tsAddon.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsAddon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddFilter,
            this.txtAddonFilter});
            this.tsAddon.Location = new System.Drawing.Point(3, 16);
            this.tsAddon.Name = "tsAddon";
            this.tsAddon.Size = new System.Drawing.Size(455, 25);
            this.tsAddon.TabIndex = 5;
            this.tsAddon.Text = "toolStrip1";
            // 
            // tsddFilter
            // 
            this.tsddFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFilterInstalled,
            this.tsmiFilterNotInstalled,
            this.tsmiFilterSubAddons,
            this.tsmiFilterBlizzard});
            this.tsddFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsddFilter.Image")));
            this.tsddFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddFilter.Name = "tsddFilter";
            this.tsddFilter.Size = new System.Drawing.Size(46, 22);
            this.tsddFilter.Text = "Filter";
            // 
            // tsmiFilterInstalled
            // 
            this.tsmiFilterInstalled.Checked = true;
            this.tsmiFilterInstalled.CheckOnClick = true;
            this.tsmiFilterInstalled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiFilterInstalled.Name = "tsmiFilterInstalled";
            this.tsmiFilterInstalled.Size = new System.Drawing.Size(141, 22);
            this.tsmiFilterInstalled.Text = "Installed";
            this.tsmiFilterInstalled.CheckedChanged += new System.EventHandler(this.tsmiFilter_CheckedChanged);
            // 
            // tsmiFilterNotInstalled
            // 
            this.tsmiFilterNotInstalled.CheckOnClick = true;
            this.tsmiFilterNotInstalled.Name = "tsmiFilterNotInstalled";
            this.tsmiFilterNotInstalled.Size = new System.Drawing.Size(141, 22);
            this.tsmiFilterNotInstalled.Text = "Not Installed";
            this.tsmiFilterNotInstalled.CheckedChanged += new System.EventHandler(this.tsmiFilter_CheckedChanged);
            // 
            // tsmiFilterSubAddons
            // 
            this.tsmiFilterSubAddons.CheckOnClick = true;
            this.tsmiFilterSubAddons.Name = "tsmiFilterSubAddons";
            this.tsmiFilterSubAddons.Size = new System.Drawing.Size(141, 22);
            this.tsmiFilterSubAddons.Text = "SubAddons";
            this.tsmiFilterSubAddons.CheckedChanged += new System.EventHandler(this.tsmiFilter_CheckedChanged);
            // 
            // tsmiFilterBlizzard
            // 
            this.tsmiFilterBlizzard.CheckOnClick = true;
            this.tsmiFilterBlizzard.Name = "tsmiFilterBlizzard";
            this.tsmiFilterBlizzard.Size = new System.Drawing.Size(141, 22);
            this.tsmiFilterBlizzard.Text = "Blizzard";
            this.tsmiFilterBlizzard.CheckedChanged += new System.EventHandler(this.tsmiFilter_CheckedChanged);
            // 
            // txtAddonFilter
            // 
            this.txtAddonFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddonFilter.Name = "txtAddonFilter";
            this.txtAddonFilter.Size = new System.Drawing.Size(200, 25);
            this.txtAddonFilter.TextChanged += new System.EventHandler(this.txtAddonFilter_TextChanged);
            // 
            // gbAddonDetails
            // 
            this.gbAddonDetails.Controls.Add(this.gbRelations);
            this.gbAddonDetails.Controls.Add(this.txtLocalVersion);
            this.gbAddonDetails.Controls.Add(this.lblLocalVersion);
            this.gbAddonDetails.Controls.Add(this.gbMappings);
            this.gbAddonDetails.Controls.Add(this.lblName);
            this.gbAddonDetails.Controls.Add(this.txtName);
            this.gbAddonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAddonDetails.Location = new System.Drawing.Point(0, 0);
            this.gbAddonDetails.Name = "gbAddonDetails";
            this.gbAddonDetails.Size = new System.Drawing.Size(342, 350);
            this.gbAddonDetails.TabIndex = 11;
            this.gbAddonDetails.TabStop = false;
            this.gbAddonDetails.Text = "Addon Details";
            // 
            // gbRelations
            // 
            this.gbRelations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRelations.Controls.Add(this.lvRelations);
            this.gbRelations.Location = new System.Drawing.Point(6, 186);
            this.gbRelations.Name = "gbRelations";
            this.gbRelations.Size = new System.Drawing.Size(327, 158);
            this.gbRelations.TabIndex = 13;
            this.gbRelations.TabStop = false;
            this.gbRelations.Text = "Relations";
            // 
            // lvRelations
            // 
            this.lvRelations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chRelationsName});
            this.lvRelations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRelations.FullRowSelect = true;
            listViewGroup7.Header = "Dependencies";
            listViewGroup7.Name = "lvgRelationDependencies";
            listViewGroup8.Header = "Contains";
            listViewGroup8.Name = "lvgRelationSubAddons";
            listViewGroup9.Header = "Part Of";
            listViewGroup9.Name = "lvgRelationSuperAddons";
            this.lvRelations.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup7,
            listViewGroup8,
            listViewGroup9});
            this.lvRelations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvRelations.Location = new System.Drawing.Point(3, 16);
            this.lvRelations.Name = "lvRelations";
            this.lvRelations.Size = new System.Drawing.Size(321, 139);
            this.lvRelations.TabIndex = 0;
            this.lvRelations.UseCompatibleStateImageBehavior = false;
            this.lvRelations.View = System.Windows.Forms.View.Details;
            // 
            // chRelationsName
            // 
            this.chRelationsName.Text = "Name";
            this.chRelationsName.Width = 100;
            // 
            // txtLocalVersion
            // 
            this.txtLocalVersion.Location = new System.Drawing.Point(216, 13);
            this.txtLocalVersion.Name = "txtLocalVersion";
            this.txtLocalVersion.ReadOnly = true;
            this.txtLocalVersion.Size = new System.Drawing.Size(98, 20);
            this.txtLocalVersion.TabIndex = 10;
            // 
            // lblLocalVersion
            // 
            this.lblLocalVersion.AutoSize = true;
            this.lblLocalVersion.Location = new System.Drawing.Point(139, 16);
            this.lblLocalVersion.Name = "lblLocalVersion";
            this.lblLocalVersion.Size = new System.Drawing.Size(71, 13);
            this.lblLocalVersion.TabIndex = 9;
            this.lblLocalVersion.Text = "Local Version";
            // 
            // gbMappings
            // 
            this.gbMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMappings.Controls.Add(this.dgvMappings);
            this.gbMappings.Location = new System.Drawing.Point(6, 39);
            this.gbMappings.Name = "gbMappings";
            this.gbMappings.Size = new System.Drawing.Size(327, 141);
            this.gbMappings.TabIndex = 8;
            this.gbMappings.TabStop = false;
            this.gbMappings.Text = "Mappings";
            // 
            // dgvMappings
            // 
            this.dgvMappings.AllowUserToAddRows = false;
            this.dgvMappings.AllowUserToDeleteRows = false;
            this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColMappingSite,
            this.dgvColMappingVersion,
            this.dgvColMappingLastUpdated});
            this.dgvMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMappings.Location = new System.Drawing.Point(3, 16);
            this.dgvMappings.Name = "dgvMappings";
            this.dgvMappings.ReadOnly = true;
            this.dgvMappings.RowHeadersVisible = false;
            this.dgvMappings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMappings.Size = new System.Drawing.Size(321, 122);
            this.dgvMappings.TabIndex = 6;
            this.dgvMappings.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvMappings_MouseClick);
            // 
            // dgvColMappingSite
            // 
            this.dgvColMappingSite.HeaderText = "Site";
            this.dgvColMappingSite.Name = "dgvColMappingSite";
            this.dgvColMappingSite.ReadOnly = true;
            // 
            // dgvColMappingVersion
            // 
            this.dgvColMappingVersion.HeaderText = "Version";
            this.dgvColMappingVersion.Name = "dgvColMappingVersion";
            this.dgvColMappingVersion.ReadOnly = true;
            // 
            // dgvColMappingLastUpdated
            // 
            this.dgvColMappingLastUpdated.HeaderText = "Last Updated";
            this.dgvColMappingLastUpdated.Name = "dgvColMappingLastUpdated";
            this.dgvColMappingLastUpdated.ReadOnly = true;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(6, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(47, 13);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(86, 20);
            this.txtName.TabIndex = 0;
            // 
            // splitContainerEx3
            // 
            this.splitContainerEx3.AlternativeCollapseDefault = false;
            this.splitContainerEx3.AlternativeCollapsePanel = Waddu.UI.Controls.SplitContainerEx.Panels.Panel2;
            this.splitContainerEx3.BottomRightLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            this.splitContainerEx3.CenterLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            this.splitContainerEx3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerEx3.DragLineOffset = 0;
            this.splitContainerEx3.DragLines = Waddu.UI.Controls.SplitContainerEx.LineMode.Normal;
            this.splitContainerEx3.DragLineWidth = 40;
            this.splitContainerEx3.Location = new System.Drawing.Point(0, 0);
            this.splitContainerEx3.Name = "splitContainerEx3";
            // 
            // splitContainerEx3.Panel1
            // 
            this.splitContainerEx3.Panel1.Controls.Add(this.dgvThreadActivity);
            this.splitContainerEx3.Panel1MaxSize = 0;
            // 
            // splitContainerEx3.Panel2
            // 
            this.splitContainerEx3.Panel2.Controls.Add(this.dgvLog);
            this.splitContainerEx3.Panel2.Controls.Add(this.tsLog);
            this.splitContainerEx3.Panel2MaxSize = 0;
            this.splitContainerEx3.Size = new System.Drawing.Size(823, 187);
            this.splitContainerEx3.SplitterDistance = 461;
            this.splitContainerEx3.SplitterWidth = 20;
            this.splitContainerEx3.TabIndex = 0;
            this.splitContainerEx3.TopLeftLine = Waddu.UI.Controls.SplitContainerEx.LineMode.Hidden;
            // 
            // dgvThreadActivity
            // 
            this.dgvThreadActivity.AllowUserToAddRows = false;
            this.dgvThreadActivity.AllowUserToDeleteRows = false;
            this.dgvThreadActivity.AllowUserToResizeRows = false;
            this.dgvThreadActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThreadActivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColThreadID,
            this.dgvColThreadState,
            this.dgvColThreadInfo});
            this.dgvThreadActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThreadActivity.Location = new System.Drawing.Point(0, 0);
            this.dgvThreadActivity.MultiSelect = false;
            this.dgvThreadActivity.Name = "dgvThreadActivity";
            this.dgvThreadActivity.ReadOnly = true;
            this.dgvThreadActivity.RowHeadersVisible = false;
            this.dgvThreadActivity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThreadActivity.Size = new System.Drawing.Size(461, 187);
            this.dgvThreadActivity.TabIndex = 0;
            this.dgvThreadActivity.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvThreadActivity_MouseClick);
            // 
            // dgvColThreadID
            // 
            this.dgvColThreadID.HeaderText = "ID";
            this.dgvColThreadID.Name = "dgvColThreadID";
            this.dgvColThreadID.ReadOnly = true;
            this.dgvColThreadID.Width = 50;
            // 
            // dgvColThreadState
            // 
            this.dgvColThreadState.HeaderText = "State";
            this.dgvColThreadState.Name = "dgvColThreadState";
            this.dgvColThreadState.ReadOnly = true;
            this.dgvColThreadState.Width = 80;
            // 
            // dgvColThreadInfo
            // 
            this.dgvColThreadInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvColThreadInfo.HeaderText = "Info";
            this.dgvColThreadInfo.Name = "dgvColThreadInfo";
            this.dgvColThreadInfo.ReadOnly = true;
            this.dgvColThreadInfo.Width = 50;
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColLogTime,
            this.dgvColLogType,
            this.dgvColLogMessage});
            this.dgvLog.ContextMenuStrip = this.ctxLog;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLog.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLog.Location = new System.Drawing.Point(0, 25);
            this.dgvLog.MultiSelect = false;
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.RowTemplate.Height = 20;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.Size = new System.Drawing.Size(342, 162);
            this.dgvLog.TabIndex = 0;
            // 
            // dgvColLogTime
            // 
            this.dgvColLogTime.HeaderText = "Time";
            this.dgvColLogTime.Name = "dgvColLogTime";
            this.dgvColLogTime.ReadOnly = true;
            // 
            // dgvColLogType
            // 
            this.dgvColLogType.HeaderText = "Type";
            this.dgvColLogType.Name = "dgvColLogType";
            this.dgvColLogType.ReadOnly = true;
            this.dgvColLogType.Width = 80;
            // 
            // dgvColLogMessage
            // 
            this.dgvColLogMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvColLogMessage.HeaderText = "Message";
            this.dgvColLogMessage.Name = "dgvColLogMessage";
            this.dgvColLogMessage.ReadOnly = true;
            this.dgvColLogMessage.Width = 75;
            // 
            // tsLog
            // 
            this.tsLog.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddLogLevel});
            this.tsLog.Location = new System.Drawing.Point(0, 0);
            this.tsLog.Name = "tsLog";
            this.tsLog.Size = new System.Drawing.Size(342, 25);
            this.tsLog.TabIndex = 1;
            this.tsLog.Text = "toolStrip1";
            // 
            // tsddLogLevel
            // 
            this.tsddLogLevel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddLogLevel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLogDebug,
            this.tsmiLogInformation,
            this.tsmiLogWarning,
            this.tsmiLogError});
            this.tsddLogLevel.Image = ((System.Drawing.Image)(resources.GetObject("tsddLogLevel.Image")));
            this.tsddLogLevel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddLogLevel.Name = "tsddLogLevel";
            this.tsddLogLevel.Size = new System.Drawing.Size(70, 22);
            this.tsddLogLevel.Text = "Log Level";
            // 
            // tsmiLogDebug
            // 
            this.tsmiLogDebug.CheckOnClick = true;
            this.tsmiLogDebug.Name = "tsmiLogDebug";
            this.tsmiLogDebug.Size = new System.Drawing.Size(137, 22);
            this.tsmiLogDebug.Text = "Debug";
            this.tsmiLogDebug.Click += new System.EventHandler(this.tsmiLogLevelItem_Click);
            // 
            // tsmiLogInformation
            // 
            this.tsmiLogInformation.CheckOnClick = true;
            this.tsmiLogInformation.Name = "tsmiLogInformation";
            this.tsmiLogInformation.Size = new System.Drawing.Size(137, 22);
            this.tsmiLogInformation.Text = "Information";
            this.tsmiLogInformation.Click += new System.EventHandler(this.tsmiLogLevelItem_Click);
            // 
            // tsmiLogWarning
            // 
            this.tsmiLogWarning.CheckOnClick = true;
            this.tsmiLogWarning.Name = "tsmiLogWarning";
            this.tsmiLogWarning.Size = new System.Drawing.Size(137, 22);
            this.tsmiLogWarning.Text = "Warning";
            this.tsmiLogWarning.Click += new System.EventHandler(this.tsmiLogLevelItem_Click);
            // 
            // tsmiLogError
            // 
            this.tsmiLogError.CheckOnClick = true;
            this.tsmiLogError.Name = "tsmiLogError";
            this.tsmiLogError.Size = new System.Drawing.Size(137, 22);
            this.tsmiLogError.Text = "Error";
            this.tsmiLogError.Click += new System.EventHandler(this.tsmiLogLevelItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 581);
            this.Controls.Add(this.splitContainerEx2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Waddu - WoW Addon Updater";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ctxAddon.ResumeLayout(false);
            this.ctxLog.ResumeLayout(false);
            this.ctxThread.ResumeLayout(false);
            this.ctxMapping.ResumeLayout(false);
            this.splitContainerEx2.Panel1.ResumeLayout(false);
            this.splitContainerEx2.Panel2.ResumeLayout(false);
            this.splitContainerEx2.ResumeLayout(false);
            this.splitContainerEx1.Panel1.ResumeLayout(false);
            this.splitContainerEx1.Panel2.ResumeLayout(false);
            this.splitContainerEx1.ResumeLayout(false);
            this.gbAddons.ResumeLayout(false);
            this.gbAddons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddons)).EndInit();
            this.tsAddon.ResumeLayout(false);
            this.tsAddon.PerformLayout();
            this.gbAddonDetails.ResumeLayout(false);
            this.gbAddonDetails.PerformLayout();
            this.gbRelations.ResumeLayout(false);
            this.gbMappings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).EndInit();
            this.splitContainerEx3.Panel1.ResumeLayout(false);
            this.splitContainerEx3.Panel2.ResumeLayout(false);
            this.splitContainerEx3.Panel2.PerformLayout();
            this.splitContainerEx3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThreadActivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.tsLog.ResumeLayout(false);
            this.tsLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem tsmiFile;
        private ToolStripMenuItem tsmiExit;
        private ToolStripMenuItem tsmiSettings;
        private ToolStripSeparator toolStripMenuItem1;
        private DataGridView dgvAddons;
        private TextBox txtName;
        private ToolStripMenuItem tsmiAddons;
        private ToolStripMenuItem tsmiCheckForUpdates;
        private ToolStripMenuItem tsmWadduAllAddons;
        private DataGridView dgvThreadActivity;
        private Label lblName;
        private DataGridView dgvLog;
        private ContextMenuStrip ctxAddon;
        private ToolStripMenuItem tsmiAddonUpdate;
        private ToolStripMenuItem tsmiAddonCheckForUpdate;
        private ContextMenuStrip ctxLog;
        private ToolStripMenuItem tsmiClearLog;
        private ContextMenuStrip ctxThread;
        private ToolStripMenuItem tsmiAbortThread;
        private ToolStrip tsAddon;
        private ToolStripMenuItem tsmiReloadLocalAddons;
        private ToolStripDropDownButton tsddFilter;
        private ToolStripMenuItem tsmiFilterNotInstalled;
        private ToolStripMenuItem tsmiFilterInstalled;
        private ToolStripMenuItem tsmiFilterSubAddons;
        private ToolStripMenuItem tsmiFilterBlizzard;
        private GroupBox gbAddons;
        private GroupBox gbAddonDetails;
        private SplitContainerEx splitContainerEx2;
        private SplitContainerEx splitContainerEx1;
        private SplitContainerEx splitContainerEx3;
        private ToolStrip tsLog;
        private ToolStripDropDownButton tsddLogLevel;
        private ToolStripMenuItem tsmiLogDebug;
        private ToolStripMenuItem tsmiLogInformation;
        private ToolStripMenuItem tsmiLogWarning;
        private ToolStripMenuItem tsmiLogError;
        private DataGridView dgvMappings;
        private GroupBox gbMappings;
        private TextBox txtLocalVersion;
        private Label lblLocalVersion;
        private DataGridViewTextBoxColumn dgvColMappingSite;
        private DataGridViewTextBoxColumn dgvColMappingVersion;
        private DataGridViewTextBoxColumn dgvColMappingLastUpdated;
        private DataGridViewTextBoxColumn dgvColThreadID;
        private DataGridViewTextBoxColumn dgvColThreadState;
        private DataGridViewTextBoxColumn dgvColThreadInfo;
        private DataGridViewTextBoxColumn dgvColLogTime;
        private DataGridViewTextBoxColumn dgvColLogType;
        private DataGridViewTextBoxColumn dgvColLogMessage;
        private ToolStripTextBox txtAddonFilter;
        private ToolStripMenuItem tsmiHelp;
        private ToolStripMenuItem tsmiCollectUnknownAddons;
        private ContextMenuStrip ctxMapping;
        private ToolStripMenuItem tsmiMappingCheckVersion;
        private ToolStripMenuItem tsmiMappingUpdate;
        private ToolStripMenuItem tsmiMappingInfo;
        private ToolStripMenuItem tsmiMappingDownload;
        private ToolStripMenuItem tsmiAddonMappings;
        private ToolStripMenuItem tsmiMappingSetAsPreferred;
        private ToolStripMenuItem tsmiAddonIgnore;
        private ToolStripMenuItem tsmiAddonUnignore;
        private ToolStripMenuItem tsmiAdmin;
        private ToolStripMenuItem tsmiHelpAbout;
        private GroupBox gbRelations;
        private ListView lvRelations;
        private ColumnHeader chRelationsName;
        private ToolStripMenuItem tsmiMappingChangeLog;
        private DataGridViewTextBoxColumn dgvColAddonName;
        private DataGridViewTextBoxColumn dgvColAddonLastUpdated;
        private DataGridViewTextBoxColumn dgvColAddonLocalVersion;
        private DataGridViewTextBoxColumn dgvColAddonPreferredMapping;
        private ToolStripMenuItem tsmiAddonSetAsUpdated;
        private ToolStripMenuItem tsmiCheckAndUpdatdAddons;
        private ToolStripSeparator toolStripMenuItem2;

    }
}

