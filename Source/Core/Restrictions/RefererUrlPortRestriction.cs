using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_port}")]
	public class RefererUrlPortRestriction : IHttpRouteRestriction
	{
		private readonly ushort _port;

		public RefererUrlPortRestriction(ushort port)
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

			return _port == request.UrlReferrer.Port;
		}
	}
}