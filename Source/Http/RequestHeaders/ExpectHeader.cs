using System.Collections.Generic;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class ExpectHeader
	{
		private const string ExpectParamsRegexPattern = ";" + CommonRegexPatterns.SP + "*" + CommonRegexPatterns.Token + "(?:=(?:" + CommonRegexPatterns.Token + "|" + CommonRegexPatterns.QuotedString + "))?";
		private const string RegexPattern = "^(?:100-continue|" + CommonRegexPatterns.Token + "(?:=(?:" + CommonRegexPatterns.Token + "|" + CommonRegexPatterns.QuotedString + ")(?:" + ExpectParamsRegexPattern + ")*)?)$";
		private readonly string _name;
		private readonly string _value;

		private ExpectHeader(string name, string value)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			_name = name;
			_value = value;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public static IEnumerable<ExpectHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, RegexPattern))
			{
				yield break;
			}

			string[] semicolonSplitParts = headerValue.SplitSemicolons();

			foreach (string semicolonSplitPart in semicolonSplitParts)
			{
				string name;
				string value;

				semicolonSplitPart.GetParameterParts(out name, out value, true);

				yield return new ExpectHeader(name, value ?? "");
			}
		}
	}
}