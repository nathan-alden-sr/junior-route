using Junior.Route.AspNetIntegration;

namespace JuniorRouteWebApplication
{
	public static class JuniorRoute
	{
		public static void Start()
		{
			JuniorRouteApplication.RegisterConfiguration<JuniorRouteConfiguration>();
		}
	}
}