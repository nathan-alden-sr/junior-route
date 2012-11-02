using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AspNetIntegration.Diagnostics.Web;
using Junior.Route.Common;
using Junior.Route.Diagnostics;
using Junior.Route.Diagnostics.Web;
using Junior.Route.Routing;

namespace Junior.Route.AspNetIntegration.Diagnostics
{
	public class AspNetDiagnosticConfiguration : IDiagnosticConfiguration
	{
		private readonly Type _cacheType;
		private readonly IEnumerable<Type> _responseGeneratorTypes;
		private readonly IEnumerable<Type> _responseHandlerTypes;

		public AspNetDiagnosticConfiguration(Type cacheType, IEnumerable<Type> responseGeneratorTypes, IEnumerable<Type> responseHandlerTypes)
		{
			cacheType.ThrowIfNull("cacheType");
			responseGeneratorTypes.ThrowIfNull("responseGeneratorTypes");
			responseHandlerTypes.ThrowIfNull("responseHandlerTypes");

			_cacheType = cacheType;
			_responseGeneratorTypes = responseGeneratorTypes;
			_responseHandlerTypes = responseHandlerTypes;
		}

		private static IEnumerable<string> AspNetViewNamespaces
		{
			get
			{
				yield break;
			}
		}

		public IEnumerable<string> LinkHeadings
		{
			get
			{
				yield return "ASP.net Integration";
			}
		}

		public IEnumerable<Routing.Route> GetRoutes(IGuidFactory guidFactory, IUrlResolver urlResolver, IHttpRuntime httpRuntime, string diagnosticsRelativeUrl)
		{
			guidFactory.ThrowIfNull("guidFactory");
			urlResolver.ThrowIfNull("urlResolver");
			httpRuntime.ThrowIfNull("httpRuntime");
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsRelativeUrl");

			yield return DiagnosticRouteHelper.Instance.GetViewRoute<AspNetView>(
				"Diagnostics ASP.net View",
				guidFactory,
				diagnosticsRelativeUrl + "/asp_net",
				ResponseResources.AspNet,
				AspNetViewNamespaces,
				httpRuntime,
				view =>
					{
						view.UrlResolver = urlResolver;
						view.Populate(_cacheType, _responseGeneratorTypes, _responseHandlerTypes);
					});
			yield return DiagnosticRouteHelper.Instance.GetStylesheetRoute("Diagnostics ASP.net View CSS", guidFactory, diagnosticsRelativeUrl + "/asp_net/css", ResponseResources.asp_net_view, httpRuntime);
		}

		public IEnumerable<DiagnosticViewLink> GetLinks(string diagnosticsRelativeUrl, string heading)
		{
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsRelativeUrl");
			heading.ThrowIfNull("heading");

			yield return new DiagnosticViewLink(diagnosticsRelativeUrl + "/asp_net", "View ASP.net integration information");
		}
	}
}