using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_host}")]
	public class RefererUrlHostRestriction : IHttpRouteRestriction
	{
		private readonly string _host;

		public RefererUrlHostRestriction(string host)
		{
			host.ThrowIfNull("host");

			_host = host;
		}

		public string Host
		{
			get
			{
				return _host;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_host, request.UrlReferrer.Host, StringComparison.OrdinalIgnoreCase);
		}
	}
}