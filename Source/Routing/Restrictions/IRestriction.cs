using System.Web;

namespace Junior.Route.Routing.Restrictions
{
	public interface IRestriction
	{
		MatchResult MatchesRequest(HttpRequestBase request);
	}
}