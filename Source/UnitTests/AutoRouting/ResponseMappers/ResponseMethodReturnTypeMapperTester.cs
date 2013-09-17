using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ResponseMappers;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Responses.Text;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ResponseMappers
{
	public static class ResponseMethodReturnTypeMapperTester
	{
		[TestFixture]
		public class When_mapping_async_response_with_return_type
		{
			[SetUp]
			public void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper.ToEnumerable(), Enumerable.Empty<IMappedDelegateContextFactory>());
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(new Endpoint());
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.MapAsync(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_response = _route.ProcessResponseAsync(_context).Result;
			}

			private ResponseMethodReturnTypeMapper _responseMethodReturnTypeMapper;
			private IContainer _container;
			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private IResponse _response;
			private IParameterMapper _parameterMapper;

			public class Endpoint
			{
				public Task<PlainResponse> Method()
				{
					return new PlainResponse("ABC", Encoding.ASCII).AsCompletedTask();
				}
			}

			[Test]
			public async void Must_call_method()
			{
				Assert.That(_response.ContentType, Is.EqualTo("text/plain"));
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.OK));
				Assert.That(await _response.GetContentAsync(), Is.EquivalentTo(Encoding.ASCII.GetBytes("ABC")));
			}

			[Test]
			public void Route_response_type_must_be_plainresponse()
			{
				Assert.That(_route.ResponseType, Is.EqualTo(typeof(PlainResponse)));
			}
		}

		[TestFixture]
		public class When_mapping_response_with_a_non_null_mapped_delegate_context
		{
			[SetUp]
			public void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_mappedDelegateContext = MockRepository.GenerateMock<IMappedDelegateContext>();
				_mappedDelegateContextFactory = MockRepository.GenerateMock<IMappedDelegateContextFactory>();
				_mappedDelegateContextFactory.Stub(arg => arg.CreateContext(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(_mappedDelegateContext);
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper.ToEnumerable(), new[] { _mappedDelegateContextFactory });
				_endpoint = new Endpoint();
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(_endpoint);
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.MapAsync(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();

				// ReSharper disable once UnusedVariable
				IResponse response = _route.ProcessResponseAsync(_context).Result;
			}

			private ResponseMethodReturnTypeMapper _responseMethodReturnTypeMapper;
			private IContainer _container;
			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private IParameterMapper _parameterMapper;
			private IMappedDelegateContextFactory _mappedDelegateContextFactory;
			private Endpoint _endpoint;
			private IMappedDelegateContext _mappedDelegateContext;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_complete_context()
			{
				_mappedDelegateContext.AssertWasCalled(arg => arg.Complete());
			}

			[Test]
			public void Must_create_context()
			{
				_mappedDelegateContextFactory.AssertWasCalled(arg => arg.CreateContext(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything));
			}

			[Test]
			public void Must_dispose_context()
			{
				_mappedDelegateContext.AssertWasCalled(arg => arg.Dispose());
			}
		}

		[TestFixture]
		public class When_mapping_response_with_null_mapped_delegate_context
		{
			[SetUp]
			public void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_mappedDelegateContextFactory = MockRepository.GenerateMock<IMappedDelegateContextFactory>();
				_mappedDelegateContextFactory.Stub(arg => arg.CreateContext(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything)).Return(null);
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper.ToEnumerable(), new[] { _mappedDelegateContextFactory });
				_endpoint = new Endpoint();
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(_endpoint);
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.MapAsync(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private ResponseMethodReturnTypeMapper _responseMethodReturnTypeMapper;
			private IContainer _container;
			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private IParameterMapper _parameterMapper;
			private IMappedDelegateContextFactory _mappedDelegateContextFactory;
			private Endpoint _endpoint;

			public class Endpoint
			{
				public void Method()
				{
				}
			}

			[Test]
			public void Must_not_throw_exception()
			{
				Assert.That(() => _route.ProcessResponseAsync(_context).Result, Throws.Nothing);
			}
		}

		[TestFixture]
		public class When_mapping_response_with_return_type
		{
			[SetUp]
			public void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper.ToEnumerable(), Enumerable.Empty<IMappedDelegateContextFactory>());
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(new Endpoint());
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.MapAsync(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_response = _route.ProcessResponseAsync(_context).Result;
			}

			private ResponseMethodReturnTypeMapper _responseMethodReturnTypeMapper;
			private IContainer _container;
			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private IResponse _response;
			private IParameterMapper _parameterMapper;

			public class Endpoint
			{
				public PlainResponse Method()
				{
					return new PlainResponse("ABC", Encoding.ASCII);
				}
			}

			[Test]
			public async void Must_call_method()
			{
				Assert.That(_response.ContentType, Is.EqualTo("text/plain"));
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.OK));
				Assert.That(await _response.GetContentAsync(), Is.EquivalentTo(Encoding.ASCII.GetBytes("ABC")));
			}

			[Test]
			public void Route_response_type_must_be_plainresponse()
			{
				Assert.That(_route.ResponseType, Is.EqualTo(typeof(PlainResponse)));
			}
		}

		[TestFixture]
		public class When_mapping_response_with_void_return_type
		{
			[SetUp]
			public void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper.ToEnumerable(), Enumerable.Empty<IMappedDelegateContextFactory>());
				_endpoint = new Endpoint();
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(_endpoint);
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.MapAsync(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_response = _route.ProcessResponseAsync(_context).Result;
			}

			private ResponseMethodReturnTypeMapper _responseMethodReturnTypeMapper;
			private IContainer _container;
			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private IResponse _response;
			private IParameterMapper _parameterMapper;
			private Endpoint _endpoint;

			public class Endpoint
			{
				public bool MethodCalled
				{
					get;
					set;
				}

				public void Method()
				{
					MethodCalled = true;
				}
			}

			[Test]
			public void Must_call_method()
			{
				Assert.That(_endpoint.MethodCalled, Is.True);
			}

			[Test]
			public void Must_return_no_content_response()
			{
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.NoContent));
			}

			[Test]
			public void Route_response_type_must_be_null()
			{
				Assert.That(_route.ResponseType, Is.Null);
			}
		}
	}
}