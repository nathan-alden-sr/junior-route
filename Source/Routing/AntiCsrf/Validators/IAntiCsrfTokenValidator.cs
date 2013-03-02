using System.Web;

namespace Junior.Route.Routing.AntiCsrf.Validators
{
	public interface IAntiCsrfTokenValidator
	{
		ValidationResult Validate(HttpRequestBase request);
	}
}