using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    internal class QueueProvider<T>
    {
        private readonly PriorityQueue<T> _priorityQueue;
        private readonly Priority _priority;

        public Queue<T> GetQueue()
        {
            return _priorityQueue.GetQueue(_priority);
        }

        public object GetRoot()
        {
            return _priorityQueue.GetQueueRoot(_priority);
        }

        public QueueProvider(PriorityQueue<T> priorityQueue, Priority priority)
        {
            _priorityQueue = priorityQueue;
            _priority = priority;
        }

        public void SetQueue(Queue<T> queue)
        {
            _priorityQueue.SetQueue(_priority, queue);
        }
    }
}
