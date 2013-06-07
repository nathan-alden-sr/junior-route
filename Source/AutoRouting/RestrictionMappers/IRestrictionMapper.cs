using System;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public interface IRestrictionMapper
	{
		Task MapAsync(Type type, MethodInfo method, Routing.Route route, IContainer container);
	}
}