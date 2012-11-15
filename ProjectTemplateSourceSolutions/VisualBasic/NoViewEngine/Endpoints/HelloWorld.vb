Imports Junior.Route.AutoRouting.RestrictionMappers.Attributes
Imports Junior.Route.Routing.Responses.Text
Imports Junior.Route.Routing

Namespace Endpoints
	Public Class HelloWorld
		<Method(HttpMethod.Get)>
		Public Function GetResponse() As HtmlResponse
			Return New HtmlResponse("<html><body style=""font-size: 3em;"">Hello, world.</body></html>")
		End Function
	End Class
End Namespace