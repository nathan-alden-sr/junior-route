using System;

namespace Junior.Route.AutoRouting.Containers
{
	public interface IContainer
	{
		T GetInstance<T>();
		object GetInstance(Type type);
	}
}