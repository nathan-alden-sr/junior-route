using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.AutoRouting.SchemeMappers.Attributes;

namespace Junior.Route.AutoRouting.SchemeMappers
{
	public class SchemeAttributeMapper : ISchemeMapper
	{
		public Task<SchemeResult> MapAsync(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			SchemeAttribute attribute = method.GetCustomAttributes(typeof(SchemeAttribute), false).Cast<SchemeAttribute>().SingleOrDefault();

			return (attribute != null ? SchemeResult.SchemeMapped(attribute.Scheme) : SchemeResult.SchemeNotMapped()).AsCompletedTask();
		}
	}
}