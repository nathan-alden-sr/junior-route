using System;
using System.Collections.Generic;

namespace Junior.Route.Routing
{
	public interface IRouteCollection : IEnumerable<Routing.Route>
	{
		Routing.Route GetRoute(Guid id);
		Routing.Route GetRoute(string name);
		IEnumerable<Routing.Route> GetRoutes(string name);
		IEnumerable<Routing.Route> GetRoutes();
	}
}