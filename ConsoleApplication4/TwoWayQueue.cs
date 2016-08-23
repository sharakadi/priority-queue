using System;
using System.Linq;

namespace ConsoleApplication4
{
    class TwoWayQueue<T>
    {
        private class TwoWayQueueItem : IDisposable 
        {
            public TwoWayQueueItem()
            {
                Id = Guid.NewGuid();
            }

            public Guid Id { get; private set; }
            public bool Dequeued { get; set; }
            public object Item { get; set; }
            public void Dispose()
            {
                var disp = Item as IDisposable;
                if (disp == null) Item = null;
                else
                {
                    disp.Dispose();
                }
            }
        }

        private TwoWayQueueItem[] _array;

        public TwoWayQueue(int size)
        {
            _array = new TwoWayQueueItem[size];
        }

        public TwoWayQueue()
        {
            _array = new TwoWayQueueItem[100];
        }

        public void EnqueueTop(T value)
        {
            lock (_array.SyncRoot)
            {
                if (_array[0] == null || _array[0].Dequeued)
                {
                    if (_array[0] != null) _array[0].Dispose();
                    _array[0] = new TwoWayQueueItem()
                    {
                        Item = value
                    };
                }
            }
        }

        public void EnqueueBottom(T value)
        {
            lock (_array.SyncRoot)
            {
                if (_array[_array.Length - 1] == null || _array[_array.Length - 1].Dequeued)
                {
                    if (_array[_array.Length - 1] != null) _array[_array.Length - 1].Dispose();
                    _array[_array.Length - 1] = new TwoWayQueueItem()
                    {
                        Item = value
                    };
                }
            }
        }

        private void FreeSlots(int count, bool right)
        {
            var avaliableCount = _array.Count(x => x == null || x.Dequeued);
            if (avaliableCount < Math.Abs(count)) throw  new Exception();
            int totalFreed = 0;

            int direction = right ? -1 : 0;
            int start = right ? _array.Length - 1 : 0;
            int end = right ? 0 : _array.Length - 1;
            while (totalFreed < count)
            {
                for (int i = start; i != end; i += direction)
                {
                    if (_array[i] != null)
                    {
                        
                    }
                }
            }
        }

        private void Resize(int length)
        {
            if (length != _array.Length)
            {
                var tmp = new TwoWayQueueItem[length];
                Array.Copy(_array, tmp, length > _array.Length ? _array.Length : length);
                _array = tmp;
            }
        }
    }
}