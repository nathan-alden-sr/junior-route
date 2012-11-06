using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class AcceptHeader
	{
		private const string MediaTypeRegexPattern = "(?:" + TypeRegexPattern + "/)(?:" + SubtypeRegexPattern + @"|\*)";
		private const string ParametersRegexPattern = @";" + CommonRegexPatterns.SP + "*" + CommonRegexPatterns.Token + "(?:=(?:" + CommonRegexPatterns.Token + "|" + CommonRegexPatterns.QuotedString + "))?";
		private const string RegexPattern = MediaTypeRegexPattern + "(?:" + ParametersRegexPattern + ")*";
		private const string SubtypeRegexPattern = CommonRegexPatterns.Token;
		private const string TypeRegexPattern = CommonRegexPatterns.Token;
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern));
		private readonly decimal _effectiveQvalue;
		private readonly Parameter[] _extensions;
		private readonly string _mediaTypes;
		private readonly Parameter[] _parameters;
		private readonly decimal? _qvalue;
		private readonly string _subtype;
		private readonly string _type;

		public AcceptHeader(string type, string subtype, IEnumerable<Parameter> parameters, decimal? qvalue, IEnumerable<Parameter> extensions)
		{
			type.ThrowIfNull("type");
			subtype.ThrowIfNull("subtype");
			parameters.ThrowIfNull("parameters");
			extensions.ThrowIfNull("extensions");

			_type = type;
			_subtype = subtype;
			_mediaTypes = String.Format("{0}/{1}", _type, _subtype);
			_parameters = parameters.ToArray();
			_qvalue = qvalue;
			_effectiveQvalue = qvalue ?? 1m;
			_extensions = extensions.ToArray();
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

		public string MediaTypes
		{
			get
			{
				return _mediaTypes;
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

		public bool MediaTypeMatches(string mediaType)
		{
			mediaType.ThrowIfNull("mediaType");

			if (!Regex.IsMatch(mediaType, MediaTypeRegexPattern))
			{
				throw new ArgumentException("Invalid media-type.", "mediaType");
			}
			if (_effectiveQvalue == 0m)
			{
				return false;
			}

			string[] typeParts = mediaType.Split('/');

			return
				((_type == "*" || String.Equals(_type, typeParts[0], StringComparison.OrdinalIgnoreCase)) &&
				 (_subtype == "*" || String.Equals(_subtype, typeParts[1], StringComparison.OrdinalIgnoreCase)));
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