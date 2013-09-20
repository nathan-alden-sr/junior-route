using System;
using System.Linq;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
using Junior.Route.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers.Attributes
{
	public static class UrlRelativePathAttributeTester
	{
		[TestFixture]
		public class When_mapping_route_restrictions_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new UrlRelativePathAttribute("relative", RequestValueComparer.CaseSensitiveRegex);
				_route = new Route.Routing.Route((string)"name", Guid.NewGuid(), (Scheme)Scheme.NotSpecified, (string)"relative");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance<IHttpRuntime>()).Return(_httpRuntime);
			}

			private UrlRelativePathAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;
			private IHttpRuntime _httpRuntime;

			[Test]
			public void Must_add_restriction()
			{
				_attribute.Map(_route, _container);

				UrlRelativePathRestriction[] restrictions = _route.GetRestrictions<UrlRelativePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].RelativePath, Is.EqualTo("relative"));
				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseSensitiveRegexComparer.Instance));
			}
		}

		[TestFixture]
		public class When_mapping_route_restrictions_without_using_comparer
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new UrlRelativePathAttribute("relative1", "relative2");
				_route = new Route.Routing.Route((string)"name", Guid.NewGuid(), (Scheme)Scheme.NotSpecified, (string)"relative");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance<IHttpRuntime>()).Return(_httpRuntime);
			}

			private UrlRelativePathAttribute _attribute;
			private Route.Routing.Route _route;
			private IContainer _container;
			private IHttpRuntime _httpRuntime;

			[Test]
			public void Must_add_restrictions()
			{
				_attribute.Map(_route, _container);

				UrlRelativePathRestriction[] restrictions = _route.GetRestrictions<UrlRelativePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));

				Assert.That(restrictions[0].RelativePath, Is.EqualTo("relative1"));
				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));

				Assert.That(restrictions[1].RelativePath, Is.EqualTo("relative2"));
				Assert.That(restrictions[1].Comparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
			}
		}
	}
}