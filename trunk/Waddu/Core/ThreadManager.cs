using System;
using System.ComponentModel;
using Waddu.Core.WorkItems;
using Waddu.Types;

namespace Waddu.Core
{
    public class ThreadManager
    {
        private static ThreadManager _instance = null;
        public static ThreadManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Not Initialized");
                }
                return _instance;
            }
        }

        public BindingList<WorkerThread> WorkerThreadList { get; private set; }
        private readonly BlockingQueue<WorkItemBase> _workQueue;

        public static void Initialize()
        {
            Logger.Instance.AddLog(LogType.Debug, "Initializing ThreadManager with {0} Threads", Config.Instance.NumberOfThreads);
            _instance = new ThreadManager();

            for (var i = 0; i < Config.Instance.NumberOfThreads; i++)
            {
                var workerThread = new WorkerThread();
                _instance.WorkerThreadList.Add(workerThread);
            }
        }

        private ThreadManager()
        {
            // Initialize Work Queue
            _workQueue = new BlockingQueue<WorkItemBase>();

            // Initialize Worker Threads
            WorkerThreadList = new BindingList<WorkerThread>();
        }

        public void AddWork(WorkItemBase workItem)
        {
            _workQueue.Enqueue(workItem);
        }

        public WorkItemBase GetWork()
        {
            return _workQueue.Dequeue();
        }

        public void Dispose()
        {
            // TODO: Clear Work Items
            foreach (var thread in WorkerThreadList)
            {
                // Tell the Thread to Stop
                thread.Stop();
                // Add Fake Work
                _workQueue.Enqueue(new WorkItemCancel());
            }
        }
    }
}
