using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class AcceptLanguageHeader
	{
		private const string LanguageRangeRegexPattern = "[" + CommonRegexPatterns.Alpha + "]{1,8}(-(?:[" + CommonRegexPatterns.Alpha + "]{1,8}))?";
		private const string LanguageRegexPattern = "(?:(?:" + LanguageRangeRegexPattern + @")|\*)";
		private const string RegexPattern = LanguageRegexPattern + @"(?:;" + CommonRegexPatterns.SP + "*(?:" + CommonRegexPatterns.Q + "=(?:" + CommonRegexPatterns.Qvalue + "))?)?";
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly decimal _effectiveQvalue;
		private readonly string _language;
		private readonly string _languagePrefix;
		private readonly string _languageRange;
		private readonly decimal? _qvalue;

		private AcceptLanguageHeader(string languagePrefix, string language, decimal? qvalue)
		{
			language.ThrowIfNull("language");

			_languagePrefix = languagePrefix;
			_language = language;
			_languageRange = (languagePrefix.IfNotNull(arg => arg + "-") ?? "") + language;
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
		}

		public string LanguagePrefix
		{
			get
			{
				return _languagePrefix;
			}
		}

		public string Language
		{
			get
			{
				return _language;
			}
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

		public bool LanguageRangeMatches(string languageRange)
		{
			languageRange.ThrowIfNull("languageRange");

			if (!Regex.IsMatch(languageRange, LanguageRegexPattern))
			{
				throw new ArgumentException("Invalid language-range.", "languageRange");
			}
			if (_effectiveQvalue == 0m)
			{
				return false;
			}

			string languagePrefix;
			string language;

			GetLanguageParts(languageRange, out languagePrefix, out language);

			return _languageRange == "*" || String.Equals(_languageRange, languageRange, StringComparison.OrdinalIgnoreCase) || String.Equals(_languagePrefix, languageRange, StringComparison.OrdinalIgnoreCase);
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

				string languagePrefix;
				string language;

				GetLanguageParts(semicolonSplitParts[0], out languagePrefix, out language);

				yield return new AcceptLanguageHeader(languagePrefix, language, qvalue);
			}
		}

		private static void GetLanguageParts(string langugageRange, out string languagePrefix, out string language)
		{
			string[] languageParts = langugageRange.Split('-');

			languagePrefix = languageParts.Length == 2 ? languageParts[0] : null;
			language = languageParts[languageParts.Length == 1 ? 0 : 1];
		}
	}
}