using System;
using System.Reflection;

using Junior.Common;
using Junior.Route.Routing;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public class HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper : IRouteRestrictionMapper
	{
		public void Map(Type type, MethodInfo method, Routing.Route route)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");

			HttpMethod httpMethod;

			if (Enum<HttpMethod>.TryParse(method.Name, true, out httpMethod))
			{
				route.RestrictByMethods(httpMethod);
			}
		}
	}
}