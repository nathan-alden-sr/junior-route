using System;
using System.Reflection;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public interface IRouteRestrictionMapper
	{
		void Map(Type type, MethodInfo method, Routing.Route route);
	}
}