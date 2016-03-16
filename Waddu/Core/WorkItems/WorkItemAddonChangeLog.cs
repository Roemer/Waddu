using System;
using System.Windows.Forms;
using Waddu.Core.BusinessObjects;
using Waddu.Types;
using Waddu.UI.Forms;

namespace Waddu.Core.WorkItems
{
    public class WorkItemAddonChangeLog : WorkItemBase
    {
        private readonly Mapping _mapping;

        public WorkItemAddonChangeLog(Mapping mapping)
        {
            _mapping = mapping;
        }

        public override void DoWork(WorkerThread workerThread)
        {
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Change Log for {1} from {2}", workerThread.ThreadId, _mapping.Addon.Name, _mapping.AddonSiteId);
            workerThread.InfoText = String.Format("Change Log for {0} from {1}", _mapping.Addon.Name, _mapping.AddonSiteId);

            var changeLog = _mapping.GetChangeLog();
            MainForm.Instance.Invoke((MethodInvoker)(() =>
            {
                using (var dlg = new ChangeLogForm(changeLog))
                {
                    dlg.ShowDialog();
                }
            }));
        }
    }
}
