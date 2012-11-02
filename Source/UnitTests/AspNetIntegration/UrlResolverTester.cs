using System;

using Junior.Route.AspNetIntegration;
using Junior.Route.Routing;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration
{
	public static class UrlResolverTester
	{
		[TestFixture]
		public class When_resolving_absolute_path
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = MockRepository.GenerateMock<IRouteCollection>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _httpRuntime);
			}

			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IRouteCollection _routeCollection;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Absolute("relative"), Is.EqualTo("/path/relative"));
			}
		}

		[TestFixture]
		public class When_resolving_route_by_id
		{
			[SetUp]
			public void SetUp()
			{
				_routeId = Guid.NewGuid();
				_routeCollection = new RouteCollection
					{
						new Route.Routing.Route("name", _routeId, "relative")
					};
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private Guid _routeId;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Route(_routeId), Is.EqualTo("/path/relative"));
			}
		}

		[TestFixture]
		public class When_resolving_route_by_name
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection
					{
						new Route.Routing.Route("name", Guid.NewGuid(), "relative")
					};
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Route("name"), Is.EqualTo("/path/relative"));
			}
		}
	}
}