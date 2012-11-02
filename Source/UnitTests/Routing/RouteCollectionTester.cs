using System;
using System.Linq;

using Junior.Route.Routing;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing
{
	public static class RouteCollectionTester
	{
		[TestFixture]
		public class When_adding_duplicate_route_ids
		{
			[SetUp]
			public void SetUp()
			{
				_id = Guid.NewGuid();
				_routeCollection = new RouteCollection { new Route.Routing.Route("name1", _id, "route") };
			}

			private RouteCollection _routeCollection;
			private Guid _id;

			[Test]
			public void Must_throw_exception_and_not_add_duplicates()
			{
				Assert.Throws<ArgumentException>(() => _routeCollection.Add(new Route.Routing.Route("name2", _id, "route")));

				Route.Routing.Route[] routes = _routeCollection.Routes.ToArray();

				Assert.That(routes, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_adding_duplicate_route_names_with_duplicates_allowed
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection(true) { new Route.Routing.Route("name", Guid.NewGuid(), "route") };
			}

			private RouteCollection _routeCollection;

			[Test]
			public void Must_add_duplicates()
			{
				Assert.DoesNotThrow(() => _routeCollection.Add(new Route.Routing.Route("name", Guid.NewGuid(), "route")));

				Route.Routing.Route[] routes = _routeCollection.Routes.ToArray();

				Assert.That(routes, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_adding_duplicate_route_names_with_duplicates_disallowed
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection { new Route.Routing.Route("name", Guid.NewGuid(), "route") };
			}

			private RouteCollection _routeCollection;

			[Test]
			public void Must_throw_exception_and_not_add_duplicates()
			{
				Assert.Throws<ArgumentException>(() => _routeCollection.Add(new Route.Routing.Route("name", Guid.NewGuid(), "route")));

				Route.Routing.Route[] routes = _routeCollection.Routes.ToArray();

				Assert.That(routes, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_enumerating_routes
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection
					{
						new Route.Routing.Route("name1", Guid.NewGuid(), "route1"),
						new Route.Routing.Route("name2", Guid.NewGuid(), "route2")
					};
			}

			private RouteCollection _routeCollection;

			[Test]
			public void Must_return_all_routes()
			{
				Route.Routing.Route[] routes = _routeCollection.ToArray();

				Assert.That(routes, Has.Length.EqualTo(2));
			}
		}
	}
}