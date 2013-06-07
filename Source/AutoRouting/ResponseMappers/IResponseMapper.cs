using System;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public interface IResponseMapper
	{
		Task MapAsync(Func<IContainer> container, Type type, MethodInfo method, Routing.Route route);
	}
}