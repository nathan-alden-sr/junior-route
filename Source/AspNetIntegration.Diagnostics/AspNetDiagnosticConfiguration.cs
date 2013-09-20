using System;
using System.Collections.Generic;
using System.Linq;

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
		private readonly Type _antiCsrfCookieManagerType;
		private readonly Type _antiCsrfNonceValidatorType;
		private readonly Type _antiCsrfResponseGeneratorType;
		private readonly Type _cacheType;
		private readonly Type[] _errorHandlerTypes;
		private readonly IGuidFactory _guidFactory;
		private readonly IHttpRuntime _httpRuntime;
		private readonly Type[] _requestFilterTypes;
		private readonly Type[] _responseGeneratorTypes;
		private readonly Type[] _responseHandlerTypes;
		private readonly IUrlResolver _urlResolver;

		public AspNetDiagnosticConfiguration(
			IGuidFactory guidFactory,
			IUrlResolver urlResolver,
			IHttpRuntime httpRuntime,
			Type cacheType,
			IEnumerable<Type> requestFilterTypes,
			IEnumerable<Type> responseGeneratorTypes,
			IEnumerable<Type> responseHandlerTypes,
			IEnumerable<Type> errorHandlerTypes,
			Type antiCsrfCookieManagerType = null,
			Type antiCsrfNonceValidatorType = null,
			Type antiCsrfResponseGeneratorType = null)
		{
			guidFactory.ThrowIfNull("guidFactory");
			urlResolver.ThrowIfNull("urlResolver");
			httpRuntime.ThrowIfNull("httpRuntime");
			cacheType.ThrowIfNull("cacheType");
			requestFilterTypes.ThrowIfNull("requestFilterTypes");
			responseGeneratorTypes.ThrowIfNull("responseGeneratorTypes");
			responseHandlerTypes.ThrowIfNull("responseHandlerTypes");
			errorHandlerTypes.ThrowIfNull("errorHandlerTypes");

			_guidFactory = guidFactory;
			_urlResolver = urlResolver;
			_httpRuntime = httpRuntime;
			_cacheType = cacheType;
			_requestFilterTypes = requestFilterTypes.ToArray();
			_responseGeneratorTypes = responseGeneratorTypes.ToArray();
			_responseHandlerTypes = responseHandlerTypes.ToArray();
			_errorHandlerTypes = errorHandlerTypes.ToArray();
			_antiCsrfCookieManagerType = antiCsrfCookieManagerType;
			_antiCsrfNonceValidatorType = antiCsrfNonceValidatorType;
			_antiCsrfResponseGeneratorType = antiCsrfResponseGeneratorType;
		}

		public IEnumerable<Routing.Route> GetRoutes(Scheme scheme, string diagnosticsRelativeUrl)
		{
			diagnosticsRelativeUrl.ThrowIfNull("diagnosticsUrl");

			yield return DiagnosticRouteHelper.Instance.GetViewRoute<AspNetView>(
				"Diagnostics ASP.net View",
				_guidFactory.Random(),
				scheme,
				diagnosticsRelativeUrl + "/asp_net",
				ResponseResources.AspNet,
				Enumerable.Empty<string>(),
				_httpRuntime,
				view =>
				{
					view.UrlResolver = _urlResolver;
					view.Populate(_cacheType, _requestFilterTypes, _responseGeneratorTypes, _responseHandlerTypes, _errorHandlerTypes, _antiCsrfCookieManagerType, _antiCsrfNonceValidatorType, _antiCsrfResponseGeneratorType);
				});
			yield return DiagnosticRouteHelper.Instance.GetStylesheetRoute("Diagnostics ASP.net View CSS", _guidFactory.Random(), scheme, diagnosticsRelativeUrl + "/asp_net/css", ResponseResources.asp_net_view, _httpRuntime);
		}

		public IEnumerable<DiagnosticViewLink> GetLinks(string diagnosticsUrl)
		{
			diagnosticsUrl.ThrowIfNull("diagnosticsUrl");

			yield return new DiagnosticViewLink("ASP.net Integration", diagnosticsUrl + "/asp_net", "View ASP.net integration information");
		}
	}
}