using System;
using System.Reflection;

using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public interface IResponseMapper
	{
		void Map(Func<IContainer> container, Type type, MethodInfo method, Routing.Route route);
	}
}