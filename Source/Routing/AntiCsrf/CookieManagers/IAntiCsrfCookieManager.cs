using System;
using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.Routing.AntiCsrf.CookieManagers
{
	public interface IAntiCsrfCookieManager
	{
		Task ConfigureCookieAsync(HttpRequestBase request, HttpResponseBase response);
		Task<Guid?> GetSessionIdAsync(HttpResponseBase response);
	}
}