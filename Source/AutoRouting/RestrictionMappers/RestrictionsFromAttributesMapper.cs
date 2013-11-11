using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public class RestrictionsFromAttributesMapper : IRestrictionMapper
	{
		public void Map(Type type, MethodInfo method, Routing.Route route, IContainer container)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");

			IEnumerable<RestrictionAttribute> attributes = method.GetCustomAttributes(false).OfType<RestrictionAttribute>();

			foreach (RestrictionAttribute attribute in attributes)
			{
				attribute.Map(route, container);
			}
		}
	}
}