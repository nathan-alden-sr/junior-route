using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;

namespace Junior.Route.Routing
{
	public class RouteCollection : IRouteCollection
	{
		private readonly bool _allowDuplicateRouteNames;
		private readonly Dictionary<Guid, Routing.Route> _routesById = new Dictionary<Guid, Routing.Route>();
		private readonly Dictionary<string, List<Routing.Route>> _routesByName = new Dictionary<string, List<Routing.Route>>();

		public RouteCollection(bool allowDuplicateRouteNames = false)
		{
			_allowDuplicateRouteNames = allowDuplicateRouteNames;
		}

		public IEnumerator<Routing.Route> GetEnumerator()
		{
			return _routesById.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Routing.Route GetRoute(Guid id)
		{
			Routing.Route route;

			return _routesById.TryGetValue(id, out route) ? route : null;
		}

		public Routing.Route GetRoute(string name)
		{
			name.ThrowIfNull("name");

			List<Routing.Route> routes;

			if (!_routesByName.TryGetValue(name, out routes))
			{
				return null;
			}
			if (routes.Count > 1)
			{
				throw new ApplicationException(String.Format("More than one route exists with name '{0}'.", name));
			}

			return routes[0];
		}

		public IEnumerable<Routing.Route> GetRoutes(string name)
		{
			name.ThrowIfNull("name");

			List<Routing.Route> routes;

			return _routesByName.TryGetValue(name, out routes) ? routes : Enumerable.Empty<Routing.Route>();
		}

		public IEnumerable<Routing.Route> GetRoutes()
		{
			return _routesById.Values;
		}

		public RouteCollection Add(IEnumerable<Routing.Route> routes)
		{
			routes.ThrowIfNull("routes");

			foreach (Routing.Route route in routes)
			{
				if (_routesById.ContainsKey(route.Id))
				{
					throw new ArgumentException(String.Format("A route with ID {0} has already been added.", route.Id), "routes");
				}
				if (!_allowDuplicateRouteNames && _routesByName.ContainsKey(route.Name))
				{
					throw new ArgumentException(String.Format("A route named '{0}' has already been added.", route.Name), "routes");
				}

				_routesById.Add(route.Id, route);

				List<Routing.Route> routeList;

				if (!_routesByName.TryGetValue(route.Name, out routeList))
				{
					routeList = new List<Routing.Route>();
					_routesByName.Add(route.Name, routeList);
				}
				routeList.Add(route);
			}

			return this;
		}

		public RouteCollection Add(params Routing.Route[] routes)
		{
			return Add((IEnumerable<Routing.Route>)routes);
		}
	}
}