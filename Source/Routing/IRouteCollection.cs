using System;
using System.Collections.Generic;

namespace Junior.Route.Routing
{
	public interface IRouteCollection : IEnumerable<Route>
	{
		Route GetRoute(Guid id);
		Route GetRoute(string name);
		IEnumerable<Route> GetRoutes(string name);
		IEnumerable<Route> GetRoutes();
	}
}