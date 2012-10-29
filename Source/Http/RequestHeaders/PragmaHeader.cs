using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class PragmaHeader
	{
		private const string ExtensionPragmaRegexPattern = CommonRegexPatterns.Token + "(?:=(?:" + CommonRegexPatterns.Token + "|" + CommonRegexPatterns.QuotedString + "))?";
		private const string RegexPattern = "(?:no-cache|" + ExtensionPragmaRegexPattern + ")";
		private static readonly string _elementsRegexPattern = String.Format("^(?:{0})$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly string _name;
		private readonly string _value;

		private PragmaHeader(string name, string value)
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

		public static IEnumerable<PragmaHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _elementsRegexPattern))
			{
				yield break;
			}

			string[] elements = headerValue.SplitElements();

			foreach (string element in elements)
			{
				string name;
				string value;

				element.GetParameterParts(out name, out value, true);

				yield return new PragmaHeader(name, value ?? "");
			}
		}
	}
}