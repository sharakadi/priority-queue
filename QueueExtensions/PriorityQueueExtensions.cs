using System.Collections;
using System.Collections.Generic;

namespace QueueExtensions
{
    public static class PriorityQueueExtensions
    {
        public static ICollection<T> GetAsICollection<T>(this PriorityQueue<T> priorityQueue, Priority requiredPriority)
        {
            var container = new PriorityQueueContainer<T>(priorityQueue, requiredPriority);
            return new QueueCollectionAdapter<T>(container);
        }

        public static ICollection GetAsCollection<T>(this PriorityQueue<T> priorityQueue, Priority requiredPriority)
        {
            var container = new PriorityQueueContainer<T>(priorityQueue, requiredPriority);
            return new QueueCollectionAdapter<T>(container);
        }

        public static IList<T> GetAsIList<T>(this PriorityQueue<T> priorityQueue, Priority requiredPriority)
        {
            var container = new PriorityQueueContainer<T>(priorityQueue, requiredPriority);
            return new QueueListAdapter<T>(container);
        } 
    }
}
