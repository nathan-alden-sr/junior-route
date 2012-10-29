using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.Routing
{
	public class RouteCollection : IRouteCollection
	{
		private readonly bool _allowDuplicateRouteNames;
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

			if (_allowDuplicateRouteNames)
			{
				_routes.AddRange(routes);
			}
			else
			{
				foreach (Route route in routes)
				{
					if (_routes.Any(arg => arg.Name == route.Name))
					{
						throw new ArgumentException(String.Format("A route named '{0}' has already been added.", route.Name));
					}

					_routes.Add(route);
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