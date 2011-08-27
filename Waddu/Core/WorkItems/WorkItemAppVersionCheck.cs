﻿using System.Windows.Forms;
using Waddu.Types;
using Waddu.UI.Forms;

namespace Waddu.Core.WorkItems
{
    public class WorkItemAppVersionCheck : WorkItemBase
    {
        public override void DoWork(WorkerThread workerThread)
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
                if (version == this.GetType().Assembly.GetName().Version.ToString())
                {
                    Logger.Instance.AddLog(LogType.Information, "Thread #{0}: No Update for Waddu available: {1}", workerThread.ThreadID, version);
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
    }
}
