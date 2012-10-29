using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class VaryHeader
	{
		private static readonly string _elementsRegexPattern = @"^(?:\*|" + CommonRegexPatterns.ListOfElements(CommonRegexPatterns.FieldName, 1) + ")";
		private readonly string _fieldName;

		private VaryHeader(string fieldName)
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

		public static IEnumerable<VaryHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<VaryHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern) ? headerValue.SplitElements().Select(arg => new VaryHeader(arg)) : Enumerable.Empty<VaryHeader>();
		}
	}
}