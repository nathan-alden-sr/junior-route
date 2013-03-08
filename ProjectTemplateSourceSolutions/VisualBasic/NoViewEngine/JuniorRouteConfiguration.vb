Imports Junior.Route.AspNetIntegration.AspNet
Imports Junior.Route.Diagnostics
Imports Junior.Route.Routing.Diagnostics
Imports Junior.Route.AspNetIntegration.Diagnostics
Imports Junior.Route.AutoRouting.RestrictionMappers.Attributes
Imports Junior.Route.AutoRouting
Imports Junior.Route.Routing.AntiCsrf.CookieManagers
Imports Junior.Route.Routing.Caching
Imports Junior.Common
Imports Junior.Route.AutoRouting.Containers
Imports JuniorRouteWebApplication.Containers
Imports Junior.Route.AspNetIntegration
Imports Junior.Route.Routing
Imports Junior.Route.AspNetIntegration.ResponseGenerators
Imports Junior.Route.AspNetIntegration.ResponseHandlers
Imports Junior.Route.AutoRouting.ParameterMappers
Imports Junior.Route.Routing.AntiCsrf.NonceValidators
Imports Junior.Route.Routing.AntiCsrf.ResponseGenerators
Imports System.Reflection

Public Class JuniorRouteConfiguration
	Inherits JuniorRouteApplicationConfiguration

	Private ReadOnly _routeCollection As IRouteCollection

	Public Sub New()
		' Declare the root endpoint namespace
		Dim endpointNamespace As String = GetType(Global_asax).Namespace + ".Endpoints"

		' Create dependencies
		Dim httpRuntime = New HttpRuntimeWrapper
		Dim urlResolver = New UrlResolver(Function() _routeCollection, httpRuntime)
		Dim endpointContainer = New EndpointContainer
		Dim restrictionContainer = New DefaultRestrictionContainer(httpRuntime)
		Dim guidFactory = endpointContainer.GetInstance (Of IGuidFactory)()
		Dim responseGenerators() As IResponseGenerator =
			    { _
				    New MostMatchingRestrictionsGenerator,
				    New UnmatchedRestrictionsGenerator,
				    New NotFoundGenerator
			    }
		Dim responseHandlers() As IResponseHandler = {New NonCacheableResponseHandler}
		Dim parameterMappers() As IParameterMapper = {New DefaultValueMapper}
		Dim cache = New NoCache
		Dim antiCsrfCookieManager = endpointContainer.GetInstance (Of IAntiCsrfCookieManager)()
		Dim antiCsrfNonceValidator = endpointContainer.GetInstance (Of IAntiCsrfNonceValidator)()
		Dim antiCsrfResponseGenerator = endpointContainer.GetInstance (Of IAntiCsrfResponseGenerator)()

		' Provide conventions to a new AutoRouteCollection instance
		Dim autoRouteCollection As AutoRouteCollection = New AutoRouteCollection
		With autoRouteCollection
			.EndpointContainer(endpointContainer)
			.RestrictionContainer(restrictionContainer)
			.Assemblies(Assembly.GetExecutingAssembly)
			.ClassesInNamespace(endpointNamespace)
			.NameAfterRelativeClassNamespaceAndClassName(endpointNamespace)
			.IdRandomly(guidFactory)
			.ResolvedRelativeUrlFromRelativeClassNamespaceAndClassName(endpointNamespace)
			.RestrictUsingAttributes (Of MethodAttribute)()
			.RestrictRelativePathsToRelativeClassNamespaceAndClassName(endpointNamespace)
			.RespondWithMethodReturnValuesThatImplementIResponse(parameterMappers)
			' Add diagnostic routes
			.AdditionalRoutes(
				DiagnosticConfigurationRoutes.Instance.GetRoutes(
					guidFactory,
					urlResolver,
					httpRuntime,
					"_diagnostics",
					New AspNetDiagnosticConfiguration(
						cache.GetType(),
						responseGenerators.Select(Function(arg) arg.GetType()),
						responseHandlers.Select(Function(arg) arg.GetType())),
					New RoutingDiagnosticConfiguration(Function() _routeCollection)))
		End With

		' Generate routes
		_routeCollection = autoRouteCollection.GenerateRouteCollection

		' Create an HTTP handler
		Dim httpHandler = New AspNetHttpHandler(
			_routeCollection,
			cache,
			responseGenerators,
			responseHandlers,
			antiCsrfCookieManager,
			antiCsrfNonceValidator,
			antiCsrfResponseGenerator)

		' Set the handler in the base class
		SetHandler(httpHandler)
	End Sub
End Class
