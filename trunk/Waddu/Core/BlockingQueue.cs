using System;
using System.Collections.Generic;
using System.Threading;

namespace Waddu.Core
{
    public class BlockingQueue<T> : IDisposable
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private Semaphore _semaphore = new Semaphore(0, int.MaxValue);

        public void Enqueue(T data)
        {
            if (data == null) throw new ArgumentNullException("data");
            lock (_queue)
            {
                _queue.Enqueue(data);
            }
            _semaphore.Release();
        }

        public T Dequeue()
        {
            _semaphore.WaitOne();
            lock (_queue)
            {
                return _queue.Dequeue();
            }
        }

        public void Dispose()
        {
            if (_semaphore != null)
            {
                _semaphore.Close();
                _semaphore = null;
            }
        }
    }
}
