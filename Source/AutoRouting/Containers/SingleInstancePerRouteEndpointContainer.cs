using System;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.AutoRouting.Containers
{
	public class SingleInstancePerRouteEndpointContainer : IContainer
	{
		private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T));
		}

		public object GetInstance(Type type)
		{
			type.ThrowIfNull("type");

			object instance;

			if (!_instances.TryGetValue(type, out instance))
			{
				Type resolvedType = ResolveType(type);

				instance = Activator.CreateInstance(resolvedType);
				_instances.Add(type, instance);
			}

			return instance;
		}

		protected virtual Type ResolveType(Type type)
		{
			type.ThrowIfNull("type");

			return type;
		}
	}
}