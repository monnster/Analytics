using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Analytics.Core
{
	public static class XmlExtensions
	{
		/// <summary>
		/// Read value of attribute with specified name.
		/// </summary>
		public static T GetValueOrDefault<T>(this XElement element, string attributeName, T defaultValue = default (T), string format = null)
		{
			var attr = element.Attribute(attributeName);
			if (null == attr)
				return defaultValue;

			return GetValue(attr.Value, defaultValue, format);
		}

		/// <summary>
		/// Read value of descendant node with specified name.
		/// </summary>
		public static T GetNodeValueOrDefault<T>(this XElement root, string nodeName, T defaultValue = default(T), string format = null)
		{
			var node = root.Descendants(nodeName).SingleOrDefault();
			if (null == node)
				return defaultValue;

			return GetValue(node.Value, defaultValue, format);
		}

		private static T GetValue<T>(string value, T defaultValue, string format)
		{
			if (string.IsNullOrEmpty(value) && typeof(T).IsNullable())
				return defaultValue;

			if (typeof(T) == typeof(DateTime) && !string.IsNullOrEmpty(format))
			{
				return (T)(object)DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
			}

			T converted;
			value.TryConvert(out converted, CultureInfo.InvariantCulture);

			return converted;
		}

	}
}