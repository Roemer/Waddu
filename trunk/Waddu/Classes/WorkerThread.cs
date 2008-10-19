using System.ComponentModel;
using System.IO;
using System.Threading;
using Waddu.AddonSites;
using Waddu.BusinessObjects;
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
            Logger.AddLog("Thread #{0}: Starting", workerThread.ThreadID);
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
                    Logger.AddLog("Thread #{0}: Stopping", workerThread.ThreadID);
                    break;
                }
                else if (wi.WorkItemType == WorkItemType.VersionCheck)
                {
                    // Get Remote Version
                    Logger.AddLog("Thread #{0}: Version Check for {1}", workerThread.ThreadID, wi.Addon.Name);
                    workerThread.InfoText = string.Format("Get Versions for \"{0}\"", wi.Addon.Name);
                    wi.Addon.GetRemoteVersions();
                }
                else if (wi.WorkItemType == WorkItemType.Update)
                {
                    // Update Addon
                    Logger.AddLog("Thread #{0}: Updating {1} from {2}", workerThread.ThreadID, wi.Addon.Name, wi.Addon.Mappings[0]);
                    workerThread.InfoText = string.Format("DL from {0}: {1}", wi.Addon.Mappings[0], wi.Addon.Name);
                    AddonSiteBase site = AddonSiteBase.GetSite(wi.Addon.Mappings[0].AddonSiteId);
                    string downloadUrl = site.GetDownloadLink(wi.Addon.Mappings[0].AddonTag);
                    if (downloadUrl == string.Empty)
                    {
                        Logger.AddLog("Thread #{0}: Download Link for {1} incorrect", workerThread.ThreadID, wi.Addon.Name);
                        continue;
                    }
                    string locUrl = Helpers.DownloadFile(downloadUrl, workerThread);
                    if (locUrl == string.Empty)
                    {
                        Logger.AddLog("Thread #{0}: Download for {1} failed", workerThread.ThreadID, wi.Addon.Name);
                        continue;
                    }

                    Logger.AddLog("Thread #{0}: Downloaded to {1}", workerThread.ThreadID, locUrl);
                    if (Config.Instance.DeleteBeforeUpdate)
                    {
                        if (wi.Addon.IsInstalled)
                        {
                            DeleteType delType;
                            foreach (Addon subAddon in wi.Addon.SubAddons)
                            {
                                delType = subAddon.Delete();
                                Logger.AddLog("Thread #{0}: SubAddon {1} {2}", workerThread.ThreadID, subAddon.Name, delType.ToString());
                            }
                            delType = wi.Addon.Delete();
                            Logger.AddLog("Thread #{0}: Addon {1} {2}", workerThread.ThreadID, wi.Addon.Name, delType.ToString());
                        }
                    }
                    Logger.AddLog("Thread #{0}: Unzipping to {1}", workerThread.ThreadID, Addon.GetFolderPath());

                    // Unzip
                    // Handle Special Cases
                    if (wi.Addon.Name == "xchar")
                    {
                        Helpers.Unzip(locUrl, Path.Combine(Addon.GetFolderPath(), "xchar"));
                    }
                    else
                    {
                        Helpers.Unzip(locUrl, Addon.GetFolderPath());
                    }
                    
                    // Delete Temp File
                    Logger.AddLog("Thread #{0}: Deleting {1}", workerThread.ThreadID, locUrl);
                    File.Delete(locUrl);
                }
                Logger.AddLog("Thread #{0}: Finished", workerThread.ThreadID);
            }
            workerThread.ThreadStatus = ThreadStatus.Stopped;
            Logger.AddLog("Thread #{0}: Stopped", workerThread.ThreadID);
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
