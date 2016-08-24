using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace ConsoleApplication4
{
    public class PriorityQueue<T> : IEnumerable<T>, IEnumerable, ICollection, ICollection<T>
    {
        private Queue<T> _lowPriority, _normalPriority, _highPriority;
        private readonly object _lowRoot = new object(), _normalRoot = new object(), _highRoot = new object();
        private object _syncRoot;

        public Priority CollectionAddDefaultPriority { get; set; }

        private void SetSyncRoots()
        {
            //_highRoot = ((ICollection)_highPriority).SyncRoot;
            //_normalRoot = ((ICollection)_normalPriority).SyncRoot;
            //_lowRoot = ((ICollection) _lowPriority).SyncRoot;
        }

        public static PriorityQueue<T> FromCollection(ICollection collection)
        {
            T[] arr = new T[collection.Count];
            collection.CopyTo(arr, 0);
            var q = new PriorityQueue<T>(arr.Length * 3);
            q._normalPriority = new Queue<T>(arr);
            q.SetSyncRoots();
            return q;
        }

        public static PriorityQueue<T> FromCollections(ICollection normalPriorityItems = null,
            ICollection highPriorityItems = null, ICollection lowPriorityItems = null)
        {
            T[] highArr = new T[0];
            T[] normalArr =  new T[0];
            T[] lowArr = new T[0];
            if (highPriorityItems != null) highPriorityItems.CopyTo(highArr, 0);
            if (normalPriorityItems != null) normalPriorityItems.CopyTo(normalArr, 0);
            if (lowPriorityItems != null) lowPriorityItems.CopyTo(lowArr, 0);
            var q = new PriorityQueue<T>(highArr.Length + normalArr.Length + lowArr.Length)
            {
                _lowPriority = new Queue<T>(lowArr),
                _highPriority = new Queue<T>(highArr),
                _normalPriority = new Queue<T>(normalArr)
            };
            q.SetSyncRoots();
            return q;
        }

        public static PriorityQueue<T> FromQueues(Queue<T> normalPriorityQueue = null,
            Queue<T> highPriorityQueue = null, Queue<T> lowPriorityQueue = null)
        {
            var q = new PriorityQueue<T>();
            if (highPriorityQueue != null) q._highPriority = highPriorityQueue;
            if (normalPriorityQueue != null) q._normalPriority = normalPriorityQueue;
            if (lowPriorityQueue != null) q._lowPriority = lowPriorityQueue;
            q.SetSyncRoots();
            return q;
        }

        public PriorityQueue()
        {
            _lowPriority = new Queue<T>();
            _normalPriority = new Queue<T>();
            _highPriority = new Queue<T>();
            SetSyncRoots();
        }

        public PriorityQueue(int totalCapacity)
        {
            int oneThird = totalCapacity/3 + 1;
            _lowPriority = new Queue<T>(oneThird);
            _normalPriority = new Queue<T>(oneThird);
            _highPriority = new Queue<T>(oneThird);
            SetSyncRoots();
        }

        public void TrimExcess()
        {
            lock(_highRoot) _highPriority.TrimExcess();
            lock (_normalRoot) _normalPriority.TrimExcess();
            lock (_lowRoot) _lowPriority.TrimExcess();
        }

        public void Enqueue(T item, Priority itemPriority)
        {
            if (itemPriority == Priority.Normal)
            {
                lock (_normalRoot)
                {
                    _normalPriority.Enqueue(item);
                }
            }
            if (itemPriority == Priority.Low)
            {
                lock (_lowRoot)
                {
                    _lowPriority.Enqueue(item);
                }
            }
            if (itemPriority == Priority.High)
            {
                lock (_highRoot)
                {
                    _highPriority.Enqueue(item);
                }
            }
        }

        private bool TryDequeueInternal(out T item)
        {
            item = default(T);
            lock (_highRoot)
            {
                try
                {
                    if (_highPriority.Count > 0)
                    {
                        item = _highPriority.Dequeue();
                        return true;
                    }
                }
                catch (InvalidOperationException ex)
                {   
                }
            }
            lock (_normalRoot)
            {
                try
                {
                    if (_normalPriority.Count > 0)
                    {
                        item = _normalPriority.Dequeue();
                        return true;
                    }
                }
                catch (InvalidOperationException ex)
                {
                }
            }
            lock (_lowRoot)
            {
                try
                {
                    if (_lowPriority.Count > 0)
                    {
                        item = _lowPriority.Dequeue();
                        return true;
                    }
                }
                catch (InvalidOperationException ex)
                {
                }
            }
            return false;
        }

        public T Dequeue()
        {
            T item;
            if (TryDequeueInternal(out item))
            {
                return item;
            }
            throw new InvalidOperationException();
        }

        public bool TryDequeue(out T item)
        {
            return TryDequeueInternal(out item);
        }

        public T Peek()
        {
            T item;
            lock (_highRoot)
            {
                item = _highPriority.Peek();
                if (item != null) return item;
            }
            lock (_normalRoot)
            {
                item = _normalPriority.Peek();
                if (item != null) return item;
            }
            lock (_lowRoot)
            {
                item = _lowPriority.Peek();
                if (item != null) return item;
            }
            return default(T);
        }

        public void Add(T item)
        {
            Enqueue(item, CollectionAddDefaultPriority);
        }

        public void Clear()
        {
            lock (_highRoot)
            {
                _highPriority.Clear();
            }
            lock (_normalRoot)
            {
                _normalPriority.Clear();
            }
            lock (_lowRoot)
            {
                _lowPriority.Clear();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_highRoot)
                lock (_normalRoot)
                    lock (_lowRoot)
                    {
                        return _highPriority.ToArray()
                            .Union(_normalPriority.ToArray())
                            .Union(_lowPriority.ToArray())
                            .ToList().GetEnumerator();
                    }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            lock (_highRoot)
                lock (_normalRoot)
                    lock (_lowRoot)
                    {
                        var arr =
                            _highPriority.ToArray()
                                .Union(_normalPriority.ToArray())
                                .Union(_lowPriority.ToArray())
                                .ToArray();
                        Array.Copy(arr, 0, array, index, arr.Length);
                    }
        }

        private bool GetContains(T item)
        {
            bool contains = false;
            lock (_highRoot)
            {
                if (_highPriority.Contains(item)) contains = true;
            }
            if (!contains)
                lock (_normalRoot)
                {
                    if (_normalPriority.Contains(item)) contains = true;
                }
            if (!contains)
                lock (_lowRoot)
                {
                    if (_lowPriority.Contains(item)) contains = true;
                }
            return contains;
        }

        public bool Contains(T item)
        {
            var contains = GetContains(item);
            return contains;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array as Array, arrayIndex);
        }

        public bool Remove(T item)
        {
            bool remove = false;
            lock (_highRoot)
            {
                var arr = _highPriority.Where(x => !x.Equals(item)).ToArray();
                if (arr.Length < _highPriority.Count)
                {
                    _highPriority = new Queue<T>(arr);
                    remove = true;
                }
            }
            lock (_normalRoot)
            {
                var arr = _normalPriority.Where(x => !x.Equals(item)).ToArray();
                if (arr.Length < _normalPriority.Count)
                {
                    _normalPriority = new Queue<T>(arr);
                    remove = true;
                }
            }
            lock (_lowRoot)
            {
                var arr = _lowPriority.Where(x => !x.Equals(item)).ToArray();
                if (arr.Length < _lowPriority.Count)
                {
                    _lowPriority = new Queue<T>(arr);
                    remove = true;
                }
            }
            return remove;
        }

        private int GetCount()
        {
            int a, b, c;
            lock (_highRoot)
                lock (_normalRoot)
                    lock (_lowRoot)
                    {
                        a = _highPriority.Count;
                        b = _normalPriority.Count;
                        c = _lowPriority.Count;
                    }
            return a + b + c;
        }

        public int Count
        {
            get
            {
                var count = GetCount();
                return count;
            }
        }

        public bool IsReadOnly { get; private set; }

        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    _syncRoot = Interlocked.CompareExchange<Object>(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        internal Queue<T> GetQueue(Priority priority)
        {
            if (priority == Priority.High) return _highPriority;
            if (priority == Priority.Normal) return _normalPriority;
            if (priority == Priority.Low) return _lowPriority;
            throw new Exception();
        }

        internal void SetQueue(Priority priority, Queue<T> queue)
        {
            if (priority == Priority.High) _highPriority = queue;
            if (priority == Priority.Normal) _normalPriority = queue;
            if (priority == Priority.Low) _lowPriority = queue;
            throw new Exception();            
        } 

        internal object GetQueueRoot(Priority priority)
        {
            if (priority == Priority.High) return _highRoot;
            if (priority == Priority.Normal) return _normalRoot;
            if (priority == Priority.Low) return _lowRoot;
            throw new Exception();
        }
    }
}