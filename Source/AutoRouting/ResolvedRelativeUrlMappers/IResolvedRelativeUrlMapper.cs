using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	public interface IResolvedRelativeUrlMapper
	{
		ResolvedRelativeUrlResult Map(Type type, MethodInfo method);
	}
}