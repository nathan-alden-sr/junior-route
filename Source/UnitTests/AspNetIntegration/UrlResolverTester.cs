using System;

using Junior.Route.AspNetIntegration;
using Junior.Route.Common;
using Junior.Route.Routing;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration
{
	public static class UrlResolverTester
	{
		[TestFixture]
		public class When_resolving_absolute_path_with_arguments
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = MockRepository.GenerateMock<IRouteCollection>();
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IRouteCollection _routeCollection;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Absolute("relative{0}", 1), Is.EqualTo("/path/relative1"));
			}
		}

		[TestFixture]
		public class When_resolving_absolute_path_without_arguments
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = MockRepository.GenerateMock<IRouteCollection>();
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IRouteCollection _routeCollection;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Absolute("relative"), Is.EqualTo("/path/relative"));
			}
		}

		[TestFixture]
		public class When_resolving_relative_url_and_appdomainappvirtualpath_is_empty_or_slash
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = MockRepository.GenerateMock<IRouteCollection>();
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private IRouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IUrlResolverConfiguration _configuration;

			[Test]
			[TestCase("")]
			[TestCase("/")]
			public void Must_prefix_url_with_only_one_slash(string appDomainAppVirtualPath)
			{
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return(appDomainAppVirtualPath);

				Assert.That(_urlResolver.Absolute("relative"), Is.EqualTo("/relative"));
			}
		}

		[TestFixture]
		public class When_resolving_route_by_id_and_id_not_found
		{
			[SetUp]
			public void SetUp()
			{
				_id = Guid.Parse("265e2da0-458d-40c1-850c-b8ceb1d798a4");
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", _id, Scheme.NotSpecified, "relative")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private Guid _id;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_throw_exception()
			{
				Assert.That(() => _urlResolver.Route(Guid.Parse("b8ce5c99-b2a2-46f3-9f48-ffdd70414c94")), Throws.InstanceOf<ArgumentException>());
			}
		}

		[TestFixture]
		public class When_resolving_route_by_id_with_arguments
		{
			[SetUp]
			public void SetUp()
			{
				_routeId = Guid.NewGuid();
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", _routeId, Scheme.NotSpecified, "relative{0}")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private Guid _routeId;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Route(_routeId, 1), Is.EqualTo("/path/relative1"));
			}
		}

		[TestFixture]
		public class When_resolving_route_by_id_without_arguments
		{
			[SetUp]
			public void SetUp()
			{
				_routeId = Guid.NewGuid();
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", _routeId, Scheme.NotSpecified, "relative")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private Guid _routeId;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Route(_routeId), Is.EqualTo("/path/relative"));
			}
		}

		[TestFixture]
		public class When_resolving_route_by_name_and_duplicate_names_exist
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection(true)
				{
					new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "relative1"),
					new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "relative2")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_throw_exception()
			{
				Assert.That(() => _urlResolver.Route("name"), Throws.InstanceOf<ArgumentException>());
			}
		}

		[TestFixture]
		public class When_resolving_route_by_name_and_name_not_found
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "relative")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_throw_exception()
			{
				Assert.That(() => _urlResolver.Route("name1"), Throws.InstanceOf<ArgumentException>());
			}
		}

		[TestFixture]
		public class When_resolving_route_by_name_with_arguments
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "relative{0}")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Route("name", 1), Is.EqualTo("/path/relative1"));
			}
		}

		[TestFixture]
		public class When_resolving_route_by_name_without_arguments
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "relative")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IUrlResolverConfiguration _configuration;

			[Test]
			public void Must_use_http_runtime_to_resolve_from_relative_path()
			{
				Assert.That(_urlResolver.Route("name"), Is.EqualTo("/path/relative"));
			}
		}

		[TestFixture]
		public class When_resolving_route_with_scheme
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", Guid.NewGuid(), Scheme.Http, "relative")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_configuration.Stub(arg => arg.HostName).Return("example.com");
				_configuration.Stub(arg => arg.GetPort(Scheme.Http)).Return(8080);
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
				_url = _urlResolver.Route("name");
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IUrlResolverConfiguration _configuration;
			private string _url;

			[Test]
			public void Must_include_host_name_and_port_in_url()
			{
				Assert.That(_url, Is.EqualTo("http://example.com:8080/path/relative"));
			}

			[Test]
			public void Must_use_supplied_urlresolverconfiguration()
			{
				_configuration.AssertWasCalled(arg => arg.HostName);
				_configuration.AssertWasCalled(arg => arg.GetPort(Scheme.Http));
			}
		}

		[TestFixture]
		public class When_resolving_route_with_scheme_overridden_by_different_scheme
		{
			[SetUp]
			public void SetUp()
			{
				_routeCollection = new RouteCollection
				{
					new Route.Routing.Route("name", Guid.NewGuid(), Scheme.Http, "relative")
				};
				_configuration = MockRepository.GenerateMock<IUrlResolverConfiguration>();
				_configuration.Stub(arg => arg.HostName).Return("example.com");
				_configuration.Stub(arg => arg.GetPort(Scheme.Https)).Return(443);
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppVirtualPath).Return("/path");
				_urlResolver = new UrlResolver(_routeCollection, _configuration, _httpRuntime);
				_url = _urlResolver.Route(Scheme.Https, "name");
			}

			private RouteCollection _routeCollection;
			private IHttpRuntime _httpRuntime;
			private UrlResolver _urlResolver;
			private IUrlResolverConfiguration _configuration;
			private string _url;

			[Test]
			public void Must_use_overriding_scheme()
			{
				Assert.That(_url, Is.EqualTo("https://example.com/path/relative"));
			}
		}
	}
}