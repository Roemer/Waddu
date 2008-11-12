using System;

namespace Waddu.Core.WorkItems
{
    public abstract class WorkItemBase
    {
        public abstract void DoWork(WorkerThread workerThread);
    }
}
