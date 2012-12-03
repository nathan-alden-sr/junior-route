Imports Junior.Route.ViewEngines.Razor.CompiledTemplateFactories
Imports Junior.Route.ViewEngines.Razor.CodeDomProviderFactories
Imports Junior.Route.AspNetIntegration
Imports Junior.Common
Imports JuniorRouteWebApplication.Endpoints
Imports Junior.Route.AutoRouting.Containers
Imports Junior.Route.Routing
Imports Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers
Imports Junior.Route.ViewEngines.Razor.TemplateAssemblyReferenceResolvers
Imports Junior.Route.ViewEngines.Razor.TemplateCompilers
Imports Junior.Route.ViewEngines.Razor.TemplateClassNameBuilders
Imports Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
Imports Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories

Namespace Containers
	Public Class EndpointContainer
		Implements IContainer

		Private ReadOnly _fileSystemRepository As FileSystemRepository

		Public Sub New(httpRuntime As IHttpRuntime, guidFactory As IGuidFactory)
			Dim pathResolver = New VisualBasicResolver(httpRuntime)
			Dim fileSystem = New FileSystem(httpRuntime)
			Dim appDomainResolver = New AppDomainResolver
			Dim compiler = New TemplateCompiler(appDomainResolver)
			Dim classNameBuilder = New RandomGuidBuilder(guidFactory)
			Dim codeBuilder = New VisualBasicBuilder
			Dim codeDomProviderFactory = New FileExtensionFactory
			Dim compiledTemplateFactory = New ActivatorFactory

			_fileSystemRepository = New FileSystemRepository(pathResolver, fileSystem, compiler, classNameBuilder, codeBuilder, codeDomProviderFactory, compiledTemplateFactory)
		End Sub

		Public Function GetInstance(Of T)() As T Implements IContainer.GetInstance
			Return GetInstance(GetType(T))
		End Function

		Public Function GetInstance(ByVal type As Type) As Object Implements IContainer.GetInstance
			If (type = GetType(HelloWorld)) Then
				Return New HelloWorld(_fileSystemRepository)
			End If

			Throw New ArgumentException(String.Format("Could not get instance for {0}.", type.FullName), "type")
		End Function
	End Class
End Namespace