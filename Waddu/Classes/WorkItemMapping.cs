using Waddu.BusinessObjects;
using Waddu.Types;

namespace Waddu.Classes
{
    public class WorkItemMapping : WorkItemBase
    {
        private Mapping _mapping = null;
        public Mapping Mapping
        {
            get { return _mapping; }
            set { _mapping = value; }
        }

        public WorkItemMapping(WorkItemType workItemType, Mapping mapping)
            : base(workItemType)
        {
            _mapping = mapping;
        }
    }
}
