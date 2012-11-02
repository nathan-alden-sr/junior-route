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
	public static class RefererUrlAbsolutePathAttributeTester
	{
		[TestFixture]
		public class When_mapping_route_restrictions_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new RefererUrlAbsolutePathAttribute("path", RequestValueComparer.CaseSensitiveRegex);
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private RefererUrlAbsolutePathAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restriction()
			{
				_attribute.Map(_route, _container);

				RefererUrlAbsolutePathRestriction[] restrictions = _route.GetRestrictions<RefererUrlAbsolutePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].AbsolutePath, Is.EqualTo("path"));
				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseSensitiveRegexRequestValueComparer.Instance));
			}
		}

		[TestFixture]
		public class When_mapping_route_restrictions_without_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new RefererUrlAbsolutePathAttribute("path1", "path2");
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private RefererUrlAbsolutePathAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;

			[Test]
			public void Must_add_restrictions()
			{
				_attribute.Map(_route, _container);

				RefererUrlAbsolutePathRestriction[] restrictions = _route.GetRestrictions<RefererUrlAbsolutePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));

				Assert.That(restrictions[0].AbsolutePath, Is.EqualTo("path1"));
				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseInsensitivePlainRequestValueComparer.Instance));

				Assert.That(restrictions[1].AbsolutePath, Is.EqualTo("path2"));
				Assert.That(restrictions[1].Comparer, Is.SameAs(CaseInsensitivePlainRequestValueComparer.Instance));
			}
		}
	}
}