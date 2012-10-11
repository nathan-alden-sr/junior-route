using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_type}")]
	public class RefererUrlHostTypeRestriction : IHttpRouteRestriction
	{
		private readonly UriHostNameType _type;

		public RefererUrlHostTypeRestriction(UriHostNameType type)
		{
			_type = type;
		}

		public UriHostNameType Type
		{
			get
			{
				return _type;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _type == request.UrlReferrer.HostNameType;
		}
	}
}