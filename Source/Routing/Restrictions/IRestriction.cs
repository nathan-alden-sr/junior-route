using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.Routing.Restrictions
{
	public interface IRestriction
	{
		Task<bool> MatchesRequestAsync(HttpRequestBase request);
	}
}