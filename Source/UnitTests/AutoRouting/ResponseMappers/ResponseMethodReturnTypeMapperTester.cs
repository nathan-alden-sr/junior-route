using System;
using System.Net;
using System.Text;
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
		public class When_mapping_response
		{
			[SetUp]
			public void SetUp()
			{
				_parameterMapper = MockRepository.GenerateMock<IParameterMapper>();
				_responseMethodReturnTypeMapper = new ResponseMethodReturnTypeMapper(_parameterMapper);
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Endpoint))).Return(new Endpoint());
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_responseMethodReturnTypeMapper.Map(() => _container, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"), _route);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_response = _route.ProcessResponse(_request);
			}

			private ResponseMethodReturnTypeMapper _responseMethodReturnTypeMapper;
			private IContainer _container;
			private Route.Routing.Route _route;
			private HttpRequestBase _request;
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
			public void Must_use_method()
			{
				Assert.That(_response.ContentType, Is.EqualTo("text/plain"));
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.OK));
				Assert.That(_response.GetContent(), Is.EquivalentTo(Encoding.ASCII.GetBytes("ABC")));
			}
		}
	}
}