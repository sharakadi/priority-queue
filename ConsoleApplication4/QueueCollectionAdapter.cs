using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    internal class QueueCollectionAdapter<T> : QueueAdapter<T>, IEnumerable<T>, IEnumerable, ICollection, ICollection<T>
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
        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    _syncRoot = Interlocked.CompareExchange<Object>(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }
        public bool IsSynchronized { get { return true; } }

        public QueueCollectionAdapter(IQueueContainer<T> queueContainer)
            : base(queueContainer)
        {
        }
    }
}
