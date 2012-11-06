using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public class RestrictionsFromAttributesMapper<T> : IRestrictionMapper
		where T : RestrictionAttribute
	{
		public void Map(Type type, MethodInfo method, Routing.Route route, IContainer container)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			IEnumerable<T> attributes = method.GetCustomAttributes(typeof(T), false).Cast<T>();

			foreach (T attribute in attributes)
			{
				attribute.Map(route, container);
			}
		}
	}

	public class RestrictionsFromAttributesMapper : IRestrictionMapper
	{
		private readonly Type _attributeType;

		public RestrictionsFromAttributesMapper(Type attributeType)
		{
			attributeType.ThrowIfNull("attributeType");

			if (!attributeType.IsSubclassOf(typeof(RestrictionAttribute)))
			{
				throw new ArgumentException(String.Format("Type must be a subclass of {0}.", typeof(RestrictionAttribute).FullName), "attributeType");
			}

			_attributeType = attributeType;
		}

		public void Map(Type type, MethodInfo method, Routing.Route route, IContainer container)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");

			IEnumerable<RestrictionAttribute> attributes = method.GetCustomAttributes(_attributeType, false).Cast<RestrictionAttribute>();

			foreach (RestrictionAttribute attribute in attributes)
			{
				attribute.Map(route, container);
			}
		}
	}
}