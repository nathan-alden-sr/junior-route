Imports Junior.Route.AspNetIntegration

Public Module JuniorRoute
	Public Sub Start()
		JuniorRouteApplication.RegisterConfiguration(Of JuniorRouteConfiguration)()
	End Sub
End Module