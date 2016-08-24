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
        private IQueueProvider<T> _queueProvider;
        private object SyncRoot { get { return _queueProvider.GetSyncRoot(); } }
        private Queue<T> Queue { get { return _queueProvider.GetQueue(); } set { _queueProvider.SetQueue(value); } }

        public QueueAdapter(IQueueProvider<T> queueProvider)
        {
            _queueProvider = queueProvider;
        }

        protected virtual IEnumerator<T> GetEnumerator()
        {
            lock (SyncRoot)
            {
                return Queue.ToArray().AsEnumerable().GetEnumerator();
            }
        }
    }
}
