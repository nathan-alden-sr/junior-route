using System.Web;

namespace Junior.Route.AspNetIntegration.RequestFilters
{
	public interface IRequestFilter
	{
		FilterResult Filter(HttpContextBase context);
	}
}