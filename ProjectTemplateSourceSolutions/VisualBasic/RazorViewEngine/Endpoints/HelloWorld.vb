Imports Junior.Route.AutoRouting.RestrictionMappers.Attributes
Imports Junior.Route.Routing.Responses.Text
Imports Junior.Route.Routing
Imports Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories
Imports Junior.Route.ViewEngines.Razor

Namespace Endpoints
	Public Class HelloWorld
		Private ReadOnly _templateRespository As ITemplateRepository

		Public Sub New(templateRespository As ITemplateRepository)
			_templateRespository = templateRespository
		End Sub

		<Method(HttpMethod.Get)>
		Public Function GetResponse() As HtmlResponse
			Dim content As String = _templateRespository.Execute("Templates\HelloWorld", New Model With {.Message = "Hello, world."})

			Return New HtmlResponse(content)
		End Function

		Public Class Model
			Property Message As String
		End Class
	End Class
End Namespace