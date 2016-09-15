# Queue Extensions
This repository contains specialized priority queue class and some extensions to .NET Framework Queue class.
* High-efficient, synchronized priority queue (PriorityQueue<T>). High, low and normal item priority avaliable;
* Collection and List adapters for System.Collections.Generic.Queue<T>;
* Unit tests.

# Priority Queue
The PriorityQueue<T> class has same methods signatures as System.Collections.Generic.Queue<T> class, but enqueued items have associated priority with them. When you call Enqueue, pass an item of type T and its priority (low, normal or high). When you dequeue item from priority queue, it will return items in specified order (by priority): high, normal and low.
It is also possible to convert priority queue to ICollection, ICollection<T> and IList<T>. Just use the respective extension methods. These wrappers are also syncronized.

# IQueueContainer<T> and DefaultQueueContainer<T>
The IQueueContainer<T> serves as a container for System.Collections.Generic.Queue<T> instance. Every other class in this repositoy uses container to manipulate queue stored inside. This allows adapters to access stored queue synchronously and change it contents. That it, when, for example, the item from queue is removed, a new queue is created and then it is stored in the same container. Make changes to queue is container according to these steps:
* Always lock container by getting it's SyncRoot -> IQueueContainer<T>.GetSyncRoot();
* Get queue -> IQueueContainer<T>.GetQueue();
* Make changes to queue, create a new queue if needed;
* Place queue object back to container -> IQueueContainer<T>.SetQueue();
* Do not forget to release SyncRoot.
This ensures that queue will always be syncronized. There is a common-purpuse DefaultQueueContainer<T> for you to use.

# Adapters for ICollection, ICollection<T> and IList<T>
There is also adapter for generic (and non-generic) collections and a list. They are usefull when it comes to treat System.Collections.Generic.Queue<T> as list or collection. You can add or remove items, get their indexes and remove items at specific index. While being synchronized inside, keep in mind that access to the adapter itself from different threads requires additional attention from programmer.

# QueueAdapter<T>
You can create your own adapter for System.Collections.Generic.Queue<T>. There is an abstact QueueAdapter<T> class with protected methods to manipulate queue stored inside the IQueueContainer<T>. All these methods are synchronized as well.

Sharkadi Andrey, 2016
