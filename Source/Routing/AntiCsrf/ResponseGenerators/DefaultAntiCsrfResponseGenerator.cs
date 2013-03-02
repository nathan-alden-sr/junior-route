using Junior.Route.Routing.AntiCsrf.Validators;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.AntiCsrf.ResponseGenerators
{
	public class DefaultAntiCsrfResponseGenerator : IAntiCsrfResponseGenerator
	{
		public ResponseResult GetResponse(ValidationResult result)
		{
			switch (result)
			{
				case ValidationResult.FormFieldMissing:
				case ValidationResult.CookieMissing:
				case ValidationResult.TokensDoNotMatch:
					Response response = Response.Unauthorized().TextPlain().Content("Anti-CSRF validation failed.");

					return ResponseResult.ResponseGenerated(response);
				default:
					return ResponseResult.ResponseNotGenerated();
			}
		}
	}
}