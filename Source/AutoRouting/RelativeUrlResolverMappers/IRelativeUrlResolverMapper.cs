using System;
using System.Reflection;

using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RelativeUrlResolverMappers
{
	public interface IRelativeUrlResolverMapper
	{
		void MapAsync(Type type, MethodInfo method, Routing.Route route, IContainer container);
	}
}