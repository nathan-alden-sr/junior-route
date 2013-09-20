using System;
using System.Linq;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
using Junior.Route.Common;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers.Attributes
{
	public static class UrlPortAttributeTester
	{
		[TestFixture]
		public class When_mapping_route_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new UrlPortAttribute(8080, 16000);
				_route = new Route.Routing.Route((string)"name", Guid.NewGuid(), (Scheme)Scheme.NotSpecified, (string)"relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private UrlPortAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restrictions()
			{
				_attribute.Map(_route, _container);

				UrlPortRestriction[] restrictions = _route.GetRestrictions<UrlPortRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));

				Assert.That(restrictions[0].Port, Is.EqualTo(8080));

				Assert.That(restrictions[1].Port, Is.EqualTo(16000));
			}
		}
	}
}