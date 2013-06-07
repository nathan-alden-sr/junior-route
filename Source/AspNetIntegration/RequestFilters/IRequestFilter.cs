using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.AspNetIntegration.RequestFilters
{
	public interface IRequestFilter
	{
		Task<FilterResult> FilterAsync(HttpContextBase context);
	}
}