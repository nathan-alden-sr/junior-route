using System;
using System.Linq;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.AntiCsrf.Validators
{
	public class DefaultAntiCsrfTokenValidator : IAntiCsrfTokenValidator
	{
		private readonly IAntiCsrfConfiguration _configuration;

		public DefaultAntiCsrfTokenValidator(IAntiCsrfConfiguration configuration)
		{
			configuration.ThrowIfNull("configuration");

			_configuration = configuration;
		}

		public ValidationResult Validate(HttpRequestBase request)
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

			string antiCsrfToken = request.Form[_configuration.FormFieldName];
			HttpCookie cookie = request.Cookies[_configuration.CookieName];

			return cookie.Value == antiCsrfToken ? ValidationResult.TokensMatch : ValidationResult.TokensDoNotMatch;
		}
	}
}