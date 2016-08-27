using System;
using System.Collections;
using System.Collections.Generic;

namespace QueueExtensions
{
    public class QueueCollectionAdapter<T> : QueueAdapter<T>, IEnumerable<T>, IEnumerable, ICollection, ICollection<T>
    {
        private object _syncRoot;

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetQueueEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            CopyToArray(array, index);
        }

        public void Add(T item)
        {
            Enqueue(item);
        }

        public void Clear()
        {
            ClearQueue();
        }

        public bool Contains(T item)
        {
            return ContainsItem(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyToArray(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return RemoveAllOccuriencies(item) > 0;
        }

        int ICollection<T>.Count { get { return ItemCount; } }
        public bool IsReadOnly { get { return false; } }
        int ICollection.Count { get { return ItemCount; } }
        public new object SyncRoot {
            get { return base.SyncRoot; }
        }
        public bool IsSynchronized { get { return true; } }

        public QueueCollectionAdapter(IQueueContainer<T> queueContainer)
            : base(queueContainer)
        {
            //SyncRoot = new object();
        }
    }
}
