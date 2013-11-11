using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class UserAgentHeader
	{
		private const string RegexPattern = "^(?:" + CommonRegexPatterns.Product + "|" + CommonRegexPatterns.Comment + ")(?:" + CommonRegexPatterns.SP + "+(?:" + CommonRegexPatterns.Product + "|" + CommonRegexPatterns.Comment + "))*$";
		private readonly string _product;

		private UserAgentHeader(string product)
		{
			product.ThrowIfNull("product");

			_product = product;
		}

		public string Product
		{
			get
			{
				return _product;
			}
		}

		public static IEnumerable<UserAgentHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<UserAgentHeader>();
			}

			return Regex.IsMatch(headerValue, RegexPattern)
				? headerValue.SplitSpaces()
					.Where(arg => !Regex.IsMatch(arg, CommonRegexPatterns.Comment))
					.Select(arg => new UserAgentHeader(arg))
				: Enumerable.Empty<UserAgentHeader>();
		}
	}
}