using System;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_field}: {_value}")]
	public class HeaderRestriction : IHttpRouteRestriction
	{
		private readonly string _field;
		private readonly string _value;
		private readonly IRequestValueComparer _valueComparer;

		public HeaderRestriction(string field, string value, IRequestValueComparer valueComparer)
		{
			field.ThrowIfNull("header");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			_field = field;
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

			return request.Headers.AllKeys.Any(header => String.Equals(_field, header, StringComparison.OrdinalIgnoreCase) && _valueComparer.Matches(_value, request.Headers[header]));
		}
	}
}