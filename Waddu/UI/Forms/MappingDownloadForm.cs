using System;
using System.Threading;
using System.Windows.Forms;
using Waddu.Core;
using Waddu.Interfaces;

namespace Waddu.UI.Forms
{
    public partial class MappingDownloadForm : Form, IDownloadProgress
    {
        private string _localPath;

        delegate bool StartDownloadDelegate(MappingDownloadForm form);
        public MappingDownloadForm(string localPath)
        {
            InitializeComponent();

            _localPath = localPath;

            Application.Idle += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs args)
        {
            // Remove the OnLoaded Event
            Application.Idle -= OnLoaded;

            var t = new Thread(ThreadProc);
            t.Start(this);
        }

        protected static void ThreadProc(object param)
        {
            var remotePaths = new[] {
                "http://waddu.flauschig.ch/mapping/waddu_mappings.xml",
                "http://www.red-demon.com/waddu/mapping/waddu_mappings.xml"
            };
            var form = param as MappingDownloadForm;
            var success = false;
            foreach (var path in remotePaths)
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
            Close();
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
