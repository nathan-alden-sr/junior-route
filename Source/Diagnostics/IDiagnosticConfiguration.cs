using System.Collections.Generic;

using Junior.Route.Common;
using Junior.Route.Diagnostics.Web;
using Junior.Route.Routing;

namespace Junior.Route.Diagnostics
{
	public interface IDiagnosticConfiguration
	{
		IEnumerable<Routing.Route> GetRoutes(Scheme scheme, string diagnosticsRelativeUrl);
		IEnumerable<DiagnosticViewLink> GetLinks(string diagnosticsUrl);
	}
}