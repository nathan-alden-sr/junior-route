using System;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.CookieManagers;
using Junior.Route.Routing.AntiCsrf.HtmlGenerators;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

namespace Junior.Route.AutoRouting.AntiCsrf.HtmlGenerators
{
	public class DefaultGenerator : IAntiCsrfHtmlGenerator
	{
		private readonly IAntiCsrfConfiguration _configuration;
		private readonly IAntiCsrfCookieManager _cookieManager;
		private readonly IGuidFactory _guidFactory;
		private readonly IAntiCsrfNonceRepository _nonceRepository;
		private readonly ISystemClock _systemClock;

		public DefaultGenerator(IAntiCsrfConfiguration configuration, IAntiCsrfCookieManager cookieManager, IAntiCsrfNonceRepository nonceRepository, IGuidFactory guidFactory, ISystemClock systemClock)
		{
			configuration.ThrowIfNull("configuration");
			cookieManager.ThrowIfNull("cookieManager");
			nonceRepository.ThrowIfNull("nonceRepository");
			guidFactory.ThrowIfNull("guidFactory");
			systemClock.ThrowIfNull("systemClock");

			_configuration = configuration;
			_cookieManager = cookieManager;
			_nonceRepository = nonceRepository;
			_guidFactory = guidFactory;
			_systemClock = systemClock;
		}

		public async Task<string> GenerateHiddenInputHtmlAsync(HttpResponseBase response)
		{
			if (!_configuration.Enabled)
			{
				return "";
			}

			Guid? sessionId = await _cookieManager.GetSessionIdAsync(response);

			if (sessionId == null)
			{
				return "";
			}

			Guid nonce = _guidFactory.Random();
			DateTime currentTimestamp = _systemClock.UtcDateTime;

			await _nonceRepository.AddAsync(sessionId.Value, nonce, currentTimestamp, currentTimestamp + _configuration.NonceDuration);

			return String.Format(@"<input type=""hidden"" name=""{0}"" value=""{1}""/>", _configuration.FormFieldName, nonce.ToString("N"));
		}
	}
}