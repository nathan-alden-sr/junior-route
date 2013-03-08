Imports Junior.Common
Imports Junior.Route.AutoRouting.RestrictionMappers.Attributes
Imports Junior.Route.Routing.AntiCsrf.HtmlGenerators
Imports Junior.Route.Routing
Imports Junior.Route.Routing.Responses.Text
Imports System.Threading.Tasks

Namespace Endpoints
	Public Class HelloWorld
		Private ReadOnly _antiCsrfHtmlGenerator As IAntiCsrfHtmlGenerator

		Public Sub New(antiCsrfHtmlGenerator As IAntiCsrfHtmlGenerator)
			antiCsrfHtmlGenerator.ThrowIfNull("antiCsrfHtmlGenerator")

			_antiCsrfHtmlGenerator = antiCsrfHtmlGenerator
		End Sub

		<Method(HttpMethod.Get)>
		Public Async Function GetResponse() As Task(Of HtmlResponse)
			Return New HtmlResponse(
				String.Format(
					"<html>" &
					"	<body style=""font-size: 3em;"">" &
					"		Hello, world." &
					"		{0}" &
					"	</body>" &
					"</html>",
					Await _antiCsrfHtmlGenerator.GenerateHiddenInputHtml()))
		End Function
	End Class
End Namespace