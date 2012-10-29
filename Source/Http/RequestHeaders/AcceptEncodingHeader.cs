using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class AcceptEncodingHeader
	{
		private const string CodingsRegexPattern = "(?:" + CommonRegexPatterns.ContentCoding + @"|\*)";
		private const string RegexPattern = CodingsRegexPattern + @"(?:;" + CommonRegexPatterns.SP + "*(?:" + CommonRegexPatterns.Q + "=(?:" + CommonRegexPatterns.Qvalue + "))?)?";
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly string _contentCoding;
		private readonly decimal _effectiveQvalue;
		private readonly decimal? _qvalue;

		private AcceptEncodingHeader(string contentCoding, decimal? qvalue)
		{
			contentCoding.ThrowIfNull("contentCoding");

			_contentCoding = contentCoding;
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
		}

		public string ContentCoding
		{
			get
			{
				return _contentCoding;
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

		public bool ContentCodingMatches(string contentCoding)
		{
			contentCoding.ThrowIfNull("contentCoding");

			if (!Regex.IsMatch(contentCoding, CodingsRegexPattern))
			{
				throw new ArgumentException("Invalid content-coding.", "contentCoding");
			}
			if (_effectiveQvalue == 0m)
			{
				return false;
			}

			return _contentCoding == "*" || String.Equals(_contentCoding, contentCoding, StringComparison.OrdinalIgnoreCase);
		}

		public static IEnumerable<AcceptEncodingHeader> ParseMany(string headerValue)
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

				yield return new AcceptEncodingHeader(semicolonSplitParts[0], qvalue);
			}
		}
	}
}