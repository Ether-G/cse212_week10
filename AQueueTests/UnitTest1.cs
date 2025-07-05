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
            var queue = new AQueue<int>(5);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            Assert.Equal(3, queue.Size);
        }

        [Fact]
        public void Dequeue_ReturnsCorrectValueAndDecreasesSize()
        {
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
            var queue = new AQueue<int>(5);
            queue.Enqueue(5);
            queue.Enqueue(10);
            Assert.True(queue.Contains(5));
            Assert.False(queue.Contains(99));
        }

        [Fact]
        public void FIFO_Order_IsMaintained()
        {
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
            var queue = new AQueue<int>(5);
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Fact]
        public void Peek_EmptyQueue_ThrowsException()
        {
            var queue = new AQueue<int>(5);
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Fact]
        public void Enqueue_FullQueue_ThrowsException()
        {
            var queue = new AQueue<int>(2);
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.Throws<InvalidOperationException>(() => queue.Enqueue(3));
        }

        [Fact]
        public void Contains_EmptyQueue_ReturnsFalse()
        {
            var queue = new AQueue<int>(5);
            Assert.False(queue.Contains(123));
        }

        [Fact]
        public void Constructor_ZeroCapacity_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new AQueue<int>(0));
        }

        [Fact]
        public void Constructor_NegativeCapacity_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new AQueue<int>(-1));
        }

        // --- Circular Buffer Tests (hopefully I correctly understood this this time... I'm sorry for submitting this on the last day every week, I work fulltime with school so sometimes submitting earlier is not possible) ---
        [Fact]
        public void CircularBuffer_WrapsAroundCorrectly()
        {
            var queue = new AQueue<int>(3);
            
            // Fill the queue
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            
            // Remove one item to make space
            queue.Dequeue();
            
            // Add another item it SHOULD wrap around
            queue.Enqueue(4);
            
            // Verify the order.
            Assert.Equal(2, queue.Dequeue());
            Assert.Equal(3, queue.Dequeue());
            Assert.Equal(4, queue.Dequeue());
        }

        [Fact]
        public void CircularBuffer_MultipleWraps_WorkCorrectly()
        {
            var queue = new AQueue<int>(2);
            
            // First cycle
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.Equal(1, queue.Dequeue());
            queue.Enqueue(3);
            Assert.Equal(2, queue.Dequeue());
            
            // Second cycle
            queue.Enqueue(4);
            Assert.Equal(3, queue.Dequeue());
            Assert.Equal(4, queue.Dequeue());
        }

        [Fact]
        public void CircularBuffer_Contains_WorksWithWrappedData()
        {
            var queue = new AQueue<int>(3);
            
            // Fill and partially empty to create wrap
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Dequeue(); // Remove 1
            queue.Enqueue(4); // Add 4, should wrap
            
            // Test contains on wrapped data
            Assert.True(queue.Contains(2));
            Assert.True(queue.Contains(3));
            Assert.True(queue.Contains(4));
            Assert.False(queue.Contains(1)); // Was removed
        }

        // --- Property Tests ---
        [Fact]
        public void Capacity_IsFixed()
        {
            var queue = new AQueue<int>(10);
            Assert.Equal(10, queue.Capacity);
            
            // Capacity should not change
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.Equal(10, queue.Capacity);
        }

        [Fact]
        public void IsEmpty_WorksCorrectly()
        {
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
            var queue = new AQueue<int>(3);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Dequeue(); // Remove 1
            queue.Enqueue(4); // Wrap around
            
            queue.Clear();
            
            Assert.Equal(0, queue.Size);
            Assert.True(queue.IsEmpty);
        }

        // --- Perf Measurement Tests ---
        [Fact]
        public void Enqueue_BigO_Analysis()
        {
            // Test with diff queue sizes
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
                
                // time to enqueue ONE additional item
                var sw = Stopwatch.StartNew();
                queue.Enqueue(999999);
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Enqueue time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // Analysis: Should be O(1) - times should be relatively constant
            Console.WriteLine("Enqueue Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) - times should be relatively constant");
        }

        [Fact]
        public void Dequeue_BigO_Analysis()
        {
            // Test with different queue sizes
            int[] capacities = { 10, 100, 1000, 10000 };
            long[] times = new long[capacities.Length];
            
            for (int i = 0; i < capacities.Length; i++)
            {
                // Create a queue and fill it completely
                var queue = new AQueue<int>(capacities[i]);
                for (int j = 0; j < capacities[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // Measure time to dequeue ONE item
                var sw = Stopwatch.StartNew();
                queue.Dequeue();
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Dequeue time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // Analysis: Should be O(1) - times should be relatively constant
            Console.WriteLine("Dequeue Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) - times should be relatively constant");
        }

        [Fact]
        public void Peek_BigO_Analysis()
        {
            // Test with different queue sizes
            int[] capacities = { 10, 100, 1000, 10000 };
            long[] times = new long[capacities.Length];
            
            for (int i = 0; i < capacities.Length; i++)
            {
                // Create a queue and fill it completely
                var queue = new AQueue<int>(capacities[i]);
                for (int j = 0; j < capacities[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // Measure time to peek at ONE item
                var sw = Stopwatch.StartNew();
                queue.Peek();
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Peek time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // Analysis: Should be O(1) - times should be relatively constant
            Console.WriteLine("Peek Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) - times should be relatively constant");
        }

        [Fact]
        public void Contains_BigO_Analysis()
        {
            // Test with different queue sizes
            int[] capacities = { 10, 100, 1000, 10000 };
            long[] times = new long[capacities.Length];
            
            for (int i = 0; i < capacities.Length; i++)
            {
                // Create a queue and fill it completely
                var queue = new AQueue<int>(capacities[i]);
                for (int j = 0; j < capacities[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // Measure time to search for ONE item (not in queue for worst case)
                var sw = Stopwatch.StartNew();
                queue.Contains(-1); // Search for item not in queue (worst case)
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Contains time for queue with capacity {capacities[i]}: {times[i]} ticks");
            }
            
            // Analysis: Should be O(n) - times should grow linearly with queue size
            Console.WriteLine("Contains Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(n) - times should grow linearly with queue size");
        }

        // --- Multiple Trials Performance Test ---
        [Fact]
        public void Performance_MultipleTrials()
        {
            Console.WriteLine("=== Multiple Trials Performance Test ===");
            
            int capacity = 1000;
            int trials = 5;
            
            for (int trial = 1; trial <= trials; trial++)
            {
                Console.WriteLine($"\nTrial {trial}:");
                
                var queue = new AQueue<int>(capacity);
                
                // Fill queue almost completely (leave space for one more)
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < capacity - 1; i++)
                {
                    queue.Enqueue(i);
                }
                sw.Stop();
                Console.WriteLine($"  Fill time: {sw.ElapsedTicks} ticks");
                
                // Measure individual operations
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
        }
    }
} 