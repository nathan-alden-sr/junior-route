using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class AcceptLanguageHeader
	{
		private const string CharsetRegexPattern = "(?:(?:" + LanguageRangeRegexPattern + @")|\*)";
		private const string LanguageRangeRegexPattern = "[" + CommonRegexPatterns.Alpha + "]{1,8}(-(?:[" + CommonRegexPatterns.Alpha + "]{1,8}))?";
		private const string RegexPattern = CharsetRegexPattern + @"(?:;" + CommonRegexPatterns.SP + "*(?:" + CommonRegexPatterns.Q + "=(?:" + CommonRegexPatterns.Qvalue + "))?)?";
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly decimal _effectiveQvalue;
		private readonly string _languageRange;
		private readonly decimal? _qvalue;

		private AcceptLanguageHeader(string languageRange, decimal? qvalue)
		{
			languageRange.ThrowIfNull("languageRange");

			_languageRange = languageRange;
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
		}

		public string LanguageRange
		{
			get
			{
				return _languageRange;
			}
		}

		public decimal? Qvalue
		{
			get
			{
				return _qvalue;
			}
		}

		public decimal EffectiveQvalue
		{
			get
			{
				return _effectiveQvalue;
			}
		}

		public static IEnumerable<AcceptLanguageHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _elementsRegexPattern))
			{
				yield break;
			}

			string[] elements = headerValue.SplitElements();

			foreach (string element in elements)
			{
				string[] semicolonSplitParts = element.SplitSemicolons();
				decimal? qvalue = null;

				if (semicolonSplitParts.Length == 2)
				{
					string[] equalSignSplitParts = semicolonSplitParts[1].SplitEqualSign();
					QvalueValidity qvalueValidity = equalSignSplitParts.ValidateQvalue();

					if (qvalueValidity != QvalueValidity.Valid)
					{
						continue;
					}

					qvalue = Decimal.Parse(equalSignSplitParts[1], NumberStyles.AllowDecimalPoint);
				}

				yield return new AcceptLanguageHeader(semicolonSplitParts[0], qvalue);
			}
		}
	}
}