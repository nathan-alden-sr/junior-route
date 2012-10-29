using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class AcceptCharsetHeader
	{
		private const string CharsetRegexPattern = "(?:" + CommonRegexPatterns.Charset + @"|\*)";
		private const string RegexPattern = CharsetRegexPattern + @"(?:;" + CommonRegexPatterns.SP + "*(?:" + CommonRegexPatterns.Q + "=(?:" + CommonRegexPatterns.Qvalue + "))?)?";
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly string _charset;
		private readonly decimal _effectiveQvalue;
		private readonly decimal? _qvalue;

		private AcceptCharsetHeader(string charset, decimal? qvalue)
		{
			charset.ThrowIfNull("charset");

			_charset = charset;
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
		}

		public string Charset
		{
			get
			{
				return _charset;
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

		public bool CharsetMatches(string charset)
		{
			charset.ThrowIfNull("charset");

			if (!Regex.IsMatch(charset, CommonRegexPatterns.Charset))
			{
				throw new ArgumentException("Invalid charset.", "charset");
			}
			if (_effectiveQvalue == 0m)
			{
				return false;
			}

			return _charset == "*" || String.Equals(_charset, charset, StringComparison.OrdinalIgnoreCase);
		}

		public static IEnumerable<AcceptCharsetHeader> ParseMany(string headerValue)
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

				yield return new AcceptCharsetHeader(semicolonSplitParts[0], qvalue);
			}
		}
	}
}