using System;
using Waddu.BusinessObjects;
using Waddu.Types;

namespace Waddu.Classes
{
    public class WorkItemAddon : WorkItemBase
    {
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

        public WorkItemAddon(WorkItemType workItemType, Addon addon)
            : base(workItemType)
        {
            _addon = addon;

        }
        public WorkItemAddon(WorkItemType workItemType, Addon addon, Mapping mapping)
            : this(workItemType, addon)
        {
            _mapping = mapping;
        }
    }
}
