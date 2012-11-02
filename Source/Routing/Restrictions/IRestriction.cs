using System.Web;

namespace Junior.Route.Routing.Restrictions
{
	public interface IRestriction
	{
		bool MatchesRequest(HttpRequestBase request);
	}
}