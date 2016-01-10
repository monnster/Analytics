using System;

namespace Analytics.Core.Utils
{
	public static class UidTool
	{
		public static string Next()
		{
			return Guid.NewGuid().ToString("n");
		}

		public static string GetTicksHex()
		{
			var historicalDate = new DateTime(1970, 1, 1, 0, 0, 0);
			var spanTillNow = DateTime.UtcNow.Subtract(historicalDate);
			var id = String.Format("{0:0}", spanTillNow.TotalSeconds);

			return long.Parse(id).ToString("X");
		}
	}

}