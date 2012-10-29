using System;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	public class RefererUrlRestriction : IRouteRestriction, IEquatable<RefererUrlRestriction>
	{
		public delegate bool RefererUrlMatchDelegate(Uri requestUri);

		private readonly RefererUrlMatchDelegate _matchDelegate;

		public RefererUrlRestriction(RefererUrlMatchDelegate matchDelegate)
		{
			matchDelegate.ThrowIfNull("matchDelegate");

			_matchDelegate = matchDelegate;
		}

		public bool Equals(RefererUrlRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_matchDelegate, other._matchDelegate);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _matchDelegate(request.UrlReferrer);
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
			return Equals((RefererUrlRestriction)obj);
		}

		public override int GetHashCode()
		{
			return (_matchDelegate != null ? _matchDelegate.GetHashCode() : 0);
		}
	}
}