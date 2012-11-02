using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.IdMappers
{
	public interface IIdMapper
	{
		IdResult Map(Type type, MethodInfo method);
	}
}