using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.IdMappers
{
	public interface IIdMapper
	{
		IdResult Id(Type type, MethodInfo method);
	}
}