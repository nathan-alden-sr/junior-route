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
		private readonly IEnumerable<Parameter> _parameters;
		private readonly decimal? _qvalue;

		private TeHeader(IEnumerable<Parameter> parameters, decimal? qvalue)
		{
			parameters.ThrowIfNull("parameters");

			_parameters = parameters;
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
		}

		public IEnumerable<Parameter> Parameters
		{
			get
			{
				return _parameters;
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
				var parameters = new HashSet<Parameter>();

				foreach (string semicolonSplitPart in semicolonSplitParts)
				{
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

					parameters.Add(new Parameter(name, value ?? ""));
				}

				if (!cancel)
				{
					yield return new TeHeader(parameters, qvalue);
				}
			}
		}
	}
}