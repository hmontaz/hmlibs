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
	public class LRUCache<TKey, TValue> : IDisposable
	{
		Dictionary<TKey, LinkedListNode<(TKey key, TValue value)>> _cache;
		LinkedList<(TKey key, TValue value)> _list;
		int _capacity;
		private bool _disposed = false;
		object _lock = new object();

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
			lock (_lock)
			{
				ThrowIfDisposed();
				if (!_cache.ContainsKey(key)) return default(TValue);
				var node = _cache[key];
				_list.Remove(node);
				_list.AddLast(node);

				return node.Value.value;
			}
		}

		/// <summary>
		/// Adds or updates an item in the cache.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void CreateEntry(TKey key, TValue value)
		{
			lock (_lock)
			{
				ThrowIfDisposed();
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
		}

		/// <summary>
		/// Clears the cache, removing all items.
		/// </summary>
		public void Clear()
		{
			lock (_lock)
			{
				ThrowIfDisposed();
				_cache.Clear();
				_list.Clear();
			}
		}

		/// <summary>
		/// Disposes the cache by clearing all stored entries.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); // Prevent finalizer from running
		}
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				// Free managed resources
				Clear();
			}

			// Free unmanaged resources here if you had any

			_disposed = true;
		}

		// Optional: Only needed if you ever add unmanaged resources
		~LRUCache()
		{
			Dispose(false);
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(LRUCache<TKey, TValue>));
		}
	}
}
