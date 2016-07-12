using System;
using System.Text;
using CacheManager.Core;
using CacheManager.Core.Internal;
using fastJSON;
using static CacheManager.Core.Utility.Guard;

namespace CacheManager.Serialization.fastJSON
{
	/// <summary>
	/// Implements the <see cref="ICacheSerializer"/> contract using <c>Newtonsoft.Json</c>.
	/// </summary>
	public class fastJSONCacheSerializer : ICacheSerializer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="fastJSONCacheSerializer"/> class.
		/// </summary>
		public fastJSONCacheSerializer()
			: this(new JSONParameters())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="fastJSONCacheSerializer"/> class.
		/// With this overload the settings for de-/serialization can be set independently.
		/// </summary>
		/// <param name="fastJsonSettings">The settings which should be used during serialization/deserialization.</param>	  
		public fastJSONCacheSerializer(JSONParameters fastJsonSettings)
		{
			NotNull(fastJsonSettings, nameof(fastJsonSettings));

			JSON.Parameters = fastJsonSettings;
		}

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Is checked by GetString")]
		public virtual object Deserialize(byte[] data, Type target)
		{
			var stringValue = Encoding.UTF8.GetString(data, 0, data.Length);

			return JSON.ToObject(stringValue, target);
		}

		/// <inheritdoc/>
		public CacheItem<T> DeserializeCacheItem<T>(byte[] value, Type valueType)
		{
			var jsonItem = (JsonCacheItem)this.Deserialize(value, typeof(JsonCacheItem));
			EnsureNotNull(jsonItem, "Could not deserialize cache item");

			var deserializedValue = this.Deserialize(jsonItem.Value, valueType);

			return jsonItem.ToCacheItem<T>(deserializedValue);
		}

		/// <inheritdoc/>
		public virtual byte[] Serialize<T>(T value)
		{
			var stringValue = JSON.ToJSON(value);
			return Encoding.UTF8.GetBytes(stringValue);
		}

		/// <inheritdoc/>
		public byte[] SerializeCacheItem<T>(CacheItem<T> value)
		{
			NotNull(value, nameof(value));
			var jsonValue = this.Serialize(value.Value);
			var jsonItem = JsonCacheItem.FromCacheItem(value, jsonValue);

			return this.Serialize(jsonItem);
		}
	}
}
