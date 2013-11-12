using System;
using System.Collections.Generic;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.CustomMapperMappers.Attributes;

namespace Junior.Route.AutoRouting.CustomMapperMappers
{
	public class CustomMapperFromAttributeMapper : ICustomMapperMapper
	{
		public void Map(Type type, MethodInfo method, Routing.Route route, IContainer container)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			IEnumerable<CustomMapperAttribute> attributes = method.GetCustomAttributes<CustomMapperAttribute>(false);

			foreach (CustomMapperAttribute attribute in attributes)
			{
				attribute.Map(route, container);
			}
		}
	}
}