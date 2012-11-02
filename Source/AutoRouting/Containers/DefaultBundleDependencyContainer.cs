using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing;

namespace Junior.Route.AutoRouting.Containers
{
	public class DefaultBundleDependencyContainer : IContainer
	{
		private readonly Dictionary<Type, object> _container;

		public DefaultBundleDependencyContainer(IHttpRuntime httpRuntime, IFileSystem fileSystem)
		{
			httpRuntime.ThrowIfNull("httpRuntime");
			fileSystem.ThrowIfNull("fileSystem");

			_container = new Dictionary<Type, object>
				{
					{ typeof(IHttpRuntime), httpRuntime },
					{ typeof(IFileSystem), fileSystem },
					{ typeof(IGuidFactory), new GuidFactory() },
					{ typeof(ISystemClock), new SystemClock() }
				};
		}

		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T));
		}

		public object GetInstance(Type type)
		{
			object value;

			if (!_container.TryGetValue(type, out value))
			{
				throw new InvalidOperationException(String.Format("Type {0} is not registered in the container.", type.FullName));
			}

			return value;
		}
	}
}