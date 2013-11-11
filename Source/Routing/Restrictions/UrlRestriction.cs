using System;
using System.Linq;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	public class UrlRestriction : IRestriction, IEquatable<UrlRestriction>
	{
		private readonly Func<Uri, bool> _matchDelegate;

		public UrlRestriction(Func<Uri, bool> matchDelegate)
		{
			matchDelegate.ThrowIfNull("matchDelegate");

			_matchDelegate = matchDelegate;
		}

		public bool Equals(UrlRestriction other)
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

		public MatchResult MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _matchDelegate(request.Url) ? MatchResult.RestrictionMatched(this.ToEnumerable()) : MatchResult.RestrictionNotMatched(Enumerable.Empty<IRestriction>(), this.ToEnumerable());
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
			return Equals((UrlRestriction)obj);
		}

		public override int GetHashCode()
		{
			return (_matchDelegate != null ? _matchDelegate.GetHashCode() : 0);
		}
	}
}