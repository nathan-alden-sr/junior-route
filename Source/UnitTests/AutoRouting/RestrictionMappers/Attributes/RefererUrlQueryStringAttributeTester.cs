using System;
using System.Linq;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
using Junior.Route.Common;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers.Attributes
{
	public static class RefererUrlQueryStringAttributeTester
	{
		[TestFixture]
		public class When_mapping_route_restrictions_using_comparer
		{
			[Test]
			[TestCase(true)]
			[TestCase(false)]
			public void Must_add_restriction(bool optional)
			{
				var route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "relative");
				var container = MockRepository.GenerateMock<IContainer>();
				var attribute = new RefererUrlQueryStringAttribute("field", RequestValueComparer.CaseSensitiveRegex, "value", RequestValueComparer.CaseInsensitiveRegex, optional);

				attribute.Map(route, container);

				RefererUrlQueryStringRestriction[] restrictions = route.GetRestrictions<RefererUrlQueryStringRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Field, Is.EqualTo("field"));
				Assert.That(restrictions[0].FieldComparer, Is.SameAs(CaseSensitiveRegexComparer.Instance));
				Assert.That(restrictions[0].Value, Is.EqualTo("value"));
				Assert.That(restrictions[0].ValueComparer, Is.SameAs(CaseInsensitiveRegexComparer.Instance));
				Assert.That(restrictions[0].Optional, Is.EqualTo(optional));
			}
		}

		[TestFixture]
		public class When_mapping_route_restrictions_without_using_comparer
		{
			[Test]
			[TestCase(true)]
			[TestCase(false)]
			public void Must_add_restriction(bool optional)
			{
				var route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "relative");
				var container = MockRepository.GenerateMock<IContainer>();
				var attribute = new RefererUrlQueryStringAttribute("field", "value", optional);

				attribute.Map(route, container);

				RefererUrlQueryStringRestriction[] restrictions = route.GetRestrictions<RefererUrlQueryStringRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Field, Is.EqualTo("field"));
				Assert.That(restrictions[0].FieldComparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
				Assert.That(restrictions[0].Value, Is.EqualTo("value"));
				Assert.That(restrictions[0].ValueComparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
				Assert.That(restrictions[0].Optional, Is.EqualTo(optional));
			}
		}
	}
}