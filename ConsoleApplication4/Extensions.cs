using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public static class Extensions
    {
        public static ICollection<T> GetAsICollection<T>(this PriorityQueue<T> priorityQueue, Priority requiredPriority)
        {
            var provider = new PriorityQueueProvider<T>(priorityQueue, requiredPriority);
            return new QueueCollectionAdapter<T>(provider);
        }

        public static ICollection GetAsCollection<T>(this PriorityQueue<T> priorityQueue, Priority requiredPriority)
        {
            var provider = new PriorityQueueProvider<T>(priorityQueue, requiredPriority);
            return new QueueCollectionAdapter<T>(provider);
        }
    }
}
