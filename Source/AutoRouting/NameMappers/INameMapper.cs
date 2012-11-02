using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.NameMappers
{
	public interface INameMapper
	{
		NameResult Map(Type type, MethodInfo method);
	}
}