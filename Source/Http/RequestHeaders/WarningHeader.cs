using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class WarningHeader
	{
		private const string WarnAgentRegexPattern = "(?:" + CommonRegexPatterns.Hostport + "|" + CommonRegexPatterns.Pseudonym + ")";
		private const string WarnCodeRegexPattern = "[" + CommonRegexPatterns.Digit + "]{3}";
		private const string WarnDateRegexPattern = @"""" + CommonRegexPatterns.HttpDate + @"""";
		private const string WarnTextRegexPattern = CommonRegexPatterns.QuotedString;
		private const string WarningValueRegexPattern = WarnCodeRegexPattern + CommonRegexPatterns.SP + WarnAgentRegexPattern + CommonRegexPatterns.SP + WarnTextRegexPattern + "(?:" + CommonRegexPatterns.SP + WarnDateRegexPattern + ")?";
		private static readonly string _elementsRegexPattern = String.Format("^(?:{0})$", CommonRegexPatterns.ListOfElements(WarningValueRegexPattern, 1));
		private readonly string _warnAgent;
		private readonly int _warnCode;
		private readonly DateTime? _warnDate;
		private readonly string _warnText;

		private WarningHeader(int warnCode, string warnAgent, string warnText, DateTime? warnDate)
		{
			warnAgent.ThrowIfNull("warnAgent");
			warnText.ThrowIfNull("warnText");
			if (warnDate != null && warnDate.Value.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("warn-date must be UTC.", "warnDate");
			}

			_warnCode = warnCode;
			_warnAgent = warnAgent;
			_warnText = warnText;
			_warnDate = warnDate;
		}

		public int WarnCode
		{
			get
			{
				return _warnCode;
			}
		}

		public string WarnAgent
		{
			get
			{
				return _warnAgent;
			}
		}

		public string WarnText
		{
			get
			{
				return _warnText;
			}
		}

		public DateTime? WarnDate
		{
			get
			{
				return _warnDate;
			}
		}

		public static IEnumerable<WarningHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<WarningHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern)
				       ? headerValue.SplitElements()
							 .Select(arg => arg.SplitOnSpacesOutsideQuotes().ToArray())
							 .Select(arg => new WarningHeader(Int32.Parse(arg[0]), arg[1], arg[2].RemoveOptionalQuotes(), arg.Length == 4 ? arg[3].RemoveOptionalQuotes().ParseHttpDate() : null))
					   : Enumerable.Empty<WarningHeader>();
		}
	}
}