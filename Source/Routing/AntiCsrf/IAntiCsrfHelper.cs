using System.Web;

using Junior.Route.Routing.AntiCsrf.Validators;

namespace Junior.Route.Routing.AntiCsrf
{
	public interface IAntiCsrfHelper
	{
		AntiCsrfData GenerateData();
		string GenerateToken();
		string GenerateHiddenInputFieldHtml(string token);
		HttpCookie GenerateCookie(string token);
		ValidationResult ValidateRequest(HttpRequestBase request);
	}
}