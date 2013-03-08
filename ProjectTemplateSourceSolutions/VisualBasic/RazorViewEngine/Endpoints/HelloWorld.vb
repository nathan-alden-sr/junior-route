Imports Junior.Common
Imports Junior.Route.AutoRouting.RestrictionMappers.Attributes
Imports Junior.Route.Routing.AntiCsrf.HtmlGenerators
Imports Junior.Route.ViewEngines.Razor.TemplateRepositories
Imports Junior.Route.Routing
Imports Junior.Route.Routing.Responses.Text
Imports System.Threading.Tasks

Namespace Endpoints
	Public Class HelloWorld
		Private ReadOnly _templateRespository As ITemplateRepository
		Private ReadOnly _antiCsrfHtmlGenerator As IAntiCsrfHtmlGenerator

		Public Sub New(templateRespository As ITemplateRepository, antiCsrfHtmlGenerator As IAntiCsrfHtmlGenerator)
			templateRespository.ThrowIfNull("templateRepository")
			antiCsrfHtmlGenerator.ThrowIfNull("antiCsrfHtmlGenerator")

			_templateRespository = templateRespository
			_antiCsrfHtmlGenerator = antiCsrfHtmlGenerator
		End Sub

		<Method(HttpMethod.Get)>
		Public Async Function GetResponse() As Task(Of HtmlResponse)
			Dim content As String = _templateRespository.Run(
				"Templates\HelloWorld",
				New Model With {
					.Message = "Hello, world.",
					.AntiCsrfHtml = Await _antiCsrfHtmlGenerator.GenerateHiddenInputHtml()
				})

				Return New HtmlResponse(Content)
		End Function

		Public Class Model
			Property Message As String
			Property AntiCsrfHtml As String
		End Class
	End Class
End Namespace