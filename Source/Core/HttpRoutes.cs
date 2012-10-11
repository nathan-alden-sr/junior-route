using System.Collections;
using System.Collections.Generic;

namespace NathanAlden.JuniorRouting.Core
{
	public class HttpRoutes : IEnumerable<HttpRoute>
	{
		private readonly List<HttpRoute> _routes = new List<HttpRoute>();

		public IEnumerator<HttpRoute> GetEnumerator()
		{
			return _routes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public HttpRoutes Add(HttpRoute route)
		{
			_routes.Add(route);

			return this;
		}
	}
}