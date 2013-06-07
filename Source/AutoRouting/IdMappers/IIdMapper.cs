using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Junior.Route.AutoRouting.IdMappers
{
	public interface IIdMapper
	{
		Task<IdResult> MapAsync(Type type, MethodInfo method);
	}
}