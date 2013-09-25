using System;
using System.Linq;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing;

namespace Junior.Route.AspNetIntegration
{
	public class UrlResolver : IUrlResolver
	{
		private readonly IUrlResolverConfiguration _configuration;
		private readonly IHttpRuntime _httpRuntime;
		private readonly Lazy<IRouteCollection> _routes;

		public UrlResolver(Func<IRouteCollection> routes, IUrlResolverConfiguration configuration, IHttpRuntime httpRuntime)
		{
			routes.ThrowIfNull("routes");
			configuration.ThrowIfNull("configuration");
			httpRuntime.ThrowIfNull("httpRuntime");

			_routes = new Lazy<IRouteCollection>(routes);
			_configuration = configuration;
			_httpRuntime = httpRuntime;
		}

		public UrlResolver(IRouteCollection routes, IUrlResolverConfiguration configuration, IHttpRuntime httpRuntime)
		{
			routes.ThrowIfNull("routes");
			configuration.ThrowIfNull("configuration");
			httpRuntime.ThrowIfNull("httpRuntime");

			_routes = new Lazy<IRouteCollection>(() => routes);
			_configuration = configuration;
			_httpRuntime = httpRuntime;
		}

		public string Absolute(Scheme scheme, string relativeUrl, params object[] args)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			string rootRelativePath = _httpRuntime.AppDomainAppVirtualPath.TrimStart('/');
			string rootPath = String.Format("{0}/{1}", rootRelativePath.Length > 0 ? "/" + rootRelativePath : "", String.Format(relativeUrl.TrimStart('/'), args));

			switch (scheme)
			{
				case Scheme.NotSpecified:
					return rootPath;
				case Scheme.Http:
					return new UriBuilder("http", _configuration.HostName, _configuration.GetPort(scheme), rootPath).GetUriWithoutOptionalPort().ToString();
				case Scheme.Https:
					return new UriBuilder("https", _configuration.HostName, _configuration.GetPort(scheme), rootPath).GetUriWithoutOptionalPort().ToString();
				default:
					throw new ArgumentOutOfRangeException("scheme");
			}
		}

		public string Absolute(string relativeUrl, params object[] args)
		{
			return Absolute(Scheme.NotSpecified, relativeUrl, args);
		}

		public string Route(Scheme scheme, string routeName, params object[] args)
		{
			routeName.ThrowIfNull("routeName");

			Routing.Route[] routes = _routes.Value.GetRoutes(routeName).ToArray();

			if (routes.Length > 1)
			{
				throw new ArgumentException(String.Format("More than one route exists with name '{0}'.", routeName), "routeName");
			}
			if (!routes.Any())
			{
				throw new ArgumentException(String.Format("Route with name '{0}' was not found.", routeName), "routeName");
			}

			return Absolute(scheme, routes[0].ResolvedRelativeUrl, args);
		}

		public string Route(string routeName, params object[] args)
		{
			routeName.ThrowIfNull("routeName");

			Routing.Route[] routes = _routes.Value.GetRoutes(routeName).ToArray();

			if (routes.Length > 1)
			{
				throw new ArgumentException(String.Format("More than one route exists with name '{0}'.", routeName), "routeName");
			}
			if (!routes.Any())
			{
				throw new ArgumentException(String.Format("Route with name '{0}' was not found.", routeName), "routeName");
			}

			return Absolute(routes[0].Scheme, routes[0].ResolvedRelativeUrl, args);
		}

		public string Route(Scheme scheme, Guid routeId, params object[] args)
		{
			Routing.Route route = _routes.Value.GetRoute(routeId);

			if (route == null)
			{
				throw new ArgumentException(String.Format("Route with ID '{0}' was not found.", routeId), "routeId");
			}

			return Absolute(route.ResolvedRelativeUrl, args);
		}

		public string Route(Guid routeId, params object[] args)
		{
			Routing.Route route = _routes.Value.GetRoute(routeId);

			if (route == null)
			{
				throw new ArgumentException(String.Format("Route with ID '{0}' was not found.", routeId), "routeId");
			}

			return Absolute(route.ResolvedRelativeUrl, args);
		}
	}
}