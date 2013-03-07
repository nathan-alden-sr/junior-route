using System;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.CookieManagers;
using Junior.Route.Routing.AntiCsrf.HtmlGenerators;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

namespace Junior.Route.AutoRouting.AntiCsrf.HtmlGenerators
{
	public class DefaultHtmlGenerator : IAntiCsrfHtmlGenerator
	{
		private readonly IAntiCsrfConfiguration _configuration;
		private readonly IAntiCsrfCookieManager _cookieManager;
		private readonly IGuidFactory _guidFactory;
		private readonly IAntiCsrfNonceRepository _nonceRepository;
		private readonly IResponseContext _responseContext;
		private readonly ISystemClock _systemClock;

		public DefaultHtmlGenerator(
			IAntiCsrfConfiguration configuration,
			IResponseContext responseContext,
			IAntiCsrfCookieManager cookieManager,
			IAntiCsrfNonceRepository nonceRepository,
			IGuidFactory guidFactory,
			ISystemClock systemClock)
		{
			configuration.ThrowIfNull("configuration");
			responseContext.ThrowIfNull("responseContext");
			cookieManager.ThrowIfNull("cookieManager");
			nonceRepository.ThrowIfNull("nonceRepository");
			guidFactory.ThrowIfNull("guidFactory");
			systemClock.ThrowIfNull("systemClock");

			_configuration = configuration;
			_responseContext = responseContext;
			_cookieManager = cookieManager;
			_nonceRepository = nonceRepository;
			_guidFactory = guidFactory;
			_systemClock = systemClock;
		}

		public async Task<string> GenerateHiddenInputHtml()
		{
			if (!_configuration.Enabled)
			{
				return "";
			}

			Guid? sessionId = _cookieManager.GetSessionId(_responseContext.Response);

			if (sessionId == null)
			{
				return "";
			}

			Guid nonce = _guidFactory.Random();
			DateTime currentTimestamp = _systemClock.UtcDateTime;

			await _nonceRepository.Add(sessionId.Value, nonce, currentTimestamp, currentTimestamp + _configuration.NonceDuration);

			return String.Format(@"<input type=""hidden"" name=""{0}"" value=""{1}""/>", _configuration.FormFieldName, nonce.ToString("N"));
		}
	}
}