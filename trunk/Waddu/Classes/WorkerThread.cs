using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Waddu.BusinessObjects;
using Waddu.Forms;
using Waddu.Types;

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
            get { return _infoText + (StatusText == string.Empty ? "" : string.Format(" ({0})", StatusText)); }
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
                WorkItem wi = ThreadManager.Instance.GetWork();
                workerThread.ThreadStatus = ThreadStatus.Processing;

                if (wi.WorkItemType == WorkItemType.Cancel)
                {
                    // Exit Thread
                    workerThread.ThreadStatus = ThreadStatus.Stopping;
                    Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Stopping", workerThread.ThreadID);
                    break;
                }
                else if (wi.WorkItemType == WorkItemType.VersionCheck)
                {
                    // Get Remote Version
                    Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Version Check for {1}", workerThread.ThreadID, wi.Addon.Name);
                    if (wi.Mapping != null)
                    {
                        workerThread.InfoText = string.Format("Get Version for \"{0}\" from {1}", wi.Addon.Name, wi.Mapping.AddonSiteId);
                        wi.Mapping.CheckRemote();
                    }
                    else
                    {
                        workerThread.InfoText = string.Format("Get Versions for \"{0}\"", wi.Addon.Name);
                        foreach (Mapping map in wi.Addon.Mappings)
                        {
                            map.CheckRemote();
                        }
                    }
                }
                // Update Addon
                else if (wi.WorkItemType == WorkItemType.Update)
                {
                    if (wi.Addon.Mappings.Count <= 0)
                    {
                        // Addon has no Mappings, skip
                        Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Addon {1} has no Mapping", workerThread.ThreadID, wi.Addon.Name);
                        continue;
                    }

                    // Download
                    Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Updating {1} from {2}", workerThread.ThreadID, wi.Addon.Name, wi.Addon.BestMapping.AddonSiteId);
                    workerThread.InfoText = string.Format("DL from {0}: {1}", wi.Addon.BestMapping.AddonSiteId, wi.Addon.Name);
                    string downloadUrl = wi.Addon.BestMapping.GetDownloadLink();
                    if (downloadUrl == string.Empty)
                    {
                        Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Download Link for {1} incorrect", workerThread.ThreadID, wi.Addon.Name);
                        continue;
                    }
                    string archiveFilePath = Helpers.DownloadFile(downloadUrl, workerThread);
                    if (archiveFilePath == string.Empty)
                    {
                        Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Download for {1} failed", workerThread.ThreadID, wi.Addon.Name);
                        continue;
                    }
                    Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Downloaded to {1}", workerThread.ThreadID, archiveFilePath);

                    // Check if 7z Exists
                    if (ArchiveHelper.Exists7z())
                    {
                        // Simple Check if the Archive looks right
                        if (!ArchiveHelper.CheckIntegrity(archiveFilePath, wi.Addon.Name))
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
                        if (wi.Addon.IsInstalled)
                        {
                            DeleteType delType;
                            foreach (Addon subAddon in wi.Addon.SubAddons)
                            {
                                delType = subAddon.Delete();
                                Logger.Instance.AddLog(LogType.Information, "Thread #{0}: SubAddon {1} {2}", workerThread.ThreadID, subAddon.Name, delType.ToString());
                            }
                            delType = wi.Addon.Delete();
                            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Addon {1} {2}", workerThread.ThreadID, wi.Addon.Name, delType.ToString());
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
            workerThread.ThreadStatus = ThreadStatus.Stopped;
            Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Stopped", workerThread.ThreadID);
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
        [Browsable(false)]
        public string StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                _statusText = value;
                NotifyPropertyChanged("InfoText");
            }
        }

        #endregion
    }
}
