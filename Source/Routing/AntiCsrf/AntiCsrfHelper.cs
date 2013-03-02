using System;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf.TokenGenerators;
using Junior.Route.Routing.AntiCsrf.Validators;

namespace Junior.Route.Routing.AntiCsrf
{
	public class AntiCsrfHelper : IAntiCsrfHelper
	{
		private readonly IAntiCsrfConfiguration _configuration;
		private readonly IAntiCsrfTokenGenerator _tokenGenerator;
		private readonly IAntiCsrfTokenValidator _tokenValidator;

		public AntiCsrfHelper(IAntiCsrfConfiguration configuration, IAntiCsrfTokenGenerator tokenGenerator, IAntiCsrfTokenValidator tokenValidator)
		{
			configuration.ThrowIfNull("configuration");
			tokenGenerator.ThrowIfNull("tokenGenerator");
			tokenValidator.ThrowIfNull("tokenValidator");

			_configuration = configuration;
			_tokenGenerator = tokenGenerator;
			_tokenValidator = tokenValidator;
		}

		public AntiCsrfData GenerateData()
		{
			string token = GenerateToken();

			return new AntiCsrfData(token, GenerateHiddenInputFieldHtml(token), GenerateCookie(token));
		}

		public string GenerateToken()
		{
			return _tokenGenerator.Generate();
		}

		public string GenerateHiddenInputFieldHtml(string token)
		{
			token.ThrowIfNull("token");

			return _configuration.Enabled ? String.Format(@"<input type=""hidden"" name=""{0}"" value=""{1}""/>", _configuration.FormFieldName, token) : "";
		}

		public HttpCookie GenerateCookie(string token)
		{
			token.ThrowIfNull("token");

			return new HttpCookie(_configuration.CookieName, token) { HttpOnly = true };
		}

		public ValidationResult ValidateRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _tokenValidator.Validate(request);
		}
	}
}