using System;
using System.Net;
using System.Web;

using Junior.Route.AutoRouting.ResponseMappers;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ResponseMappers
{
	public static class NoContentMapperTester
	{
		[TestFixture]
		public class When_mapping_response
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new NoContentMapper();
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_mapper.MapAsync(() => null, typeof(string), typeof(string).GetMethod("Trim", Type.EmptyTypes), _route);
				_response = _route.ProcessResponseAsync(_context).Result;
			}

			private NoContentMapper _mapper;
			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private IResponse _response;

			[Test]
			public void Must_map_no_content()
			{
				Assert.That(_route.ResponseType, Is.Null);
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.NoContent));
			}
		}
	}
}