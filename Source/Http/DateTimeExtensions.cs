using System;

namespace Junior.Route.Http
{
	public static class DateTimeExtensions
	{
		public static string ToHttpDate(this DateTime value)
		{
			return value.ToString("ddd, dd MMM yyyy HH:mm:ss") + " GMT";
		}
	}
}