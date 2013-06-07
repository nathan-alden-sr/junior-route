using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.Routing.AntiCsrf.NonceValidators
{
	public interface IAntiCsrfNonceValidator
	{
		Task<ValidationResult> ValidateAsync(HttpRequestBase request);
	}
}