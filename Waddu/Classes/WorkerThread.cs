using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Waddu.BusinessObjects;
using Waddu.Forms;
using Waddu.Types;
using System.Collections.Generic;
using System;

namespace Waddu.Classes
{
    public class WorkerThread : INotifyPropertyChanged, IDownloadProgress
    {
        private Thread _thread;

        public int ThreadID
        {
            get { return _thread.ManagedThreadId; }
        }

        private ThreadStatus _threadStatus;
        public ThreadStatus ThreadStatus
        {
            get { return _threadStatus; }
            set { _threadStatus = value; NotifyPropertyChanged("ThreadStatus"); }
        }

        private string _infoText;
        public string InfoText
        {
            get { return _infoText + (_statusText == string.Empty ? "" : string.Format(" ({0})", _statusText)); }
            set { _infoText = value; NotifyPropertyChanged("InfoText"); }
        }

        public WorkerThread()
        {
            _thread = new Thread(new ParameterizedThreadStart(ThreadProc));

            _thread.Start(this);
        }

        public void Stop()
        {
            ThreadStatus = ThreadStatus.Stopping;
        }

        protected static void ThreadProc(object param)
        {
            WorkerThread workerThread = param as WorkerThread;
            Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Starting", workerThread.ThreadID);
            while (true)
            {
                workerThread.InfoText = "";
                workerThread.ThreadStatus = ThreadStatus.Idle;
                WorkItemBase wi = ThreadManager.Instance.GetWork();
                workerThread.ThreadStatus = ThreadStatus.Processing;

                try
                {
                    if (wi.WorkItemType == WorkItemType.WadduVersionCheck)
                    {
                        // Version Check for Waddu
                        WadduVersionCheck(workerThread);
                    }
                    else if (wi.WorkItemType == WorkItemType.Cancel)
                    {
                        // Exit Thread
                        workerThread.ThreadStatus = ThreadStatus.Stopping;
                        Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Stopping", workerThread.ThreadID);
                        break;
                    }
                    else if (wi.WorkItemType == WorkItemType.VersionCheck)
                    {
                        Addon addon = ((WorkItemAddon)wi).Addon;
                        Mapping mapping = ((WorkItemAddon)wi).Mapping;

                        // Get Remote Version
                        Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Version Check for {1}", workerThread.ThreadID, addon.Name);

                        if (mapping != null)
                        {
                            // Only one Mapping
                            workerThread.InfoText = string.Format("Get Version for \"{0}\" from {1}", addon.Name, mapping.AddonSiteId);
                            mapping.CheckRemote();
                        }
                        else
                        {
                            // All (or no) Mappings
                            workerThread.InfoText = string.Format("Get Versions for \"{0}\"", addon.Name);
                            foreach (Mapping map in addon.Mappings)
                            {
                                map.CheckRemote();
                            }
                        }
                    }
                    // Update Addon
                    else if (wi.WorkItemType == WorkItemType.Update)
                    {
                        Addon addon = ((WorkItemAddon)wi).Addon;
                        Mapping mapping = ((WorkItemAddon)wi).Mapping;

                        if (addon.Mappings.Count <= 0)
                        {
                            // Addon has no Mappings, skip
                            Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} has no Mapping", workerThread.ThreadID, addon.Name);
                            continue;
                        }

                        // Define the Mapping to use
                        if (mapping == null)
                        {
                            // If no specific Mapping given, use Preferred Mapping
                            mapping = addon.PreferredMapping;
                        }
                        // Download
                        Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Updating {1} from {2}", workerThread.ThreadID, addon.Name, mapping.AddonSiteId);
                        workerThread.InfoText = string.Format("DL from {0}: {1}", mapping.AddonSiteId, addon.Name);
                        string downloadUrl = mapping.GetDownloadLink();
                        if (downloadUrl == string.Empty)
                        {
                            Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Download Link for {1} incorrect", workerThread.ThreadID, addon.Name);
                            continue;
                        }
                        string archiveFilePath = WebHelper.DownloadFileToTemp(downloadUrl, workerThread);
                        if (archiveFilePath == string.Empty)
                        {
                            Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Download for {1} failed", workerThread.ThreadID, addon.Name);
                            continue;
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
                                        continue;
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

                        // Delete Temp File
                        Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Deleting {1}", workerThread.ThreadID, archiveFilePath);
                        File.Delete(archiveFilePath);
                    }
                    Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Finished", workerThread.ThreadID);
                }
                catch (Exception ex)
                {
                    Logger.Instance.AddLog(LogType.Error, "Thread #{0}: Error {1}", workerThread.ThreadID, ex.Message);
                    MainForm.Instance.Invoke((MethodInvoker)delegate()
                    {
                        using (ErrorForm dlg = new ErrorForm(ex))
                        {
                            dlg.StartPosition = FormStartPosition.CenterParent;
                            dlg.ShowDialog();
                        }
                    });
                }
            }
            workerThread.ThreadStatus = ThreadStatus.Stopped;
            Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Stopped", workerThread.ThreadID);
        }

        private static void WadduVersionCheck(WorkerThread workerThread)
        {
            string[] remotePaths = new string[] {
                "http://waddu.flauschig.ch/download/latest.txt",
                "http://www.red-demon.com/waddu/download/latest.txt"
            };

            workerThread.InfoText = "Version Check for Waddu";
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Version Check for Waddu", workerThread.ThreadID);
            bool success = false;
            string version = string.Empty;
            foreach (string path in remotePaths)
            {
                success = WebHelper.GetString(path, out version);
                if (success) { break; }
            }
            if (success)
            {
                if (version == workerThread.GetType().Assembly.GetName().Version.ToString())
                {
                    MainForm.Instance.Invoke((MethodInvoker)delegate()
                    {
                        MessageBox.Show("You have the newest Version");
                    });
                }
                else
                {
                    MainForm.Instance.Invoke((MethodInvoker)delegate()
                    {
                        using (UpdateAvailableForm dlg = new UpdateAvailableForm(version))
                        {
                            dlg.StartPosition = FormStartPosition.CenterParent;
                            dlg.ShowDialog();
                        }
                    });
                }
            }
            else
            {
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Could not check the Version", workerThread.ThreadID);
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #region IDownloadProgress Members
        private string _statusText = string.Empty;
        public void DownloadStatusChanged(long currentBytes, long totalBytes)
        {
            _statusText = string.Empty;
            if (currentBytes >= 0 && totalBytes >= 0)
            {
                _statusText = string.Format("{0} of {1}", Helpers.FormatBytes(currentBytes), Helpers.FormatBytes(totalBytes));
            }
            NotifyPropertyChanged("InfoText");
        }
        #endregion
    }
}
