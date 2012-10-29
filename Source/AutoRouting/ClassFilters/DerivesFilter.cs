using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public class DerivesFilter<TBase> : IClassFilter
		where TBase : class
	{
		public bool Matches(Type type)
		{
			type.ThrowIfNull("type");

			return type.IsSubclassOf(typeof(TBase));
		}
	}

	public class DerivesFilter : IClassFilter
	{
		private readonly Type _baseType;

		public DerivesFilter(Type baseType)
		{
			baseType.ThrowIfNull("baseType");

			if (!baseType.IsClass)
			{
				throw new ArgumentException("Type must be a class type.", "baseType");
			}

			_baseType = baseType;
		}

		public bool Matches(Type type)
		{
			type.ThrowIfNull("type");

			return type.IsSubclassOf(_baseType);
		}
	}
}