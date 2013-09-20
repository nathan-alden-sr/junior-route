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
	public static class UrlQueryAttributeTester
	{
		[TestFixture]
		public class When_mapping_route_restrictions_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new UrlQueryAttribute("query", RequestValueComparer.CaseSensitiveRegex);
				_route = new Route.Routing.Route((string)"name", Guid.NewGuid(), (Scheme)Scheme.NotSpecified, (string)"relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private UrlQueryAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restriction()
			{
				_attribute.Map(_route, _container);

				UrlQueryRestriction[] restrictions = _route.GetRestrictions<UrlQueryRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Query, Is.EqualTo("query"));
				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseSensitiveRegexComparer.Instance));
			}
		}

		[TestFixture]
		public class When_mapping_route_restrictions_without_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new UrlQueryAttribute("query1", "query2");
				_route = new Route.Routing.Route((string)"name", Guid.NewGuid(), (Scheme)Scheme.NotSpecified, (string)"relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private UrlQueryAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restrictions()
			{
				_attribute.Map(_route, _container);

				UrlQueryRestriction[] restrictions = _route.GetRestrictions<UrlQueryRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));

				Assert.That(restrictions[0].Query, Is.EqualTo("query1"));
				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));

				Assert.That(restrictions[1].Query, Is.EqualTo("query2"));
				Assert.That(restrictions[1].Comparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
			}
		}
	}
}