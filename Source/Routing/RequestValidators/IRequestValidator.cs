using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.Routing.RequestValidators
{
	public interface IRequestValidator
	{
		Task<ValidateResult> Validate(HttpRequestBase request, HttpResponseBase response);
	}
}