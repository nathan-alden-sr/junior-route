using System;
using System.Reflection;

using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.CustomMapperMappers
{
	public interface ICustomMapperMapper
	{
		void Map(Type type, MethodInfo method, Routing.Route route, IContainer container);
	}
}