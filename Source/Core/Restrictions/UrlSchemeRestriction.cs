using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_scheme}")]
	public class UrlSchemeRestriction : IHttpRouteRestriction
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

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_scheme, request.Url.Scheme);
		}
	}
}