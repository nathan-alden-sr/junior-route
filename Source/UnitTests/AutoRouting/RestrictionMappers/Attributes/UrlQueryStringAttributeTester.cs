using System;
using System.Linq;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers.Attributes
{
	public static class UrlQueryStringAttributeTester
	{
		[TestFixture]
		public class When_mapping_route_restrictions_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new UrlQueryStringAttribute("field", RequestValueComparer.CaseSensitiveRegex, "value", RequestValueComparer.CaseInsensitiveRegex);
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private UrlQueryStringAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restriction()
			{
				_attribute.Map(_route, _container);

				UrlQueryStringRestriction[] restrictions = _route.GetRestrictions<UrlQueryStringRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Field, Is.EqualTo("field"));
				Assert.That(restrictions[0].FieldComparer, Is.SameAs(CaseSensitiveRegexComparer.Instance));
				Assert.That(restrictions[0].Value, Is.EqualTo("value"));
				Assert.That(restrictions[0].ValueComparer, Is.SameAs(CaseInsensitiveRegexComparer.Instance));
			}
		}

		[TestFixture]
		public class When_mapping_route_restrictions_without_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new UrlQueryStringAttribute("field", "value");
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private UrlQueryStringAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restriction()
			{
				_attribute.Map(_route, _container);

				UrlQueryStringRestriction[] restrictions = _route.GetRestrictions<UrlQueryStringRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Field, Is.EqualTo("field"));
				Assert.That(restrictions[0].FieldComparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
				Assert.That(restrictions[0].Value, Is.EqualTo("value"));
				Assert.That(restrictions[0].ValueComparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
			}
		}
	}
}