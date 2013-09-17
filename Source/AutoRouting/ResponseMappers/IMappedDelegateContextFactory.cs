using System;
using System.Reflection;
using System.Web;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public interface IMappedDelegateContextFactory
	{
		IMappedDelegateContext CreateContext(HttpContextBase context, Type type, MethodInfo method);
	}
}