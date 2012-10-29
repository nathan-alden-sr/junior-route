using System;
using System.Text.RegularExpressions;

namespace Junior.Route.Http.RequestHeaders
{
	public class IfRangeHeader
	{
		private readonly EntityTag _entityTag;
		private readonly DateTime? _httpDate;

		private IfRangeHeader(EntityTag entityTag, DateTime? httpDate)
		{
			if (entityTag == null && httpDate == null)
			{
				throw new ArgumentException("ETag and http-date cannot both be null.");
			}

			_entityTag = entityTag;
			_httpDate = httpDate;
		}

		public EntityTag EntityTag
		{
			get
			{
				return _entityTag;
			}
		}

		public DateTime? HttpDate
		{
			get
			{
				return _httpDate;
			}
		}

		public static IfRangeHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			return Regex.IsMatch(headerValue, CommonRegexPatterns.HttpDate) ? new IfRangeHeader(null, headerValue.ParseHttpDate()) : new IfRangeHeader(EntityTag.Parse(headerValue), null);
		}
	}
}