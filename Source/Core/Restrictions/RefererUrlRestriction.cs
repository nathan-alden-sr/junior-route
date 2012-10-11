using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_url}")]
	public class RefererUrlRestriction : IHttpRouteRestriction
	{
		public delegate bool RefererUrlMatchDelegate(Uri uri, Uri requestUri);

		private readonly RefererUrlMatchDelegate _matchDelegate;
		private readonly Uri _url;

		public RefererUrlRestriction(Uri url, RefererUrlMatchDelegate matchDelegate)
		{
			url.ThrowIfNull("url");
			matchDelegate.ThrowIfNull("matchDelegate");

			_url = url;
			_matchDelegate = matchDelegate;
		}

		public Uri Url
		{
			get
			{
				return _url;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _matchDelegate(_url, request.UrlReferrer);
		}
	}
}