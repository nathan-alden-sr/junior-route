using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.AspNetIntegration.ErrorHandlers
{
	public interface IErrorHandler
	{
		Task<HandleResult> Handle(HttpContextBase context);
	}
}