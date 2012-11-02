using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class CookieRestriction : IRestriction, IEquatable<CookieRestriction>
	{
		private readonly string _name;
		private readonly IRequestValueComparer _nameComparer;
		private readonly string _value;
		private readonly IRequestValueComparer _valueComparer;

		public CookieRestriction(string name, IRequestValueComparer nameComparer, string value, IRequestValueComparer valueComparer)
		{
			name.ThrowIfNull("cookie");
			nameComparer.ThrowIfNull("cookieComparer");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			_name = name;
			_nameComparer = nameComparer;
			_value = value;
			_valueComparer = valueComparer;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public IRequestValueComparer NameComparer
		{
			get
			{
				return _nameComparer;
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
				return String.Format("{0}={1}", _name, _value);
			}
		}

		public bool Equals(CookieRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_nameComparer, other._nameComparer) && String.Equals(_name, other._name) && String.Equals(_value, other._value) && Equals(_valueComparer, other._valueComparer);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			IEnumerable<string> matchingKeys = request.Cookies.AllKeys.Where(arg => _nameComparer.Matches(_name, arg));

			return matchingKeys.Any(arg => _valueComparer.Matches(_value, request.Cookies[arg].Value));
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
			return Equals((CookieRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (_nameComparer != null ? _nameComparer.GetHashCode() : 0);

				hashCode = (hashCode * 397) ^ (_name != null ? _name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (_value != null ? _value.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (_valueComparer != null ? _valueComparer.GetHashCode() : 0);

				return hashCode;
			}
		}
	}
}