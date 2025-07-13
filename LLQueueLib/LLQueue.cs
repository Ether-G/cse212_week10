using System;
using System.Collections.Generic;

namespace LLQueueLib
{
    /// <summary>
    /// A generic queue implementation using LinkedList<T> as the underlying storage mechanism.
    /// This implementation provides O(1) enqueue, O(1) dequeue, O(1) peek, and O(n) contains operations.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the queue</typeparam>
    public class LLQueue<T>
    {
        private LinkedList<T> _items;

        /// <summary>
        /// Initializes a new instance of the LLQueue<T> class.
        /// </summary>
        public LLQueue()
        {
            _items = new LinkedList<T>();
        }

        /// <summary>
        /// Gets the current number of elements in the queue.
        /// Time Complexity: O(1)
        /// </summary>
        public int Size
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Adds the specified value to the back of the queue.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="item">The item to add to the queue</param>
        public void Enqueue(T item)
        {
            _items.AddLast(item);
        }

        /// <summary>
        /// Removes and returns the value at the front of the queue.
        /// Time Complexity: O(1)
        /// </summary>
        /// <returns>The item at the front of the queue</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty</exception>
        public T Dequeue()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Cannot dequeue from an empty queue.");
            }

            if (_items.First == null)
            {
                throw new InvalidOperationException("Queue is in an invalid state.");
            }

            T item = _items.First.Value;
            _items.RemoveFirst();
            return item;
        }

        /// <summary>
        /// Returns the value at the front of the queue without removing it.
        /// Time Complexity: O(1)
        /// </summary>
        /// <returns>The item at the front of the queue</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty</exception>
        public T Peek()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Cannot peek at an empty queue.");
            }

            if (_items.First == null)
            {
                throw new InvalidOperationException("Queue is in an invalid state.");
            }

            return _items.First.Value;
        }

        /// <summary>
        /// Determines whether the queue contains the specified value.
        /// Time Complexity: O(n)
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns>True if the item is found, false otherwise</returns>
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Removes all elements from the queue.
        /// Time Complexity: O(n)
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Returns a string representation of the queue.
        /// </summary>
        /// <returns>A string showing the queue type and current size</returns>
        public override string ToString()
        {
            return $"LLQueue<{typeof(T).Name}> [Size: {Size}]";
        }
    }
} 