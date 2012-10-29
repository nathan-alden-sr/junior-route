using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class ConnectionHeader
	{
		private const string ConnectionTokenRegexPattern = CommonRegexPatterns.Token;
		private const string RegexPattern = ConnectionTokenRegexPattern;
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly string _connectionToken;

		private ConnectionHeader(string connectionToken)
		{
			connectionToken.ThrowIfNull("connectionToken");

			_connectionToken = connectionToken;
		}

		public string ConnectionToken
		{
			get
			{
				return _connectionToken;
			}
		}

		public static IEnumerable<ConnectionHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<ConnectionHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern) ? headerValue.SplitElements().Select(arg => new ConnectionHeader(arg)) : Enumerable.Empty<ConnectionHeader>();
		}
	}
}