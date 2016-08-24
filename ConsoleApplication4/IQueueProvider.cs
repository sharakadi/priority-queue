using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public interface IQueueProvider<T>
    {
        Queue<T> GetQueue();
        object GetSyncRoot();
        void SetQueue(Queue<T> queue);
    }
}
