using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    internal class QueueCollectionAdapter<T> : IEnumerable<T>, IEnumerable, ICollection, ICollection<T>
    {
        private readonly PriorityQueueProvider<T> _provider;
        private object _syncRoot;

        private object QueueRoot { get { return _provider.GetSyncRoot(); } }

        private Queue<T> Queue 
        {
            get { return _provider.GetQueue(); }
        }

        public QueueCollectionAdapter(PriorityQueueProvider<T> provider)
        {
            _provider = provider;
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (QueueRoot)
                return Queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            lock (QueueRoot)
               Queue.CopyTo((T[])array, index);
        }

        public void Add(T item)
        {
            lock (QueueRoot)
                Queue.Enqueue(item);
        }

        public void Clear()
        {
            lock (QueueRoot) Queue.Clear();
        }

        public bool Contains(T item)
        {
            lock (QueueRoot) return Queue.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (QueueRoot) Queue.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            bool remove = false;
            lock (QueueRoot)
            {
                var arr = Queue.Where(x => !x.Equals(item)).ToArray();
                if (arr.Length < Queue.Count)
                {
                    _provider.SetQueue(new Queue<T>(arr));
                    remove = true;
                }
            }
            return remove;
        }

        int ICollection<T>.Count { get { lock (QueueRoot) return Queue.Count; } }
        public bool IsReadOnly { get { return false; } }
        int ICollection.Count { get { lock (QueueRoot) return Queue.Count; } }
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
    }
}
