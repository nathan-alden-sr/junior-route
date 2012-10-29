using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class IfNoneMatchHeader
	{
		private const string EntityTagRegexPattern = "(?:" + CommonRegexPatterns.EntityTag + "|" + CommonRegexPatterns.Token + ")";
		private static readonly string _elementsRegexPattern = String.Format(@"^(?:\*|{0})$", CommonRegexPatterns.ListOfElements(EntityTagRegexPattern, 1));
		private readonly EntityTag _entityTag;

		private IfNoneMatchHeader(EntityTag entityTag)
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

		public static IEnumerable<IfNoneMatchHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _elementsRegexPattern))
			{
				return Enumerable.Empty<IfNoneMatchHeader>();
			}

			return headerValue == "*"
				       ? new IfNoneMatchHeader(new EntityTag("*", false)).ToEnumerable()
				       : headerValue.SplitElements().Select(arg => new IfNoneMatchHeader(EntityTag.Parse(arg)));
		}
	}
}