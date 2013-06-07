using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class HttpSessionStateBaseMapper : IParameterMapper
	{
		public Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType)
		{
			context.ThrowIfNull("context");
			parameterType.ThrowIfNull("parameterType");

			return (parameterType == typeof(HttpSessionStateBase)).AsCompletedTask();
		}

		public Task<MapResult> MapAsync(HttpContextBase context, Type type, MethodInfo method, ParameterInfo parameter)
		{
			context.ThrowIfNull("context");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			return MapResult.ValueMapped(context.Session).AsCompletedTask();
		}
	}
}