using FluentAssertions;
using hmlib.Types.Caching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hmlib.Tests.Types.Caching
{
	public class LRUCacheTests
	{
		[Fact]
		public void LRUCache_AddAndRetrieveItems()
		{
			var cache = new LRUCache<int, string>(3);
			cache.CreateEntry(1, "One");
			cache.CreateEntry(2, "Two");
			cache.CreateEntry(3, "Three");
			cache.Get(1).Should().Be("One");
			cache.Get(2).Should().Be("Two");
			cache.Get(3).Should().Be("Three");
		}
		[Fact]
		public void LRUCache_EvictionTest()
		{
			var cache = new LRUCache<int, string>(2);
			cache.CreateEntry(1, "One");
			cache.CreateEntry(2, "Two");
			cache.Get(1).Should().Be("One");
			cache.Get(2).Should().Be("Two");
			// Adding a new item should evict the least recently used item (1)
			cache.CreateEntry(3, "Three");
			cache.Get(1).Should().Be(null);
			cache.Get(2).Should().Be("Two");
			cache.Get(3).Should().Be("Three");
		}
		[Fact]
		public void LRUCache_UpdateExistingItem()
		{
			var cache = new LRUCache<int, string>(2);
			cache.CreateEntry(1, "One");
			cache.CreateEntry(2, "Two");
			cache.Get(1).Should().Be("One");
			cache.Get(2).Should().Be("Two");
			// Update item 1
			cache.CreateEntry(1, "Updated One");
			cache.Get(1).Should().Be("Updated One");
			cache.Get(2).Should().Be("Two");
		}
		[Fact]
		public void LRUCache_EmptyCache()
		{
			var cache = new LRUCache<int, string>(2);
			cache.Get(1).Should().Be(null); // Should return null for non-existent key
			cache.CreateEntry(1, "One");
			cache.Get(1).Should().Be("One"); // Should return "One" after adding it
			cache.CreateEntry(2, "Two");
			cache.CreateEntry(3, "Three"); // This should evict "One"
			cache.Get(1).Should().Be(null); // Should return null for evicted key
			cache.Get(2).Should().Be("Two");
			cache.Get(3).Should().Be("Three");
		}
		[Fact]
		public void LRUCache_ClearCache()
		{
			var cache = new LRUCache<int, string>(3);
			cache.CreateEntry(1, "One");
			cache.CreateEntry(2, "Two");
			cache.CreateEntry(3, "Three");
			cache.Clear();
			cache.Get(1).Should().Be(null); // Should return null after clearing
			cache.Get(2).Should().Be(null);
			cache.Get(3).Should().Be(null);
		}

		[Fact]
		public void LRUCache_ShouldBePerformant_WhenAddingMillionItems()
		{
			// Arrange
			const int itemCount = 1_000_000;
			const int capacity = 1_000_000;
			var cache = new LRUCache<int, int>(capacity);
			var stopwatch = Stopwatch.StartNew();

			// Act
			for (int i = 0; i < itemCount; i++)
			{
				cache.CreateEntry(i, i);
			}

			stopwatch.Stop();

			const int maxExpectedMilliseconds = 1000;

			stopwatch.ElapsedMilliseconds.Should()
				.BeLessThan(maxExpectedMilliseconds, $"Adding {itemCount} items took too long: {stopwatch.ElapsedMilliseconds} ms");
		}

		[Fact]
		public void LRUCache_ShouldBeDisposable()
		{
			var cache = new LRUCache<int, string>(3);
			cache.CreateEntry(1, "One");
			cache.CreateEntry(2, "Two");
			cache.CreateEntry(3, "Three");
			// Ensure the cache is not disposed yet
			cache.Get(1).Should().Be("One");
			// Dispose the cache
			cache.Dispose();
			// After disposing, it should not throw an exception when trying to access it
			Action act = () => cache.Get(1);
			act.Should().Throw<ObjectDisposedException>();
		}
		//test if the cache is thread-safe
		[Fact]
		public void LRUCache_ShouldNotThrow_WhenAccessedFromMultipleThreads()
		{
			// Arrange
			var cache = new LRUCache<int, int>(1000);
			var tasks = new List<Task>();
			int errors = 0;

			// Act
			for (int t = 0; t < 10; t++) // 10 parallel tasks
			{
				tasks.Add(Task.Run(() =>
				{
					var rand = new Random();
					for (int i = 0; i < 100_000; i++)
					{
						int key = rand.Next(0, 500); // Some collisions
						try
						{
							cache.CreateEntry(key, key);
							var _ = cache.Get(key);
						}
						catch
						{
							Interlocked.Increment(ref errors);
						}
					}
				}));
			}

			Task.WaitAll(tasks.ToArray());

			// Assert
			//Assert.True(errors == 0, $"Encountered {errors} errors during multithreaded access.");
			errors.Should().Be(0, $"Encountered {errors} errors during multithreaded access.");
		}
	}
}
