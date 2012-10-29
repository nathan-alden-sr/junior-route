using System;

namespace Junior.Route.Http.RequestHeaders
{
	public class IfUnmodifiedSinceHeader
	{
		private readonly DateTime _httpDate;

		private IfUnmodifiedSinceHeader(DateTime httpDate)
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

		public static IfUnmodifiedSinceHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			DateTime? date = headerValue.ParseHttpDate();

			return date != null ? new IfUnmodifiedSinceHeader(date.Value) : null;
		}
	}
}