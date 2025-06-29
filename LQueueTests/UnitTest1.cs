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

        // --- performance measurement ---
        // these are not strict pass/fail, but will output timing info for analysis
        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void Enqueue_Performance(int size)
        {
            var queue = new LQueue<int>();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < size; i++)
                queue.Enqueue(i);
            sw.Stop();
            // o(1) amortized, o(n) when resizing
            // console.writeline($"enqueue {size} items: {sw.elapsedticks} ticks");
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void Dequeue_Performance(int size)
        {
            var queue = new LQueue<int>();
            for (int i = 0; i < size; i++)
                queue.Enqueue(i);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < size; i++)
                queue.Dequeue();
            sw.Stop();
            // o(n) due to shifting
            // console.writeline($"dequeue {size} items: {sw.elapsedticks} ticks");
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void Peek_Performance(int size)
        {
            var queue = new LQueue<int>();
            for (int i = 0; i < size; i++)
                queue.Enqueue(i);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
                queue.Peek();
            sw.Stop();
            // o(1)
            // console.writeline($"peek 1000 times: {sw.elapsedticks} ticks");
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void Contains_Performance(int size)
        {
            var queue = new LQueue<int>();
            for (int i = 0; i < size; i++)
                queue.Enqueue(i);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                queue.Contains(i);
                queue.Contains(-i - 1);
            }
            sw.Stop();
            // o(n)
            // console.writeline($"contains 200 times: {sw.elapsedticks} ticks");
        }

        // --- memory usage analysis ---
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
                    // console.writeline($"capacity grew from {lastcapacity} to {queue.capacity} at size {queue.size}");
                    lastCapacity = queue.Capacity;
                }
            }
            Assert.True(growths > 0); // should grow at least once
        }
    }
}