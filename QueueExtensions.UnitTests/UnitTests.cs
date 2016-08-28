using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private void AssertContainsSubcollection(ICollection collection, ICollection subcollection)
        {
            foreach (var item in subcollection)
            {
                Assert.Contains(item, collection);
            }
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

            lock (queue.SyncRoot)
            {
                Assert.AreEqual(true, Monitor.IsEntered(queue.SyncRoot));

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
            Assert.AreEqual(false, Monitor.IsEntered(queue.SyncRoot));
        }

        [TestCase]
        public void QueueAsCollectionTest()
        {
            var queue = new PriorityQueue<Item>();
            var items = CreateItems(10);
            Enqueue(queue, Priority.Normal, items);
            Assert.AreEqual(10, queue.Count, 0);

            queue.CollectionAddDefaultPriority = Priority.High;
            var collection = (ICollection<Item>) queue;
            Assert.AreSame(queue.SyncRoot, ((ICollection)queue).SyncRoot);

            lock (((ICollection)queue).SyncRoot)
            {
                Assert.AreEqual(true, Monitor.IsEntered(((ICollection)queue).SyncRoot));
                Assert.AreEqual(true, Monitor.IsEntered(queue.SyncRoot));

                var newItem = new Item() {Name = "COLLECTION", TimeStamp = DateTime.Now};
                collection.Add(newItem);
                Assert.AreEqual(11, queue.Count, 0);

                Item dequeuedItem = null;
                dequeuedItem = queue.Dequeue();
                Assert.AreSame(newItem, dequeuedItem);
                Assert.AreEqual(10, collection.Count, 0);
            }

            Assert.AreEqual(false, Monitor.IsEntered(((ICollection)queue).SyncRoot));
            Assert.AreEqual(false, Monitor.IsEntered(queue.SyncRoot));
        }

        [TestCase]
        public void QueueContainerTest()
        {
            var queue = new Queue<Item>();
            IQueueContainer<Item> container = new DefaultQueueContainer<Item>(queue);
          
            Assert.AreSame(queue, container.GetQueue());
            Assert.AreNotSame(((ICollection) queue).SyncRoot, container.GetSyncRoot());

            lock (container.GetSyncRoot())
            {
                Assert.AreEqual(true, Monitor.IsEntered(container.GetSyncRoot()));
            }
            Assert.AreEqual(false, Monitor.IsEntered(container.GetSyncRoot()));

            container.SetQueue(new Queue<Item>());
            Assert.AreNotSame(queue, container.GetQueue());
        }

        [TestCase]
        public void CollectionAdapterTest()
        {
            var queue = new Queue<Item>();
            Item[] items = CreateItems(3);

            IQueueContainer<Item> container = new DefaultQueueContainer<Item>(queue);
            var collection = new QueueCollectionAdapter<Item>(container);

            Assert.NotNull(collection as ICollection);
            Assert.NotNull(collection as ICollection<Item>);

            var iCollection = (ICollection<Item>) collection;

            lock (((ICollection)iCollection).SyncRoot)
            {
                Assert.AreEqual(true, Monitor.IsEntered(((ICollection)iCollection).SyncRoot));
                Assert.AreEqual(container.GetSyncRoot(), ((ICollection)iCollection).SyncRoot);
                
                iCollection.Add(items[0]);
                iCollection.Add(items[1]);
                iCollection.Add(items[2]);
                Assert.AreEqual(3, iCollection.Count, 0);
                Assert.AreEqual(3, container.GetQueue().Count, 0);
                Assert.AreEqual(queue.ToArray(), iCollection.ToArray());

                Assert.AreEqual(true, iCollection.Contains(items[0]));
                Assert.AreEqual(true, iCollection.Contains(items[1]));
                Assert.AreEqual(true, iCollection.Contains(items[2]));

                queue.Dequeue();
                queue.Dequeue();
                queue.Dequeue();

                Assert.AreEqual(0, iCollection.Count, 0);
            }

            Assert.AreEqual(false, Monitor.IsEntered(((ICollection)iCollection).SyncRoot));
        }

        [TestCase]
        public void ListAdapterTest()
        {
            var queue = new Queue<Item>();
            Item[] items = CreateItems(3);

            IQueueContainer<Item> container = new DefaultQueueContainer<Item>(queue);
            var list = new QueueListAdapter<Item>(container);

            Assert.NotNull(list as IList<Item>);

            var iList = (IList<Item>) list;
            list.Add(items[0]);
            list.Add(items[1]);
            list.Add(items[2]);
            Assert.AreEqual(3, list.Count, 0);
            Assert.AreEqual(list.Count, queue.Count, 0);

            Assert.AreEqual(queue.ToArray(), list.ToArray());
            Assert.AreSame(items[0], iList[0]);
            Assert.AreSame(items[1], iList[1]);
            Assert.AreSame(items[2], iList[2]);

            items[1].Name = "EDIT";

            Assert.AreSame(items[1], iList[1]);
            Assert.AreEqual(queue.ToArray(), list.ToArray());

            iList.RemoveAt(1);
            queue = container.GetQueue();
            Assert.AreEqual(2, iList.Count, 0);
            Assert.AreEqual(list.Count, queue.Count);
            Assert.AreEqual(queue.ToArray(), list.ToArray());

            iList.Insert(1, items[1]);
            queue = container.GetQueue();
            Assert.AreEqual(3, iList.Count, 0);
            Assert.AreEqual(list.Count, queue.Count);
            Assert.AreEqual(queue.ToArray(), list.ToArray());
            Assert.AreSame(items[0], iList[0]);
            Assert.AreSame(items[1], iList[1]);
            Assert.AreSame(items[2], iList[2]);
        }

        [TestCase]
        public void PriorityQueueParallelTest()
        {
            var queue = new PriorityQueue<Item>();
            var high = CreateItems(100000);
            var normal = CreateItems(100000);
            var low = CreateItems(100000);
            var total = high.Length + normal.Length + low.Length;
            var sw = Stopwatch.StartNew();

            var highTask = Task.Run(() =>
            {
                Console.WriteLine("highTask ID#{0}", Thread.CurrentThread.ManagedThreadId);
                foreach (var i in Enumerable.Range(0, high.Length))
                {
                    queue.Enqueue(high[i], Priority.High);
                }
            });
            var removeTask = Task.Run(() =>
            {
                Console.WriteLine("removeTask ID#{0}", Thread.CurrentThread.ManagedThreadId);
                while (queue.Count < 100) { }
                var list = queue.GetAsIList(Priority.Normal);
                for (int j = 0; j < normal.Length; j++)
                {
                    lock (((QueueListAdapter<Item>)list).SyncRoot)
                    {
                        if (j > queue.Count || queue.Count == 0) continue;
                        if (j%10 == 0) list.RemoveAt(j);
                    }
                }
            });
            var normalTask = Task.Run(() =>
            {
                Console.WriteLine("normalTask ID#{0}", Thread.CurrentThread.ManagedThreadId);
                var collection = queue.GetAsICollection(Priority.Normal);
                foreach (var i in Enumerable.Range(0, normal.Length))
                {
                    collection.Add(normal[i]);
                }
            });
            var lowTask = Task.Run(() =>
            {
                Console.WriteLine("lowTask ID#{0}", Thread.CurrentThread.ManagedThreadId);
                var list = queue.GetAsIList(Priority.Low);
                foreach (var i in Enumerable.Range(0, low.Length))
                {
                    list.Add(low[i]);
                }
            });

            List<Item> dequeued = new List<Item>(total);
            while (!highTask.IsCompleted || !normalTask.IsCompleted || !lowTask.IsCompleted || !removeTask.IsCompleted ||
                   queue.Count > 0)
            {
                if (highTask.Exception != null) throw highTask.Exception;
                if (normalTask.Exception != null) throw normalTask.Exception;
                if (lowTask.Exception != null) throw lowTask.Exception;
                if (queue.Count > 0)
                    dequeued.Add(queue.Dequeue());
            }

            sw.Stop();
            Console.WriteLine("Time elapsed: {0} ms.", sw.ElapsedMilliseconds);

            Assert.AreEqual(0, queue.Count, 0);
            Assert.AreNotEqual(total, dequeued.Count);
            AssertContainsSubcollection(dequeued, high);
            AssertContainsSubcollection(dequeued, normal);
            AssertContainsSubcollection(dequeued, low);
        }

        [TestCase]
        public void AdaptersParallelTest()
        {
            //Assert.Inconclusive();
            var queue = new Queue<Item>();

            var container = new DefaultQueueContainer<Item>(queue);


        }
    }
}
