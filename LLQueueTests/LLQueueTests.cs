using System;
using Xunit;
using LLQueueLib;
using System.Diagnostics;

namespace LLQueueTests
{
    public class LLQueueUnitTests
    {
        // --- Basic Functionality Tests ---
        
        /// <summary>
        /// Tests that Enqueue correctly adds items and updates the size.
        /// Expected: Size should increase by 1 for each enqueued item.
        /// </summary>
        [Fact]
        public void Enqueue_IncreasesSizeCorrectly()
        {
            var queue = new LLQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            Assert.Equal(3, queue.Size);
        }

        /// <summary>
        /// Tests that Dequeue returns the correct value and decreases the size.
        /// Expected: Should return the first item enqueued and reduce size by 1.
        /// </summary>
        [Fact]
        public void Dequeue_ReturnsCorrectValueAndDecreasesSize()
        {
            var queue = new LLQueue<int>();
            queue.Enqueue(10);
            queue.Enqueue(20);
            int val = queue.Dequeue();
            Assert.Equal(10, val);
            Assert.Equal(1, queue.Size);
        }

        /// <summary>
        /// Tests that Peek returns the front item without removing it.
        /// Expected: Should return the first item but keep the size unchanged.
        /// </summary>
        [Fact]
        public void Peek_ReturnsFrontWithoutRemoving()
        {
            var queue = new LLQueue<string>();
            queue.Enqueue("A");
            queue.Enqueue("B");
            string peeked = queue.Peek();
            Assert.Equal("A", peeked);
            Assert.Equal(2, queue.Size);
        }

        /// <summary>
        /// Tests that Contains correctly identifies existing and non-existing values.
        /// Expected: Should return true for existing items, false for non-existing items.
        /// </summary>
        [Fact]
        public void Contains_FindsExistingAndNonExistingValues()
        {
            var queue = new LLQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(10);
            Assert.True(queue.Contains(5));
            Assert.False(queue.Contains(99));
        }

        // --- Edge Case Tests ---
        
