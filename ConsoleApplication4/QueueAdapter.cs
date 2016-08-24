using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public abstract class QueueAdapter<T>
    {
        private readonly IQueueContainer<T> _queueContainer;
        private object SyncRoot { get { return _queueContainer.GetSyncRoot(); } }
        private Queue<T> Queue { get { return _queueContainer.GetQueue(); } set { _queueContainer.SetQueue(value); } }

        public QueueAdapter(IQueueContainer<T> queueContainer)
        {
            _queueContainer = queueContainer;
        }

        protected IEnumerator<T> GetQueueEnumerator()
        {
            lock (SyncRoot)
            {
                return Queue.ToArray().AsEnumerable().GetEnumerator();
            }
        }

        protected void CopyToArray(Array array, int index)
        {
            lock (SyncRoot)
                Queue.CopyTo((T[])array, index);
        }

        protected void Enqueue(T item)
        {
            lock (SyncRoot)
                Queue.Enqueue(item);
        }

        protected void ClearQueue()
        {
            lock (SyncRoot) Queue.Clear();
        }

        protected bool ContainsItem(T item)
        {
            lock (SyncRoot) return Queue.Contains(item);
        }

        protected void CopyToArray(T[] array, int arrayIndex)
        {
            lock (SyncRoot) Queue.CopyTo(array, arrayIndex);
        }

        protected int RemoveAllOccuriencies(T item)
        {
            int count = 0;
            lock (SyncRoot)
            {
                var arr = Queue.Where(x => !x.Equals(item)).ToArray();
                if (arr.Length < Queue.Count)
                {
                    Queue = new Queue<T>(arr);
                    count = arr.Length;
                }
            }
            return count;
        }

        protected int ItemCount { get { lock (SyncRoot) return Queue.Count; } }

        protected void RemoveElementAt(int index)
        {
            lock (SyncRoot)
            {
                var array = Queue.ToArray();
                if (index < 0 || index > array.Length) throw new IndexOutOfRangeException();
                var newArray = new T[array.Length - 1];
                Array.Copy(array, 0, newArray, 0, index + 1);
                Array.Copy(array, index, newArray, index + 1, array.Length - index);
                Queue = new Queue<T>(newArray);
            }
        }

        protected T GetElementAt(int index)
        {
            lock (SyncRoot)
            {
                var array = Queue.ToArray();
                if (index < 0 || index > array.Length) throw  new IndexOutOfRangeException();
                return array[index];
            }
        }

        protected void SetElementValueAt(int index, T value)
        {
            lock (SyncRoot)
            {
                var array = Queue.ToArray();
                if (index < 0 || index > array.Length) throw new IndexOutOfRangeException();
                array[index] = value;
                _queueContainer.SetQueue(new Queue<T>(array));
            }
        }
    }
}
