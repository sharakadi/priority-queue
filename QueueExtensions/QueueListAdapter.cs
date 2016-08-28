using System.Collections;
using System.Collections.Generic;

namespace QueueExtensions
{
    public  class QueueListAdapter<T> : QueueAdapter<T>, IList<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            return GetQueueEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        public int Count { get { return ItemCount; }}
        public bool IsReadOnly { get { return false; }}
        public int IndexOf(T item)
        {
            return GetIndexOfFirstElement(item);
        }

        public void Insert(int index, T item)
        {
            InsertElementAt(index, item);
        }

        public void RemoveAt(int index)
        {
            RemoveElementAt(index);
        }

        public T this[int index]
        {
            get { return GetElementAt(index); }
            set { SetElementValueAt(index, value); }
        }

        public QueueListAdapter(IQueueContainer<T> queueContainer)
            : base(queueContainer)
        {
        }

        public new object SyncRoot
        {
            get { return base.SyncRoot; }
        }
    }
}
