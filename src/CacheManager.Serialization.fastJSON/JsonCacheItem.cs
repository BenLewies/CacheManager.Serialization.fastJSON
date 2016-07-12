using CacheManager.Core;
using System;

namespace CacheManager.Serialization.fastJSON
{
	/// <summary>
	/// The json cache item will be used to serialize a <see cref="CacheItem{T}"/>.			   
	/// </summary>
	internal class JsonCacheItem
	{
		public DateTime CreatedUtc { get; set; }

		public ExpirationMode ExpirationMode { get; set; }

		public TimeSpan ExpirationTimeout { get; set; }

		public string Key { get; set; }

		public DateTime LastAccessedUtc { get; set; }

		public string Region { get; set; }

		public byte[] Value { get; set; }

		public static JsonCacheItem FromCacheItem<TCacheValue>(CacheItem<TCacheValue> item, byte[] value)
			=> new JsonCacheItem()
			{
				CreatedUtc = item.CreatedUtc,
				ExpirationMode = item.ExpirationMode,
				ExpirationTimeout = item.ExpirationTimeout,
				Key = item.Key,
				LastAccessedUtc = item.LastAccessedUtc,
				Region = item.Region,
				Value = value
			};

		public CacheItem<T> ToCacheItem<T>(object value)
		{
			var item = string.IsNullOrWhiteSpace(this.Region) ?
				new CacheItem<T>(this.Key, (T)value, this.ExpirationMode, this.ExpirationTimeout) :
				new CacheItem<T>(this.Key, this.Region, (T)value, this.ExpirationMode, this.ExpirationTimeout);

			item.LastAccessedUtc = this.LastAccessedUtc;

			return item.WithCreated(this.CreatedUtc);
		}
	}
}
