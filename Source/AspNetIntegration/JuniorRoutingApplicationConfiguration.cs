using NathanAlden.JuniorRouting.Core;

namespace NathanAlden.JuniorRouting.AspNetIntegration
{
	public class JuniorRoutingApplicationConfiguration
	{
		private readonly HttpRoutes _routes = new HttpRoutes();

		public HttpRoutes Routes
		{
			get
			{
				return _routes;
			}
		}
	}
}