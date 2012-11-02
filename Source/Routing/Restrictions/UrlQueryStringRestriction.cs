using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("DebuggerDisplay,nq")]
	public class UrlQueryStringRestriction : IRestriction, IEquatable<UrlQueryStringRestriction>
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

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return String.Format("{0}={1}", _field, _value);
			}
		}

		public bool Equals(UrlQueryStringRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_field, other._field) && Equals(_fieldComparer, other._fieldComparer) && String.Equals(_value, other._value) && Equals(_valueComparer, other._valueComparer);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			NameValueCollection queryString = HttpUtility.ParseQueryString(request.Url.Query);
			IEnumerable<string> matchingKeys = queryString.AllKeys.Where(arg => _fieldComparer.Matches(_field, arg));

			return matchingKeys.Any(arg => _valueComparer.Matches(_value, queryString[arg]));
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((UrlQueryStringRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (_field != null ? _field.GetHashCode() : 0);

				hashCode = (hashCode * 397) ^ (_fieldComparer != null ? _fieldComparer.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (_value != null ? _value.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (_valueComparer != null ? _valueComparer.GetHashCode() : 0);

				return hashCode;
			}
		}
	}
}