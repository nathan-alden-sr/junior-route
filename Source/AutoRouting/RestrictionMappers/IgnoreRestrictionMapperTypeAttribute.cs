using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class IgnoreRestrictionMapperTypeAttribute : Attribute
	{
		private readonly Type[] _ignoredTypes;

		public IgnoreRestrictionMapperTypeAttribute(params Type[] ignoredTypes)
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