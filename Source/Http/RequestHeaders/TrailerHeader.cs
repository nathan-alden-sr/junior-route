using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class TrailerHeader
	{
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(CommonRegexPatterns.FieldName, 1));
		private readonly string _fieldName;

		private TrailerHeader(string fieldName)
		{
			fieldName.ThrowIfNull("fieldName");

			_fieldName = fieldName;
		}

		public string FieldName
		{
			get
			{
				return _fieldName;
			}
		}

		public static IEnumerable<TrailerHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<TrailerHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern) ? headerValue.SplitElements().Select(arg => new TrailerHeader(arg)) : Enumerable.Empty<TrailerHeader>();
		}
	}
}