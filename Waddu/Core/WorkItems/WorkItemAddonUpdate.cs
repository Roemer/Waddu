using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Waddu.Core.BusinessObjects;
using Waddu.Types;
using Waddu.UI.Forms;

namespace Waddu.Core.WorkItems
{
    public class WorkItemAddonUpdate : WorkItemBase
    {
        private Mapping _mapping;
        private Addon _addon;

        private WorkItemAddonUpdate()
        {
            _addon = null;
            _mapping = null;
        }

        public WorkItemAddonUpdate(Addon addon)
            : this()
        {
            _addon = addon;
        }

        public WorkItemAddonUpdate(Mapping mapping)
            : this()
        {
            _addon = mapping.Addon;
            _mapping = mapping;
        }

        public override void DoWork(WorkerThread workerThread)
        {
            Addon addon = _addon;
            Mapping mapping = _mapping;

            if (addon.Mappings.Count <= 0)
            {
                // Addon has no Mappings, skip
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} has no Mapping", workerThread.ThreadID, addon.Name);
                return;
            }

            // Define the Mapping to use
            if (mapping == null)
            {
                // If no specific Mapping given, use Preferred Mapping
                mapping = addon.PreferredMapping;
            }

            // If the Mapping still is undefined (like no Update Check was made)
            if (mapping == null)
            {
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} has no Mapping to Update", workerThread.ThreadID, addon.Name);
                return;
            }

            // Download
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Updating {1} from {2}", workerThread.ThreadID, addon.Name, mapping.AddonSiteId);
            workerThread.InfoText = string.Format("DL from {0}: {1}", mapping.AddonSiteId, addon.Name);
            string downloadUrl = mapping.GetDownloadLink();
            if (downloadUrl == string.Empty)
            {
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Download Link for {1} incorrect", workerThread.ThreadID, addon.Name);
                return;
            }
            string archiveFilePath = WebHelper.DownloadFileToTemp(downloadUrl, workerThread);
            if (archiveFilePath == string.Empty)
            {
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Download for {1} failed", workerThread.ThreadID, addon.Name);
                return;
            }
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Downloaded to {1}", workerThread.ThreadID, archiveFilePath);

            bool has7z = ArchiveHelper.Exists7z();
            List<string> archiveFolderList = new List<string>();
            // Check if 7z Exists
            if (has7z)
            {
                // Get the Folder List (for Deletion)
                archiveFolderList = ArchiveHelper.GetRootFolders(archiveFilePath);

                // Simple Check if the Archive looks right
                if (!ArchiveHelper.CheckIntegrity(archiveFilePath, addon.Name))
                {
                    // If now, warn us
                    using (ArchiveContentForm f = new ArchiveContentForm(archiveFilePath))
                    {
                        if (f.ShowDialog() != DialogResult.OK)
                        {
                            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: cancelled by User Request", workerThread.ThreadID);
                            return;
                        }
                    }
                }
            }

            // Delete Old
            if (Config.Instance.DeleteBeforeUpdate)
            {
                if (addon.IsInstalled)
                {
                    if (!has7z)
                    {
                        MessageBox.Show("Please install 7z to get sure that the right Folders get Deleted");
                        // Straight-Forward Delete
                        DeleteType delType;
                        foreach (Addon subAddon in addon.SubAddons)
                        {
                            delType = subAddon.Delete();
                            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: SubAddon {1} {2}", workerThread.ThreadID, subAddon.Name, delType.ToString());
                        }
                        delType = addon.Delete();
                        Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Addon {1} {2}", workerThread.ThreadID, addon.Name, delType.ToString());
                    }
                    else
                    {
                        // Delete by Archive Content
                        DeleteType delType;
                        List<string> deletedList = new List<string>();
                        foreach (string archiveFolder in archiveFolderList)
                        {
                            delType = Addon.DeleteByName(archiveFolder);
                            if (delType == DeleteType.Deleted || delType == DeleteType.MovedToTrash)
                            {
                                deletedList.Add(archiveFolder);
                            }
                            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Folder {1} {2}", workerThread.ThreadID, archiveFolder, delType.ToString());
                        }
                        foreach (Addon subAddon in addon.SubAddons)
                        {
                            if (!deletedList.Contains(subAddon.Name))
                            {
                                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: SubAddon {1} was not in Archive", workerThread.ThreadID, subAddon.Name);
                            }
                        }
                        if (!deletedList.Contains(addon.Name))
                        {
                            Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} was not in Archive", workerThread.ThreadID, addon.Name);
                        }
                    }
                }
            }
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Expanding to {1}", workerThread.ThreadID, Addon.GetFolderPath());

            // Expand
            ArchiveHelper.Expand(archiveFilePath, Addon.GetFolderPath());

            // Set the Updated Date
            UpdateStatusList.Set(addon.Name, mapping.RemoteVersion);
            UpdateStatusList.Save();

            // Delete Temp File
            Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Deleting {1}", workerThread.ThreadID, archiveFilePath);
            File.Delete(archiveFilePath);

            // Mark as just updated
            addon.LocalVersionUpdated();
        }
    }
}
