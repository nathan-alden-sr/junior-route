using System;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting;
using Junior.Route.AutoRouting.AuthenticationStrategies;
using Junior.Route.AutoRouting.ClassFilters;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.IdMappers;
using Junior.Route.AutoRouting.MethodFilters;
using Junior.Route.AutoRouting.NameMappers;
using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers;
using Junior.Route.AutoRouting.ResponseMappers;
using Junior.Route.AutoRouting.RestrictionMappers;
using Junior.Route.Routing.AuthenticationProviders;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting
{
	public static class AutoRouteCollectionTester
	{
		[TestFixture]
		public class When_allowing_duplicate_route_names_and_attempting_to_add_duplicate_route_names
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper
					.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = IdResult.IdMapped(Guid.NewGuid()))
					.Return(null);
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_autoRouteCollection = new AutoRouteCollection(true)
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper);
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;

			public class Endpoint
			{
				public void Method1()
				{
				}

				public void Method2()
				{
				}
			}

			[Test]
			public void Must_not_throw_exception()
			{
				Assert.DoesNotThrow(() => _autoRouteCollection.GenerateRouteCollection());
			}
		}

		[TestFixture]
		public class When_assigning_authentication_provider
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_authenticationProvider = MockRepository.GenerateMock<IAuthenticationProvider>();
				_authenticationStrategy = MockRepository.GenerateMock<IAuthenticationStrategy>();
				_authenticationStrategy.Stub(arg => arg.MustAuthenticate(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(true);
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper)
					.Authenticate(_authenticationProvider, _authenticationStrategy);
				_routes = _autoRouteCollection.GenerateRouteCollection().ToArray();
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;
			private IAuthenticationProvider _authenticationProvider;
			private IAuthenticationStrategy _authenticationStrategy;
			private Route.Routing.Route[] _routes;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_use_strategy_to_assign_provider()
			{
				var request = MockRepository.GenerateMock<HttpRequestBase>();

				_routes[0].Authenticate(request);

				_authenticationStrategy.AssertWasCalled(arg => arg.MustAuthenticate(typeof(Endpoint), typeof(Endpoint).GetMethod("Method")));
				_authenticationProvider.AssertWasCalled(arg => arg.Authenticate(request, _routes[0]));
			}
		}

		[TestFixture]
		public class When_attempting_to_add_duplicate_route_ids
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper
					.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = NameResult.NameMapped(Guid.NewGuid().ToString()))
					.Return(null);
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper);
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;

			public class Endpoint
			{
				public void Method1()
				{
				}

				public void Method2()
				{
				}
			}

			[Test]
			public void Must_throw_exception()
			{
				Assert.Throws<ArgumentException>(() => _autoRouteCollection.GenerateRouteCollection());
			}
		}

		[TestFixture]
		public class When_disallowing_duplicate_route_names_and_attempting_to_add_duplicate_route_names
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper
					.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = IdResult.IdMapped(Guid.NewGuid()))
					.Return(null);
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper);
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;

			public class Endpoint
			{
				public void Method1()
				{
				}

				public void Method2()
				{
				}
			}

			[Test]
			public void Must_throw_exception()
			{
				Assert.Throws<ArgumentException>(() => _autoRouteCollection.GenerateRouteCollection());
			}
		}

		[TestFixture]
		public class When_filtering_classes
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(IncludedEndpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_responseMapper = MockRepository.GenerateMock<IResponseMapper>();
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper)
					.ResponseMapper(_responseMapper);
				_routes = _autoRouteCollection.GenerateRouteCollection().ToArray();
			}

			public class IncludedEndpoint
			{
				public void Method()
				{
				}
			}

			public class ExcludedEndpoint
			{
				public void Method()
				{
				}
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;
			private Route.Routing.Route[] _routes;
			private IResponseMapper _responseMapper;

			[Test]
			public void Must_consider_correct_classes()
			{
				Assert.That(_routes, Has.Length.EqualTo(1));
				_responseMapper.AssertWasCalled(
					arg => arg.Map(
						Arg<Func<IContainer>>.Is.Anything,
						Arg<Type>.Is.Equal(typeof(IncludedEndpoint)),
						Arg<MethodInfo>.Is.Equal(typeof(IncludedEndpoint).GetMethod("Method")),
						Arg<Route.Routing.Route>.Is.Anything));
				_responseMapper.AssertWasNotCalled(
					arg => arg.Map(
						Arg<Func<IContainer>>.Is.Anything,
						Arg<Type>.Is.Equal(typeof(ExcludedEndpoint)),
						Arg<MethodInfo>.Is.Anything,
						Arg<Route.Routing.Route>.Is.Anything));
			}
		}

		[TestFixture]
		public class When_filtering_methods
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_methodFilter = MockRepository.GenerateMock<IMethodFilter>();
				_methodFilter
					.Stub(arg => arg.Matches(Arg<MethodInfo>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = ((MethodInfo)arg.Arguments.First()).Name == "IncludedMethod")
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_responseMapper = MockRepository.GenerateMock<IResponseMapper>();
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.MethodFilters(_methodFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper)
					.ResponseMapper(_responseMapper);
				_routes = _autoRouteCollection.GenerateRouteCollection().ToArray();
			}

			public class Endpoint
			{
				public void IncludedMethod()
				{
				}

				public void ExcludedMethod()
				{
				}
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;
			private Route.Routing.Route[] _routes;
			private IResponseMapper _responseMapper;
			private IMethodFilter _methodFilter;

			[Test]
			public void Must_consider_correct_methods()
			{
				Assert.That(_routes, Has.Length.EqualTo(1));
				_responseMapper.AssertWasCalled(
					arg => arg.Map(
						Arg<Func<IContainer>>.Is.Anything,
						Arg<Type>.Is.Equal(typeof(Endpoint)),
						Arg<MethodInfo>.Is.Equal(typeof(Endpoint).GetMethod("IncludedMethod")),
						Arg<Route.Routing.Route>.Is.Anything));
				_responseMapper.AssertWasNotCalled(
					arg => arg.Map(
						Arg<Func<IContainer>>.Is.Anything,
						Arg<Type>.Is.Anything,
						Arg<MethodInfo>.Is.Equal("ExcludedMethod"),
						Arg<Route.Routing.Route>.Is.Anything));
			}
		}

		[TestFixture]
		public class When_mapping_ids
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper1 = MockRepository.GenerateMock<IIdMapper>();
				_idMapper1.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.Parse("1dffe3ee-1ade-4aa2-835a-9cb91b7e31c4")));
				_idMapper2 = MockRepository.GenerateMock<IIdMapper>();
				_idMapper2.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.Parse("493e725c-cbc1-4ea4-b6d1-350018d4542d")));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_responseMapper = MockRepository.GenerateMock<IResponseMapper>();
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper1)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper)
					.ResponseMapper(_responseMapper);
				_routes = _autoRouteCollection.GenerateRouteCollection().ToArray();
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper1;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;
			private IResponseMapper _responseMapper;
			private Route.Routing.Route[] _routes;
			private IIdMapper _idMapper2;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_assign_mapped_id()
			{
				Assert.That(_routes[0].Id, Is.EqualTo(Guid.Parse("1dffe3ee-1ade-4aa2-835a-9cb91b7e31c4")));
			}

			[Test]
			public void Must_map_using_first_matching_mapper()
			{
				_idMapper1.AssertWasCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything));
				_idMapper2.AssertWasNotCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything));
			}
		}

		[TestFixture]
		public class When_mapping_names
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper1 = MockRepository.GenerateMock<INameMapper>();
				_nameMapper1.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name1"));
				_nameMapper2 = MockRepository.GenerateMock<INameMapper>();
				_nameMapper2.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name2"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_responseMapper = MockRepository.GenerateMock<IResponseMapper>();
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper1, _nameMapper2)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper)
					.ResponseMapper(_responseMapper);
				_routes = _autoRouteCollection.GenerateRouteCollection().ToArray();
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper1;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;
			private IResponseMapper _responseMapper;
			private INameMapper _nameMapper2;
			private Route.Routing.Route[] _routes;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_assign_mapped_name()
			{
				Assert.That(_routes[0].Name, Is.EqualTo("name1"));
			}

			[Test]
			public void Must_map_using_first_matching_mapper()
			{
				_nameMapper1.AssertWasCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything));
				_nameMapper2.AssertWasNotCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything));
			}
		}

		[TestFixture]
		public class When_mapping_resolved_relative_urls
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper1 = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper1.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative1"));
				_resolvedRelativeUrlMapper2 = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper2.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative2"));
				_responseMapper = MockRepository.GenerateMock<IResponseMapper>();
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper1)
					.ResponseMapper(_responseMapper);
				_routes = _autoRouteCollection.GenerateRouteCollection().ToArray();
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper1;
			private IResponseMapper _responseMapper;
			private Route.Routing.Route[] _routes;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper2;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_assign_mapped_resolved_relative_url()
			{
				Assert.That(_routes[0].ResolvedRelativeUrl, Is.EqualTo("relative1"));
			}

			[Test]
			public void Must_map_using_first_matching_mapper()
			{
				_resolvedRelativeUrlMapper1.AssertWasCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything));
				_resolvedRelativeUrlMapper2.AssertWasNotCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything));
			}
		}

		[TestFixture]
		public class When_mapping_responses
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_responseMapper = MockRepository.GenerateMock<IResponseMapper>();
				_responseMapper.Stub(arg => arg.Map(Arg<Func<IContainer>>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<Route.Routing.Route>.Is.Anything));
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper)
					.ResponseMapper(_responseMapper);
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;
			private IResponseMapper _responseMapper;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_map_using_mapper()
			{
				_autoRouteCollection.GenerateRouteCollection();

				_responseMapper.AssertWasCalled(arg => arg.Map(Arg<Func<IContainer>>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<Route.Routing.Route>.Is.Anything));
			}
		}

		[TestFixture]
		public class When_mapping_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_classFilter = MockRepository.GenerateMock<IClassFilter>();
				_classFilter
					.Stub(arg => arg.Matches(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (Type)arg.Arguments.First() == typeof(Endpoint))
					.Return(false);
				_idMapper = MockRepository.GenerateMock<IIdMapper>();
				_idMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(IdResult.IdMapped(Guid.NewGuid()));
				_nameMapper = MockRepository.GenerateMock<INameMapper>();
				_nameMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(NameResult.NameMapped("name"));
				_resolvedRelativeUrlMapper = MockRepository.GenerateMock<IResolvedRelativeUrlMapper>();
				_resolvedRelativeUrlMapper.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative"));
				_responseMapper = MockRepository.GenerateMock<IResponseMapper>();
				_restrictionMapper1 = MockRepository.GenerateMock<IRestrictionMapper>();
				_restrictionMapper1
					.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<Route.Routing.Route>.Is.Anything, Arg<IContainer>.Is.Anything))
					.WhenCalled(arg => ((Route.Routing.Route)arg.Arguments.Skip(2).First()).RestrictByMethods("GET"));
				_restrictionMapper2 = MockRepository.GenerateMock<IRestrictionMapper>();
				_restrictionMapper2
					.Stub(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<Route.Routing.Route>.Is.Anything, Arg<IContainer>.Is.Anything))
					.WhenCalled(arg => ((Route.Routing.Route)arg.Arguments.Skip(2).First()).RestrictByMethods("POST"));
				_autoRouteCollection = new AutoRouteCollection()
					.Assemblies(Assembly.GetExecutingAssembly())
					.ClassFilters(_classFilter)
					.NameMappers(_nameMapper)
					.IdMappers(_idMapper)
					.ResolvedRelativeUrlMappers(_resolvedRelativeUrlMapper)
					.ResponseMapper(_responseMapper)
					.RestrictionMappers(_restrictionMapper1, _restrictionMapper2);
			}

			private AutoRouteCollection _autoRouteCollection;
			private IClassFilter _classFilter;
			private IIdMapper _idMapper;
			private INameMapper _nameMapper;
			private IResolvedRelativeUrlMapper _resolvedRelativeUrlMapper;
			private IResponseMapper _responseMapper;
			private Route.Routing.Route[] _routes;
			private IRestrictionMapper _restrictionMapper1;
			private IRestrictionMapper _restrictionMapper2;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_assign_mapped_restrictions()
			{
				var container = MockRepository.GenerateMock<IContainer>();

				_autoRouteCollection.RestrictionContainer(container);
				_routes = _autoRouteCollection.GenerateRouteCollection().ToArray();

				MethodRestriction[] methodRestrictions = _routes[0].GetRestrictions<MethodRestriction>().ToArray();

				Assert.That(methodRestrictions, Has.Length.EqualTo(2));
			}

			[Test]
			public void Must_map_using_all_mappers()
			{
				var container = MockRepository.GenerateMock<IContainer>();

				_autoRouteCollection.RestrictionContainer(container);
				_autoRouteCollection.GenerateRouteCollection();

				_restrictionMapper1.AssertWasCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<Route.Routing.Route>.Is.Anything, Arg<IContainer>.Is.Anything));
				_restrictionMapper1.AssertWasCalled(arg => arg.Map(Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<Route.Routing.Route>.Is.Anything, Arg<IContainer>.Is.Anything));
			}

			[Test]
			public void Must_require_restriction_container()
			{
				Assert.Throws<InvalidOperationException>(() => _autoRouteCollection.GenerateRouteCollection());
			}
		}
	}
}