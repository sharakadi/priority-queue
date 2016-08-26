using System.Collections.Generic;
using ConsoleApplication4;

namespace QueueExtensions
{
    internal class PriorityQueueContainer<T> : IQueueContainer<T>
    {
        private readonly PriorityQueue<T> _priorityQueue;
        private readonly Priority _priority;

        public Queue<T> GetQueue()
        {
            return _priorityQueue.GetQueue(_priority);
        }

        public object GetSyncRoot()
        {
            return _priorityQueue.GetQueueRoot(_priority);
        }

        public PriorityQueueContainer(PriorityQueue<T> priorityQueue, Priority priority)
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
