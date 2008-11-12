using Waddu.BusinessObjects;
using Waddu.Types;

namespace Waddu.Classes.WorkItems
{
    public class WorkItemAddonVersionCheck : WorkItemBase
    {
        private Mapping _mapping;
        private Addon _addon;

        private WorkItemAddonVersionCheck()
        {
            _addon = null;
            _mapping = null;
        }

        public WorkItemAddonVersionCheck(Addon addon)
            : this()
        {
            _addon = addon;
        }

        public WorkItemAddonVersionCheck(Mapping mapping)
            : this()
        {
            _mapping = mapping;
        }

        public override void DoWork(WorkerThread workerThread)
        {
            Addon addon = _addon;
            Mapping mapping = _mapping;
            if (_addon == null)
            {
                addon = mapping.Addon;
            }

            // Get Remote Version
            Logger.Instance.AddLog(LogType.Information, "Thread #{0}: Version Check for {1}", workerThread.ThreadID, addon.Name);

            if (mapping != null)
            {
                // Only one Mapping
                workerThread.InfoText = string.Format("Get Version for \"{0}\" from {1}", addon.Name, mapping.AddonSiteId);
                mapping.CheckRemote();
            }
            else
            {
                // All (or no) Mappings
                workerThread.InfoText = string.Format("Get Versions for \"{0}\"", addon.Name);
                foreach (Mapping map in addon.Mappings)
                {
                    map.CheckRemote();
                }
            }
        }
    }
}
