using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

namespace Junior.Route.Routing.AntiCsrf.NonceValidators
{
	public class DefaultValidator : IAntiCsrfNonceValidator
	{
		private readonly IAntiCsrfConfiguration _configuration;
		private readonly IAntiCsrfNonceRepository _nonceRepository;
		private readonly ISystemClock _systemClock;

		public DefaultValidator(IAntiCsrfConfiguration configuration, IAntiCsrfNonceRepository nonceRepository, ISystemClock systemClock)
		{
			configuration.ThrowIfNull("configuration");
			nonceRepository.ThrowIfNull("nonceRepository");
			systemClock.ThrowIfNull("systemClock");

			_configuration = configuration;
			_nonceRepository = nonceRepository;
			_systemClock = systemClock;
		}

		public async Task<ValidationResult> Validate(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			if (!_configuration.Enabled)
			{
				return ValidationResult.ValidationDisabled;
			}
			bool isPostNeedingValidation = (String.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) && _configuration.ValidateHttpPost);
			bool isPutNeedingValidation = (String.Equals(request.HttpMethod, "PUT", StringComparison.OrdinalIgnoreCase) && _configuration.ValidateHttpPut);
			bool isDeleteNeedingValidation = (String.Equals(request.HttpMethod, "DELETE", StringComparison.OrdinalIgnoreCase) && _configuration.ValidateHttpDelete);

			if (!isPostNeedingValidation && !isPutNeedingValidation && !isDeleteNeedingValidation)
			{
				return ValidationResult.ValidationSkipped;
			}
			if (!request.Form.AllKeys.Contains(_configuration.FormFieldName))
			{
				return ValidationResult.FormFieldMissing;
			}
			if (!request.Cookies.AllKeys.Contains(_configuration.CookieName))
			{
				return ValidationResult.CookieMissing;
			}

			Guid nonce;

			if (!Guid.TryParse(request.Form[_configuration.FormFieldName], out nonce))
			{
				return ValidationResult.FormFieldInvalid;
			}

			HttpCookie cookie = request.Cookies[_configuration.CookieName];
			Guid sessionId;

			if (!Guid.TryParse(cookie.Value, out sessionId))
			{
				return ValidationResult.CookieInvalid;
			}

			return await _nonceRepository.Exists(sessionId, nonce, _systemClock.UtcDateTime) ? ValidationResult.NonceValid : ValidationResult.NonceInvalid;
		}
	}
}