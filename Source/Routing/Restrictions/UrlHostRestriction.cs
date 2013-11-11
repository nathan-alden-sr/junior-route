using System;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class UrlHostRestriction : IRestriction, IEquatable<UrlHostRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _host;

		public UrlHostRestriction(string host, IRequestValueComparer comparer)
		{
			host.ThrowIfNull("host");
			comparer.ThrowIfNull("comparer");

			_host = host;
			_comparer = comparer;
		}

		public string Host
		{
			get
			{
				return _host;
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
				return _host;
			}
		}

		public bool Equals(UrlHostRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_host, other._host) && Equals(_comparer, other._comparer);
		}

		public MatchResult MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_host, request.Url.Host, StringComparison.OrdinalIgnoreCase)
				? MatchResult.RestrictionMatched(this.ToEnumerable())
				: MatchResult.RestrictionNotMatched(Enumerable.Empty<IRestriction>(), this.ToEnumerable());
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
			return Equals((UrlHostRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_host != null ? _host.GetHashCode() : 0) * 397) ^ (_comparer != null ? _comparer.GetHashCode() : 0);
			}
		}
	}
}