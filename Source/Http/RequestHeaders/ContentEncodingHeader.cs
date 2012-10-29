using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class ContentEncodingHeader
	{
		private const string RegexPattern = CommonRegexPatterns.ContentCoding;
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly string _contentCoding;

		private ContentEncodingHeader(string contentCoding)
		{
			contentCoding.ThrowIfNull("contentCoding");

			_contentCoding = contentCoding;
		}

		public string ContentCoding
		{
			get
			{
				return _contentCoding;
			}
		}

		public static IEnumerable<ContentEncodingHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<ContentEncodingHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern) ? headerValue.SplitElements().Select(arg => new ContentEncodingHeader(arg)) : Enumerable.Empty<ContentEncodingHeader>();
		}
	}
}