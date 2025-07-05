using System;
using System.Collections.Generic;

namespace AQueueLib
{
    // a generic queue implementation using fixed size array as the storage mechanism.
    // this implementation provides o(1) enqueue, o(1) dequeue, o(1) peek, and o(n) contains operations.
    public class AQueue<T>
    {
        private T[] _items;
        private int _front;  // index of the front element
        private int _rear;   // index of the next available position
        private int _size;   // number of elements currently in the queue

        public AQueue(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
            }

            _items = new T[capacity];
            _front = 0;
            _rear = 0;
            _size = 0;
        }

        // number of elements in the queue
        public int Size
        {
            get { return _size; }
        }

        // total capacity of the queue
        public int Capacity
        {
            get { return _items.Length; }
        }

        // whether the queue is full
        public bool IsFull
        {
            get { return _size == _items.Length; }
        }

        // whether the queue is empty
        public bool IsEmpty
        {
            get { return _size == 0; }
        }

        // add item to the back of the queue
        public void Enqueue(T item)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Cannot enqueue to a full queue.");
            }

            _items[_rear] = item;
            _rear = (_rear + 1) % _items.Length;
            _size++;
        }

        // remove and return the front item
        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot dequeue from an empty queue.");
            }

            T item = _items[_front];
            _items[_front] = default(T)!;
            _front = (_front + 1) % _items.Length;
            _size--;
            return item;
        }

        // return the front item without removing it
        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot peek at an empty queue.");
            }

            return _items[_front];
        }

        // check if item exists in the queue
        public bool Contains(T item)
        {
            if (IsEmpty)
            {
                return false;
            }

            int current = _front;
            for (int i = 0; i < _size; i++)
            {
                if (EqualityComparer<T>.Default.Equals(_items[current], item))
                {
                    return true;
                }
                current = (current + 1) % _items.Length;
            }
            return false;
        }

        // remove all elements from the queue
        public void Clear()
        {
            if (!IsEmpty)
            {
                int current = _front;
                for (int i = 0; i < _size; i++)
                {
                    _items[current] = default(T)!;
                    current = (current + 1) % _items.Length;
                }
            }
            _front = 0;
            _rear = 0;
            _size = 0;
        }

        public override string ToString()
        {
            return $"AQueue<{typeof(T).Name}> [Size: {Size}, Capacity: {Capacity}, Front: {_front}, Rear: {_rear}]";
        }
    }
} 