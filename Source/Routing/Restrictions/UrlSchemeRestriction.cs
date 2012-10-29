using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class UrlSchemeRestriction : IRouteRestriction, IEquatable<UrlSchemeRestriction>
	{
		private readonly string _scheme;

		public UrlSchemeRestriction(string scheme)
		{
			scheme.ThrowIfNull("scheme");

			_scheme = scheme;
		}

		public string Scheme
		{
			get
			{
				return _scheme;
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

		public bool Equals(UrlSchemeRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_scheme, other._scheme);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_scheme, request.UrlReferrer.Scheme);
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
			return Equals((UrlSchemeRestriction)obj);
		}

		public override int GetHashCode()
		{
			return (_scheme != null ? _scheme.GetHashCode() : 0);
		}
	}
}