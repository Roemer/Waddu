using System;
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

            // Used to Create a new Mapping File
            Mapper.CreateMapping(Path.Combine(Application.StartupPath, "waddu_mappings.xml"));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Initialize the WorkerThread Status Display
            dgvThreadActivity.AutoGenerateColumns = false;
            dgvThreadActivity.Columns[0].DataPropertyName = "ThreadID";
            dgvThreadActivity.Columns[1].DataPropertyName = "ThreadStatus";
            dgvThreadActivity.Columns[2].DataPropertyName = "InfoText";
            dgvThreadActivity.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Setup Log Display
            dgvLog.AutoGenerateColumns = false;
            dgvLog.ColumnHeadersVisible = false;
            dgvThreadActivity.Columns[0].DataPropertyName = "Date";
            dgvThreadActivity.Columns[1].DataPropertyName = "Type";
            dgvThreadActivity.Columns[2].DataPropertyName = "Message";
            dgvLog.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Initialize Logger
            Logger.Instance.LogEntry += new LogEntryEventHandler(Logger_LogEntry);
            Logger.Instance.LogEntry += new LogEntryEventHandler(Logger_LogEntryFile);

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

        private void Logger_LogEntry(LogEntry entry)
        {
            if (InvokeRequired)
            {
                Invoke(new LogEntryEventHandler(Logger_LogEntry), entry);
                return;
            }
            // Reload Log
            dgvLog.DataSource = Logger.Instance.GetEntries(LogType.Debug);
        }

        private void Logger_LogEntryFile(LogEntry entry)
        {
            string logFileName = "log.txt";
            string logFilePath = Path.Combine(Application.StartupPath, logFileName);
            lock (Logger.Instance)
            {
                File.AppendAllText(logFilePath, entry.ToString() + Environment.NewLine);
            }
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

        private string GetRemoteVersion(Addon addon)
        {
            return addon.GetRemoteVersions();
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

        private void tsmiCheckSingleAddon_Click(object sender, EventArgs e)
        {
            if (dgvAddons.SelectedRows.Count > 0)
            {
                Addon addon = dgvAddons.SelectedRows[0].DataBoundItem as Addon;
                if (addon != null)
                {
                    ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.VersionCheck, addon, AddonSiteId.wowace));
                }
            }
        }

        private void tsmiUpdateSingleAddon_Click(object sender, EventArgs e)
        {
            if (dgvAddons.SelectedRows.Count > 0)
            {
                Addon addon = dgvAddons.SelectedRows[0].DataBoundItem as Addon;
                if (addon != null)
                {
                    ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.Update, addon, AddonSiteId.wowace));
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
                ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.VersionCheck, addon, AddonSiteId.blizzard));
            }
        }

        private void tsmiUpdateAllAddons_Click(object sender, EventArgs e)
        {
            BindingList<Addon> addonList = dgvAddons.DataSource as BindingList<Addon>;
            foreach (Addon addon in addonList)
            {
                foreach (Mapping mapping in addon.Mappings)
                {
                    AddonSiteBase site = AddonSiteBase.GetSite(mapping.AddonSiteId);
                    if (site != null)
                    {
                        ThreadManager.Instance.AddWork(new WorkItem(WorkItemType.Update, addon, mapping.AddonSiteId));
                        break;
                    }
                }
            }
        }

        private void dgvAddons_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Select Row with RightClick
                    dgvAddons.Rows[e.RowIndex].Selected = true;
                }
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
                    }
                }
            }
        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            dgvLog.Rows.Clear();
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

                    txtRemoteVersion.Text = addon.RemoteVersions;

                    ttMainForm.SetToolTip(linkInfo, addon.Name);
                    linkInfo.Tag = addon;
                    linkDownload.Tag = addon;
                }
            }
        }

        private void tsmiFilter_CheckedChanged(object sender, EventArgs e)
        {
            LoadLocalAddons();
        }

        private void linkInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Addon addon = (sender as LinkLabel).Tag as Addon;
            AddonSiteBase site = AddonSiteBase.GetSite(addon.Mappings[0].AddonSiteId);
            string infoUrl = site.GetInfoLink(addon.Mappings[0].AddonTag);
            Process.Start(infoUrl);
        }

        private void linkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Addon addon = (sender as LinkLabel).Tag as Addon;
            AddonSiteBase site = AddonSiteBase.GetSite(addon.Mappings[0].AddonSiteId);
            string downloadUrl = site.GetDownloadLink(addon.Mappings[0].AddonTag);
            Process.Start(downloadUrl);
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

            // Reload Log
            dgvLog.Rows.Clear();
            dgvLog.DataSource = Logger.Instance.GetEntries(LogType.Information);
        }
    }
}
