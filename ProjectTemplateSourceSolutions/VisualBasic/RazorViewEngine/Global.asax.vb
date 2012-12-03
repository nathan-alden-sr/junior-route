Imports Junior.Route.AspNetIntegration

Public Class Global_asax
	Inherits HttpApplication

	Public Sub New()
		JuniorRouteApplication.AttachToHttpApplication(Me)
	End Sub
End Class