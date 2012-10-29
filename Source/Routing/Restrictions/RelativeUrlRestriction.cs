using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class RelativeUrlRestriction : IRouteRestriction, IEquatable<RelativeUrlRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _relativeUrl;

		public RelativeUrlRestriction(string relativeUrl, IRequestValueComparer comparer)
		{
			relativeUrl.ThrowIfNull("url");
			comparer.ThrowIfNull("comparer");

			_relativeUrl = relativeUrl;
			_comparer = comparer;
		}

		public string RelativeUrl
		{
			get
			{
				return _relativeUrl;
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
				return _relativeUrl;
			}
		}

		public bool Equals(RelativeUrlRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_comparer, other._comparer) && String.Equals(_relativeUrl, other._relativeUrl);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_relativeUrl, GetRelativeUrl(request));
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
			return Equals((RelativeUrlRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_comparer != null ? _comparer.GetHashCode() : 0) * 397) ^ (_relativeUrl != null ? _relativeUrl.GetHashCode() : 0);
			}
		}

		private static string GetRelativeUrl(HttpRequestBase request)
		{
			string virtualPath = HttpRuntime.AppDomainAppVirtualPath;
			string absolutePathAndQuery = new Uri(request.Url.AbsoluteUri).PathAndQuery;

			return absolutePathAndQuery.Remove(0, virtualPath.Length).TrimStart('/');
		}
	}
}