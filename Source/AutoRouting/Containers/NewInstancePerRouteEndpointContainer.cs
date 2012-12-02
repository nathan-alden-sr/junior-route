using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.Containers
{
	public class NewInstancePerRouteEndpointContainer : IContainer
	{
		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T));
		}

		public object GetInstance(Type type)
		{
			type.ThrowIfNull("type");

			Type resolvedType = ResolveType(type);

			return Activator.CreateInstance(resolvedType);
		}

		protected virtual Type ResolveType(Type type)
		{
			type.ThrowIfNull("type");

			return type;
		}
	}
}