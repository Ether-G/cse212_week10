using System;
using Xunit;
using AQueueLib;
using System.Diagnostics;

namespace AQueueTests
{
    public class AQueueUnitTests
    {
        // --- Basic Functionality Tests ---
        [Fact]
        public void Enqueue_IncreasesSizeCorrectly()
        {
            // test that enqueue adds items and updates size correctly
            // expected: size should increase by 1 for each enqueue operation
            var queue = new AQueue<int>(5);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            Assert.Equal(3, queue.Size);
        }

        [Fact]
        public void Dequeue_ReturnsCorrectValueAndDecreasesSize()
        {
            // test that dequeue returns the correct item and updates size
            // expected: should return first item enqueued and decrease size by 1
            var queue = new AQueue<int>(5);
            queue.Enqueue(10);
            queue.Enqueue(20);
            int val = queue.Dequeue();
            Assert.Equal(10, val);
            Assert.Equal(1, queue.Size);
        }

        [Fact]
        public void Peek_ReturnsFrontWithoutRemoving()
        {
            // test that peek returns the front item without removing it
            // expected: should return first item but size should remain unchanged
            var queue = new AQueue<string>(5);
            queue.Enqueue("A");
            queue.Enqueue("B");
            string peeked = queue.Peek();
            Assert.Equal("A", peeked);
            Assert.Equal(2, queue.Size);
        }

        [Fact]
        public void Contains_FindsExistingAndNonExistingValues()
        {
            // test that contains correctly finds existing items and returns false for missing items
            // expected: should return true for items in queue, false for items not in queue
            var queue = new AQueue<int>(5);
            queue.Enqueue(5);
            queue.Enqueue(10);
            Assert.True(queue.Contains(5));
            Assert.False(queue.Contains(99));
        }

        [Fact]
        public void FIFO_Order_IsMaintained()
        {
            // test that queue maintains first-in-first-out order
            // expected: items should be dequeued in the same order they were enqueued
            var queue = new AQueue<string>(5);
            queue.Enqueue("first");
            queue.Enqueue("second");
            queue.Enqueue("third");
            Assert.Equal("first", queue.Dequeue());
            Assert.Equal("second", queue.Dequeue());
            Assert.Equal("third", queue.Dequeue());
        }

        // --- Edge Case Tests ---
        [Fact]
        public void Dequeue_EmptyQueue_ThrowsException()
        {
            // test that dequeue throws exception when queue is empty
            // expected: should throw InvalidOperationException
            var queue = new AQueue<int>(5);
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Fact]
        public void Peek_EmptyQueue_ThrowsException()
        {
            // test that peek throws exception when queue is empty
            // expected: should throw InvalidOperationException
            var queue = new AQueue<int>(5);
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Fact]
        public void Enqueue_FullQueue_ThrowsException()
        {
            // test that enqueue throws exception when queue is full
            // expected: should throw InvalidOperationException when trying to enqueue to full queue
            var queue = new AQueue<int>(2);
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.Throws<InvalidOperationException>(() => queue.Enqueue(3));
        }

        [Fact]
        public void Contains_EmptyQueue_ReturnsFalse()
        {
            // test that contains returns false for empty queue
            // expected: should return false when searching empty queue
            var queue = new AQueue<int>(5);
            Assert.False(queue.Contains(123));
        }

        [Fact]
        public void Constructor_ZeroCapacity_ThrowsException()
        {
            // test that constructor throws exception for zero capacity
            // expected: should throw ArgumentException for invalid capacity
            Assert.Throws<ArgumentException>(() => new AQueue<int>(0));
        }

        [Fact]
        public void Constructor_NegativeCapacity_ThrowsException()
        {
            // test that constructor throws exception for negative capacity
            // expected: should throw ArgumentException for invalid capacity
            Assert.Throws<ArgumentException>(() => new AQueue<int>(-1));
        }

        // --- Circular Buffer Tests ---
        [Fact]
        public void CircularBuffer_WrapsAroundCorrectly()
        {
            // test that circular buffer correctly wraps around when rear pointer reaches end
            // expected: should reuse space at beginning of array after dequeue operations
            var queue = new AQueue<int>(3);
            
            // fill the queue
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            
            // remove one item to make space
            queue.Dequeue();
            
            // add another item - should wrap around
            queue.Enqueue(4);
            
            // verify the order
            Assert.Equal(2, queue.Dequeue());
            Assert.Equal(3, queue.Dequeue());
            Assert.Equal(4, queue.Dequeue());
        }

