using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class IgnoreResolvedRelativeUrlMapperTypeAttribute : Attribute
	{
		private readonly Type[] _ignoredTypes;

		public IgnoreResolvedRelativeUrlMapperTypeAttribute(params Type[] ignoredTypes)
		{
			ignoredTypes.ThrowIfNull("ignoredTypes");

			_ignoredTypes = ignoredTypes;
		}

		public Type[] IgnoredTypes
		{
			get
			{
				return _ignoredTypes;
			}
		}
	}
}