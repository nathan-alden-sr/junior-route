using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_port}")]
	public class UrlPortRestriction : IHttpRouteRestriction
	{
		private readonly ushort _port;

		public UrlPortRestriction(ushort port)
		{
			_port = port;
		}

		public ushort Port
		{
			get
			{
				return _port;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _port == request.Url.Port;
		}
	}
}