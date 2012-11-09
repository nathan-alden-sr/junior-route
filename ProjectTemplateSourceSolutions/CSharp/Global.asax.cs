using System.Web;

using Junior.Route.AspNetIntegration;

namespace JuniorRouteWebApplication
{
	public class Global : HttpApplication
	{
		public Global()
		{
			JuniorRouteApplication.AttachToHttpApplication(this);
		}
	}
}