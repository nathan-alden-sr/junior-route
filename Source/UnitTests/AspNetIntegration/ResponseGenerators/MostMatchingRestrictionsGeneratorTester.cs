using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.Routing;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseGenerators
{
	public static class MostMatchingRestrictionsGeneratorTester
	{
		[TestFixture]
		public class When_determining_response_from_no_matching_routes
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new MostMatchingRestrictionsGenerator();
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private MostMatchingRestrictionsGenerator _generator;
			private HttpContextBase _context;

			[Test]
			public async void Must_use_response_of_route_with_most_matching_restrictions()
			{
				ResponseResult result = await _generator.GetResponseAsync(_context, Enumerable.Empty<RouteMatchResult>());

				Assert.That(result.ResultType, Is.EqualTo(ResponseResultType.ResponseNotGenerated));
			}
		}

		[TestFixture]
		public class When_determining_response_from_set_of_matching_routes
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new MostMatchingRestrictionsGenerator();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://localhost"));
				_request.Stub(arg => arg.HttpMethod).Return("GET");
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_context.Stub(arg => arg.Response).Return(_response);
				_route1Response = new Response(200);
				_route1 = new Route.Routing.Route("name1", Guid.NewGuid(), "relative1");
				_route1.RestrictByMethods(HttpMethod.Get);
				_route1.RespondWith(_route1Response);
				_route2Response = new Response(200);
				_route2 = new Route.Routing.Route("name2", Guid.NewGuid(), "relative2");
				_route2.RestrictByMethods(HttpMethod.Get);
				_route2.RestrictByUrl(uri => true);
				_route2.RespondWith(_route2Response);
				_routeMatchResults = new[]
					{
						new RouteMatchResult(_route1, _route1.MatchesRequestAsync(_request).Result),
						new RouteMatchResult(_route2, _route2.MatchesRequestAsync(_request).Result)
					};
			}

			private MostMatchingRestrictionsGenerator _generator;
			private HttpRequestBase _request;
			private Route.Routing.Route _route1;
			private Route.Routing.Route _route2;
			private IEnumerable<RouteMatchResult> _routeMatchResults;
			private Response _route1Response;
			private Response _route2Response;
			private HttpContextBase _context;
			private HttpResponseBase _response;

			[Test]
			public async void Must_use_response_of_route_with_most_matching_restrictions()
			{
				ResponseResult result = await _generator.GetResponseAsync(_context, _routeMatchResults);

				Assert.That(result.CacheKey, Is.EqualTo(_route2.Id.ToString()));
				Assert.That(result.ResultType, Is.EqualTo(ResponseResultType.ResponseGenerated));
				Assert.That(await result.Response, Is.SameAs(_route2Response));
			}
		}

		[TestFixture]
		public class When_determining_response_from_set_of_matching_routes_with_more_than_one_best_match
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new MostMatchingRestrictionsGenerator();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://localhost"));
				_request.Stub(arg => arg.HttpMethod).Return("GET");
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_route1Response = new Response(200);
				_route1 = new Route.Routing.Route("name1", Guid.NewGuid(), "relative1");
				_route1.RestrictByMethods(HttpMethod.Get);
				_route1.RespondWith(_route1Response);
				_route2Response = new Response(200);
				_route2 = new Route.Routing.Route("name2", Guid.NewGuid(), "relative2");
				_route2.RestrictByMethods(HttpMethod.Get);
				_route2.RespondWith(_route2Response);
				_routeMatchResults = new List<RouteMatchResult>
					{
						new RouteMatchResult(_route1, _route1.MatchesRequestAsync(_request).Result),
						new RouteMatchResult(_route2, _route2.MatchesRequestAsync(_request).Result)
					};
			}

			private MostMatchingRestrictionsGenerator _generator;
			private HttpRequestBase _request;
			private Route.Routing.Route _route1;
			private Route.Routing.Route _route2;
			private List<RouteMatchResult> _routeMatchResults;
			private Response _route1Response;
			private Response _route2Response;
			private HttpContextBase _context;

			[Test]
			public async void Must_generate_multiple_choices_response()
			{
				ResponseResult result = await _generator.GetResponseAsync(_context, _routeMatchResults);

				Assert.That(result.CacheKey, Is.Null);
				Assert.That(result.ResultType, Is.EqualTo(ResponseResultType.ResponseGenerated));
				Assert.That((await result.Response).StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.MultipleChoices));
			}
		}
	}
}