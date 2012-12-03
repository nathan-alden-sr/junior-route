using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AspNetIntegration;
using Junior.Route.AspNetIntegration.AspNet;
using Junior.Route.AspNetIntegration.Diagnostics;
using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.AutoRouting;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.Diagnostics;
using Junior.Route.Routing;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Diagnostics;

using JuniorRouteWebApplication.Containers;

namespace JuniorRouteWebApplication
{
	public class JuniorRouteConfiguration : JuniorRouteApplicationConfiguration
	{
		private readonly IRouteCollection _routeCollection;

		public JuniorRouteConfiguration()
		{
			// Declare the root endpoint namespace
			string endpointNamespace = typeof(Global).Namespace + ".Endpoints";

			// Create dependencies
			var guidFactory = new GuidFactory();
			var httpRuntime = new HttpRuntimeWrapper();
			var urlResolver = new UrlResolver(() => _routeCollection, httpRuntime);
			var endpointContainer = new EndpointContainer(httpRuntime, guidFactory);
			var restrictionContainer = new DefaultRestrictionContainer(httpRuntime);
			var responseGenerators = new IResponseGenerator[]
				{
					new MostMatchingRestrictionsGenerator(),
					new UnmatchedRestrictionsGenerator(),
					new NotFoundGenerator()
				};
			var responseHandlers = new IResponseHandler[] { new NonCacheableResponseHandler() };
			var parameterMappers = new IParameterMapper[] { new DefaultValueMapper() };
			var cache = new NoCache();

			// Provide conventions to a new AutoRouteCollection instance
			AutoRouteCollection autoRouteCollection = new AutoRouteCollection()
				.EndpointContainer(endpointContainer)
				.RestrictionContainer(restrictionContainer)
				.Assemblies(Assembly.GetExecutingAssembly())
				.ClassesInNamespace(endpointNamespace)
				.NameAfterRelativeClassNamespaceAndClassName(endpointNamespace)
				.IdRandomly(guidFactory)
				.ResolvedRelativeUrlFromRelativeClassNamespaceAndClassName(endpointNamespace)
				.RestrictHttpMethodsToMethodsNamedAfterStandardHttpMethods()
				.RestrictRelativePathsToRelativeClassNamespaceAndClassName(endpointNamespace)
				.RespondWithMethodReturnValuesThatImplementIResponse(parameterMappers)
				// Add diagnostics routes
				.AdditionalRoutes(
					DiagnosticConfigurationRoutes.Instance.GetRoutes(
						guidFactory,
						urlResolver,
						httpRuntime,
						"_diagnostics",
						new AspNetDiagnosticConfiguration(
							cache.GetType(),
							responseGenerators.Select(arg => arg.GetType()),
							responseHandlers.Select(arg => arg.GetType())),
						new RoutingDiagnosticConfiguration(() => _routeCollection)));

			// Generate routes
			_routeCollection = autoRouteCollection.GenerateRouteCollection();

			// Create an HTTP handler
			var httpHandler = new AspNetHttpHandler(_routeCollection, cache, responseGenerators, responseHandlers);

			// Set the handler in the base class
			SetHandler(httpHandler);
		}
	}
}