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
		private readonly Lazy<IRouteCollection> _routes;

		public RoutingDiagnosticConfiguration(Func<IRouteCollection> routes)
		{
			routes.ThrowIfNull("routes");

			_routes = new Lazy<IRouteCollection>(routes);
		}

		public RoutingDiagnosticConfiguration(IRouteCollection routes)
		{
			routes.ThrowIfNull("routes");

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

		public IEnumerable<Route> GetRoutes(IGuidFactory guidFactory, IUrlResolver urlResolver, IHttpRuntime httpRuntime, string diagnosticsRelativeUrl)
		{
			guidFactory.ThrowIfNull("guidFactory");
			urlResolver.ThrowIfNull("urlResolver");
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsUrl");

			yield return DiagnosticRouteHelper.Instance.GetViewRoute<RouteTableView>(
				"Diagnostics Route Table View",
				guidFactory,
				diagnosticsRelativeUrl + "/route_table",
				ResponseResources.RouteTable,
				RouteTableViewNamespaces,
				httpRuntime,
				view => view.Populate(urlResolver, _routes.Value, diagnosticsRelativeUrl));
			yield return DiagnosticRouteHelper.Instance.GetStylesheetRoute("Diagnostics Route Table View CSS", guidFactory, diagnosticsRelativeUrl + "/route_table/css", ResponseResources.route_table_view, httpRuntime);
		}

		public IEnumerable<DiagnosticViewLink> GetLinks(string diagnosticsUrl)
		{
			diagnosticsUrl.ThrowIfNull("diagnosticsUrl");

			yield return new DiagnosticViewLink("Routes", diagnosticsUrl + "/route_table", "View route table");
		}
	}
}