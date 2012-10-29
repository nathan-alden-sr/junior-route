using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class IfMatchHeader
	{
		private const string EntityTagRegexPattern = "(?:" + CommonRegexPatterns.EntityTag + "|" + CommonRegexPatterns.Token + ")";
		private static readonly string _elementsRegexPattern = String.Format(@"^(?:\*|{0})$", CommonRegexPatterns.ListOfElements(EntityTagRegexPattern, 1));
		private readonly EntityTag _entityTag;

		private IfMatchHeader(EntityTag entityTag)
		{
			entityTag.ThrowIfNull("entityTag");

			_entityTag = entityTag;
		}

		public EntityTag EntityTag
		{
			get
			{
				return _entityTag;
			}
		}

		public static IEnumerable<IfMatchHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _elementsRegexPattern))
			{
				return Enumerable.Empty<IfMatchHeader>();
			}

			return headerValue == "*"
				       ? new IfMatchHeader(new EntityTag("*", false)).ToEnumerable()
				       : headerValue.SplitElements().Select(arg => new IfMatchHeader(EntityTag.Parse(arg)));
		}
	}
}