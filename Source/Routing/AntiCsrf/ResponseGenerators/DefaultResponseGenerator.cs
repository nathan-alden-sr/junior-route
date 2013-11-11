using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf.NonceValidators;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.AntiCsrf.ResponseGenerators
{
	public class DefaultResponseGenerator : IAntiCsrfResponseGenerator
	{
		public Task<ResponseResult> GetResponseAsync(ValidationResult result)
		{
			switch (result)
			{
				case ValidationResult.CookieInvalid:
				case ValidationResult.CookieMissing:
				case ValidationResult.FormFieldMissing:
				case ValidationResult.FormFieldInvalid:
				case ValidationResult.NonceInvalid:
					Response response = new Response().Unauthorized().TextPlain().Content("Anti-CSRF validation failed.");

					return ResponseResult.ResponseGenerated(response).AsCompletedTask();
				default:
					return ResponseResult.ResponseNotGenerated().AsCompletedTask();
			}
		}
	}
}