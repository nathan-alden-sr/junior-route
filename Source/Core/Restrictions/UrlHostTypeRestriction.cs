using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_type}")]
	public class UrlHostTypeRestriction : IHttpRouteRestriction
	{
		private readonly UriHostNameType _type;

		public UrlHostTypeRestriction(UriHostNameType type)
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

			return _type == request.Url.HostNameType;
		}
	}
}