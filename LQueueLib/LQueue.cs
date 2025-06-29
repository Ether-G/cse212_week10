using System;
using System.Collections.Generic;

namespace LQueueLib
{
    // a generic queue implementation using list<t> as the underlying storage mechanism.
    // this implementation provides o(1) enqueue (amortized), o(n) dequeue, o(1) peek, and o(n) contains operations.
    public class LQueue<T>
    {
        private List<T> _items;

        // initializes a new instance of the lqueue<t> class.
        public LQueue()
        {
            _items = new List<T>();
        }

        // initializes a new instance of the lqueue<t> class with the specified initial capacity.
        public LQueue(int capacity)
        {
            _items = new List<T>(capacity);
        }

        // gets the current number of elements in the queue.
        // time complexity: o(1)
        public int Size
        {
            get { return _items.Count; }
        }

        // gets the total number of elements the queue can store before resizing occurs.
        // time complexity: o(1)
        public int Capacity
        {
            get { return _items.Capacity; }
        }

        // adds the specified value to the back of the queue.
        // time complexity: o(1) amortized, o(n) when resizing occurs
        public void Enqueue(T item)
        {
            _items.Add(item);
        }

        // removes and returns the value at the front of the queue.
        // time complexity: o(n) due to element shifting in list<t>
        // throws an exception if the queue is empty
        public T Dequeue()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("cannot dequeue from an empty queue.");
            }

            T item = _items[0];
            _items.RemoveAt(0);
            return item;
        }

        // returns the value at the front of the queue without removing it.
        // time complexity: o(1)
        // throws an exception if the queue is empty
        public T Peek()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("cannot peek at an empty queue.");
            }

            return _items[0];
        }

        // determines whether the queue contains the specified value.
        // time complexity: o(n)
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        // removes all elements from the queue.
        // time complexity: o(n)
        public void Clear()
        {
            _items.Clear();
        }

        // returns a string representation of the queue.
        public override string ToString()
        {
            return $"LQueue<{typeof(T).Name}> [Size: {Size}, Capacity: {Capacity}]";
        }
    }
} 