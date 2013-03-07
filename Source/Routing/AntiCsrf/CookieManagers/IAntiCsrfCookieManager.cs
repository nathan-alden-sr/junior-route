using System;
using System.Web;

namespace Junior.Route.Routing.AntiCsrf.CookieManagers
{
	public interface IAntiCsrfCookieManager
	{
		void ConfigureCookie(HttpRequestBase request, HttpResponseBase response);
		Guid? GetSessionId(HttpResponseBase response);
	}
}