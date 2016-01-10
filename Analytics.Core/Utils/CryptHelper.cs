using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Analytics.Core.Utils
{
	/// <summary>
	/// Helper class to count md5-checksums.
	/// </summary>
	public static class CryptHelper
	{
		/// <summary>
		/// Get md5-hash for specified string.
		/// </summary>
		public static string Md5Hash(string input)
		{
			using (HashAlgorithm algorithm = MD5.Create())
			{
				return ComputeHash(input, algorithm);
			}
		}

		/// <summary>
		/// Get sha1-hash for specified string.
		/// </summary>
		public static string Sha1Hash(string input)
		{
			using (HashAlgorithm algorithm = SHA1.Create())
			{
				return ComputeHash(input, algorithm);
			}
		}

		/// <summary>
		/// Computes sha-512 hash for specified string.
		/// </summary>
		public static string Sha512Hash(string input)
		{
			using (HashAlgorithm algorithm = SHA512.Create())
			{
				return ComputeHash(input, algorithm);
			}
		}

		private static string ComputeHash(string input, HashAlgorithm algorithm)
		{
			Encoding encoding = new UTF8Encoding();

			var sb = new StringBuilder();
			foreach (var element in algorithm.ComputeHash(encoding.GetBytes(input)))
			{
				sb.Append(element.ToString("x2", CultureInfo.InvariantCulture));
			}
			return sb.ToString();
		}
	}
}