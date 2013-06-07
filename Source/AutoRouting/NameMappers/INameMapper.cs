using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Junior.Route.AutoRouting.NameMappers
{
	public interface INameMapper
	{
		Task<NameResult> MapAsync(Type type, MethodInfo method);
	}
}