using System;
using System.Linq;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers.Attributes
{
	public static class RefererUrlHostTypeAttributeTester
	{
		[TestFixture]
		public class When_mapping_route_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new RefererUrlHostTypeAttribute(UriHostNameType.Basic, UriHostNameType.Dns);
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private RefererUrlHostTypeAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restrictions()
			{
				_attribute.Map(_route, _container);

				RefererUrlHostTypeRestriction[] restrictions = _route.GetRestrictions<RefererUrlHostTypeRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));

				Assert.That(restrictions[0].Type, Is.EqualTo(UriHostNameType.Basic));

				Assert.That(restrictions[1].Type, Is.EqualTo(UriHostNameType.Dns));
			}
		}
	}
}