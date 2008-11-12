using System.Windows.Forms;
using Waddu.BusinessObjects;
using Waddu.Forms;
using Waddu.Types;

namespace Waddu.Classes.WorkItems
{
    public class WorkItemAddonChangeLog : WorkItemBase
    {
        private Mapping _mapping;

        public WorkItemAddonChangeLog(Mapping mapping)
        {
            _mapping = mapping;
        }

        public override void DoWork(WorkerThread workerThread)
        {
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Change Log for {1} from {2}", workerThread.ThreadID, _mapping.Addon.Name, _mapping.AddonSiteId);
            workerThread.InfoText = string.Format("Change Log for {0} from {1}", _mapping.Addon.Name, _mapping.AddonSiteId);

            string changeLog = _mapping.GetChangeLog();
            MainForm.Instance.Invoke((MethodInvoker)delegate()
            {
                using (ChangeLogForm dlg = new ChangeLogForm(changeLog))
                {
                    dlg.ShowDialog();
                }
            });
        }
    }
}
