using System;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.NameMappers.Attributes;

namespace Junior.Route.AutoRouting.NameMappers
{
	public class NameAttributeMapper : INameMapper
	{
		public NameResult Map(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			NameAttribute attribute = method.GetCustomAttributes(typeof(NameAttribute), false).Cast<NameAttribute>().SingleOrDefault();

			return attribute != null ? NameResult.NameMapped(attribute.Name) : NameResult.NameNotMapped();
		}
	}
}