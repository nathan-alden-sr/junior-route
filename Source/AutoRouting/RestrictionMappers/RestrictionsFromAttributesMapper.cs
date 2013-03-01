using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public class RestrictionsFromAttributesMapper<T> : RestrictionsFromAttributesMapper
		where T : RestrictionAttribute
	{
		public RestrictionsFromAttributesMapper()
			: base(typeof(T))
		{
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

			IEnumerable<RestrictionAttribute> restrictionAttributes = method.GetCustomAttributes(_attributeType, false).Cast<RestrictionAttribute>();

			foreach (RestrictionAttribute restrictionAttribute in restrictionAttributes)
			{
				restrictionAttribute.Map(route, container);
			}
		}
	}
}