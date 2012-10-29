using System.Web;

namespace Junior.Route.Routing.Restrictions
{
	public interface IRouteRestriction
	{
		bool MatchesRequest(HttpRequestBase request);
	}
}