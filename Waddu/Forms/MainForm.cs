﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Waddu.AddonSites;
using Waddu.BusinessObjects;
using Waddu.Classes;
using Waddu.Types;
using System.Diagnostics;

namespace Waddu.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.Text += " v." + this.GetType().Assembly.GetName().Version;

            // Create / Update the Mapping File
            Mapper.CreateMapping(Path.Combine(Application.StartupPath, "waddu_mappings.xml"));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Setup Addon Display
            dgvAddons.AutoGenerateColumns = false;
            dgvColAddonName.DataPropertyName = "Name";
            dgvColAddonLocalVersion.DataPropertyName = "LocalVersion";
            dgvColAddonBestMapping.DataPropertyName = "BestMapping";
            dgvColAddonBestMapping.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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

            // Initialize Logger
            SetLogLevel(Config.Instance.LogLevel);
            Logger.Instance.LogEntry += new LogEntryEventHandler(Logger_LogEntry);

            // Initialize the Thread Manager
            ThreadManager.Initialize();

            // Associate DataSources
            dgvThreadActivity.DataSource = ThreadManager.Instance.WorkerThreadList;

            if (!Directory.Exists(Config.Instance.WowFolderPath))
            {
                Config.Instance.WowFolderPath = GetWoWFolder();
                Config.Instance.SaveSettings();
            }

            // Load local Addons
            LoadLocalAddons();
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
            BindingList<Addon> dispList = new BindingList<Addon>(); 
            foreach (Addon addon in AddonList.Instance.Addons)
            {
                // Filter out Blizzard Addons
                if (!tsmiFilterBlizzard.Checked && addon.Mappings.Count > 0 && addon.Mappings[0].AddonSiteId == AddonSiteId.blizzard)
                {
                    continue;
                }

                bool addonExists = addon.IsInstalled;

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

                // Filter out by name
                if (!addon.Name.ToUpper().Contains(txtAddonFilter.Text.ToUpper()))
                {
                    continue;
                }

                // Add the Addon
                dispList.Add(addon);

                // Add Sub Addons if wanted
                if (tsmiFilterSubAddons.Checked)
                {
                    foreach (Addon subAddon in addon.SubAddons)
                    {
                        dispList.Add(subAddon);
                    }
                }
            }
            dgvAddons.DataSource = dispList;
        }

        private string GetWoWFolder()
        {
            // Try get the Folder from the Registry
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
            string localWoWPath = string.Empty;
            if (key != null)
            {
                localWoWPath = key.GetValue("InstallPath", "").ToString();
                key.Close();
            }
            // If not found, ask for it
            if (localWoWPath == string.Empty)
            {
                localWoWPath = Helpers.BrowseForWoWFolder(@"c:\");
            }
            return localWoWPath;
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm dlg = new SettingsForm())
            {
                dlg.ShowDialog();
            }
        }

        private void tsmiAddonCheckForUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAddons.SelectedRows.Count > 0)
            {
                Addon addon = dgvAddons.SelectedRows[0].DataBoundItem as Addon;
                if (addon != null)
                {
                    ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.VersionCheck, addon));
                }
            }
        }

        private void tsmiAddonUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAddons.SelectedRows.Count > 0)
            {
                Addon addon = dgvAddons.SelectedRows[0].DataBoundItem as Addon;
                if (addon != null)
                {
                    ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.Update, addon));
                }
            }
        }

        private void tsmiReloadLocalAddons_Click(object sender, EventArgs e)
        {
            LoadLocalAddons();
        }

        private void tsmiCheckForUpdates_Click(object sender, EventArgs e)
        {
            BindingList<Addon> addonList = dgvAddons.DataSource as BindingList<Addon>;
            foreach (Addon addon in addonList)
            {
                ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.VersionCheck, addon));
            }
        }

        private void tsmiUpdateAllAddons_Click(object sender, EventArgs e)
        {
            BindingList<Addon> addonList = dgvAddons.DataSource as BindingList<Addon>;
            foreach (Addon addon in addonList)
            {
                ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.Update, addon));
            }
        }

        private void dgvAddons_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && e.RowIndex >= 0)
            {
                Addon addon = dgv.Rows[e.RowIndex].DataBoundItem as Addon;
                if (addon != null)
                {
                    if (addon.IsSubAddon)
                    {
                        e.CellStyle.ForeColor = Color.LightGray;
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
                        if (addon.Mappings.Count > 1)
                        {
                            if (e.ColumnIndex == dgvColAddonBestMapping.Index)
                            {
                                e.CellStyle.BackColor = Color.LightCoral;
                            }
                        }
                    }
                }
            }
        }

        private void tsmiAbortThread_Click(object sender, EventArgs e)
        {

        }

        private void dgvAddons_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    Addon addon = e.Row.DataBoundItem as Addon;

                    lbSubAddons.Items.Clear();
                    foreach (Addon subAddon in addon.SubAddons)
                    {
                        lbSubAddons.Items.Add(subAddon.Name);
                    }

                    txtName.Text = addon.Name;
                    txtLocalVersion.Text = addon.LocalVersion;
                    dgvMappings.DataSource = addon.Mappings;
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
            else if (tsmiLogInformation.Checked)
            {
                return LogType.Information;
            }
            else if (tsmiLogWarning.Checked)
            {
                return LogType.Warning;
            }
            else if (tsmiLogError.Checked)
            {
                return LogType.Error;
            }
            return LogType.Information;
        }

        private void RefreshLog()
        {
            dgvLog.DataSource = Logger.Instance.GetEntries(Config.Instance.LogLevel);
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

            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi != null)
            {
                tsmi.Checked = true;
            }

            LogType logLevel = GetLogLevel();

            Config.Instance.LogLevel = logLevel;
            Config.Instance.SaveSettings();

            // Reload Log
            RefreshLog();
        }
        #endregion

        #region Help
        private void tsmiCollectUnknownAddons_Click(object sender, EventArgs e)
        {
            string unkAddons = string.Empty;
            foreach (Addon addon in AddonList.Instance.Addons)
            {
                if (addon.Mappings.Count == 0)
                {
                    unkAddons += "- " + addon.Name + Environment.NewLine;
                }
            }
            if (unkAddons != string.Empty)
            {
                using (UnknownAddonsForm dlg = new UnknownAddonsForm())
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
        #endregion

        #region Addon
        private void dgvAddons_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo h = dgv.HitTest(e.X, e.Y);
                if (h.RowIndex >= 0)
                {
                    dgv.CurrentCell = dgv.Rows[h.RowIndex].Cells[h.ColumnIndex];
                    tsmiAddonMappings.DropDownItems.Clear();

                    Addon addon = dgv.CurrentRow.DataBoundItem as Addon;
                    if (addon.Mappings.Count > 1)
                    {
                        tsmiAddonMappings.Visible = true;
                        foreach (Mapping map in addon.Mappings)
                        {
                            ToolStripMenuItem item = new ToolStripMenuItem();
                            item.Text = map.AddonSiteId.ToString();
                            item.Tag = map;
                            item.Click += new EventHandler(tsmiAddonMappingsItem_Click);
                            tsmiAddonMappings.DropDownItems.Add(item);
                        }
                    }
                    else
                    {
                        tsmiAddonMappings.Visible = false;
                    }

                    ctxAddon.Show(dgv, e.Location);
                }
            }
        }

        /// <summary>
        /// Sets the selected Mapping as the Best Mapping for the Addon
        /// </summary>
        private void tsmiAddonMappingsItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            Mapping map = tsmi.Tag as Mapping;
            map.Addon.BestMapping = map;
        }
        #endregion

        #region Mappings
        /// <summary>
        /// Shows the Context Menu when clicked on a Mapping
        /// </summary>
        private void dgvMappings_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo h = dgv.HitTest(e.X, e.Y);
                if (h.RowIndex >= 0)
                {
                    dgv.CurrentCell = dgv.Rows[h.RowIndex].Cells[h.ColumnIndex];
                    ctxMapping.Show(dgv, e.Location);
                }
            }
        }

        /// <summary>
        /// Adds an Version Request for a specific Mapping to the WorkQueue
        /// </summary>
        private void tsmiMappingCheckVersion_Click(object sender, EventArgs e)
        {
            if (dgvMappings.SelectedRows.Count > 0)
            {
                Mapping map = dgvMappings.SelectedRows[0].DataBoundItem as Mapping;
                ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.VersionCheck, map.Addon, map));
            }
        }

        /// <summary>
        /// Adds an Update for a specific Mapping to the WorkQueue
        /// </summary>
        private void tsmiMappingUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMappings.SelectedRows.Count > 0)
            {
                Mapping map = dgvMappings.SelectedRows[0].DataBoundItem as Mapping;
                ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.Update, map.Addon, map));
            }
        }

        private void tsmiMappingSetAsBest_Click(object sender, EventArgs e)
        {
            if (dgvMappings.SelectedRows.Count > 0)
            {
                Mapping map = dgvMappings.SelectedRows[0].DataBoundItem as Mapping;
                map.Addon.BestMapping = map;
            }
        }

        private void tsmiMappingInfo_Click(object sender, EventArgs e)
        {
            Mapping map = dgvMappings.CurrentRow.DataBoundItem as Mapping;
            Process.Start(map.GetInfoLink());
        }

        private void tsmiMappingDownload_Click(object sender, EventArgs e)
        {
            Mapping map = dgvMappings.CurrentRow.DataBoundItem as Mapping;
            Process.Start(map.GetDownloadLink());
        }
        #endregion
    }
}