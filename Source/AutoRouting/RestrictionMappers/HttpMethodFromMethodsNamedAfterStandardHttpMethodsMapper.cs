using System;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.Routing;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public class HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper : IRestrictionMapper
	{
		public void Map(Type type, MethodInfo method, Routing.Route route, IContainer container)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			HttpMethod httpMethod;
			string methodName = method.Name.TrimEnd("Async");

			if (Enum<HttpMethod>.TryParse(methodName, true, out httpMethod))
			{
				route.RestrictByMethods(httpMethod);
			}
		}
	}
}