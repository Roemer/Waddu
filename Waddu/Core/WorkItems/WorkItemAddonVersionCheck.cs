using Waddu.Core.BusinessObjects;
using Waddu.Types;

namespace Waddu.Core.WorkItems
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

            // Check if the Addon needs updating and which Mapping is the Best
            AddonUpdateStats stats = UpdateStatusList.Get(addon.Name);
            foreach (Mapping addonMapping in addon.Mappings)
            {
                if (stats != null)
                {
                    if (stats.LastUpdated > addonMapping.LastUpdated)
                    {
                        // Skip if the Local Date is bigger than the Remote Date
                        continue;
                    }
                }

                // If no Mapping defined yet, define it now
                if (addon.PreferredMapping == null)
                {
                    addon.PreferredMapping = addonMapping;
                    continue;
                }

                // Assign by Preferred
                AddonSiteId preferred;
                if (Config.Instance.GetPreferredMapping(addon, out preferred))
                {
                    if (addonMapping.AddonSiteId == preferred)
                    {
                        addon.PreferredMapping = addonMapping;
                    }
                    // If a Preferred Mapping is set, skip all others
                    continue;
                }

                // Assign by NoLib Setting
                if (Config.Instance.PreferNoLib)
                {
                    if (addonMapping.AddonSiteId == AddonSiteId.wowace || addonMapping.AddonSiteId == AddonSiteId.curseforge)
                    {
                        addon.PreferredMapping = addonMapping;
                        continue;
                    }
                }

                // Check if the Mapping has a newer Date
                if (addonMapping.LastUpdated > addon.PreferredMapping.LastUpdated)
                {
                    addon.PreferredMapping = addonMapping;
                    continue;
                }

                // Assign by Priority
                int indexOld = Config.Instance.AddonSites.IndexOf(addon.PreferredMapping.AddonSiteId);
                int indexNew = Config.Instance.AddonSites.IndexOf(addonMapping.AddonSiteId);
                if (indexNew >= 0 && indexNew < indexOld)
                {
                    addon.PreferredMapping = addonMapping;
                    continue;
                }
            }
        }
    }
}
