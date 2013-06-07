using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers
{
	public static class HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapperTester
	{
		[TestFixture]
		public class When_mapping_methods_named_after_http_methods
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper();
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper _mapper;
			private Route.Routing.Route _route;
			private IContainer _container;

			public class Endpoint
			{
				public void Get()
				{
				}

				public void POST()
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Get")]
			[TestCase(typeof(Endpoint), "POST")]
			public void Must_add_restrictions(Type type, string methodName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);

				_mapper.MapAsync(type, methodInfo, _route, _container);

				MethodRestriction[] restrictions = _route.GetRestrictions<MethodRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Method, Is.EqualTo(methodName).Using((IComparer)StringComparer.OrdinalIgnoreCase));
			}
		}

		[TestFixture]
		public class When_mapping_methods_not_named_after_http_methods
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper();
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_container = MockRepository.GenerateMock<IContainer>();
			}

			private HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper _mapper;
			private Route.Routing.Route _route;
			private IContainer _container;

			public class Endpoint
			{
				public void Another()
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Another")]
			public void Must_add_restrictions(Type type, string methodName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);

				_mapper.MapAsync(type, methodInfo, _route, _container);

				IEnumerable<MethodRestriction> restrictions = _route.GetRestrictions<MethodRestriction>();

				Assert.That(restrictions, Is.Empty);
			}
		}
	}
}