using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.AutoRouting.NameMappers.Attributes;

namespace Junior.Route.AutoRouting.NameMappers
{
	public class NameAttributeMapper : INameMapper
	{
		public Task<NameResult> MapAsync(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			NameAttribute attribute = method.GetCustomAttributes(typeof(NameAttribute), false).Cast<NameAttribute>().SingleOrDefault();

			return (attribute != null ? NameResult.NameMapped(attribute.Name) : NameResult.NameNotMapped()).AsCompletedTask();
		}
	}
}