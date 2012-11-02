using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class RefererUrlQueryRestriction : IRestriction, IEquatable<RefererUrlQueryRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _query;

		public RefererUrlQueryRestriction(string query, IRequestValueComparer comparer)
		{
			query.ThrowIfNull("query");
			comparer.ThrowIfNull("comparer");

			_query = query;
			_comparer = comparer;
		}

		public string Query
		{
			get
			{
				return _query;
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
				return _query;
			}
		}

		public bool Equals(RefererUrlQueryRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_comparer, other._comparer) && String.Equals(_query, other._query);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_query, request.UrlReferrer.Query);
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
			return Equals((RefererUrlQueryRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_comparer != null ? _comparer.GetHashCode() : 0) * 397) ^ (_query != null ? _query.GetHashCode() : 0);
			}
		}
	}
}