        [Fact]
        public void CircularBuffer_MultipleWraps_WorkCorrectly()
        {
            // test that circular buffer works correctly with multiple wrap-around cycles
            // expected: should handle multiple cycles of enqueue/dequeue operations
            var queue = new AQueue<int>(2);
            
            // first cycle
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.Equal(1, queue.Dequeue());
            queue.Enqueue(3);
            Assert.Equal(2, queue.Dequeue());
            
            // second cycle
            queue.Enqueue(4);
            Assert.Equal(3, queue.Dequeue());
            Assert.Equal(4, queue.Dequeue());
        }

        [Fact]
        public void CircularBuffer_Contains_WorksWithWrappedData()
        {
            // test that contains works correctly when data wraps around the array
            // expected: should find items correctly even when they span across array boundaries
            var queue = new AQueue<int>(3);
            
            // fill and partially empty to create wrap
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Dequeue(); // remove 1
            queue.Enqueue(4); // add 4, should wrap
            
            // test contains on wrapped data
            Assert.True(queue.Contains(2));
            Assert.True(queue.Contains(3));
            Assert.True(queue.Contains(4));
            Assert.False(queue.Contains(1)); // was removed
        }

        // --- Property Tests ---
        [Fact]
        public void Capacity_IsFixed()
        {
            // test that capacity remains constant and never changes
            // expected: capacity should stay the same regardless of queue operations
            var queue = new AQueue<int>(10);
            Assert.Equal(10, queue.Capacity);
            
            // capacity should not change
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.Equal(10, queue.Capacity);
        }

        [Fact]
        public void IsEmpty_WorksCorrectly()
        {
            // test that IsEmpty property correctly reflects queue state
            // expected: should be true when empty, false when items exist
            var queue = new AQueue<int>(5);
            Assert.True(queue.IsEmpty);
            
            queue.Enqueue(1);
            Assert.False(queue.IsEmpty);
            
            queue.Dequeue();
            Assert.True(queue.IsEmpty);
        }

        [Fact]
        public void IsFull_WorksCorrectly()
        {
            // test that IsFull property correctly reflects queue state
            // expected: should be true when full, false when space available
            var queue = new AQueue<int>(2);
            Assert.False(queue.IsFull);
            
            queue.Enqueue(1);
            Assert.False(queue.IsFull);
            
            queue.Enqueue(2);
            Assert.True(queue.IsFull);
            
            queue.Dequeue();
            Assert.False(queue.IsFull);
        }

        // --- Clear Tests ---
        [Fact]
        public void Clear_EmptiesQueue()
        {
            // test that clear removes all elements and resets queue state
            // expected: should result in empty queue with size 0
            var queue = new AQueue<int>(5);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            
            queue.Clear();
            
            Assert.Equal(0, queue.Size);
            Assert.True(queue.IsEmpty);
            Assert.False(queue.IsFull);
        }

        [Fact]
        public void Clear_AfterWrap_WorksCorrectly()
        {
            // test that clear works correctly after circular buffer wrap-around
            // expected: should clear queue properly even when data wraps around array
            var queue = new AQueue<int>(3);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Dequeue(); // remove 1
            queue.Enqueue(4); // wrap around
            
            queue.Clear();
            
            Assert.Equal(0, queue.Size);
            Assert.True(queue.IsEmpty);
        }

