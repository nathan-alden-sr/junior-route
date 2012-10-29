using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public class ImplementsInterfaceFilter<TInterface> : IClassFilter
		where TInterface : class
	{
		public ImplementsInterfaceFilter()
		{
			if (!typeof(TInterface).IsInterface)
			{
				throw new InvalidGenericTypeArgumentException("Type must be an interface type.", "TInterface");
			}
		}

		public bool Matches(Type type)
		{
			type.ThrowIfNull("type");

			return typeof(TInterface).IsInterface && type.ImplementsInterface<TInterface>();
		}
	}

	public class ImplementsInterfaceFilter : IClassFilter
	{
		private readonly Type _interfaceType;

		public ImplementsInterfaceFilter(Type interfaceType)
		{
			interfaceType.ThrowIfNull("interfaceType");

			if (!interfaceType.IsInterface)
			{
				throw new ArgumentException("Type must be an interface type.", "interfaceType");
			}

			_interfaceType = interfaceType;
		}

		public bool Matches(Type type)
		{
			type.ThrowIfNull("type");

			return type.ImplementsInterface(_interfaceType);
		}
	}
}