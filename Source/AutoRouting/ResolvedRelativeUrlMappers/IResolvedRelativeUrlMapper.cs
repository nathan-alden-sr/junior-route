using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	public interface IResolvedRelativeUrlMapper
	{
		ResolvedRelativeUrlResult ResolvedRelativeUrl(Type type, MethodInfo method);
	}
}