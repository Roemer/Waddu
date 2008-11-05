using System;
using System.Threading;
using System.Windows.Forms;
using Waddu.Classes;

namespace Waddu.Forms
{
    public partial class MappingDownloadForm : Form, IDownloadProgress
    {
        private string _remotePath;
        private string _localPath;

        delegate bool StartDownloadDelegate(MappingDownloadForm form);
        public MappingDownloadForm(string remotePath, string localPath)
        {
            InitializeComponent();

            _remotePath = remotePath;
            _localPath = localPath;

            Thread t = new Thread(new ParameterizedThreadStart(ThreadProc));
            t.Start(this);
        }

        protected static void ThreadProc(object param)
        {
            MappingDownloadForm form = param as MappingDownloadForm;
            Helpers.DownloadFile(form._remotePath, form._localPath, form);
            form.CloseForm();
        }

        private delegate void CloseEventHandler();
        private void CloseForm()
        {
            if (InvokeRequired)
            {
                Invoke(new CloseEventHandler(CloseForm));
                return;
            }
            this.Close();
        }

        private delegate void SetStatusEventHandler(long curr, long tot);
        private void SetStatus(long curr, long tot)
        {
            if (InvokeRequired)
            {
                Invoke(new SetStatusEventHandler(SetStatus), curr, tot);
                return;
            }
            if (curr >= 0 && tot >= 0)
            {
                lblStatus.Text = string.Format("{0} of {1}", Helpers.FormatBytes(curr), Helpers.FormatBytes(tot));
                pbStatus.Value = Convert.ToInt32(curr / (double)tot * 100);
            }
            else
            {
                lblStatus.Text = string.Empty;
            }
        }

        #region IDownloadProgress Members
        public void DownloadStatusChanged(long currentBytes, long totalBytes)
        {
            SetStatus(currentBytes, totalBytes);
        }
        #endregion
    }
}
