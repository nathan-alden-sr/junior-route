using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class RefererUrlAuthorityRestriction : IRouteRestriction, IEquatable<RefererUrlAuthorityRestriction>
	{
		private readonly string _authority;

		public RefererUrlAuthorityRestriction(string authority)
		{
			authority.ThrowIfNull("authority");

			_authority = authority;
		}

		public string Authority
		{
			get
			{
				return _authority;
			}
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return _authority;
			}
		}

		public bool Equals(RefererUrlAuthorityRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_authority, other._authority);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_authority, request.UrlReferrer.Authority);
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
			return Equals((RefererUrlAuthorityRestriction)obj);
		}

		public override int GetHashCode()
		{
			return (_authority != null ? _authority.GetHashCode() : 0);
		}
	}
}