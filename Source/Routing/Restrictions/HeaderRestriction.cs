using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class HeaderRestriction : IRestriction, IEquatable<HeaderRestriction>
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

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return String.Format("{0}: {1}", _field, _value);
			}
		}

		public bool Equals(HeaderRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_field, other._field) && String.Equals(_value, other._value) && Equals(_valueComparer, other._valueComparer);
		}

		public Task<bool> MatchesRequestAsync(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return request.Headers.AllKeys.Any(header => String.Equals(_field, header, StringComparison.OrdinalIgnoreCase) && _valueComparer.Matches(_value, request.Headers[header])).AsCompletedTask();
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
			return Equals((HeaderRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (_field != null ? _field.GetHashCode() : 0);

				hashCode = (hashCode * 397) ^ (_value != null ? _value.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (_valueComparer != null ? _valueComparer.GetHashCode() : 0);

				return hashCode;
			}
		}
	}

	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class HeaderRestriction<T> : IRestriction
	{
		private readonly string _field;
		private readonly Func<T, bool> _matchDelegate;
		private readonly Func<string, IEnumerable<T>> _parseDelegate;

		public HeaderRestriction(string field, Func<string, IEnumerable<T>> parseDelegate, Func<T, bool> matchDelegate)
		{
			field.ThrowIfNull("header");
			parseDelegate.ThrowIfNull("parseDelegate");
			matchDelegate.ThrowIfNull("matchDelegate");

			_field = field;
			_parseDelegate = parseDelegate;
			_matchDelegate = matchDelegate;
		}

		public HeaderRestriction(string field, Func<string, T> parseDelegate, Func<T, bool> matchDelegate)
			: this(field, headerValue => parseDelegate(headerValue).ToEnumerable(), matchDelegate)
		{
		}

		public string Field
		{
			get
			{
				return _field;
			}
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return _field;
			}
		}

		public Task<bool> MatchesRequestAsync(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return (request.Headers.AllKeys.Any(header => String.Equals(_field, header, StringComparison.OrdinalIgnoreCase)) && _parseDelegate(request.Headers[_field]).Any(_matchDelegate)).AsCompletedTask();
		}
	}
}