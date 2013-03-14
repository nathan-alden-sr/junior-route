using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
			public async void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper);
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(new Endpoint());
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.Map(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_response = await _route.ProcessResponse(_context);
			}

			private ResponseMethodReturnTypeMapper _responseMethodReturnTypeMapper;
			private IContainer _container;
			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private IResponse _response;
			private IParameterMapper _parameterMapper;

			public class Endpoint
			{
				public async Task<PlainResponse> Method()
				{
					return await Task.FromResult(new PlainResponse("ABC", Encoding.ASCII));
				}
			}

			[Test]
			public void Must_call_method()
			{
				Assert.That(_response.ContentType, Is.EqualTo("text/plain"));
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.OK));
				Assert.That(_response.GetContent(), Is.EquivalentTo(Encoding.ASCII.GetBytes("ABC")));
			}

			[Test]
			public void Route_response_type_must_be_plainresponse()
			{
				Assert.That(_route.ResponseType, Is.EqualTo(typeof(PlainResponse)));
			}
		}

		[TestFixture]
		public class When_mapping_response_with_return_type
		{
			[SetUp]
			public async void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper);
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(new Endpoint());
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.Map(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_response = await _route.ProcessResponse(_context);
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
			public void Must_call_method()
			{
				Assert.That(_response.ContentType, Is.EqualTo("text/plain"));
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.OK));
				Assert.That(_response.GetContent(), Is.EquivalentTo(Encoding.ASCII.GetBytes("ABC")));
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
			public async void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper);
				_endpoint = new Endpoint();
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(_endpoint);
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.Map(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_response = await _route.ProcessResponse(_context);
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