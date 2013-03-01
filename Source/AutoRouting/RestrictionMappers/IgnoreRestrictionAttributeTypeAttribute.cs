using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class IgnoreRestrictionAttributeTypeAttribute : Attribute
	{
		private readonly Type[] _ignoredTypes;

		public IgnoreRestrictionAttributeTypeAttribute(params Type[] ignoredTypes)
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