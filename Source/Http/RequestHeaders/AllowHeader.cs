using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class AllowHeader
	{
		private const string MethodRegexPattern = CommonRegexPatterns.Token;
		private const string RegexPattern = MethodRegexPattern;
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern));
		private readonly string _method;

		private AllowHeader(string method)
		{
			method.ThrowIfNull("charset");

			_method = method;
		}

		public string Method
		{
			get
			{
				return _method;
			}
		}

		public static IEnumerable<AllowHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<AllowHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern) ? headerValue.SplitElements().Select(arg => new AllowHeader(arg)) : Enumerable.Empty<AllowHeader>();
		}
	}
}