using System;
using System.Linq;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing;

namespace Junior.Route.AspNetIntegration
{
	public class UrlResolver : IUrlResolver
	{
		private readonly IHttpRuntime _httpRuntime;
		private readonly Lazy<IRouteCollection> _routes;

		public UrlResolver(Func<IRouteCollection> routes, IHttpRuntime httpRuntime)
		{
			routes.ThrowIfNull("routes");
			httpRuntime.ThrowIfNull("httpRuntime");

			_routes = new Lazy<IRouteCollection>(routes);
			_httpRuntime = httpRuntime;
		}

		public UrlResolver(IRouteCollection routes, IHttpRuntime httpRuntime)
		{
			routes.ThrowIfNull("routes");
			httpRuntime.ThrowIfNull("httpRuntime");

			_routes = new Lazy<IRouteCollection>(() => routes);
			_httpRuntime = httpRuntime;
		}

		public string Absolute(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			string rootUrl = _httpRuntime.AppDomainAppVirtualPath.TrimStart('/');

			return String.Format("/{0}/{1}", rootUrl, relativeUrl.TrimStart('/'));
		}

		public string Route(string routeName)
		{
			routeName.ThrowIfNull("routeName");

			Routing.Route[] routes = _routes.Value.Where(arg => arg.Name == routeName).ToArray();

			if (routes.Length > 1)
			{
				throw new ArgumentException(String.Format("More than one route exists with name '{0}'.", routeName));
			}
			if (!routes.Any())
			{
				throw new ArgumentException(String.Format("Route with name '{0}' was not found.", routeName));
			}

			return Absolute(routes[0].ResolvedRelativeUrl);
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