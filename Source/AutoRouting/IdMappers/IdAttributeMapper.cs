using System;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.IdMappers.Attributes;

namespace Junior.Route.AutoRouting.IdMappers
{
	public class IdAttributeMapper : IIdMapper
	{
		public IdResult Map(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			IdAttribute attribute = method.GetCustomAttributes(typeof(IdAttribute), false).Cast<IdAttribute>().SingleOrDefault();

			return attribute != null ? IdResult.IdMapped(attribute.Id) : IdResult.IdNotMapped();
		}
	}
}