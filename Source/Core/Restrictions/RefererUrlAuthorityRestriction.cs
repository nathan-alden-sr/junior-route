using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_authority}")]
	public class RefererUrlAuthorityRestriction : IHttpRouteRestriction
	{
		private readonly string _authority;

		public RefererUrlAuthorityRestriction(string authority)
		{
			authority.ThrowIfNull("authority");

			_authority = authority;
		}

		public string Authority
		{
			get
			{
				return _authority;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_authority, request.UrlReferrer.Authority);
		}
	}
}