using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_field}={_value}")]
	public class UrlQueryStringRestriction : IHttpRouteRestriction
	{
		private readonly string _field;
		private readonly IRequestValueComparer _fieldComparer;
		private readonly string _value;
		private readonly IRequestValueComparer _valueComparer;

		public UrlQueryStringRestriction(string field, IRequestValueComparer fieldComparer, string value, IRequestValueComparer valueComparer)
		{
			field.ThrowIfNull("key");
			fieldComparer.ThrowIfNull("keyComparer");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			_field = field;
			_fieldComparer = fieldComparer;
			_value = value;
			_valueComparer = valueComparer;
		}

		public string Field
		{
			get
			{
				return _field;
			}
		}

		public IRequestValueComparer FieldComparer
		{
			get
			{
				return _fieldComparer;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public IRequestValueComparer ValueComparer
		{
			get
			{
				return _valueComparer;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			IEnumerable<string> matchingKeys = request.QueryString.AllKeys.Where(arg => _fieldComparer.Matches(_field, arg));

			return matchingKeys.Any(arg => _valueComparer.Matches(_value, request.QueryString[arg]));
		}
	}
}