using System;
using System.Reflection;

using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public interface IRestrictionMapper
	{
		void Map(Type type, MethodInfo method, Routing.Route route, IContainer container);
	}
}