using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AspNetIntegration.Diagnostics.Web;
using Junior.Route.Common;
using Junior.Route.Diagnostics;
using Junior.Route.Diagnostics.Web;

namespace Junior.Route.AspNetIntegration.Diagnostics
{
	public class AspNetDiagnosticConfiguration : IDiagnosticConfiguration
	{
		private readonly Type _cacheType;
		private readonly IEnumerable<Type> _matchedResponseHandlerTypes;
		private readonly IEnumerable<Type> _routeMatchingStrategyTypes;
		private readonly IEnumerable<Type> _unmatchedResponseHandlerTypes;

		public AspNetDiagnosticConfiguration(Type cacheType, IEnumerable<Type> routeMatchingStrategyTypes, IEnumerable<Type> matchedResponseHandlerTypes, IEnumerable<Type> unmatchedResponseHandlerTypes)
		{
			cacheType.ThrowIfNull("cacheType");
			routeMatchingStrategyTypes.ThrowIfNull("responseGeneratorTypes");
			matchedResponseHandlerTypes.ThrowIfNull("matchedResponseHandlerTypes");
			unmatchedResponseHandlerTypes.ThrowIfNull("unmatchedResponseHandlerTypes");

			_cacheType = cacheType;
			_routeMatchingStrategyTypes = routeMatchingStrategyTypes;
			_matchedResponseHandlerTypes = matchedResponseHandlerTypes;
			_unmatchedResponseHandlerTypes = unmatchedResponseHandlerTypes;
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

		public IEnumerable<Routing.Route> GetRoutes(IGuidFactory guidFactory, IUrlResolver urlResolver, string diagnosticsRelativeUrl)
		{
			guidFactory.ThrowIfNull("guidFactory");
			urlResolver.ThrowIfNull("urlResolver");
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsRelativeUrl");

			yield return DiagnosticRouteHelper.Instance.GetViewRoute<AspNetView>(
				"Diagnostics ASP.net View",
				guidFactory,
				diagnosticsRelativeUrl + "/asp_net",
				ResponseResources.AspNet,
				AspNetViewNamespaces,
				view =>
					{
						view.UrlResolver = urlResolver;
						view.Populate(_cacheType, _routeMatchingStrategyTypes, _matchedResponseHandlerTypes, _unmatchedResponseHandlerTypes);
					});
			yield return DiagnosticRouteHelper.Instance.GetStylesheetRoute("Diagnostics ASP.net View CSS", guidFactory, diagnosticsRelativeUrl + "/asp_net/css", ResponseResources.asp_net_view);
		}

		public IEnumerable<DiagnosticViewLink> GetLinks(string diagnosticsRelativeUrl, string heading)
		{
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsRelativeUrl");
			heading.ThrowIfNull("heading");

			yield return new DiagnosticViewLink(diagnosticsRelativeUrl + "/asp_net", "View ASP.net integration information");
		}
	}
}