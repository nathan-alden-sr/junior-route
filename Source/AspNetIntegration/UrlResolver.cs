using System;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing;

namespace Junior.Route.AspNetIntegration
{
	public class UrlResolver : IUrlResolver
	{
		private readonly Lazy<IRouteCollection> _routes;

		public UrlResolver(Func<IRouteCollection> routes)
		{
			routes.ThrowIfNull("routes");

			_routes = new Lazy<IRouteCollection>(routes);
		}

		public UrlResolver(IRouteCollection routes)
		{
			routes.ThrowIfNull("routes");

			_routes = new Lazy<IRouteCollection>(() => routes);
		}

		public string Absolute(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			string rootUrl = HttpRuntime.AppDomainAppVirtualPath.TrimStart('/');

			return String.Format("/{0}/{1}", rootUrl, relativeUrl.TrimStart('/'));
		}

		public string Route(string routeName)
		{
			routeName.ThrowIfNull("routeName");

			Routing.Route route = _routes.Value.SingleOrDefault(arg => arg.Name == routeName);

			if (route == null)
			{
				throw new ArgumentException(String.Format("Route with name '{0}' was not found.", routeName));
			}

			return Absolute(route.ResolvedRelativeUrl);
		}

		public string Route(Guid routeId)
		{
			Routing.Route route = _routes.Value.SingleOrDefault(arg => arg.Id == routeId);

			if (route == null)
			{
				throw new ArgumentException(String.Format("Route with ID '{0}' was not found.", routeId));
			}

			return Absolute(route.ResolvedRelativeUrl);
		}
	}
}