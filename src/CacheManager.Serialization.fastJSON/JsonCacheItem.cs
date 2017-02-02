using System;
using CacheManager.Core;

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
			=> new JsonCacheItem
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
			var item = string.IsNullOrWhiteSpace(Region) ?
				new CacheItem<T>(Key, (T)value, ExpirationMode, ExpirationTimeout) :
				new CacheItem<T>(Key, Region, (T)value, ExpirationMode, ExpirationTimeout);

			item.LastAccessedUtc = LastAccessedUtc;

			return item.WithCreated(CreatedUtc);
		}
	}
}
