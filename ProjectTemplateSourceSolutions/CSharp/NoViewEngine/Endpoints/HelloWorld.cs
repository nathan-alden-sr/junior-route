using Junior.Route.Routing.Responses.Text;

namespace JuniorRouteWebApplication.Endpoints
{
	public class HelloWorld
	{
		public HtmlResponse Get()
		{
			return new HtmlResponse(@"<html><body style=""font-size: 3em;"">Hello, world.</body></html>");
		}
	}
}