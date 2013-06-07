using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.AutoRouting.AuthenticationStrategies.Attributes;

namespace Junior.Route.AutoRouting.AuthenticationStrategies
{
	public class AuthenticateAttributeStrategy : IAuthenticationStrategy
	{
		public Task<bool> MustAuthenticateAsync(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			return method.GetCustomAttributes(typeof(AuthenticateAttribute), false).Any().AsCompletedTask();
		}
	}
}