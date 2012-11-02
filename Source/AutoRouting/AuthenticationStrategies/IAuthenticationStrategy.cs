using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.AuthenticationStrategies
{
	public interface IAuthenticationStrategy
	{
		bool MustAuthenticate(Type type, MethodInfo method);
	}
}