using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public interface ICustomRestrictionMapper
	{
		void Map(Routing.Route route, IContainer container);
	}
}