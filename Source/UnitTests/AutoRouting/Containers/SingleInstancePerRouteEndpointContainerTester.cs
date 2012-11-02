using Junior.Route.AutoRouting.Containers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.Containers
{
	public static class SingleInstancePerRouteEndpointContainerTester
	{
		[TestFixture]
		public class When_getting_instance_of_same_type_multiple_times
		{
			[SetUp]
			public void SetUp()
			{
				_container = new SingleInstancePerRouteEndpointContainer();
			}

			private SingleInstancePerRouteEndpointContainer _container;

			public class Foo
			{
			}

			[Test]
			public void Must_get_same_instance()
			{
				var foo1 = _container.GetInstance<Foo>();
				var foo2 = _container.GetInstance<Foo>();

				Assert.That(foo1, Is.SameAs(foo2));

				object foo3 = _container.GetInstance(typeof(Foo));
				object foo4 = _container.GetInstance(typeof(Foo));

				Assert.That(foo3, Is.SameAs(foo4));
			}
		}
	}
}