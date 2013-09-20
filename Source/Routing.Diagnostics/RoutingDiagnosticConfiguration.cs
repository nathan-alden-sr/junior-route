using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Diagnostics;
using Junior.Route.Diagnostics.Web;
using Junior.Route.Routing.Diagnostics.Web;

namespace Junior.Route.Routing.Diagnostics
{
	public class RoutingDiagnosticConfiguration : IDiagnosticConfiguration
	{
		private readonly IGuidFactory _guidFactory;
		private readonly IHttpRuntime _httpRuntime;
		private readonly Lazy<IRouteCollection> _routes;
		private readonly IUrlResolver _urlResolver;

		public RoutingDiagnosticConfiguration(IGuidFactory guidFactory, IUrlResolver urlResolver, IHttpRuntime httpRuntime, Func<IRouteCollection> routes)
		{
			guidFactory.ThrowIfNull("guidFactory");
			urlResolver.ThrowIfNull("urlResolver");
			httpRuntime.ThrowIfNull("httpRuntime");
			routes.ThrowIfNull("routes");

			_guidFactory = guidFactory;
			_urlResolver = urlResolver;
			_httpRuntime = httpRuntime;
			_routes = new Lazy<IRouteCollection>(routes);
		}

		public RoutingDiagnosticConfiguration(IGuidFactory guidFactory, IUrlResolver urlResolver, IHttpRuntime httpRuntime, IRouteCollection routes)
		{
			guidFactory.ThrowIfNull("guidFactory");
			urlResolver.ThrowIfNull("urlResolver");
			httpRuntime.ThrowIfNull("httpRuntime");
			routes.ThrowIfNull("routes");

			_guidFactory = guidFactory;
			_urlResolver = urlResolver;
			_httpRuntime = httpRuntime;
			_routes = new Lazy<IRouteCollection>(() => routes);
		}

		private static IEnumerable<string> RouteTableViewNamespaces
		{
			get
			{
				yield return "System";
				yield return "System.Linq";
				yield return "Junior.Route.Routing.RequestValueComparers";
				yield return "Junior.Route.Routing.Restrictions";
			}
		}

		public IEnumerable<Route> GetRoutes(Scheme scheme, string diagnosticsRelativeUrl)
		{
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsUrl");

			yield return DiagnosticRouteHelper.Instance.GetViewRoute<RouteTableView>(
				"Diagnostics Route Table View",
				_guidFactory.Random(),
				scheme,
				diagnosticsRelativeUrl + "/route_table",
				ResponseResources.RouteTable,
				RouteTableViewNamespaces,
				_httpRuntime,
				view => view.Populate(_urlResolver, _routes.Value, diagnosticsRelativeUrl));
			yield return DiagnosticRouteHelper.Instance.GetStylesheetRoute("Diagnostics Route Table View CSS", _guidFactory.Random(), scheme, diagnosticsRelativeUrl + "/route_table/css", ResponseResources.route_table_view, _httpRuntime);
		}

		public IEnumerable<DiagnosticViewLink> GetLinks(string diagnosticsUrl)
		{
			diagnosticsUrl.ThrowIfNull("diagnosticsUrl");

			yield return new DiagnosticViewLink("Routes", diagnosticsUrl + "/route_table", "View route table");
		}
	}
}