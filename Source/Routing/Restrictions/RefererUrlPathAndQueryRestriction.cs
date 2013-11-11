using System;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class RefererUrlPathAndQueryRestriction : IRestriction, IEquatable<RefererUrlPathAndQueryRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _pathAndQuery;

		public RefererUrlPathAndQueryRestriction(string pathAndQuery, IRequestValueComparer comparer)
		{
			pathAndQuery.ThrowIfNull("absolutePath");
			comparer.ThrowIfNull("comparer");

			_pathAndQuery = pathAndQuery;
			_comparer = comparer;
		}

		public string PathAndQuery
		{
			get
			{
				return _pathAndQuery;
			}
		}

		public IRequestValueComparer Comparer
		{
			get
			{
				return _comparer;
			}
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return _pathAndQuery;
			}
		}

		public bool Equals(RefererUrlPathAndQueryRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_comparer, other._comparer) && String.Equals(_pathAndQuery, other._pathAndQuery);
		}

		public MatchResult MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_pathAndQuery, request.UrlReferrer.PathAndQuery) ? MatchResult.RestrictionMatched(this.ToEnumerable()) : MatchResult.RestrictionNotMatched(Enumerable.Empty<IRestriction>(), this.ToEnumerable());
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
			return Equals((RefererUrlPathAndQueryRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_comparer != null ? _comparer.GetHashCode() : 0) * 397) ^ (_pathAndQuery != null ? _pathAndQuery.GetHashCode() : 0);
			}
		}
	}
}