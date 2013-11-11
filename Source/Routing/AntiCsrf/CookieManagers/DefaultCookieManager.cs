using System;
using System.Linq;
using System.Threading.Tasks;
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

		public Task ConfigureCookieAsync(HttpRequestBase request, HttpResponseBase response)
		{
			request.ThrowIfNull("request");
			response.ThrowIfNull("response");

			string cookieName = _configuration.CookieName;
			string sessionId = request.Cookies.AllKeys.Contains(cookieName) ? request.Cookies[cookieName].Value : _guidFactory.Random().ToString("N");

			response.Cookies.Remove(cookieName);

			var cookie = new HttpCookie(cookieName, sessionId) { HttpOnly = true };

			response.Cookies.Add(cookie);

			return Task.Factory.Empty();
		}

		public Task<Guid?> GetSessionIdAsync(HttpResponseBase response)
		{
			response.ThrowIfNull("response");

			if (!response.Cookies.AllKeys.Contains(_configuration.CookieName))
			{
				return null;
			}

			Guid sessionId;

			return (Guid.TryParse(response.Cookies[_configuration.CookieName].Value, out sessionId) ? sessionId : (Guid?)null).AsCompletedTask();
		}
	}
}