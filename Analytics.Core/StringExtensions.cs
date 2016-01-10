using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Analytics.Core
{
	/// <summary>
	/// Provides a set of string extensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Replaces the format item in a this string with the string representation of 
		/// a corresponding object in a specified array with <see cref="CultureInfo.InvariantCulture" />
		/// format provider.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <returns></returns>
		[StringFormatMethod("format"), NotNull]
		public static string Fill([NotNull] this string format, [NotNull] params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

		/// <summary>
		/// Scans string with specified regular expression and applies specified action for found groups.
		/// </summary>
		/// <param name="value">A string.</param>
		/// <param name="regex">A regular expression.</param>
		/// <param name="action">A performed action.</param>
		public static void Scan([NotNull] this string value, [NotNull] string regex, [NotNull] Action<string> action)
		{
			var matches = Regex.Matches(value, regex);
			for (var i = 0; i < matches.Count; i++)
			{
				var match = matches[i];
				action(match.Groups[1].Value);
			}
		}

		/// <summary>
		/// Scans string with specified regular expression and applies specified action for found groups.
		/// </summary>
		/// <param name="value">A string.</param>
		/// <param name="regex">A regular expression.</param>
		/// <param name="action">A performed action.</param>
		public static void Scan([NotNull] this string value, [NotNull] string regex, [NotNull] Action<string, string> action)
		{
			var matches = Regex.Matches(value, regex);
			for (var i = 0; i < matches.Count; i++)
			{
				var match = matches[i];
				action(match.Groups[1].Value, match.Groups[2].Value);
			}
		}

		/// <summary>
		/// Scans string with specified regular expression and applies specified action for found groups.
		/// </summary>
		/// <param name="value">A string.</param>
		/// <param name="regex">A regular expression.</param>
		/// <param name="action">A performed action.</param>
		public static void Scan(
			[NotNull] this string value,
			[NotNull] string regex,
			[NotNull] Action<string, string, string> action)
		{
			var matches = Regex.Matches(value, regex);
			for (var i = 0; i < matches.Count; i++)
			{
				var match = matches[i];
				action(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
			}
		}

		/// <summary>
		/// Checks if string matches regular expression.
		/// </summary>
		/// <param name="value">A string.</param>
		/// <param name="regex">A regular expression.</param>
		/// <returns>Match result.</returns>
		public static bool IsMatch(
			[NotNull] this string value,
			[NotNull] string regex)
		{
			return new Regex(regex).IsMatch(value);
		}

		/// <summary>
		/// Determines is this and other string are eqalus with StringComparison.OrdinalIgnoreCase.
		/// </summary>
		public static bool EqualsIgnoreCase(this string value, string otherValue)
		{
			return string.Equals(value, otherValue, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Intent string and substrings.
		/// </summary>
		/// <param name="value">A string.</param>
		/// <param name="indent">An indent string.</param>
		/// <returns>A string with specified indents.</returns>
		/// <remarks>
		/// For example (indent: "  "):
		///   Input:  "a string\r\n  other sting"
		///   Output: "  a string\r\n    other sting"
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "1#")]
		public static string Indent(this string value, [NotNull] string indent)
		{
			return string.Join("\n", value.Split('\n').Select(v => indent + v));
		}

		public static bool ContainsIgnoreCase(this string value, string pattern)
		{
			return value.ToUpper(CultureInfo.CurrentCulture).Contains(pattern.ToUpper(CultureInfo.CurrentCulture));
		}

		/// <summary>
		/// Shourtcut for string.Join().
		/// </summary>
		/// <returns></returns>
		public static string Join(this IEnumerable<string> collection, string separator)
		{
			return string.Join(separator, collection);
		}

		/// <summary>
		/// Converts byte array to string.
		/// </summary>
		public static string BytesToString(this byte[] bytes)
		{
			Argument.NotNull(bytes, "Bytes are required.");

			var sb = new StringBuilder();
			foreach (var element in bytes)
			{
				sb.Append(element.ToString("x2", CultureInfo.InvariantCulture));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Converts a string like "FAC016" to byte array [250, 192, 22]
		/// </summary>
		public static byte[] StringToBytes(this string str)
		{
			if (string.IsNullOrEmpty(str))
				return new byte[0];

			if (str.Length % 2 == 1)
				throw new InvalidOperationException("String '{0}' cannot be converted to byte array.".Fill(str));

			var byteArr = new byte[str.Length / 2];
			var i = 0;
			var j = 0;
			do
			{
				var val = Convert.ToByte(str.Substring(i, 2), 16);
				byteArr[j++] = val;
				i += 2;
			} while (i < str.Length);
			return byteArr;
		}

		public static string ToBase64(this string original)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(original);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string FromBase64(this string original)
		{
			var bytes = Convert.FromBase64String(original);
			return Encoding.UTF8.GetString(bytes);
		}


		/// <summary>
		/// Leaves only specified amount of chars from right side of string.
		/// ex.: "12345".Right(3) returns "345"
		/// ex.: "12345".Right(6) returns "12345"
		/// </summary>
		public static string Right(this string value, int size)
		{
			if (value.Length > size)
				return value.Substring(value.Length - size);

			return value;
		}

		/// <summary>
		/// Leaves only specified amount of chars from left side of string.
		/// ex.: "12345".Left(3) returns "123"
		/// ex.: "12345".Left(6) returns "12345"
		/// </summary>
		public static string Left(this string value, int size)
		{
			if (value.Length > size)
				return value.Substring(0, size);

			return value;

		}

		/// <summary>
		/// Converts string encoding.
		/// </summary>
		public static string ConvertEncoding(this string original, Encoding from, Encoding to)
		{
			var bytes = from.GetBytes(original);
			//var converted = Encoding.Convert(from, to, bytes);
			return to.GetString(bytes);
		}

		/// <summary>
		/// Converts string with wildcards to regex string.
		/// </summary>
		public static string WildcardToRegex(this string pattern)
		{
			return "^" + Regex.Escape(pattern)
							  .Replace(@"\*", ".*")
							  .Replace(@"\?", ".")
					   + "$";
		}

		/// <summary>
		/// Checks if string has a valid identifier
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool HasId(this string value)
		{
			return !string.IsNullOrEmpty(value); // uid default length
		}

		public static string RemoveXmlNamespaces(this string xml)
		{
			XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xml));

			return xmlDocumentWithoutNs.ToString();
		}

		private static XElement RemoveAllNamespaces(XElement xmlDocument)
		{
			if (!xmlDocument.HasElements)
			{
				var xElement = new XElement(xmlDocument.Name.LocalName);
				xElement.Value = xmlDocument.Value;

				foreach (XAttribute attribute in xmlDocument.Attributes())
					xElement.Add(attribute);

				return xElement;
			}
			return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
		}

		public static string FixDecimalSeparator(this string str)
		{
			var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			return str
				.Replace(",", decimalSeparator)
				.Replace(".", decimalSeparator);
		}
	}
}