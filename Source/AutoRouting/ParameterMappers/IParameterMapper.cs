using System;
using System.Reflection;
using System.Web;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public interface IParameterMapper
	{
		bool CanMapType(HttpContextBase context, Type parameterType);
		MapResult Map(HttpContextBase context, Type type, MethodInfo method, ParameterInfo parameter);
	}
}