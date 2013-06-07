using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public interface IParameterMapper
	{
		Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType);
		Task<MapResult> MapAsync(HttpContextBase context, Type type, MethodInfo method, ParameterInfo parameter);
	}
}