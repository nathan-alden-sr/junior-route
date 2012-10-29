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
		private readonly IRouteCollection _routes;

		public RoutingDiagnosticConfiguration(IRouteCollection routes)
		{
			routes.ThrowIfNull("routes");

			_routes = routes;
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

		public IEnumerable<string> LinkHeadings
		{
			get
			{
				yield return "Routes";
			}
		}

		public IEnumerable<Route> GetRoutes(IGuidFactory guidFactory, IUrlResolver urlResolver, string diagnosticsRelativeUrl)
		{
			guidFactory.ThrowIfNull("guidFactory");
			urlResolver.ThrowIfNull("urlResolver");
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsRelativeUrl");

			yield return DiagnosticRouteHelper.Instance.GetViewRoute<RouteTableView>(
				"Diagnostics Route Table View",
				guidFactory,
				diagnosticsRelativeUrl + "/route_table",
				ResponseResources.RouteTable,
				RouteTableViewNamespaces,
				view => view.Populate(urlResolver, _routes, diagnosticsRelativeUrl));
			yield return DiagnosticRouteHelper.Instance.GetStylesheetRoute("Diagnostics Route Table View CSS", guidFactory, diagnosticsRelativeUrl + "/route_table/css", ResponseResources.route_table_view);
		}

		public IEnumerable<DiagnosticViewLink> GetLinks(string diagnosticsRelativeUrl, string heading)
		{
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsRelativeUrl");
			heading.ThrowIfNull("heading");

			yield return new DiagnosticViewLink(diagnosticsRelativeUrl + "/route_table", "View route table");
		}
	}
}