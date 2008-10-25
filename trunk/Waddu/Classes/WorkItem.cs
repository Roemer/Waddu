using System;
using Waddu.BusinessObjects;
using Waddu.Types;

namespace Waddu.Classes
{
    public class WorkItem
    {
        private WorkItemType _workItemType;
        public WorkItemType WorkItemType
        {
            get { return _workItemType; }
        }

        private Addon _addon = null;
        public Addon Addon
        {
            get { return _addon; }
        }

        private Mapping _mapping = null;
        public Mapping Mapping
        {
            get { return _mapping; }
            set { _mapping = value; }
        }

        public WorkItem(WorkItemType workItemType)
        {
            if (workItemType != WorkItemType.Cancel)
            {
                throw new Exception("WorkItem needs an Addon");
            }
        }
        public WorkItem(WorkItemType workItemType, Addon addon)
        {
            _workItemType = workItemType;
            _addon = addon;

        }
        public WorkItem(WorkItemType workItemType, Addon addon, Mapping mapping)
            : this(workItemType, addon)
        {
            _mapping = mapping;
        }
    }
}
