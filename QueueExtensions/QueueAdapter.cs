using System;
using System.Collections.Generic;
using System.Linq;

namespace QueueExtensions
{
    /// <summary>
    /// Абстрактный класс, предоставляющий доступ к очереди с возможностями ее модификации: вставку, удаление, замену элементов и т.д., в т.ч. по индексу
    /// </summary>
    /// <typeparam name="T">Тип элементов очереди</typeparam>
    public abstract class QueueAdapter<T>
    {
        private readonly IQueueContainer<T> _queueContainer;
        protected object SyncRoot { get { return _queueContainer.GetSyncRoot(); } }
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
                if (index < 0 || index > array.Length-1) throw new IndexOutOfRangeException();
                var newArray = new T[array.Length - 1];
                Array.Copy(array, 0, newArray, 0, index);
                Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
                Queue = new Queue<T>(newArray);
            }
        }

        protected void InsertElementAt(int index, T item)
        {
            lock (SyncRoot)
            {
                var array = Queue.ToArray();
                if (index < 0 || index > array.Length-1) throw new IndexOutOfRangeException();
                var newArray = new T[array.Length + 1];
                Array.Copy(array, 0, newArray, 0, index);
                Array.Copy(array, index, newArray, index + 1, array.Length - index);
                newArray[index] = item;
                Queue = new Queue<T>(newArray);                
            }
        }

        protected T GetElementAt(int index)
        {
            lock (SyncRoot)
            {
                var array = Queue.ToArray();
                if (index < 0 || index > array.Length-1) throw  new IndexOutOfRangeException();
                return array[index];
            }
        }

        protected void SetElementValueAt(int index, T value)
        {
            lock (SyncRoot)
            {
                var array = Queue.ToArray();
                if (index < 0 || index > array.Length-1) throw new IndexOutOfRangeException();
                array[index] = value;
                _queueContainer.SetQueue(new Queue<T>(array));
            }
        }

        protected int GetIndexOfFirstElement(T item)
        {
            lock (SyncRoot)
            {
                var array = Queue.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Equals(item)) return i;
                }
            }
            return -1;
        }
    }
}
