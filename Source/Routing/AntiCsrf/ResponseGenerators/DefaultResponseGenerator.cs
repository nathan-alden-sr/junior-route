using Junior.Route.Routing.AntiCsrf.NonceValidators;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.AntiCsrf.ResponseGenerators
{
	public class DefaultResponseGenerator : IAntiCsrfResponseGenerator
	{
		public ResponseResult GetResponse(ValidationResult result)
		{
			switch (result)
			{
				case ValidationResult.CookieInvalid:
				case ValidationResult.CookieMissing:
				case ValidationResult.FormFieldMissing:
				case ValidationResult.FormFieldInvalid:
				case ValidationResult.NonceInvalid:
					Response response = Response.Unauthorized().TextPlain().Content("Anti-CSRF validation failed.");

					return ResponseResult.ResponseGenerated(response);
				default:
					return ResponseResult.ResponseNotGenerated();
			}
		}
	}
}