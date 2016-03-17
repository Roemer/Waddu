using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Waddu.Core;
using Waddu.Core.BusinessObjects;
using Waddu.Core.WorkItems;
using Waddu.Types;

namespace Waddu.UI.Forms
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;

        public MainForm()
        {
            Instance = this;

            InitializeComponent();

            // Set the Version
            Text += " v." + GetType().Assembly.GetName().Version;

            // Create / Update the Mapping File
            Mapper.CreateMapping(Path.Combine(Application.StartupPath, "waddu_mappings.xml"));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Setup Addon Display
            dgvAddons.AutoGenerateColumns = false;
            dgvColAddonName.DataPropertyName = "Name";
            dgvColAddonLastUpdated.DataPropertyName = "LastUpdated";
            dgvColAddonLocalVersion.DataPropertyName = "LocalVersion";
            dgvColAddonPreferredMapping.DataPropertyName = "PreferredMapping";
            dgvColAddonPreferredMapping.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Setup WorkerThread Status Display
            dgvThreadActivity.AutoGenerateColumns = false;
            dgvColThreadID.DataPropertyName = "ThreadID";
            dgvColThreadState.DataPropertyName = "ThreadStatus";
            dgvColThreadInfo.DataPropertyName = "InfoText";
            dgvColThreadInfo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Setup Log Display
            dgvLog.AutoGenerateColumns = false;
            dgvLog.ColumnHeadersVisible = false;
            dgvColLogTime.DataPropertyName = "Date";
            dgvColLogType.DataPropertyName = "Type";
            dgvColLogMessage.DataPropertyName = "Message";
            dgvColLogMessage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Setup Mappings Display
            dgvMappings.AutoGenerateColumns = false;
            dgvColMappingSite.DataPropertyName = "AddonSiteId";
            dgvColMappingVersion.DataPropertyName = "RemoteVersion";
            dgvColMappingVersion.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvColMappingLastUpdated.DataPropertyName = "LastUpdated";

            // Initialize Relation Display
            lvRelations.Layout += lvRelations_Layout;

            // Initialize Logger
            SetLogLevel(Config.Instance.LogLevel);
            Logger.Instance.LogEntry += Logger_LogEntry;

            // Initialize the Thread Manager
            ThreadManager.Initialize();

            // Init Local Status List
            UpdateStatusList.Load();

            // Associate DataSources
            dgvThreadActivity.DataSource = ThreadManager.Instance.WorkerThreadList;

            // Fire OnLoaded after everything is Done
            Application.Idle += OnLoaded;
        }

        private void lvRelations_Layout(object sender, LayoutEventArgs e)
        {
            var lv = sender as ListView;
            lv.Columns[0].Width = lv.ClientSize.Width - 5;
        }

        // This gets fired after the Form is shown
        private void OnLoaded(object sender, EventArgs args)
        {
            // Remove the OnLoaded Event
            Application.Idle -= OnLoaded;

            // Check for a WoW Folder
            if (!Directory.Exists(Config.Instance.WowFolderPath))
            {
                Config.Instance.WowFolderPath = GetWoWFolder();
                Config.Instance.SaveSettings();
            }

            // Check for 7z
            if (!ArchiveHelper.Exists7z())
            {
                MessageBox.Show(@"Please install and check the Path for the Files ""7z.exe and ""7zFM.exe"" in the Settings", "7-Zip not found");
            }

            // Load local Addons
            LoadLocalAddons();

            // Checks for a newer Version of Waddu
            ThreadManager.Instance.AddWork(new WorkItemAppVersionCheck());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Stop Logging
            Logger.Instance.LogEntry -= Logger_LogEntry;
            // Cleanup the Thread Manager (Close all Threads)
            ThreadManager.Instance.Dispose();
            // Base Closing
            base.OnClosing(e);
        }

        private void LoadLocalAddons()
        {
            var dispList = new BindingList<Addon>();
            foreach (var addon in AddonList.Instance.Addons)
            {
                // Filter out Blizzard Addons
                if (!tsmiFilterBlizzard.Checked && addon.Mappings.Count > 0 && addon.Mappings[0].AddonSiteId == AddonSiteId.blizzard)
                {
                    continue;
                }

                var addonExists = addon.IsInstalled;

                // Filter out Not Installed Addons
                if (!tsmiFilterNotInstalled.Checked && !addonExists)
                {
                    continue;
                }

                // Filter out Installed Addons
                if (!tsmiFilterInstalled.Checked && addonExists)
                {
                    continue;
                }

                // Filter out by Name
                if (!addon.Name.ToUpper().Contains(txtAddonFilter.Text.ToUpper()))
                {
                    continue;
                }

                // Filter out Sub Addons
                if (!tsmiFilterSubAddons.Checked && addon.IsSubAddon && !addon.IsMain)
                {
                    continue;
                }

                // Add the Addon
                dispList.Add(addon);
            }
            dgvAddons.DataSource = dispList;
        }

        private string GetWoWFolder()
        {
            // Try get the Folder from the Registry
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
            var localWoWPath = string.Empty;
            if (key != null)
            {
                localWoWPath = key.GetValue("InstallPath", "").ToString();
                key.Close();
            }
            // If not found, ask for it
            if (localWoWPath == string.Empty)
            {
                localWoWPath = Helpers.BrowseForFolder(@"c:\", FolderBrowseType.Enum.WoW);
            }
            return localWoWPath;
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            using (var dlg = new SettingsForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    AddonList.Instance = null;
                    LoadLocalAddons();
                }
            }
        }

        private void tsmiReloadLocalAddons_Click(object sender, EventArgs e)
        {
            LoadLocalAddons();
        }

        private void tsmiCheckForUpdates_Click(object sender, EventArgs e)
        {
            var addonList = dgvAddons.DataSource as BindingList<Addon>;
            foreach (var addon in addonList)
            {
                if (!addon.IsIgnored)
                {
                    ThreadManager.Instance.AddWork(new WorkItemAddonVersionCheck(addon));
                }
            }
        }

        private void tsmiUpdateAllAddons_Click(object sender, EventArgs e)
        {
            var addonList = dgvAddons.DataSource as BindingList<Addon>;
            foreach (var addon in addonList)
            {
                if (!addon.IsIgnored)
                {
                    ThreadManager.Instance.AddWork(new WorkItemAddonUpdate(addon));
                }
            }
        }

        private void tsmiCheckAndUpdatdAddons_Click(object sender, EventArgs e)
        {
            var addonList = dgvAddons.DataSource as BindingList<Addon>;
            // Add Version Check
            foreach (var addon in addonList)
            {
                if (!addon.IsIgnored)
                {
                    ThreadManager.Instance.AddWork(new WorkItemAddonVersionCheck(addon));
                }
            }
            // Add Update
            foreach (var addon in addonList)
            {
                if (!addon.IsIgnored)
                {
                    ThreadManager.Instance.AddWork(new WorkItemAddonUpdate(addon));
                }
            }
        }

        private void tsmiFilter_CheckedChanged(object sender, EventArgs e)
        {
            LoadLocalAddons();
        }


        private void txtAddonFilter_TextChanged(object sender, EventArgs e)
        {
            LoadLocalAddons();
        }

        #region Threads
        private void dgvThreadActivity_MouseClick(object sender, MouseEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (e.Button == MouseButtons.Right)
            {
                var h = dgv.HitTest(e.X, e.Y);
                if (h.RowIndex >= 0)
                {
                    // Select the Cell
                    dgv.CurrentCell = dgv.Rows[h.RowIndex].Cells[h.ColumnIndex];
                    // Get the Addon
                    var thread = dgv.CurrentRow.DataBoundItem as WorkerThread;

                    // Assign the Thread
                    tsmiAbortThread.Tag = thread;

                    // Show the Context Menu
                    ctxThread.Show(dgv, e.Location);
                }
            }
        }

        private void tsmiAbortThread_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            var thread = tsmi.Tag as WorkerThread;
            thread.Abort();
        }
        #endregion

        #region Log
        private void SetLogLevel(LogType logLevel)
        {
            tsmiLogDebug.Checked = logLevel == LogType.Debug;
            tsmiLogInformation.Checked = logLevel == LogType.Information;
            tsmiLogWarning.Checked = logLevel == LogType.Warning;
            tsmiLogError.Checked = logLevel == LogType.Error;
        }

        private LogType GetLogLevel()
        {
            if (tsmiLogDebug.Checked)
            {
                return LogType.Debug;
            }
            if (tsmiLogInformation.Checked)
            {
                return LogType.Information;
            }
            if (tsmiLogWarning.Checked)
            {
                return LogType.Warning;
            }
            if (tsmiLogError.Checked)
            {
                return LogType.Error;
            }
            return LogType.Information;
        }

        private void RefreshLog()
        {
            dgvLog.DataSource = Logger.Instance.GetEntries(Config.Instance.LogLevel);
            if (dgvLog.Rows.Count > 0)
            {
                dgvLog.FirstDisplayedScrollingRowIndex = dgvLog.Rows.Count - 1;
            }
        }

        private void Logger_LogEntry(LogEntry entry)
        {
            if (InvokeRequired)
            {
                Invoke(new LogEntryEventHandler(Logger_LogEntry), entry);
                return;
            }
            // Reload Log
            RefreshLog();
        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            Logger.Instance.Clear();
            RefreshLog();
        }

        private void tsmiLogLevelItem_Click(object sender, EventArgs e)
        {
            tsmiLogDebug.Checked = false;
            tsmiLogInformation.Checked = false;
            tsmiLogWarning.Checked = false;
            tsmiLogError.Checked = false;

            var tsmi = sender as ToolStripMenuItem;
            if (tsmi != null)
            {
                tsmi.Checked = true;
            }

            var logLevel = GetLogLevel();

            Config.Instance.LogLevel = logLevel;
            Config.Instance.SaveSettings();

            // Reload Log
            RefreshLog();
        }
        #endregion

        #region Help
        // Collects all Unknown Addons
        private void tsmiCollectUnknownAddons_Click(object sender, EventArgs e)
        {
            var unkAddons = string.Empty;
            foreach (var addon in AddonList.Instance.Addons)
            {
                if (addon.Mappings.Count == 0 && !addon.IsSubAddon)
                {
                    unkAddons += "- " + addon.Name + Environment.NewLine;
                }
            }
            if (unkAddons != string.Empty)
            {
                using (var dlg = new UnknownAddonsForm())
                {
                    dlg.UnknownAddonsText = unkAddons;
                    dlg.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("You have no unknown Addons!");
            }
        }

        // Show About Screen
        private void tsmiHelpAbout_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            Helpers.CenterFormTo(form, this);
            form.Show(this);
        }
        #endregion

        #region Addon
        private void dgvAddons_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv != null && e.RowIndex >= 0)
            {
                var addon = dgv.Rows[e.RowIndex].DataBoundItem as Addon;
                if (addon != null)
                {
                    if (addon.IsSubAddon && !addon.IsMain)
                    {
                        e.CellStyle.ForeColor = Color.LightGray;
                    }
                    else if (addon.IsIgnored)
                    {
                        e.CellStyle.BackColor = Color.Yellow;
                    }
                    else if (!addon.IsInstalled)
                    {
                        e.CellStyle.BackColor = Color.LightGray;
                    }
                    else if (addon.IsUnhandled)
                    {
                        e.CellStyle.BackColor = Color.Red;
                    }
                    else if (addon.Mappings.Count > 0)
                    {
                        if (addon.Mappings[0].AddonSiteId == AddonSiteId.blizzard)
                        {
                            e.CellStyle.ForeColor = Color.Gray;
                            e.CellStyle.BackColor = Color.LightGray;
                        }
                    }
                }
            }
        }

        private void dgvAddons_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    var addon = e.Row.DataBoundItem as Addon;

                    lvRelations.BeginUpdate();
                    lvRelations.Columns[0].Width = -2;
                    lvRelations.Items.Clear();
                    // Dependencies
                    foreach (var dep in TocHelper.GetDependencies(addon))
                    {
                        var i = new ListViewItem(dep, lvRelations.Groups[0]);
                        lvRelations.Items.Add(i);
                    }
                    // SubAddons
                    foreach (var subAddon in addon.SubAddons)
                    {
                        var i = new ListViewItem(subAddon.Name, lvRelations.Groups[1]);
                        lvRelations.Items.Add(i);
                    }
                    // SuperAddons
                    foreach (var superAddon in addon.SuperAddons)
                    {
                        var i = new ListViewItem(superAddon.Name, lvRelations.Groups[2]);
                        lvRelations.Items.Add(i);
                    }
                    lvRelations.EndUpdate();
                    // Other Info
                    txtName.Text = addon.Name;
                    txtLocalVersion.Text = addon.LocalVersion;
                    dgvMappings.DataSource = addon.Mappings;
                }
            }
        }

        private void dgvAddons_MouseClick(object sender, MouseEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (e.Button == MouseButtons.Right)
            {
                var h = dgv.HitTest(e.X, e.Y);
                if (h.RowIndex >= 0)
                {
                    // Select the Cell
                    dgv.CurrentCell = dgv.Rows[h.RowIndex].Cells[h.ColumnIndex];
                    // Get the Addon
                    var addon = dgv.CurrentRow.DataBoundItem as Addon;

                    // Assign the Addon
                    tsmiAddonCheckForUpdate.Tag = addon;
                    tsmiAddonUpdate.Tag = addon;
                    tsmiAddonSetAsUpdated.Tag = addon;

                    // Handle Ignore
                    if (addon.IsIgnored)
                    {
                        tsmiAddonUnignore.Tag = addon;
                        tsmiAddonUnignore.Visible = true;
                        tsmiAddonIgnore.Visible = false;
                    }
                    else
                    {
                        tsmiAddonIgnore.Tag = addon;
                        tsmiAddonIgnore.Visible = true;
                        tsmiAddonUnignore.Visible = false;
                    }

                    // Handle Mappings
                    tsmiAddonMappings.DropDownItems.Clear();
                    if (addon.Mappings.Count > 1)
                    {
                        tsmiAddonMappings.Visible = true;
                        foreach (var map in addon.Mappings)
                        {
                            var item = new ToolStripMenuItem();
                            item.Text = map.AddonSiteId.ToString();
                            item.Tag = map;
                            item.Click += tsmiAddonMappingsItem_Click;
                            tsmiAddonMappings.DropDownItems.Add(item);
                        }
                    }
                    else
                    {
                        tsmiAddonMappings.Visible = false;
                    }

                    // Show the Context Menu
                    ctxAddon.Show(dgv, e.Location);
                }
            }
        }

        private void tsmiAddonCheckForUpdate_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            var addon = tsmi.Tag as Addon;
            ThreadManager.Instance.AddWork(new WorkItemAddonVersionCheck(addon));
        }

        private void tsmiAddonUpdate_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            var addon = tsmi.Tag as Addon;
            ThreadManager.Instance.AddWork(new WorkItemAddonUpdate(addon));
        }

        /// <summary>
        /// Sets the selected Mapping as the Preferred Mapping for the Addon
        /// </summary>
        private void tsmiAddonMappingsItem_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            var map = tsmi.Tag as Mapping;
            map.Addon.PreferredMapping = map;
            Config.Instance.SetPreferredMapping(map);
            Config.Instance.SaveSettings();
        }

        private void tsmiAddonIgnore_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            var addon = tsmi.Tag as Addon;
            addon.IsIgnored = true;
        }

        private void tsmiAddonUnignore_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            var addon = tsmi.Tag as Addon;
            addon.IsIgnored = false;
        }

        private void tsmiAddonSetAsUpdated_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            var addon = tsmi.Tag as Addon;
            UpdateStatusList.Set(addon.Name, addon.LocalVersion);
            UpdateStatusList.Save();
            addon.LocalVersionUpdated();
        }
        #endregion

        #region Mappings
        /// <summary>
        /// Shows the Context Menu when clicked on a Mapping
        /// </summary>
        private void dgvMappings_MouseClick(object sender, MouseEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (e.Button == MouseButtons.Right)
            {
                var h = dgv.HitTest(e.X, e.Y);
                if (h.RowIndex >= 0)
                {
                    dgv.CurrentCell = dgv.Rows[h.RowIndex].Cells[h.ColumnIndex];
                    ctxMapping.Show(dgv, e.Location);
                }
            }
        }

        /// <summary>
        /// Shows the Change Log for a Specific Mapping
        /// </summary>
        private void tsmiMappingChangeLog_Click(object sender, EventArgs e)
        {
            if (dgvMappings.SelectedRows.Count > 0)
            {
                var map = dgvMappings.SelectedRows[0].DataBoundItem as Mapping;
                ThreadManager.Instance.AddWork(new WorkItemAddonChangeLog(map));
            }
        }

        /// <summary>
        /// Adds an Version Request for a specific Mapping to the WorkQueue
        /// </summary>
        private void tsmiMappingCheckVersion_Click(object sender, EventArgs e)
        {
            if (dgvMappings.SelectedRows.Count > 0)
            {
                var map = dgvMappings.SelectedRows[0].DataBoundItem as Mapping;
                ThreadManager.Instance.AddWork(new WorkItemAddonVersionCheck(map));
            }
        }

        /// <summary>
        /// Adds an Update for a specific Mapping to the WorkQueue
        /// </summary>
        private void tsmiMappingUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMappings.SelectedRows.Count > 0)
            {
                var map = dgvMappings.SelectedRows[0].DataBoundItem as Mapping;
                ThreadManager.Instance.AddWork(new WorkItemAddonUpdate(map));
            }
        }

        private void tsmiMappingSetAsPreferred_Click(object sender, EventArgs e)
        {
            if (dgvMappings.SelectedRows.Count > 0)
            {
                var map = dgvMappings.SelectedRows[0].DataBoundItem as Mapping;
                map.Addon.PreferredMapping = map;

                Config.Instance.SetPreferredMapping(map);
                Config.Instance.SaveSettings();
            }
        }

        private void tsmiMappingInfo_Click(object sender, EventArgs e)
        {
            var map = dgvMappings.CurrentRow.DataBoundItem as Mapping;
            Process.Start(map.GetInfoLink());
        }

        private void tsmiMappingDownload_Click(object sender, EventArgs e)
        {
            var map = dgvMappings.CurrentRow.DataBoundItem as Mapping;
            Process.Start(map.GetFilePath());
        }
        #endregion

        #region Admin
        private void tsmiAdmin_Click(object sender, EventArgs e)
        {
            using (var dlg = new AdminForm())
            {
                dlg.ShowDialog();
            }
        }
        #endregion
    }
}
