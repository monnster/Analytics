using System;
using NLog;

namespace Analytics.Core
{
	/// <summary>
	/// Helper methods for working with exceptions.
	/// </summary>
	public static class ExceptionHelper
	{
		/// <summary>
		/// Execute action and log exception if occured.
		/// </summary>
		/// <typeparam name="T">Type of return value.</typeparam>
		/// <param name="logger">Logger to write exception.</param>
		/// <param name="action">Action to execute.</param>
		/// <param name="faultHandler">Fault handler to execute in case of exception in action.</param>
		/// <returns>Either <paramref name="action"/> result, or <paramref name="faultHandler"/> result in case of exception.</returns>
		public static T SupressException<T>(this Logger logger, Func<T> action, Func<T> faultHandler)
		{
			try
			{
				return action();
			}
			catch (Exception ex)
			{
				logger.Error("Exception executing action.", ex);
				return faultHandler();
			}
		}

		/// <summary>
		/// Execute action and log exception if occured.
		/// </summary>
		/// <param name="logger">Logger to write exception.</param>
		/// <param name="action">Action to execute.</param>
		/// <param name="faultHandler">Fault handler to execute in case of exception in action.</param>
		public static void SupressException(this Logger logger, Action action, Action faultHandler)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				logger.Error("Exception executing action.", ex);
				faultHandler();
			}
		}
	}
}