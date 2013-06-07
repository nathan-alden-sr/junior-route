using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{_host,nq}")]
	public class RefererUrlHostRestriction : IRestriction, IEquatable<RefererUrlHostRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _host;

		public RefererUrlHostRestriction(string host, IRequestValueComparer comparer)
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

		public bool Equals(RefererUrlHostRestriction other)
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

		public Task<bool> MatchesRequestAsync(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_host, request.UrlReferrer.Host).AsCompletedTask();
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
			return Equals((RefererUrlHostRestriction)obj);
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