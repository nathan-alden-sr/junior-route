using System;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class MissingHeaderRestriction : IRestriction, IEquatable<MissingHeaderRestriction>
	{
		private readonly string _field;

		public MissingHeaderRestriction(string field)
		{
			field.ThrowIfNull("header");

			_field = field;
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
				return String.Format("No {0} header", _field);
			}
		}

		public bool Equals(MissingHeaderRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_field, other._field);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return request.Headers.AllKeys.All(arg => arg != _field);
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
			return Equals((MissingHeaderRestriction)obj);
		}

		public override int GetHashCode()
		{
			return (_field != null ? _field.GetHashCode() : 0);
		}
	}
}