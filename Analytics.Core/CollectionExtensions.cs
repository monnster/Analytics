using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Analytics.Core
{
	public static class CollectionExtensions
	{
		/// <summary>
		/// Apply action to each element in collection.
		/// </summary>
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			Argument.NotNull(collection, "Collection is required");

			foreach (var item in collection)
				action(item);
		}

		/// <summary>
		/// Replaces one list with another.
		/// </summary>
		/// <param name="source">List which values you want to replace.</param>
		/// <param name="newValues">New values for the source list.</param>
		public static void Replace<T>(this IList<T> source, IEnumerable<T> newValues)
		{
			Argument.NotNull(source, "Source collection is required.");
			Argument.NotNull(newValues, "Collection with new values is required.");

			source.Clear();
			newValues.ForEach(source.Add);
		}

		/// <summary>
		/// Replaces one dictionary with another.
		/// </summary>
		/// <param name="source">Dictionary which values you wish to replace.</param>
		/// <param name="newValues">New values for source dictionary.</param>
		public static void Replace<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> newValues)
		{
			Argument.NotNull(source, "Source dictionary is required.");
			Argument.NotNull(newValues, "Dictionary with new values is required.");

			source.Clear();
			newValues.ForEach(pair => source.Add(pair.Key, pair.Value));
		}

		/// <summary>
		/// Converts NameValueCollection to lookup.
		/// </summary>
		/// <param name="collection">Collection to be converted.</param>
		/// <returns>Lookup containing converted values.</returns>
		public static ILookup<string, string> ToLookup(this NameValueCollection collection)
		{
			Argument.NotNull(collection, "Collection is required.");

			var pairs =
				from key in collection.Cast<String>()
				from value in collection.GetValues(key)
				select new { key, value };

			return pairs.ToLookup(pair => pair.key, pair => pair.value);
		}

		/// <summary>
		/// Gets dictionary value for specified key, or default value if key not found.
		/// </summary>
		/// <param name="collection">Collection to be searched.</param>
		/// <param name="key">Collection key.</param>
		/// <param name="defaultValue">Default value to return if key not found.</param>
		public static TValue GetValueOrDefault<TKey, TValue>(
			this IDictionary<TKey, TValue> collection,
			TKey key,
			TValue defaultValue = default(TValue)
			)
		{
			Argument.NotNull(collection, "Collection is required.");

			TValue value;

			if (!collection.TryGetValue(key, out value))
				value = defaultValue;

			return value;
		}

		/// <summary>
		/// Merges first dictionary with second.
		/// If an entry with the same key is found in both dictionaries, it will be replaced with the value from second one
		/// </summary>
		public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
		{
			Argument.NotNull(first, "Dictionary to be merged is required");
			Argument.NotNull(second, "Dictionary being merged with is required");

			foreach (var entry in second)
			{
				first[entry.Key] = entry.Value;
			}
		}

		//public static bool Equals<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
		//	where TValue: IEquatable<TValue>
		//{
		//	Argument.NotNull(first, "First dictionary to compare is required.");
		//	Argument.NotNull(first, "Second dictionary to compare is required.");

		//	foreach (var entry in second)
		//	{
		//		TValue value;
		//		if (first.TryGetValue(entry.Key, out value))
		//		{
		//			if(value != entry.Value)
		//		}
		//	}

		//}

	}

}