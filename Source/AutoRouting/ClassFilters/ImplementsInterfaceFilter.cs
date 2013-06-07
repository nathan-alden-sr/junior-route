using System;
using System.Threading.Tasks;

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

		public Task<bool> MatchesAsync(Type type)
		{
			type.ThrowIfNull("type");

			return (typeof(TInterface).IsInterface && type.ImplementsInterface<TInterface>()).AsCompletedTask();
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

		public Task<bool> MatchesAsync(Type type)
		{
			type.ThrowIfNull("type");

			return type.ImplementsInterface(_interfaceType).AsCompletedTask();
		}
	}
}