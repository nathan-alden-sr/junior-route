using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Junior.Route.AutoRouting.SchemeMappers
{
	public interface ISchemeMapper
	{
		Task<SchemeResult> MapAsync(Type type, MethodInfo method);
	}
}