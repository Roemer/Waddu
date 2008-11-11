using System;

namespace Waddu.Classes.WorkItems
{
    public abstract class WorkItemBase
    {
        public abstract void DoWork(WorkerThread workerThread);
    }
}
