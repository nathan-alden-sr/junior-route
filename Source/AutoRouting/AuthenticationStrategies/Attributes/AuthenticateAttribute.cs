using System;

namespace Junior.Route.AutoRouting.AuthenticationStrategies.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class AuthenticateAttribute : Attribute
	{
	}
}