        /// <summary>
        /// Tests that Dequeue throws an exception when called on an empty queue.
        /// Expected: Should throw InvalidOperationException.
        /// </summary>
        [Fact]
        public void Dequeue_EmptyQueue_ThrowsException()
        {
            var queue = new LLQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        /// <summary>
        /// Tests that Peek throws an exception when called on an empty queue.
        /// Expected: Should throw InvalidOperationException.
        /// </summary>
        [Fact]
        public void Peek_EmptyQueue_ThrowsException()
        {
            var queue = new LLQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        /// <summary>
        /// Tests that Contains returns false when called on an empty queue.
        /// Expected: Should return false for any item.
        /// </summary>
        [Fact]
        public void Contains_EmptyQueue_ReturnsFalse()
        {
            var queue = new LLQueue<int>();
            Assert.False(queue.Contains(123));
        }

        /// <summary>
        /// Tests that the queue maintains FIFO (First In, First Out) order.
        /// Expected: Items should be dequeued in the same order they were enqueued.
        /// </summary>
        [Fact]
        public void FIFO_Order_IsMaintained()
        {
            var queue = new LLQueue<string>();
            queue.Enqueue("first");
            queue.Enqueue("second");
            queue.Enqueue("third");
            Assert.Equal("first", queue.Dequeue());
            Assert.Equal("second", queue.Dequeue());
            Assert.Equal("third", queue.Dequeue());
        }

        // --- Performance Measurement Tests ---
        
        /// <summary>
        /// Measures and analyzes the performance of Enqueue operations.
        /// Runs multiple trials to ensure consistent results.
        /// Expected: O(1) - times should be relatively constant regardless of queue size.
        /// </summary>
        [Fact]
        public void Enqueue_Performance_Analysis()
        {
            int[] sizes = { 100, 1000, 10000, 100000 };
            int trials = 5;
            long[,] times = new long[sizes.Length, trials];
            
            Console.WriteLine("=== Enqueue Performance Analysis ===");
            
            for (int i = 0; i < sizes.Length; i++)
            {
                for (int trial = 0; trial < trials; trial++)
                {
                    // Create a queue with the specified number of items
                    var queue = new LLQueue<int>();
                    for (int j = 0; j < sizes[i]; j++)
                    {
                        queue.Enqueue(j);
                    }
                    
                    // Measure time to enqueue ONE additional item
                    var sw = Stopwatch.StartNew();
                    queue.Enqueue(999999);
                    sw.Stop();
                    times[i, trial] = sw.ElapsedTicks;
                }
                
                // Calculate average time for this size
                long avgTime = 0;
                for (int trial = 0; trial < trials; trial++)
                {
                    avgTime += times[i, trial];
                }
                avgTime /= trials;
                
                Console.WriteLine($"Queue size {sizes[i]}: Average time = {avgTime} ticks");
            }
            
            // Analysis: Check if times are relatively constant (O(1))
            long time100 = 0, time1000 = 0, time10000 = 0, time100000 = 0;
            for (int trial = 0; trial < trials; trial++)
            {
                time100 += times[0, trial];
                time1000 += times[1, trial];
                time10000 += times[2, trial];
                time100000 += times[3, trial];
            }
            time100 /= trials; time1000 /= trials; time10000 /= trials; time100000 /= trials;
            
            Console.WriteLine($"\nPerformance Ratios:");
            Console.WriteLine($"1000/100: {time1000 / (double)time100:F2}");
            Console.WriteLine($"10000/1000: {time10000 / (double)time1000:F2}");
            Console.WriteLine($"100000/10000: {time100000 / (double)time10000:F2}");
            Console.WriteLine("Expected: O(1) - ratios should be close to 1.0");
        }

        /// <summary>
        /// Measures and analyzes the performance of Dequeue operations.
        /// Runs multiple trials to ensure consistent results.
        /// Expected: O(1) - times should be relatively constant regardless of queue size.
        /// </summary>
        [Fact]
        public void Dequeue_Performance_Analysis()
        {
            int[] sizes = { 100, 1000, 10000, 100000 };
            int trials = 5;
            long[,] times = new long[sizes.Length, trials];
            
            Console.WriteLine("=== Dequeue Performance Analysis ===");
            
            for (int i = 0; i < sizes.Length; i++)
            {
                for (int trial = 0; trial < trials; trial++)
                {
                    // Create a queue with the specified number of items
                    var queue = new LLQueue<int>();
                    for (int j = 0; j < sizes[i]; j++)
                    {
                        queue.Enqueue(j);
                    }
                    
                    // Measure time to dequeue ONE item
                    var sw = Stopwatch.StartNew();
                    queue.Dequeue();
                    sw.Stop();
                    times[i, trial] = sw.ElapsedTicks;
                }
                
                // Calculate average time for this size
                long avgTime = 0;
                for (int trial = 0; trial < trials; trial++)
                {
                    avgTime += times[i, trial];
                }
                avgTime /= trials;
                
                Console.WriteLine($"Queue size {sizes[i]}: Average time = {avgTime} ticks");
            }
            
            // Analysis: Check if times are relatively constant (O(1))
            long time100 = 0, time1000 = 0, time10000 = 0, time100000 = 0;
            for (int trial = 0; trial < trials; trial++)
            {
                time100 += times[0, trial];
                time1000 += times[1, trial];
                time10000 += times[2, trial];
                time100000 += times[3, trial];
            }
            time100 /= trials; time1000 /= trials; time10000 /= trials; time100000 /= trials;
            
            Console.WriteLine($"\nPerformance Ratios:");
            Console.WriteLine($"1000/100: {time1000 / (double)time100:F2}");
            Console.WriteLine($"10000/1000: {time10000 / (double)time1000:F2}");
            Console.WriteLine($"100000/10000: {time100000 / (double)time10000:F2}");
            Console.WriteLine("Expected: O(1) - ratios should be close to 1.0");
        }

        /// <summary>
        /// Measures and analyzes the performance of Peek operations.
        /// Runs multiple trials to ensure consistent results.
        /// Expected: O(1) - times should be relatively constant regardless of queue size.
        /// </summary>
        [Fact]
        public void Peek_Performance_Analysis()
        {
            int[] sizes = { 100, 1000, 10000, 100000 };
            int trials = 5;
            long[,] times = new long[sizes.Length, trials];
            
            Console.WriteLine("=== Peek Performance Analysis ===");
            
            for (int i = 0; i < sizes.Length; i++)
            {
                for (int trial = 0; trial < trials; trial++)
                {
                    // Create a queue with the specified number of items
                    var queue = new LLQueue<int>();
                    for (int j = 0; j < sizes[i]; j++)
                    {
                        queue.Enqueue(j);
                    }
                    
                    // Measure time to peek at ONE item
                    var sw = Stopwatch.StartNew();
                    queue.Peek();
                    sw.Stop();
                    times[i, trial] = sw.ElapsedTicks;
                }
                
                // Calculate average time for this size
                long avgTime = 0;
                for (int trial = 0; trial < trials; trial++)
                {
                    avgTime += times[i, trial];
                }
                avgTime /= trials;
                
                Console.WriteLine($"Queue size {sizes[i]}: Average time = {avgTime} ticks");
            }
            
            // Analysis: Check if times are relatively constant (O(1))
            long time100 = 0, time1000 = 0, time10000 = 0, time100000 = 0;
            for (int trial = 0; trial < trials; trial++)
            {
                time100 += times[0, trial];
                time1000 += times[1, trial];
                time10000 += times[2, trial];
                time100000 += times[3, trial];
            }
            time100 /= trials; time1000 /= trials; time10000 /= trials; time100000 /= trials;
            
            Console.WriteLine($"\nPerformance Ratios:");
            Console.WriteLine($"1000/100: {time1000 / (double)time100:F2}");
            Console.WriteLine($"10000/1000: {time10000 / (double)time1000:F2}");
            Console.WriteLine($"100000/10000: {time100000 / (double)time10000:F2}");
            Console.WriteLine("Expected: O(1) - ratios should be close to 1.0");
        }

        /// <summary>
        /// Measures and analyzes the performance of Contains operations.
        /// Runs multiple trials to ensure consistent results.
        /// Expected: O(n) - times should grow linearly with queue size.
        /// </summary>
        [Fact]
        public void Contains_Performance_Analysis()
        {
            int[] sizes = { 100, 1000, 10000, 100000 };
            int trials = 5;
            long[,] times = new long[sizes.Length, trials];
            
            Console.WriteLine("=== Contains Performance Analysis ===");
            
            for (int i = 0; i < sizes.Length; i++)
            {
                for (int trial = 0; trial < trials; trial++)
                {
                    // Create a queue with the specified number of items
                    var queue = new LLQueue<int>();
                    for (int j = 0; j < sizes[i]; j++)
                    {
                        queue.Enqueue(j);
                    }
                    
                    // Measure time to search for ONE item (not in queue for worst case)
                    var sw = Stopwatch.StartNew();
                    queue.Contains(-1); // Search for item not in queue (worst case)
                    sw.Stop();
                    times[i, trial] = sw.ElapsedTicks;
                }
                
                // Calculate average time for this size
                long avgTime = 0;
                for (int trial = 0; trial < trials; trial++)
                {
                    avgTime += times[i, trial];
                }
                avgTime /= trials;
                
                Console.WriteLine($"Queue size {sizes[i]}: Average time = {avgTime} ticks");
            }
            
            // Analysis: Check if times grow linearly (O(n))
            long time100 = 0, time1000 = 0, time10000 = 0, time100000 = 0;
            for (int trial = 0; trial < trials; trial++)
            {
                time100 += times[0, trial];
                time1000 += times[1, trial];
                time10000 += times[2, trial];
                time100000 += times[3, trial];
            }
            time100 /= trials; time1000 /= trials; time10000 /= trials; time100000 /= trials;
            
            Console.WriteLine($"\nPerformance Ratios:");
            Console.WriteLine($"1000/100: {time1000 / (double)time100:F2} (expected ~10)");
            Console.WriteLine($"10000/1000: {time10000 / (double)time1000:F2} (expected ~10)");
            Console.WriteLine($"100000/10000: {time100000 / (double)time10000:F2} (expected ~10)");
            Console.WriteLine("Expected: O(n) - ratios should be approximately 10x");
        }

        /// <summary>
        /// Tests that the queue can handle multiple data types correctly.
        /// Expected: Should work with different data types without issues.
        /// </summary>
        [Fact]
        public void Generic_Type_Support()
        {
            // Test with integers
            var intQueue = new LLQueue<int>();
            intQueue.Enqueue(1);
            intQueue.Enqueue(2);
            Assert.Equal(1, intQueue.Dequeue());
            
            // Test with strings
            var stringQueue = new LLQueue<string>();
            stringQueue.Enqueue("hello");
            stringQueue.Enqueue("world");
            Assert.Equal("hello", stringQueue.Dequeue());
            
            // Test with custom objects
            var objectQueue = new LLQueue<object>();
            objectQueue.Enqueue(new { Name = "Test", Value = 42 });
            Assert.NotNull(objectQueue.Dequeue());
        }

        /// <summary>
        /// Tests that the queue can handle large numbers of operations correctly.
        /// Expected: Should maintain correct FIFO order and size throughout operations.
        /// </summary>
        [Fact]
        public void Large_Scale_Operations()
        {
            var queue = new LLQueue<int>();
            int operations = 10000;
            
            // Enqueue many items
            for (int i = 0; i < operations; i++)
            {
                queue.Enqueue(i);
                Assert.Equal(i + 1, queue.Size);
            }
            
            // Dequeue all items in correct order
            for (int i = 0; i < operations; i++)
            {
                Assert.Equal(i, queue.Dequeue());
                Assert.Equal(operations - i - 1, queue.Size);
            }
            
            Assert.Equal(0, queue.Size);
        }

        /// <summary>
        /// Tests the Clear method functionality.
        /// Expected: Should remove all items and set size to 0.
        /// </summary>
        [Fact]
        public void Clear_RemovesAllItems()
        {
            var queue = new LLQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            
            Assert.Equal(3, queue.Size);
            queue.Clear();
            Assert.Equal(0, queue.Size);
            Assert.False(queue.Contains(1));
        }

        /// <summary>
        /// Tests the ToString method.
        /// Expected: Should return a meaningful string representation.
        /// </summary>
        [Fact]
        public void ToString_ReturnsMeaningfulRepresentation()
        {
            var queue = new LLQueue<string>();
            queue.Enqueue("test");
            string result = queue.ToString();
            Assert.Contains("LLQueue", result);
            Assert.Contains("Size: 1", result);
        }
    }
} 