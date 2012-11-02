using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class RefererUrlSchemeRestriction : IRestriction, IEquatable<RefererUrlSchemeRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _scheme;

		public RefererUrlSchemeRestriction(string scheme, IRequestValueComparer comparer)
		{
			scheme.ThrowIfNull("scheme");
			comparer.ThrowIfNull("comparer");

			_scheme = scheme;
			_comparer = comparer;
		}

		public string Scheme
		{
			get
			{
				return _scheme;
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
				return _scheme;
			}
		}

		public bool Equals(RefererUrlSchemeRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_scheme, other._scheme) && Equals(_comparer, other._comparer);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_scheme, request.UrlReferrer.Scheme);
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
			return Equals((RefererUrlSchemeRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_scheme != null ? _scheme.GetHashCode() : 0) * 397) ^ (_comparer != null ? _comparer.GetHashCode() : 0);
			}
		}
	}
}