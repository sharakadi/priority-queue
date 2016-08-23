//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleApplication4
//{
//    internal class QueueCollectionAdapter<T> : IEnumerable<T>, IEnumerable, ICollection, ICollection<T>
//    {
//        private readonly Queue<T> _queue;
//        private readonly object _syncRoot;

//        public QueueCollectionAdapter(Queue<T> queue)
//        {
//            _queue = queue;
//            _syncRoot = ((ICollection) _queue).SyncRoot;
//        }

//        public IEnumerator<T> GetEnumerator()
//        {
//            lock (_syncRoot)
//            return _queue.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        public void CopyTo(Array array, int index)
//        {
//            lock (_syncRoot)
//            _queue.CopyTo((T[]) array, index);
//        }

//        public void Add(T item)
//        {
//            lock (_syncRoot)     
//                _queue.Enqueue(item);
//        }

//        public void Clear()
//        {
//            lock (_syncRoot)  _queue.Clear();
//        }

//        public bool Contains(T item)
//        {
//            lock (_syncRoot) return _queue.Contains(item);
//        }

//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            lock (_syncRoot) _queue.CopyTo(array, arrayIndex);
//        }

//        public bool Remove(T item)
//        {
//            lock (_syncRoot)
//            {
//                var arr = 
//            }
//        }

//        int ICollection<T>.Count { get; private set; }
//        public bool IsReadOnly { get; private set; }
//        int ICollection.Count { get; private set; }
//        public object SyncRoot { get; private set; }
//        public bool IsSynchronized { get; private set; }
//    }
//}
