using System.Web;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	public interface IHttpRouteRestriction
	{
		bool MatchesRequest(HttpRequestBase request);
	}
}