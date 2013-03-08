Imports System.Linq.Expressions
Imports JuniorRouteWebApplication.Endpoints
Imports Junior.Route.AutoRouting.AntiCsrf.HtmlGenerators
Imports Junior.Route.Routing.AntiCsrf.HtmlGenerators
Imports Junior.Route.Routing.AntiCsrf.CookieManagers
Imports Junior.Route.Routing.AntiCsrf
Imports Junior.Route.AutoRouting
Imports Junior.Common
Imports Junior.Route.AutoRouting.Containers
Imports Junior.Route.Routing.AntiCsrf.NonceRepositories
Imports Junior.Route.Routing.AntiCsrf.NonceValidators
Imports Junior.Route.Routing.AntiCsrf.ResponseGenerators
Imports System.Reflection

Namespace Containers
	Public Class EndpointContainer
		Implements IContainer
		Private ReadOnly _concreteTypesByRequestedType As Dictionary(Of Type, Type) = New Dictionary(Of Type, Type)
		Private ReadOnly _instancesByRequestedType As Dictionary(Of Type, Object) = New Dictionary(Of Type, Object)
		Private ReadOnly _lockObject = New Object()

		Public Sub New()
			' Common
			AddMapping (Of IGuidFactory, GuidFactory)()
			AddMapping (Of ISystemClock, SystemClock)()
			AddMapping (Of IResponseContext, ResponseContext)()

			' Anti-CSRF
			AddMapping (Of IAntiCsrfConfiguration, ConfigurationSectionAntiCsrfConfiguration)()
			AddMapping (Of IAntiCsrfCookieManager, DefaultCookieManager)()
			AddMapping (Of IAntiCsrfHtmlGenerator, DefaultHtmlGenerator)()
			AddMapping (Of IAntiCsrfNonceRepository, MemoryCacheNonceRepository)()
			AddMapping (Of IAntiCsrfNonceValidator, DefaultNonceValidator)()
			AddMapping (Of IAntiCsrfResponseGenerator, DefaultResponseGenerator)()

			' Endpoints
			AddMapping (Of HelloWorld, HelloWorld)()
		End Sub

		Public Function GetInstance (Of T)() As T Implements IContainer.GetInstance
			Return GetInstance(GetType(T))
		End Function

		Public Function GetInstance(ByVal type As Type) As Object Implements IContainer.GetInstance
			type.ThrowIfNull("type")

			SyncLock _lockObject
				Dim existingInstance As Object = Nothing

				If _instancesByRequestedType.TryGetValue(type, existingInstance) Then
					Return existingInstance
				End If

				Dim concreteType As Type = Nothing

				If Not _concreteTypesByRequestedType.TryGetValue(type, concreteType) Then
					Return Nothing
				End If

				Dim constructorInfos As ConstructorInfo() = concreteType.GetConstructors(BindingFlags.Public + BindingFlags.Instance)

				If constructorInfos.Length = 0 Then
					Throw New ArgumentException(String.Format("No public instance constructors found for type {0}.", concreteType.FullName))
				End If

				Dim pair As KeyValuePair(Of ConstructorInfo, ParameterInfo()) = constructorInfos _
					    .Select(Function(arg) New With {.constructorInfo = arg, .parameterInfos = arg.GetParameters()}) _
					    .ToDictionary(Function(arg) arg.constructorInfo, Function(arg) arg.parameterInfos) _
					    .OrderByDescending(Function(arg) arg.Value.Length) _
					    .Single()
				Dim parameterValues As IEnumerable(Of Object) = pair.Value.Select(
					Function(parameterInfo)
						Dim parameterValue As Object = If(GetInstance(parameterInfo.ParameterType), If(parameterInfo.HasDefaultValue, parameterInfo.DefaultValue, Nothing))

						If (parameterValue Is Nothing) Then
							Throw New ApplicationException(String.Format("Unable to map parameter '{0}' of type {1}.", parameterInfo, pair.Key.DeclaringType.FullName))
						End If

						Return parameterValue
					End Function)
				Dim newExpression As NewExpression = Expression.[New](pair.Key, parameterValues.Select(AddressOf Expression.Constant))
				Dim newInstance = Expression.Lambda(newExpression).Compile().DynamicInvoke()

				_instancesByRequestedType.Add(type, newInstance)

				Return newInstance
			End SyncLock
		End Function

		Private Sub AddMapping (Of TRequestedType, TConcreteType As TRequestedType)()
			SyncLock _lockObject
				_concreteTypesByRequestedType.Add(GetType(TRequestedType), GetType(TConcreteType))
			End SyncLock
		End Sub
	End Class
End Namespace