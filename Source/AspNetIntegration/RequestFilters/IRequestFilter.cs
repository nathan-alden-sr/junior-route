using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.AspNetIntegration.RequestFilters
{
	public interface IRequestFilter
	{
		Task<FilterResult> Filter(HttpContextBase context);
	}
}