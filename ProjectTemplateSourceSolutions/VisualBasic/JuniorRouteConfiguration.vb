Imports Junior.Route.AspNetIntegration.AspNet
Imports Junior.Route.Diagnostics
Imports Junior.Route.Routing.Diagnostics
Imports Junior.Route.AspNetIntegration.Diagnostics
Imports Junior.Route.AutoRouting.RestrictionMappers.Attributes
Imports Junior.Route.AutoRouting
Imports Junior.Route.Routing.Caching
Imports Junior.Route.AutoRouting.Containers
Imports Junior.Common
Imports Junior.Route.AspNetIntegration
Imports Junior.Route.Routing
Imports Junior.Route.AspNetIntegration.ResponseGenerators
Imports Junior.Route.AspNetIntegration.ResponseHandlers
Imports Junior.Route.AutoRouting.ParameterMappers
Imports System.Reflection

Public Class JuniorRouteConfiguration
	Inherits JuniorRouteApplicationConfiguration

	Private ReadOnly _routeCollection As IRouteCollection

	Public Sub New()
		' Declare the root endpoint namespace
		Dim endpointNamespace As String = GetType(Global_asax).Namespace + ".Endpoints"

		' Create dependencies
		Dim guidFactory = New GuidFactory
		Dim httpRuntime = New HttpRuntimeWrapper
		Dim urlResolver = New UrlResolver(Function() _routeCollection, httpRuntime)
		Dim restrictionContainer = New DefaultRestrictionContainer(httpRuntime)
		Dim responseGenerators() As IResponseGenerator =
			    { _
				    New MostMatchingRestrictionsGenerator,
				    New UnmatchedRestrictionsGenerator,
				    New NotFoundGenerator
			    }
		Dim responseHandlers() As IResponseHandler = {New NonCacheableResponseHandler}
		Dim parameterMappers() As IParameterMapper = {New DefaultValueMapper}
		Dim cache = New NoCache

		' Provide conventions to a new AutoRouteCollection instance
		Dim autoRouteCollection As AutoRouteCollection = New AutoRouteCollection
		With autoRouteCollection
			.RestrictionContainer(restrictionContainer)
			.Assemblies(Assembly.GetExecutingAssembly)
			.ClassesInNamespace(endpointNamespace)
			.NameAfterRelativeClassNamespaceAndClassName(endpointNamespace)
			.IdRandomly(guidFactory)
			.ResolvedRelativeUrlFromRelativeClassNamespacesAndClassNames(endpointNamespace)
			.RestrictUsingAttributes (Of MethodAttribute)()
			.RestrictRelativeUrlsToRelativeClassNamespacesAndClassNames(endpointNamespace)
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
		Dim httpHandler = New AspNetHttpHandler(_routeCollection, cache, responseGenerators, responseHandlers)

		' Set the handler in the base class
		SetHandler(httpHandler)
	End Sub
End Class