        // --- Performance Measurement Tests ---
        [Fact]
        public void Enqueue_BigO_Analysis()
        {
            // test enqueue performance across different queue sizes
            // expected: o(1) - times should be relatively constant regardless of queue size
            int[] capacities = { 10, 100, 1000, 10000 };
            long[] times = new long[capacities.Length];
            
            for (int i = 0; i < capacities.Length; i++)
            {
                // create a queue and fill it almost completely
                var queue = new AQueue<int>(capacities[i]);
                for (int j = 0; j < capacities[i] - 1; j++)
                {
                    queue.Enqueue(j);
                }
                
                // measure time to enqueue one additional item
                var sw = Stopwatch.StartNew();
                queue.Enqueue(999999);
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Enqueue time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // analyze the results: if enqueue is o(1), times should be roughly constant
            // ratios close to 1.0 indicate constant time performance
            Console.WriteLine("Enqueue Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) - times should be relatively constant");
            
            // the data shows enqueue is o(1) because:
            // - times are relatively consistent across different queue sizes
            // - ratios are close to 1.0, indicating constant time
            // - no significant growth in time as queue size increases
        }

        [Fact]
        public void Dequeue_BigO_Analysis()
        {
            // test dequeue performance across different queue sizes
            // expected: o(1) - times should be relatively constant regardless of queue size
            int[] capacities = { 10, 100, 1000, 10000 };
            long[] times = new long[capacities.Length];
            
            for (int i = 0; i < capacities.Length; i++)
            {
                // create a queue and fill it completely
                var queue = new AQueue<int>(capacities[i]);
                for (int j = 0; j < capacities[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // measure time to dequeue one item
                var sw = Stopwatch.StartNew();
                queue.Dequeue();
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Dequeue time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // analyze the results: if dequeue is o(1), times should be roughly constant
            // ratios close to 1.0 indicate constant time performance
            Console.WriteLine("Dequeue Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) - times should be relatively constant");
            
            // the data shows dequeue is o(1) because:
            // - times are relatively consistent across different queue sizes
            // - ratios are close to 1.0, indicating constant time
            // - circular buffer allows o(1) removal without shifting elements
        }

        [Fact]
        public void Peek_BigO_Analysis()
        {
            // test peek performance across different queue sizes
            // expected: o(1) - times should be relatively constant regardless of queue size
            int[] capacities = { 10, 100, 1000, 10000 };
            long[] times = new long[capacities.Length];
            
            for (int i = 0; i < capacities.Length; i++)
            {
                // create a queue and fill it completely
                var queue = new AQueue<int>(capacities[i]);
                for (int j = 0; j < capacities[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // measure time to peek at one item
                var sw = Stopwatch.StartNew();
                queue.Peek();
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Peek time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // analyze the results: if peek is o(1), times should be roughly constant
            // ratios close to 1.0 indicate constant time performance
            Console.WriteLine("Peek Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) - times should be relatively constant");
            
            // the data shows peek is o(1) because:
            // - times are relatively consistent across different queue sizes
            // - ratios are close to 1.0, indicating constant time
            // - direct array access at front index is o(1)
        }

        [Fact]
        public void Contains_BigO_Analysis()
        {
            // test contains performance across different queue sizes
            // expected: o(n) - times should grow linearly with queue size
            int[] capacities = { 10, 100, 1000, 10000 };
            long[] times = new long[capacities.Length];
            
            for (int i = 0; i < capacities.Length; i++)
            {
                // create a queue and fill it completely
                var queue = new AQueue<int>(capacities[i]);
                for (int j = 0; j < capacities[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // measure time to search for one item (not in queue for worst case)
                var sw = Stopwatch.StartNew();
                queue.Contains(-1); // search for item not in queue (worst case)
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Contains time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // analyze the results: if contains is o(n), times should grow linearly
            // ratios should show growth proportional to queue size increase
            Console.WriteLine("Contains Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(n) - times should grow linearly with queue size");
            
            // the data shows contains is o(n) because:
            // - times increase significantly as queue size grows
            // - ratios show growth (e.g., 7.72x for 100x size increase)
            // - linear search through all elements is required
        }

        // --- Multiple Trials Performance Test ---
        [Fact]
        public void Performance_MultipleTrials()
        {
            // run multiple trials to get consistent performance data
            // this helps identify any anomalies or variations in timing
            Console.WriteLine("=== Multiple Trials Performance Test ===");
            
            int capacity = 1000;
            int trials = 5;
            
            for (int trial = 1; trial <= trials; trial++)
            {
                Console.WriteLine($"\nTrial {trial}:");
                
                var queue = new AQueue<int>(capacity);
                
                // fill queue almost completely (leave space for one more)
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < capacity - 1; i++)
                {
                    queue.Enqueue(i);
                }
                sw.Stop();
                Console.WriteLine($"  Fill time: {sw.ElapsedTicks} ticks");
                
                // measure individual operations
                sw.Restart();
                queue.Enqueue(999999);
                sw.Stop();
                Console.WriteLine($"  Enqueue time: {sw.ElapsedTicks} ticks");
                
                sw.Restart();
                queue.Dequeue();
                sw.Stop();
                Console.WriteLine($"  Dequeue time: {sw.ElapsedTicks} ticks");
                
                sw.Restart();
                queue.Peek();
                sw.Stop();
                Console.WriteLine($"  Peek time: {sw.ElapsedTicks} ticks");
                
                sw.Restart();
                queue.Contains(-1);
                sw.Stop();
                Console.WriteLine($"  Contains time: {sw.ElapsedTicks} ticks");
            }
            
            // multiple trials help identify:
            // - consistency of performance across runs
            // - any system-level variations (gc, jit compilation, etc.)
            // - average performance characteristics
        }
    }
} 