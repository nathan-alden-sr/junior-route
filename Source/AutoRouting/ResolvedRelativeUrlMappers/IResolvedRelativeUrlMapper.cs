using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	public interface IResolvedRelativeUrlMapper
	{
		Task<ResolvedRelativeUrlResult> MapAsync(Type type, MethodInfo method);
	}
}