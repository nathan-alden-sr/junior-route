using System;
using System.Reflection;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class DefaultValueMapper : IParameterMapper
	{
		public bool CanMapType(HttpRequestBase request, Type parameterType)
		{
			request.ThrowIfNull("request");
			parameterType.ThrowIfNull("parameterType");

			return true;
		}

		public MapResult Map(HttpRequestBase request, Type type, MethodInfo method, ParameterInfo parameter)
		{
			request.ThrowIfNull("request");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			return MapResult.ValueMapped(parameter.ParameterType.GetDefaultValue());
		}
	}
}