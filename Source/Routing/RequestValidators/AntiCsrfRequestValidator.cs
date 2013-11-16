using System;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf.CookieManagers;
using Junior.Route.Routing.AntiCsrf.NonceValidators;
using Junior.Route.Routing.AntiCsrf.ResponseGenerators;

namespace Junior.Route.Routing.RequestValidators
{
	public class AntiCsrfRequestValidator : IRequestValidator
	{
		private readonly IAntiCsrfCookieManager _antiCsrfCookieManager;
		private readonly IAntiCsrfNonceValidator _antiCsrfNonceValidator;
		private readonly IAntiCsrfResponseGenerator _antiCsrfResponseGenerator;

		public AntiCsrfRequestValidator(IAntiCsrfCookieManager antiCsrfCookieManager, IAntiCsrfNonceValidator antiCsrfNonceValidator, IAntiCsrfResponseGenerator antiCsrfResponseGenerator)
		{
			antiCsrfCookieManager.ThrowIfNull("antiCsrfCookieManager");
			antiCsrfNonceValidator.ThrowIfNull("antiCsrfNonceValidator");
			antiCsrfResponseGenerator.ThrowIfNull("antiCsrfResponseGenerator");

			_antiCsrfCookieManager = antiCsrfCookieManager;
			_antiCsrfNonceValidator = antiCsrfNonceValidator;
			_antiCsrfResponseGenerator = antiCsrfResponseGenerator;
		}

		public async Task<ValidateResult> Validate(HttpRequestBase request, HttpResponseBase response)
		{
			request.ThrowIfNull("request");
			response.ThrowIfNull("response");

			if (!String.IsNullOrEmpty(request.ContentType))
			{
				try
				{
					var contentType = new ContentType(request.ContentType);

					if (String.Equals(contentType.MediaType, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) || String.Equals(contentType.MediaType, "multipart/form-data", StringComparison.OrdinalIgnoreCase))
					{
						ValidationResult validationResult = await _antiCsrfNonceValidator.ValidateAsync(request);
						ResponseResult responseResult = await _antiCsrfResponseGenerator.GetResponseAsync(validationResult);

						if (responseResult.ResultType == ResponseResultType.ResponseGenerated)
						{
							return ValidateResult.ResponseGenerated(responseResult.Response);
						}
					}
				}
				catch (FormatException)
				{
				}
			}

			await _antiCsrfCookieManager.ConfigureCookieAsync(request, response);

			return ValidateResult.RequestValidated();
		}
	}
}