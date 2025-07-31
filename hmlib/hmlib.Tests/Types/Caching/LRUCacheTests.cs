using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using hmlib.Types.Caching;

namespace hmlib.Tests.Types.Caching
{
	public class LRUCacheTests
	{
		[Fact]
		public void LRUCache_AddAndRetrieveItems()
		{
			var cache = new LRUCache<int, string>(3);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			cache.Put(3, "Three");
			cache.Get(1).Should().Be("One");
			cache.Get(2).Should().Be("Two");
			cache.Get(3).Should().Be("Three");
		}
		[Fact]
		public void LRUCache_EvictionTest()
		{
			var cache = new LRUCache<int, string>(2);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			cache.Get(1).Should().Be("One");
			cache.Get(2).Should().Be("Two");
			// Adding a new item should evict the least recently used item (1)
			cache.Put(3, "Three");
			cache.Get(1).Should().Be(null);
			cache.Get(2).Should().Be("Two");
			cache.Get(3).Should().Be("Three");
		}
		[Fact]
		public void LRUCache_UpdateExistingItem()
		{
			var cache = new LRUCache<int, string>(2);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			cache.Get(1).Should().Be("One");
			cache.Get(2).Should().Be("Two");
			// Update item 1
			cache.Put(1, "Updated One");
			cache.Get(1).Should().Be("Updated One");
			cache.Get(2).Should().Be("Two");
		}
		[Fact]
		public void LRUCache_EmptyCache()
		{
			var cache = new LRUCache<int, string>(2);
			cache.Get(1).Should().Be(null); // Should return null for non-existent key
			cache.Put(1, "One");
			cache.Get(1).Should().Be("One"); // Should return "One" after adding it
			cache.Put(2, "Two");
			cache.Put(3, "Three"); // This should evict "One"
			cache.Get(1).Should().Be(null); // Should return null for evicted key
			cache.Get(2).Should().Be("Two");
			cache.Get(3).Should().Be("Three");
		}
		[Fact]
		public void LRUCache_ClearCache()
		{
			var cache = new LRUCache<int, string>(3);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			cache.Put(3, "Three");
			cache.Clear();
			cache.Get(1).Should().Be(null); // Should return null after clearing
			cache.Get(2).Should().Be(null);
			cache.Get(3).Should().Be(null);
		}
	}
}
