using System;
using System.Linq;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.AntiCsrf.CookieManagers
{
	public class DefaultCookieManager : IAntiCsrfCookieManager
	{
		private readonly IAntiCsrfConfiguration _configuration;
		private readonly IGuidFactory _guidFactory;

		public DefaultCookieManager(IAntiCsrfConfiguration configuration, IGuidFactory guidFactory)
		{
			configuration.ThrowIfNull("configuration");
			guidFactory.ThrowIfNull("guidFactory");

			_configuration = configuration;
			_guidFactory = guidFactory;
		}

		public void ConfigureCookie(HttpRequestBase request, HttpResponseBase response)
		{
			request.ThrowIfNull("request");
			response.ThrowIfNull("response");

			if (request.Cookies.AllKeys.Contains(_configuration.CookieName))
			{
				response.Cookies.Set(request.Cookies[_configuration.CookieName]);
				return;
			}

			string sessionId = _guidFactory.Random().ToString("N");
			var cookie = new HttpCookie(_configuration.CookieName, sessionId) { HttpOnly = true };

			response.Cookies.Add(cookie);
		}

		public Guid? GetSessionId(HttpResponseBase response)
		{
			response.ThrowIfNull("response");

			if (!response.Cookies.AllKeys.Contains(_configuration.CookieName))
			{
				return null;
			}

			Guid sessionId;

			return Guid.TryParse(response.Cookies[_configuration.CookieName].Value, out sessionId) ? sessionId : (Guid?)null;
		}
	}
}