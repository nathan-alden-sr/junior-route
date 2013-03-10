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
				Assert.That(() => _routeCollection.Add(new Route.Routing.Route("name2", _id, "route")), Throws.InstanceOf<ArgumentException>());

				Route.Routing.Route[] routes = _routeCollection.GetRoutes().ToArray();

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
				Assert.That(() => _routeCollection.Add(new Route.Routing.Route("name", Guid.NewGuid(), "route")), Throws.Nothing);

				Route.Routing.Route[] routes = _routeCollection.GetRoutes().ToArray();

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
				Assert.That(() => _routeCollection.Add(new Route.Routing.Route("name", Guid.NewGuid(), "route")), Throws.InstanceOf<ArgumentException>());

				Route.Routing.Route[] routes = _routeCollection.GetRoutes().ToArray();

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

		[TestFixture]
		public class When_getting_all_routes
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
				Route.Routing.Route[] routes = _routeCollection.GetRoutes().ToArray();

				Assert.That(routes, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_getting_route_by_id
		{
			[SetUp]
			public void SetUp()
			{
				_routeId = Guid.Parse("f2d43d2b-1075-40bd-9403-8be9f2ad585c");
				_route = new Route.Routing.Route("test1", _routeId, "test1");
				_routeCollection = new RouteCollection
					{
						_route,
						new Route.Routing.Route("test2", Guid.NewGuid(), "test2")
					};
			}

			private RouteCollection _routeCollection;
			private Guid _routeId;
			private Route.Routing.Route _route;

			[Test]
			public void Must_get_correct_route()
			{
				Assert.That(_routeCollection.GetRoute(_routeId), Is.SameAs(_route));
			}
		}

		[TestFixture]
		public class When_getting_route_by_name
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("test1", Guid.NewGuid(), "test1");
				_routeCollection = new RouteCollection
					{
						_route,
						new Route.Routing.Route("test2", Guid.NewGuid(), "test2")
					};
			}

			private RouteCollection _routeCollection;
			private Route.Routing.Route _route;

			[Test]
			public void Must_get_correct_route()
			{
				Assert.That(_routeCollection.GetRoute("test1"), Is.SameAs(_route));
			}
		}
	}
}