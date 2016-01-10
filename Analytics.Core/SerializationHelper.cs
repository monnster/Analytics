using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Analytics.Core
{
	/// <summary>
	/// Provides serialization utilities.
	/// </summary>
	public static class SerializationHelper
	{
		private static JsonSerializerSettings _enumToStringSerializerSettings = new JsonSerializerSettings();

		static SerializationHelper()
		{
			_enumToStringSerializerSettings.Converters.Add(new StringEnumConverter());
		}

		/// <summary>
		/// Serializes an entity using binary serialization. To deserialize the object use
		/// <see cref="DeserializeBinary{T}"/>.
		/// </summary>
		/// <param name="entity">Entity to serialize.</param>
		/// <returns>Binary data.</returns>
		public static byte[] SerializeBinary(object entity)
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new BinaryFormatter();
				serializer.Serialize(stream, entity);
				return stream.ToArray();
			}
		}

		/// <summary>
		/// Deserializes an entity serialized by <see cref="SerializeBinary"/>.
		/// </summary>
		/// <typeparam name="T">Entity type.</typeparam>
		/// <param name="data">Binary data.</param>
		/// <returns>Deserialized data.</returns>
		public static T DeserializeBinary<T>(byte[] data)
		{
			using (var stream = new MemoryStream(data))
			{
				var serializer = new BinaryFormatter();
				return (T)serializer.Deserialize(stream);
			}
		}

		/// <summary>
		/// Serializes object usin JSON serializer.
		/// </summary>
		public static string SerializeJson<T>(T entity)
		{
			return JsonConvert.SerializeObject(entity);
		}

		/// <summary>
		/// Serializes object usin JSON serializer.
		/// </summary>
		public static string SerializeJsonEx<T>(T entity, bool indent, bool enumToString = false)
		{
			return JsonConvert.SerializeObject(
				entity,
				indent
					? Formatting.Indented
					: Formatting.None,
				enumToString
					? _enumToStringSerializerSettings
					: null);
		}

		/// <summary>
		/// Deserializes object from string using JSON serializer.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="str"></param>
		/// <returns></returns>
		public static T DeserializeJson<T>(string str)
		{
			return JsonConvert.DeserializeObject<T>(str ?? "");
		}
	}
}