using System;

namespace Analytics.Core
{
	/// <summary>
	/// Contains various necessary argument checks (Design-by-Contract).
	/// </summary>
	public static class Argument
	{
		/// <summary>
		/// Ensures that argument is not null.
		/// </summary>
		/// <param name="value">Pass argument you need to check.</param>
		/// <param name="message">Error message for the exception.</param>
		/// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
		[AssertionMethod]
		public static void NotNull(
			[AssertionCondition(AssertionConditionType.IsNotNull)] object value,
			[NotNull] string message
			)
		{
			if (null == value)
			{
				throw new ArgumentNullException(message);
			}
		}

		/// <summary>
		/// Ensures that string argument is not null and is not empty.
		/// </summary>
		/// <param name="value">Pass argument you need to check.</param>
		/// <param name="message">Error message for the exception.</param>
		/// <exception cref="ArgumentException"><paramref name="value" /> is <c>null</c>.</exception>
		[AssertionMethod]
		public static void NotNullOrEmpty(
			[AssertionCondition(AssertionConditionType.IsNotNull)]  string value,
			[NotNull] string message
			)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException(message);
			}
		}

		/// <summary>
		/// Perform custom check on the argument.
		/// </summary>
		/// <param name="condition">Condition to check.</param>
		/// <param name="errorMessage">Error message for the exception.</param>
		[AssertionMethod]
		public static void Require(
			[AssertionCondition(AssertionConditionType.IsTrue)] bool condition,
			[NotNull] string errorMessage)
		{
			if (!condition)
			{
				throw new ArgumentException(errorMessage);
			}
		}
	}
}