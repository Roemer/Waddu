using System;
using System.Threading;
using System.Windows.Forms;
using Waddu.Classes;

namespace Waddu.Forms
{
    public partial class MappingDownloadForm : Form, IDownloadProgress
    {
        private string _localPath;

        delegate bool StartDownloadDelegate(MappingDownloadForm form);
        public MappingDownloadForm(string localPath)
        {
            InitializeComponent();

            _localPath = localPath;

            Thread t = new Thread(new ParameterizedThreadStart(ThreadProc));
            t.Start(this);
        }

        protected static void ThreadProc(object param)
        {
            string[] remotePaths = new string[] {
                "http://waddu.flauschig.ch/mapping/waddu_mappings.xml",
                "http://www.red-demon.com/waddu/mapping/waddu_mappings.xml"
            };
            MappingDownloadForm form = param as MappingDownloadForm;
            bool success = false;
            foreach (string path in remotePaths)
            {
                success = WebHelper.DownloadFile(path, form._localPath, form);
                if (success) { break; }
            }
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
