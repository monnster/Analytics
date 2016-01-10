using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NLog;

namespace Analytics.Core
{
	/// <summary>
	/// Set of extension methods for Type
	/// </summary>
	public static class TypeExtensions
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// If <paramref name="source"/> is generic type, get the type of it's generic argument (recursive)
		/// </summary>
		public static Type GetGenericArgumentType(this Type source)
		{
			Argument.NotNull(source, "Source type is required");

			if (!source.IsGenericType)
				return source;

			var genericArgument = source.GetGenericArguments()[0];

			if (genericArgument.IsGenericType)
				return genericArgument.GetGenericArgumentType();

			return genericArgument;
		}

		/// <summary>
		/// Equivalent to default(..) language construct, but non-generic
		/// </summary>
		public static object GetDefaultValue(this Type type)
		{
			Argument.NotNull(type, "Source type is required");

			if (type.IsValueType)
				return Activator.CreateInstance(type);

			return null;
		}

		/// <summary>
		/// Try convert object to strongly-typed value
		/// </summary>
		public static bool TryConvert<T>(this object value, out T result)
		{
			return value.TryConvert(out result, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Checks if specified type is Nullable{T}.
		/// </summary>
		public static bool IsNullable(this Type type)
		{
			return Nullable.GetUnderlyingType(type) != null;
		}

		/// <summary>
		/// Try convert object to strongly-typed value using given format provider
		/// </summary>
		public static bool TryConvert<T>(this object value, out T result, IFormatProvider formatProvider)
		{
			Type destType;
			if ((destType = Nullable.GetUnderlyingType(typeof(T))) == null)
			{
				destType = typeof(T);
			}

			try
			{
				result = (T)Convert.ChangeType(value, destType, formatProvider);

				return true;
			}
			catch (InvalidCastException)
			{
				_logger.Debug("Error converting value: cannot cast field value '{0}' to required object type {1}"
					.Fill(value, typeof(T)));
			}
			catch (FormatException)
			{
				_logger.Debug("Error converting value: field value '{0}' was not in correct format for type {1}"
					.Fill(value, typeof(T)));
			}
			catch (OverflowException)
			{
				_logger.Debug("Error converting value: overflow exception encountered while converting field value '{0}' to required object type {1}"
					.Fill(value, typeof(T)));
			}

			result = default(T);
			return false;
		}

		public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
		{
			Argument.NotNull(assembly, "Assembly is requred");
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(t => t != null);
			}
		}

		public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == cur)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}
	}


}