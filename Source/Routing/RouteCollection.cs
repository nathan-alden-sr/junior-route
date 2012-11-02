using System;
using System.Collections;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.Routing
{
	public class RouteCollection : IRouteCollection
	{
		private readonly bool _allowDuplicateRouteNames;
		private readonly HashSet<Guid> _routeIds = new HashSet<Guid>();
		private readonly HashSet<string> _routeNames = new HashSet<string>();
		private readonly HashSet<Route> _routes = new HashSet<Route>();

		public RouteCollection(bool allowDuplicateRouteNames = false)
		{
			_allowDuplicateRouteNames = allowDuplicateRouteNames;
		}

		public IEnumerable<Route> Routes
		{
			get
			{
				return _routes;
			}
		}

		public IEnumerator<Route> GetEnumerator()
		{
			return _routes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public RouteCollection Add(IEnumerable<Route> routes)
		{
			routes.ThrowIfNull("routes");

			foreach (Route route in routes)
			{
				if (_routeIds.Contains(route.Id))
				{
					throw new ArgumentException(String.Format("A route with ID {0} has already been added.", route.Id), "routes");
				}
				if (!_allowDuplicateRouteNames && _routeNames.Contains(route.Name))
				{
					throw new ArgumentException(String.Format("A route named '{0}' has already been added.", route.Name), "routes");
				}

				_routes.Add(route);
				_routeNames.Add(route.Name);
				if (!_allowDuplicateRouteNames)
				{
					_routeIds.Add(route.Id);
				}
			}

			return this;
		}

		public RouteCollection Add(params Route[] routes)
		{
			return Add((IEnumerable<Route>)routes);
		}
	}
}