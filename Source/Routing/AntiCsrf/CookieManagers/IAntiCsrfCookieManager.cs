using System;
using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.Routing.AntiCsrf.CookieManagers
{
	public interface IAntiCsrfCookieManager
	{
		Task ConfigureCookie(HttpRequestBase request, HttpResponseBase response);
		Task<Guid?> GetSessionId(HttpResponseBase response);
	}
}