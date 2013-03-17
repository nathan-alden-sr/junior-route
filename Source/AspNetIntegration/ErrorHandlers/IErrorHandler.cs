using System.Web;

namespace Junior.Route.AspNetIntegration.ErrorHandlers
{
	public interface IErrorHandler
	{
		HandleResult Handle(HttpContextBase context);
	}
}