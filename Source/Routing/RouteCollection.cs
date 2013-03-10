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
		private readonly Dictionary<Guid, Route> _routesById = new Dictionary<Guid, Route>();
		private readonly Dictionary<string, List<Route>> _routesByName = new Dictionary<string, List<Route>>();

		public RouteCollection(bool allowDuplicateRouteNames = false)
		{
			_allowDuplicateRouteNames = allowDuplicateRouteNames;
		}

		public IEnumerator<Route> GetEnumerator()
		{
			return _routesById.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Route GetRoute(Guid id)
		{
			return _routesById[id];
		}

		public Route GetRoute(string name)
		{
			name.ThrowIfNull("name");

			return _routesByName[name].Single();
		}

		public IEnumerable<Route> GetRoutes(string name)
		{
			name.ThrowIfNull("name");

			return _routesByName[name];
		}

		public IEnumerable<Route> GetRoutes()
		{
			return _routesById.Values;
		}

		public RouteCollection Add(IEnumerable<Route> routes)
		{
			routes.ThrowIfNull("routes");

			foreach (Route route in routes)
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

				List<Route> routeList;

				if (!_routesByName.TryGetValue(route.Name, out routeList))
				{
					routeList = new List<Route>();
					_routesByName.Add(route.Name, routeList);
				}
				routeList.Add(route);
			}

			return this;
		}

		public RouteCollection Add(params Route[] routes)
		{
			return Add((IEnumerable<Route>)routes);
		}
	}
}