using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Waddu.Classes.Interfaces;
using Waddu.Classes.WorkItems;
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
                    if (wi.GetType() == typeof(WorkItemCancel))
                    {
                        // Exit Thread
                        workerThread.ThreadStatus = ThreadStatus.Stopping;
                        Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Stopping", workerThread.ThreadID);
                        break;
                    }

                    // Do the Work
                    wi.DoWork(workerThread);

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
