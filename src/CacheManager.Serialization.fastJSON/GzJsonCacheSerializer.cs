﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using CacheManager.Core.Internal;
using CacheManager.Core.Utility;
using fastJSON;

namespace CacheManager.Serialization.fastJSON
{
	/// <summary>
	/// Implements the <see cref="ICacheSerializer"/> contract using <c>Newtonsoft.Json</c> and the <see cref="GZipStream "/> loseless compression.
	/// </summary>
	public class GzJsonCacheSerializer : FastJsonCacheSerializer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GzJsonCacheSerializer"/> class.
		/// </summary>
		public GzJsonCacheSerializer()
			: base(new JSONParameters())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GzJsonCacheSerializer"/> class.
		/// With this overload the settings for de-/serialization can be set independently.
		/// </summary>
		/// <param name="fastJsonSettings">The settings which should be used during serialization/deserialization.</param>
		public GzJsonCacheSerializer(JSONParameters fastJsonSettings)
		: base(fastJsonSettings)
		{
		}

		/// <inheritdoc/>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Is checked by GetString")]
		public override object Deserialize(byte[] data, Type target)
		{
			var compressedData = Decompression(data);

			return base.Deserialize(compressedData, target);
		}

		/// <inheritdoc/>
		public override byte[] Serialize<T>(T value)
		{
			var data = base.Serialize(value);

			return Compression(data);
		}

		/// <summary>
		/// Compress the serialized <paramref name="data"/> using <see cref="GZipStream "/>.
		/// </summary>
		/// <param name="data">The data which should be compressed.</param>
		/// <returns>The compressed data.</returns>
		protected virtual byte[] Compression(byte[] data)
		{
			Guard.NotNull(data, nameof(data));

			using (var bytesBuilder = new MemoryStream())
			{
				using (var gzWriter = new GZipStream(bytesBuilder, CompressionMode.Compress))
				{
					gzWriter.Write(data, 0, data.Length);
				}

				return bytesBuilder.ToArray();
			}
		}

		/// <summary>
		/// Decompress the <paramref name="compressedData"/> into the base serialized data.
		/// </summary>
		/// <param name="compressedData">The data which should be decompressed.</param>
		/// <returns>The uncompressed data.</returns>
		protected virtual byte[] Decompression(byte[] compressedData)
		{
			Guard.NotNull(compressedData, nameof(compressedData));

			using (var inputStream = new MemoryStream(compressedData))
			using (var gzReader = new GZipStream(inputStream, CompressionMode.Decompress))
			using (var bytesBuilder = new MemoryStream())
			{
				gzReader.CopyTo(bytesBuilder);
				return bytesBuilder.ToArray();
			}
		}
	}
}
