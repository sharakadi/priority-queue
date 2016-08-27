using System.Collections;
using System.Collections.Generic;

namespace QueueExtensions
{
    public class DefaultQueueContainer<T> : IQueueContainer<T>
    {
        private Queue<T> _queue;
        private readonly object _syncRoot = new object();

        public DefaultQueueContainer(Queue<T> queue)
        {
            _queue = queue;
        }

        public Queue<T> GetQueue()
        {
            return _queue;
        }

        public object GetSyncRoot()
        {
            //return ((ICollection) _queue).SyncRoot;
            return _syncRoot;
        }

        public void SetQueue(Queue<T> queue)
        {
            _queue = queue;
        }
    }
}
