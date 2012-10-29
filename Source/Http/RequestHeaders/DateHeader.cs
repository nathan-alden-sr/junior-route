using System;

namespace Junior.Route.Http.RequestHeaders
{
	public class DateHeader
	{
		private readonly DateTime _httpDate;

		private DateHeader(DateTime httpDate)
		{
			if (httpDate.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("http-date must be UTC.", "httpDate");
			}

			_httpDate = httpDate;
		}

		public DateTime HttpDate
		{
			get
			{
				return _httpDate;
			}
		}

		public static DateHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			DateTime? date = headerValue.ParseHttpDate();

			return date != null ? new DateHeader(date.Value) : null;
		}
	}
}