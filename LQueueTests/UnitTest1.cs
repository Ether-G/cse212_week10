using System;
using Xunit;
using LQueueLib;
using System.Diagnostics;

namespace LQueueTests
{
    public class LQueueUnitTests
    {
        // --- basic functionality tests ---
        [Fact]
        public void Enqueue_IncreasesSizeCorrectly()
        {
            var queue = new LQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            Assert.Equal(3, queue.Size);
        }

        [Fact]
        public void Dequeue_ReturnsCorrectValueAndDecreasesSize()
        {
            var queue = new LQueue<int>();
            queue.Enqueue(10);
            queue.Enqueue(20);
            int val = queue.Dequeue();
            Assert.Equal(10, val);
            Assert.Equal(1, queue.Size);
        }

        [Fact]
        public void Peek_ReturnsFrontWithoutRemoving()
        {
            var queue = new LQueue<string>();
            queue.Enqueue("A");
            queue.Enqueue("B");
            string peeked = queue.Peek();
            Assert.Equal("A", peeked);
            Assert.Equal(2, queue.Size);
        }

        [Fact]
        public void Contains_FindsExistingAndNonExistingValues()
        {
            var queue = new LQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(10);
            Assert.True(queue.Contains(5));
            Assert.False(queue.Contains(99));
        }

        // --- edge case tests ---
        [Fact]
        public void Dequeue_EmptyQueue_ThrowsException()
        {
            var queue = new LQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Fact]
        public void Peek_EmptyQueue_ThrowsException()
        {
            var queue = new LQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Fact]
        public void Contains_EmptyQueue_ReturnsFalse()
        {
            var queue = new LQueue<int>();
            Assert.False(queue.Contains(123));
        }

        [Fact]
        public void FIFO_Order_IsMaintained()
        {
            var queue = new LQueue<string>();
            queue.Enqueue("first");
            queue.Enqueue("second");
            queue.Enqueue("third");
            Assert.Equal("first", queue.Dequeue());
            Assert.Equal("second", queue.Dequeue());
            Assert.Equal("third", queue.Dequeue());
        }

        // --- I Fixed the Big-O performance measurement, sorry for doing this wrong last week Prof deBry ---
        [Fact]
        public void Enqueue_BigO_Analysis()
        {
            // Test sizes: 10, 100, 1000, 10000
            int[] sizes = { 10, 100, 1000, 10000 };
            long[] times = new long[sizes.Length];
            
            for (int i = 0; i < sizes.Length; i++)
            {
                // Create a queue with the specified number of items
                var queue = new LQueue<int>();
                for (int j = 0; j < sizes[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // Measure time to enqueue ONE additional item
                var sw = Stopwatch.StartNew();
                queue.Enqueue(999999);
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Enqueue time for queue with {sizes[i]} items: {times[i]} ticks");
            }
            
            // Analysis: If times are at least close to constant, it's O(1)
            // If times grow linearly with size, it's O(n)
            Console.WriteLine("Enqueue Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) amortized - times should be relatively constant");
        }

        [Fact]
        public void Dequeue_BigO_Analysis()
        {
            // Test sizes: 10, 100, 1000, 10000
            int[] sizes = { 10, 100, 1000, 10000 };
            long[] times = new long[sizes.Length];
            
            for (int i = 0; i < sizes.Length; i++)
            {
                // Create a queue with the specified number of items
                var queue = new LQueue<int>();
                for (int j = 0; j < sizes[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // Measure time to dequeue ONE item
                var sw = Stopwatch.StartNew();
                queue.Dequeue(); // Remove one item
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Dequeue time for queue with {sizes[i]} items: {times[i]} ticks");
            }
            
            // Analysis: If times grow linearly with size, it's O(n)
            Console.WriteLine("Dequeue Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(n) - times should grow linearly with queue size");
        }

        [Fact]
        public void Peek_BigO_Analysis()
        {
            // Test sizes: 10, 100, 1000, 10000
            int[] sizes = { 10, 100, 1000, 10000 };
            long[] times = new long[sizes.Length];
            
            for (int i = 0; i < sizes.Length; i++)
            {
                // Create a queue with the specified number of items
                var queue = new LQueue<int>();
                for (int j = 0; j < sizes[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // Measure time to peek at ONE item
                var sw = Stopwatch.StartNew();
                queue.Peek(); // Peek at one item
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Peek time for queue with {sizes[i]} items: {times[i]} ticks");
            }
            
            // Analysis: If times are constant, it's O(1)
            Console.WriteLine("Peek Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(1) - times should be relatively constant");
        }

        [Fact]
        public void Contains_BigO_Analysis()
        {
            // Test sizes: 10, 100, 1000, 10000
            int[] sizes = { 10, 100, 1000, 10000 };
            long[] times = new long[sizes.Length];
            
            for (int i = 0; i < sizes.Length; i++)
            {
                // Create a queue with the specified number of items
                var queue = new LQueue<int>();
                for (int j = 0; j < sizes[i]; j++)
                {
                    queue.Enqueue(j);
                }
                
                // Measure time to search for ONE item (not in queue for worst case)
                var sw = Stopwatch.StartNew();
                queue.Contains(-1); // Search for item not in queue (worst case)
                sw.Stop();
                times[i] = sw.ElapsedTicks;
                
                Console.WriteLine($"Contains time for queue with {sizes[i]} items: {times[i]} ticks");
            }
            
            // Analysis: If times grow linearly with size, it's O(n)
            Console.WriteLine("Contains Big-O Analysis:");
            Console.WriteLine($"Time ratio (1000/10): {times[2] / (double)times[0]:F2}");
            Console.WriteLine($"Time ratio (10000/100): {times[3] / (double)times[1]:F2}");
            Console.WriteLine("Expected: O(n) - times should grow linearly with queue size");
        }

        // --- memory usage ---
        [Fact]
        public void Capacity_Grows_AsExpected()
        {
            var queue = new LQueue<int>();
            int lastCapacity = queue.Capacity;
            int growths = 0;
            for (int i = 0; i < 100; i++)
            {
                queue.Enqueue(i);
                if (queue.Capacity != lastCapacity)
                {
                    growths++;
                    Console.WriteLine($"Capacity grew from {lastCapacity} to {queue.Capacity} at size {queue.Size}");
                    lastCapacity = queue.Capacity;
                }
            }
            Assert.True(growths > 0);
        }
    }
}