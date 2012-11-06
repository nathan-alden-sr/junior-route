using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class TeHeader
	{
		private const string ParametersRegexPattern = @";" + CommonRegexPatterns.SP + "*" + CommonRegexPatterns.Token + "(?:=" + CommonRegexPatterns.Token + "|" + CommonRegexPatterns.QuotedString + ")?";
		private const string RegexPattern = "(?:trailers|(?:" + CommonRegexPatterns.TransferExtension + ")(?:" + ParametersRegexPattern + ")?)";
		private static readonly string _elementsRegexPattern = String.Format("^(?:{0})$", CommonRegexPatterns.ListOfElements(RegexPattern));
		private readonly decimal _effectiveQvalue;
		private readonly Parameter[] _parameters;
		private readonly decimal? _qvalue;
		private readonly string _tCoding;

		private TeHeader(string tCoding, decimal? qvalue, IEnumerable<Parameter> parameters)
		{
			tCoding.ThrowIfNull("tCoding");
			parameters.ThrowIfNull("parameters");

			_tCoding = tCoding;
			_parameters = parameters.ToArray();
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
		}

		public string TCoding
		{
			get
			{
				return _tCoding;
			}
		}

		public decimal? Qvalue
		{
			get
			{
				return _qvalue;
			}
		}

		public IEnumerable<Parameter> Parameters
		{
			get
			{
				return _parameters;
			}
		}

		public decimal EffectiveQvalue
		{
			get
			{
				return _effectiveQvalue;
			}
		}

		public bool TCodingMatches(string tCoding)
		{
			tCoding.ThrowIfNull("tCoding");

			if (!Regex.IsMatch(tCoding, CommonRegexPatterns.Token))
			{
				throw new ArgumentException("Invalid t-coding.", "tCoding");
			}

			return _effectiveQvalue > 0m && String.Equals(_tCoding, tCoding, StringComparison.OrdinalIgnoreCase);
		}

		public static IEnumerable<TeHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _elementsRegexPattern))
			{
				yield break;
			}

			string[] elements = headerValue.SplitElements();

			foreach (string element in elements)
			{
				decimal? qvalue = null;
				string[] semicolonSplitParts = element.SplitSemicolons();
				bool cancel = false;
				string tCoding = semicolonSplitParts[0];
				var parameters = new HashSet<Parameter>();

				for (int i = 1; i < semicolonSplitParts.Length; i++)
				{
					string semicolonSplitPart = semicolonSplitParts[i];
					string[] equalSignSplitParts = semicolonSplitPart.SplitEqualSign();
					QvalueValidity qvalueValidity = equalSignSplitParts.ValidateQvalue();
					string name;
					string value;

					semicolonSplitPart.GetParameterParts(out name, out value, true);

					if (qvalueValidity == QvalueValidity.Valid)
					{
						qvalue = Decimal.Parse(value);
					}
					else if (qvalueValidity == QvalueValidity.Invalid)
					{
						cancel = true;
						break;
					}
					else
					{
						parameters.Add(new Parameter(name, value ?? ""));
					}
				}

				if (!cancel)
				{
					yield return new TeHeader(tCoding, qvalue, parameters);
				}
			}
		}
	}
}