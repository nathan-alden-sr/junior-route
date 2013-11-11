using System;

using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RelativeUrlResolverMappers.Attributes
{
	public abstract class RelativeUrlResolverAttribute : Attribute
	{
		public abstract void Map(Routing.Route route, IContainer container);
	}
}