using System;
using System.ComponentModel;
using Waddu.Properties;
using Waddu.Types;

namespace Waddu.Classes
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

        private BindingList<WorkerThread> _workerThreadList;
        public BindingList<WorkerThread> WorkerThreadList
        {
            get { return _workerThreadList; }
        }

        private BlockingQueue<WorkItemBase> _workQueue;

        public static void Initialize()
        {
            Logger.Instance.AddLog(LogType.Debug, "Initializing ThreadManager with {0} Threads", Config.Instance.NumberOfThreads);
            _instance = new ThreadManager();

            for (int i = 0; i < Config.Instance.NumberOfThreads; i++)
            {
                WorkerThread workerThread = new WorkerThread();
                _instance._workerThreadList.Add(workerThread);
            }
        }

        private ThreadManager()
        {
            // Initialize Work Queue
            _workQueue = new BlockingQueue<WorkItemBase>();

            // Initialize Worker Threads
            _workerThreadList = new BindingList<WorkerThread>();
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
            foreach (WorkerThread thread in _workerThreadList)
            {
                // Tell the Thread to Stop
                thread.Stop();
                // Add Fake Work
                _workQueue.Enqueue(new WorkItemEmpty(WorkItemType.Cancel));
            }
        }
    }
}
