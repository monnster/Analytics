using System;
using System.Configuration;
using System.Dynamic;

namespace Analytics.Core.Utils
{
	public class FluentConfiguration : DynamicObject
	{
		private readonly string _path;
		private static readonly Lazy<FluentConfiguration> _instance
			= new Lazy<FluentConfiguration>(() => new FluentConfiguration(null));

		private FluentConfiguration(string path)
		{
			_path = path;
		}

		public static FluentConfiguration Instance
		{
			get { return _instance.Value; }
		}

		#region DynamicObject overrides

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var memberName = string.IsNullOrEmpty(_path)
				? binder.Name
				: "{0}.{1}".Fill(_path, binder.Name);

			result = new FluentConfiguration(memberName);
			return true;
		}

		#endregion


		public override string ToString()
		{
			return _path;
		}

		#region Implicit convertations

		public static implicit operator string(FluentConfiguration value)
		{
			return ConfigurationManager.AppSettings[value._path];
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "This special implicit converter for fluent work with XML data")]
		public static implicit operator long(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			long result;
			if (long.TryParse(strValue, out result))
			{
				return result;
			}
			throw new FormatException("Can't convert \"{0}\" to Int64.".Fill(strValue));
		}

		public static implicit operator long?(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			long result;
			if (long.TryParse(strValue, out result))
			{
				return result;
			}
			return null;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "This special implicit converter for fluent work with XML data")]
		public static implicit operator int(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			int result;
			if (int.TryParse(strValue, out result))
			{
				return result;
			}
			throw new FormatException("Can't convert \"{0}\" to Int32.".Fill(strValue));
		}

		public static implicit operator int?(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			int result;
			if (int.TryParse(strValue, out result))
			{
				return result;
			}
			return null;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
		public static implicit operator bool(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			bool result;
			if (bool.TryParse(strValue, out result))
			{
				return result;
			}
			throw new FormatException("Can't convert \"{0}\" to Boolean.".Fill(strValue));
		}

		public static implicit operator bool?(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			bool result;
			if (bool.TryParse(strValue, out result))
			{
				return result;
			}
			return null;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "This special implicit converter for fluent work with XML data")]
		public static implicit operator decimal(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			decimal result;
			if (decimal.TryParse(strValue, out result))
			{
				return result;
			}
			throw new FormatException("Can't convert \"{0}\" to Int32.".Fill(strValue));
		}

		public static implicit operator decimal?(FluentConfiguration value)
		{
			var strValue = ConfigurationManager.AppSettings[value._path];
			decimal result;
			if (decimal.TryParse(strValue, out result))
			{
				return result;
			}
			return null;
		}

		#endregion
	}
}