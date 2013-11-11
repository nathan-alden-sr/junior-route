using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RelativeUrlResolverMappers.Attributes;

namespace Junior.Route.AutoRouting.RelativeUrlResolverMappers
{
	public class RelativeUrlResolversFromAttributesMapper : IRelativeUrlResolverMapper
	{
		public void MapAsync(Type type, MethodInfo method, Routing.Route route, IContainer container)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			IEnumerable<RelativeUrlResolverAttribute> attributes = method.GetCustomAttributes(false).OfType<RelativeUrlResolverAttribute>();

			foreach (RelativeUrlResolverAttribute attribute in attributes)
			{
				attribute.Map(route, container);
			}
		}
	}
}