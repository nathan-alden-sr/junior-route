using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class ContentLanguageHeader
	{
		private const string LanguageTagRegexPattern = PrimaryTagRegexPattern + "(?:-" + SubtagRegexPattern + ")*";
		private const string PrimaryTagRegexPattern = "[" + CommonRegexPatterns.Alpha + "]{1,8}";
		private const string SubtagRegexPattern = "[" + CommonRegexPatterns.Alpha + "]{1,8}";
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(LanguageTagRegexPattern, 1));
		private readonly string _languageTag;

		private ContentLanguageHeader(string languageTag)
		{
			languageTag.ThrowIfNull("languageTag");

			_languageTag = languageTag;
		}

		public string LanguageTag
		{
			get
			{
				return _languageTag;
			}
		}

		public static IEnumerable<ContentLanguageHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<ContentLanguageHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern) ? headerValue.SplitElements().Select(arg => new ContentLanguageHeader(arg)) : Enumerable.Empty<ContentLanguageHeader>();
		}
	}
}