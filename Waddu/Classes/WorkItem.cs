using Waddu.BusinessObjects;
using Waddu.Types;
using System;

namespace Waddu.Classes
{
    public class WorkItem
    {
        private WorkItemType _workItemType;
        public WorkItemType WorkItemType
        {
            get { return _workItemType; }
        }

        private Addon _addon;
        public Addon Addon
        {
            get { return _addon; }
        }

        private AddonSiteId _addonSiteId;
        public AddonSiteId AddonSiteId
        {
            get { return _addonSiteId; }
        }

        public WorkItem(WorkItemType workItemType)
        {
            if (workItemType != WorkItemType.Cancel)
            {
                throw new Exception("WorkItem needs Addon and AddonSiteId");
            }
        }
        public WorkItem(WorkItemType workItemType, Addon addon, AddonSiteId addonSiteId)
        {
            _workItemType = workItemType;
            _addon = addon;
            _addonSiteId = AddonSiteId;
        }
    }
}
