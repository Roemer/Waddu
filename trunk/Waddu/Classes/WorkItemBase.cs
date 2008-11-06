using Waddu.Types;

namespace Waddu.Classes
{
    public abstract class WorkItemBase
    {
        private WorkItemType _workItemType;
        public WorkItemType WorkItemType
        {
            get { return _workItemType; }
        }

        public WorkItemBase(WorkItemType workItemType)
        {
            _workItemType = workItemType;
        }
    }
}
