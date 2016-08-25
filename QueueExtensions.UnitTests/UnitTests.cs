using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication4;
using NUnit.Framework;

namespace QueueExtensions.UnitTests
{

    [TestFixture]
    public class UnitTests
    {
        internal class Item
        {
            public string Name { get; set; }
            public DateTime TimeStamp { get; set; }
        }

        private Item[] CreateItems(int count)
        {
            return Enumerable.Range(1, count).Select(x => new Item()
            {
                Name = "N" + x,
                TimeStamp = DateTime.Now
            }).ToArray();
        }

        private void Enqueue<T>(PriorityQueue<T> priorityQueue, Priority priority, params T[] items)
        {
            lock (priorityQueue.SyncRoot)
                foreach (var item in items)
                {
                    priorityQueue.Enqueue(item, priority);
                }
        }

        [TestCase]
        public void EnqueueAndDequeueTest()
        {
            var queue = new PriorityQueue<Item>();
            Item dequeuedItem = null;
            Item item1 = new Item() {Name = "TEST-HIGH", TimeStamp = DateTime.Now},
                item2 = new Item() {Name = "TEST-NORMAL", TimeStamp = DateTime.Now},
                item3 = new Item() {Name = "TEST-LOW", TimeStamp = DateTime.Now};

            queue.Enqueue(item1, Priority.High);
            queue.Enqueue(item2, Priority.Normal);
            queue.Enqueue(item3, Priority.Low);

            Assert.AreEqual(3, queue.Count, 0);

            dequeuedItem = queue.Dequeue();
            Assert.AreSame(item1, dequeuedItem);

            dequeuedItem = queue.Dequeue();
            Assert.AreSame(item2, dequeuedItem);

            dequeuedItem = queue.Dequeue();
            Assert.AreSame(item3, dequeuedItem);

            Assert.AreEqual(0, queue.Count, 0);
        }

        [TestCase]
        public void CollectionTest()
        {
            var queue = new PriorityQueue<Item>();
            var items = CreateItems(10);
            Enqueue(queue, Priority.Normal, items);
            Assert.AreEqual(10, queue.Count, 0);

            var collection = queue.GetAsICollection(Priority.Normal);
            collection.Add(new Item() {Name="COLLECTION", TimeStamp = DateTime.Now});
            Assert.AreEqual(11, queue.Count, 0);
        }
    }
}
