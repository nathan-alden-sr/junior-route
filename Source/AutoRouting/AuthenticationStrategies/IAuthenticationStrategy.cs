using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Junior.Route.AutoRouting.AuthenticationStrategies
{
	public interface IAuthenticationStrategy
	{
		Task<bool> MustAuthenticateAsync(Type type, MethodInfo method);
	}
}