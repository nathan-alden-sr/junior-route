using System;
using System.Reflection;
using System.Web;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public interface IParameterMapper
	{
		bool CanMapType(HttpRequestBase request, Type parameterType);
		MapResult Map(HttpRequestBase request, Type type, MethodInfo method, ParameterInfo parameter);
	}
}