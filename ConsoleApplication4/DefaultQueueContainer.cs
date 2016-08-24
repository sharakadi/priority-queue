using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class DefaultQueueContainer<T> : IQueueContainer<T>
    {
        private Queue<T> _queue;

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
            return ((ICollection) _queue).SyncRoot;
        }

        public void SetQueue(Queue<T> queue)
        {
            _queue = queue;
        }
    }
}
