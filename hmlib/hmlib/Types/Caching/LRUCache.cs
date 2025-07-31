using System;
using System.Collections.Generic;
using System.Text;

namespace hmlib.Types.Caching
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class LRUCache<TKey, TValue>
	{
		Dictionary<TKey, LinkedListNode<(TKey key, TValue value)>> _cache;
		LinkedList<(TKey key, TValue value)> _list;
		int _capacity;

		/// <summary>
		/// Initializes a new instance of the LRUCache with a specified capacity.
		/// </summary>
		/// <param name="capacity">The maximum number of items the cache can hold.</param>
		public LRUCache(int capacity)
		{
			_capacity = capacity;
			_cache = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>();
			_list = new LinkedList<(TKey, TValue)>();
		}

		/// <summary>
		/// Retrieves an item from the cache by its key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>The value associated with the key, or default(TValue) if not found.</returns>
		public TValue Get(TKey key)
		{
			if (!_cache.ContainsKey(key)) return default(TValue);

			var node = _cache[key];
			_list.Remove(node);
			_list.AddLast(node);

			return node.Value.value;
		}

		/// <summary>
		/// Adds or updates an item in the cache.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void Put(TKey key, TValue value)
		{
			if (_cache.ContainsKey(key))
			{
				var node = _cache[key];
				_list.Remove(node);
			}
			else
			{
				if (_capacity == _list.Count)
				{
					var n = _list.First;
					_list.Remove(n);
					_cache.Remove(n.Value.key);
				}
			}
			var newNode = _list.AddLast((key, value));
			_cache[key] = newNode;
		}

		/// <summary>
		/// Clears the cache, removing all items.
		/// </summary>
		public void Clear()
		{
			_cache.Clear();
			_list.Clear();
		}
	}
}
