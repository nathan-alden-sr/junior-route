using System;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers.Attributes;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	public class ResolvedRelativeUrlAttributeMapper : IResolvedRelativeUrlMapper
	{
		public ResolvedRelativeUrlResult ResolvedRelativeUrl(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			ResolvedRelativeUrlAttribute attribute = method.GetCustomAttributes(typeof(ResolvedRelativeUrlAttribute), false).Cast<ResolvedRelativeUrlAttribute>().SingleOrDefault();

			return attribute != null
				       ? ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped(attribute.ResolvedRelativeUrl)
				       : ResolvedRelativeUrlResult.ResolvedRelativeUrlNotMapped();
		}
	}
}