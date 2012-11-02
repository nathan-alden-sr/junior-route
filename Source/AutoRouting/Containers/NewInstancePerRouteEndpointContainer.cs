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

			return Activator.CreateInstance(type);
		}
	}
}