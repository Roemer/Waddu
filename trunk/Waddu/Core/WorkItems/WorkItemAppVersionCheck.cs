using System.Windows.Forms;
using Waddu.Types;
using Waddu.UI.Forms;

namespace Waddu.Core.WorkItems
{
    public class WorkItemAppVersionCheck : WorkItemBase
    {
        public override void DoWork(WorkerThread workerThread)
        {
            string[] remotePaths = {
                "http://waddu.flauschig.ch/download/latest.txt",
                "http://www.red-demon.com/waddu/download/latest.txt"
            };

            workerThread.InfoText = "Version Check for Waddu";
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Version Check for Waddu", workerThread.ThreadId);
            var success = false;
            var version = string.Empty;
            foreach (var path in remotePaths)
            {
                success = WebHelper.GetString(path, out version);
                if (success) { break; }
            }
            if (success)
            {
                if (version == GetType().Assembly.GetName().Version.ToString())
                {
                    Logger.Instance.AddLog(LogType.Information, "Thread #{0}: No Update for Waddu available: {1}", workerThread.ThreadId, version);
                }
                else
                {
                    MainForm.Instance.Invoke((MethodInvoker)(() =>
                    {
                        using (var dlg = new UpdateAvailableForm(version))
                        {
                            dlg.StartPosition = FormStartPosition.CenterParent;
                            dlg.ShowDialog();
                        }
                    }));
                }
            }
            else
            {
                Logger.Instance.AddLog(LogType.Warning, "Thread #{0}: Could not check the Version", workerThread.ThreadId);
            }
        }
    }
}
