@Inherits Junior.Route.ViewEngines.Razor.Template(Of JuniorRouteWebApplication.Endpoints.HelloWorld.Model)
@Code
	Layout = "Templates\Layout"
End Code
<div style="font-size: 3em;">@Model.Message</div>
@Model.AntiCsrfHtml
@Include("Templates\Message")
@Section Time
	@DateTime.Now.ToLongDateString()
	@DateTime.Now.ToLongTimeString()
End Section