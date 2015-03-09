using System;
using System.Threading;
using System.Windows.Forms;
using Waddu.Core.WorkItems;
using Waddu.Interfaces;
using Waddu.Types;
using Waddu.UI.Forms;

namespace Waddu.Core
{
    public class WorkerThread : ObservableObject, IDownloadProgress
    {
        private Thread _thread;

        public int ThreadId { get { return _thread.ManagedThreadId; } }

        public ThreadStatus ThreadStatus
        {
            get { return GetProperty<ThreadStatus>(); }
            set { SetProperty(value); }
        }

        private string _infoText;
        public string InfoText
        {
            get { return _infoText + (_statusText == String.Empty ? "" : String.Format(" ({0})", _statusText)); }
            set { _infoText = value; OnPropertyChanged(() => InfoText); }
        }

        public WorkerThread()
        {
            Start();
        }

        private void Start()
        {
            _thread = new Thread(ThreadProc);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start(this);
        }

        public void Abort()
        {
            try
            {
                _thread.Abort();
            }
            catch
            {
                // Handle gracefully
            }
            Start();
        }

        public void Stop()
        {
            ThreadStatus = ThreadStatus.Stopping;
        }

        protected static void ThreadProc(object param)
        {
            var workerThread = (WorkerThread)param;
            Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Starting", workerThread.ThreadId);
            while (true)
            {
                workerThread.InfoText = "";
                workerThread.ThreadStatus = ThreadStatus.Idle;
                var wi = ThreadManager.Instance.GetWork();
                workerThread.ThreadStatus = ThreadStatus.Processing;

                try
                {
                    if (wi.GetType() == typeof(WorkItemCancel))
                    {
                        // Exit Thread
                        workerThread.ThreadStatus = ThreadStatus.Stopping;
                        Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Stopping", workerThread.ThreadId);
                        break;
                    }

                    // Do the Work
                    wi.DoWork(workerThread);

                    Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Finished", workerThread.ThreadId);
                }
                catch (Exception ex)
                {
                    Logger.Instance.AddLog(LogType.Error, "Thread #{0}: Error {1}", workerThread.ThreadId, ex.Message);
                    MainForm.Instance.Invoke((MethodInvoker)(() =>
                    {
                        using (var dlg = new ErrorForm(ex))
                        {
                            dlg.StartPosition = FormStartPosition.CenterParent;
                            dlg.ShowDialog();
                        }
                    }));
                }
            }
            workerThread.ThreadStatus = ThreadStatus.Stopped;
            Logger.Instance.AddLog(LogType.Debug, "Thread #{0}: Stopped", workerThread.ThreadId);
        }

        #region IDownloadProgress Members
        private string _statusText = string.Empty;
        public void DownloadStatusChanged(long currentBytes, long totalBytes)
        {
            _statusText = string.Empty;
            if (currentBytes >= 0 && totalBytes >= 0)
            {
                _statusText = string.Format("{0} of {1}", Helpers.FormatBytes(currentBytes), Helpers.FormatBytes(totalBytes));
            }
            OnPropertyChanged(() => InfoText);
        }
        #endregion
    }
}
