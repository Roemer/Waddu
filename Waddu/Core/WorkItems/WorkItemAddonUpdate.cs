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
        private readonly Mapping _mapping;
        private readonly Addon _addon;

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
            workerThread.InfoText = _addon.Name;

            var mapping = _mapping;

            if (_addon.Mappings.Count <= 0)
            {
                // Addon has no Mappings, skip
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} has no Mapping", workerThread.ThreadId, _addon.Name);
                return;
            }

            // Define the Mapping to use
            if (mapping == null)
            {
                // If no specific Mapping given, use Preferred Mapping
                mapping = _addon.PreferredMapping;
            }

            // If the Mapping still is undefined (like no Update Check was made)
            if (mapping == null)
            {
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} has no Mapping to Update", workerThread.ThreadId, _addon.Name);
                return;
            }

            // Get the File Path / Url
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Updating {1} from {2}", workerThread.ThreadId, _addon.Name, mapping.AddonSiteId);
            var fileUrl = mapping.GetFilePath();
            if (fileUrl == string.Empty)
            {
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: File for {1} incorrect", workerThread.ThreadId, _addon.Name);
                return;
            }

            string archiveFilePath;
            if (fileUrl.ToLower().StartsWith("http:"))
            {
                // Download
                workerThread.InfoText = string.Format("DL from {0}: {1}", mapping.AddonSiteId, _addon.Name);
                archiveFilePath = WebHelper.DownloadFileToTemp(fileUrl, workerThread);

                // Check if the Download was correct
                if (archiveFilePath == string.Empty)
                {
                    Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Download for {1} failed", workerThread.ThreadId, _addon.Name);
                    return;
                }
                Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Downloaded to {1}", workerThread.ThreadId, archiveFilePath);
            }
            else
            {
                // A File was selected
                archiveFilePath = fileUrl;
            }

            var has7z = ArchiveHelper.Exists7z();
            var archiveFolderList = new List<string>();
            // Check if 7z Exists
            if (has7z)
            {
                // Get the Folder List (for Deletion)
                archiveFolderList = ArchiveHelper.GetRootFolders(archiveFilePath);

                // Simple Check if the Archive looks right
                if (!ArchiveHelper.CheckIntegrity(archiveFilePath, _addon.Name))
                {
                    // If now, warn us
                    using (var f = new ArchiveContentForm(archiveFilePath))
                    {
                        if (f.ShowDialog() != DialogResult.OK)
                        {
                            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: cancelled by User Request", workerThread.ThreadId);
                            return;
                        }
                    }
                }
            }

            // Delete Old
            if (Config.Instance.DeleteBeforeUpdate)
            {
                if (_addon.IsInstalled)
                {
                    if (!has7z)
                    {
                        MessageBox.Show("Please install 7z to get sure that the right Folders get Deleted");
                        // Straight-Forward Delete
                        DeleteType delType;
                        foreach (var subAddon in _addon.SubAddons)
                        {
                            delType = subAddon.Delete();
                            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: SubAddon {1} {2}", workerThread.ThreadId, subAddon.Name, delType.ToString());
                        }
                        delType = _addon.Delete();
                        Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Addon {1} {2}", workerThread.ThreadId, _addon.Name, delType.ToString());
                    }
                    else
                    {
                        // Delete by Archive Content
                        var deletedList = new List<string>();
                        foreach (var archiveFolder in archiveFolderList)
                        {
                            var delType = Addon.DeleteByName(archiveFolder);
                            if (delType == DeleteType.Deleted || delType == DeleteType.MovedToTrash)
                            {
                                deletedList.Add(archiveFolder);
                            }
                            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Folder {1} {2}", workerThread.ThreadId, archiveFolder, delType.ToString());
                        }
                        foreach (var subAddon in _addon.SubAddons)
                        {
                            if (!deletedList.Contains(subAddon.Name))
                            {
                                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: SubAddon {1} was not in Archive", workerThread.ThreadId, subAddon.Name);
                            }
                        }
                        if (!deletedList.Contains(_addon.Name))
                        {
                            Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} was not in Archive", workerThread.ThreadId, _addon.Name);
                        }
                    }
                }
            }
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Expanding to {1}", workerThread.ThreadId, Addon.GetFolderPath());

            // Expand
            ArchiveHelper.Expand(archiveFilePath, Addon.GetFolderPath());

            // Set the Updated Date
            UpdateStatusList.Set(_addon.Name, mapping.RemoteVersion);
            UpdateStatusList.Save();

            // Delete Temp File
            Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Deleting {1}", workerThread.ThreadId, archiveFilePath);
            File.Delete(archiveFilePath);

            // Mark as just updated
            _addon.LocalVersionUpdated();
        }
    }
}
