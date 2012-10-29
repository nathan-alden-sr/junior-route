using System;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.NamingStrategies.Attributes;

namespace Junior.Route.AutoRouting.NamingStrategies
{
	public class NameAttributeStrategy : INamingStrategy
	{
		public NamingResult Name(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			NameAttribute attribute = method.GetCustomAttributes(typeof(NameAttribute), false).Cast<NameAttribute>().SingleOrDefault();

			return attribute != null ? NamingResult.RouteNamed(attribute.Name) : NamingResult.RouteNotNamed();
		}
	}
}