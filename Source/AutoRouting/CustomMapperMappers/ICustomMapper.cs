using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.CustomMapperMappers
{
	public interface ICustomMapper
	{
		void Map(Routing.Route route, IContainer container);
	}
}