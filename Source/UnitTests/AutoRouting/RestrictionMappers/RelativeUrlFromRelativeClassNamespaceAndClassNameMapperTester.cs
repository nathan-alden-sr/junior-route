using System;
using System.Linq;
using System.Reflection;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers;
using Junior.Route.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers
{
	public static class UrlRelativePathFromRelativeClassNamespaceAndClassNameMapperTester
	{
		[TestFixture]
		public class When_mapping_relative_urls
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route((string)"name", Guid.NewGuid(), (Scheme)Scheme.NotSpecified, (string)"relative");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance<IHttpRuntime>()).Return(_httpRuntime);
			}

			private Route.Routing.Route _route;
			private IContainer _container;
			private IHttpRuntime _httpRuntime;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", true, "_", "auto_routing/restriction_mappers/endpoint")]
			[TestCase(typeof(Endpoint), "Method", false, "_", "Auto_Routing/Restriction_Mappers/Endpoint")]
			[TestCase(typeof(Endpoint), "Method", false, "-", "Auto-Routing/Restriction-Mappers/Endpoint")]
			public void Must_add_restrictions(Type type, string methodName, bool makeLowercase, string wordSeparator, string expectedUrlRelativePath)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				var mapper = new UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper("Junior.Route.UnitTests", true, makeLowercase, wordSeparator);

				mapper.MapAsync(type, methodInfo, _route, _container);

				UrlRelativePathRestriction[] restrictions = _route.GetRestrictions<UrlRelativePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseSensitivePlainComparer.Instance));
				Assert.That(restrictions[0].RelativePath, Is.EqualTo(expectedUrlRelativePath));
			}
		}
	}
}