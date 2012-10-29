using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.NamingStrategies
{
	public interface INamingStrategy
	{
		NamingResult Name(Type type, MethodInfo method);
	}
}