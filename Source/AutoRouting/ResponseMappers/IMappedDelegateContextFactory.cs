using System;
using System.Reflection;
using System.Web;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public interface IMappedDelegateContextFactory
	{
		IDisposable CreateContext(HttpContextBase context, Type type, MethodInfo method);
	}
}