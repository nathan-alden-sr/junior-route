using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class AcceptHeader
	{
		private const string ParametersRegexPattern = @";" + CommonRegexPatterns.SP + "*" + CommonRegexPatterns.Token + "(?:=(?:" + CommonRegexPatterns.Token + "|" + CommonRegexPatterns.QuotedString + "))?";
		private const string RegexPattern = "(?:" + TypeRegexPattern + "/)(?:" + SubtypeRegexPattern + @"|\*)(?:" + ParametersRegexPattern + ")*";
		private const string SubtypeRegexPattern = CommonRegexPatterns.Token;
		private const string TypeRegexPattern = CommonRegexPatterns.Token;
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern));
		private readonly decimal _effectiveQvalue;
		private readonly IEnumerable<Parameter> _extensions;
		private readonly IEnumerable<Parameter> _parameters;
		private readonly decimal? _qvalue;
		private readonly string _subtype;
		private readonly string _type;
		private readonly string _typeAndSubtype;

		public AcceptHeader(string type, string subtype, IEnumerable<Parameter> parameters, decimal? qvalue, IEnumerable<Parameter> extensions)
		{
			type.ThrowIfNull("type");
			subtype.ThrowIfNull("subtype");
			parameters.ThrowIfNull("parameters");
			extensions.ThrowIfNull("extensions");

			_type = type;
			_subtype = subtype;
			_typeAndSubtype = String.Format("{0}/{1}", _type, _subtype);
			_parameters = parameters;
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
			_extensions = extensions;
		}

		public string Type
		{
			get
			{
				return _type;
			}
		}

		public string Subtype
		{
			get
			{
				return _subtype;
			}
		}

		public string TypeAndSubtype
		{
			get
			{
				return _typeAndSubtype;
			}
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

		public decimal? EffectiveQvalue
		{
			get
			{
				return _effectiveQvalue;
			}
		}

		public IEnumerable<Parameter> Extensions
		{
			get
			{
				return _extensions;
			}
		}

		public static IEnumerable<AcceptHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _elementsRegexPattern))
			{
				yield break;
			}

			string[] elements = headerValue.SplitElements();

			foreach (string element in elements)
			{
				string[] semicolonSplitParts = element.SplitSemicolons();
				string[] typeParts = semicolonSplitParts[0].Split('/');
				string type = typeParts[0];
				string subtype = typeParts[1];
				var parameters = new HashSet<Parameter>();
				decimal? qvalue = null;
				var extensions = new HashSet<Parameter>();
				bool cancel = false;

				for (int i = 1; i < semicolonSplitParts.Length; i++)
				{
					string[] equalSignSplitParts = semicolonSplitParts[i].SplitEqualSign();
					QvalueValidity qvalueValidity = equalSignSplitParts.ValidateQvalue();

					if (qvalueValidity == QvalueValidity.Invalid)
					{
						cancel = true;
						break;
					}
					if (qvalueValidity == QvalueValidity.Valid)
					{
						qvalue = Decimal.Parse(equalSignSplitParts[1], NumberStyles.AllowDecimalPoint);
					}
					else if (qvalue == null)
					{
						parameters.Add(new Parameter(equalSignSplitParts[0], equalSignSplitParts[1]));
					}
					else
					{
						extensions.Add(new Parameter(equalSignSplitParts[0], equalSignSplitParts[1]));
					}
				}

				if (!cancel)
				{
					yield return new AcceptHeader(type, subtype, parameters, qvalue, extensions);
				}
			}
		}
	}
}