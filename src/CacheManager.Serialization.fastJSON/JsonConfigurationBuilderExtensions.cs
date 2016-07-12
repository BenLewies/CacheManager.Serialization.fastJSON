using CacheManager.Core;
using fastJSON;
using static CacheManager.Core.Utility.Guard;

namespace CacheManager.Serialization.fastJSON
{
	/// <summary>
	/// Extensions for the configuration builder for the <code>fastJSON</code> based <see cref="CacheManager.Core.Internal.ICacheSerializer"/>.
	/// </summary>
	public static class JsonConfigurationBuilderExtensions
	{
		/// <summary>
		/// Configures the cache manager to use the <code>fastJSON</code> based cache serializer.
		/// </summary>
		/// <param name="part">The configuration part.</param>
		/// <returns>The builder instance.</returns>
		public static ConfigurationBuilderCachePart WithfastJSONSerializer(this ConfigurationBuilderCachePart part)
		{
			NotNull(part, nameof(part));

			return part.WithSerializer(typeof(fastJSONCacheSerializer));
		}

		/// <summary>
		/// Configures the cache manager to use the <code>fastJSON</code> based cache serializer.
		/// </summary>
		/// <param name="part">The configuration part.</param>
		/// <param name="fastJsonSettings">The settings to be used during serialization/deserialization.</param>	
		/// <returns>The builder instance.</returns>
		public static ConfigurationBuilderCachePart WithfastJSONSerializer(this ConfigurationBuilderCachePart part, JSONParameters fastJsonSettings)
		{
			NotNull(part, nameof(part));

			return part.WithSerializer(typeof(fastJSONCacheSerializer), fastJsonSettings);
		}

		/// <summary>
		/// Configures the cache manager to use the <code>fastJSON</code> based cache serializer with compression.
		/// </summary>
		/// <param name="part">The configuration part.</param>
		/// <returns>The builder instance.</returns>
		public static ConfigurationBuilderCachePart WithGzJsonSerializer(this ConfigurationBuilderCachePart part)
		{
			NotNull(part, nameof(part));

			return part.WithSerializer(typeof(GzJsonCacheSerializer));
		}

		/// <summary>
		/// Configures the cache manager to use the <code>fastJSON</code> based cache serializer with compression.
		/// </summary>
		/// <param name="part">The configuration part.</param>
		/// <param name="fastJsonSettings">The settings to be used during serialization/deserialization.</param>  
		/// <returns>The builder instance.</returns>
		public static ConfigurationBuilderCachePart WithGzJsonSerializer(this ConfigurationBuilderCachePart part, JSONParameters fastJsonSettings)
		{
			NotNull(part, nameof(part));

			return part.WithSerializer(typeof(GzJsonCacheSerializer), fastJsonSettings);
		}
	}
}
