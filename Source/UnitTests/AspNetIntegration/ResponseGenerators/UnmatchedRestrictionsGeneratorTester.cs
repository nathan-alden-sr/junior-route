using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseGenerators
{
	public static class UnmatchedRestrictionsGeneratorTester
	{
		[TestFixture]
		public class When_closest_matching_route_matched_on_url_relative_path_and_has_unmatched_accept_charset_header_restriction
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new UnmatchedRestrictionsGenerator();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_matchedRestrictions = new[]
					{
						new UrlRelativePathRestriction("", CaseInsensitivePlainRequestValueComparer.Instance, _httpRuntime)
					};
				_unmatchedRestrictions = new IRestriction[]
					{
						new HeaderRestriction<AcceptCharsetHeader>("Accept-Charset", (Func<string, IEnumerable<AcceptCharsetHeader>>)AcceptCharsetHeader.ParseMany, header => false)
					};
				_routeMatchResults = new[]
					{
						new RouteMatchResult(_route, MatchResult.RouteNotMatched(_matchedRestrictions, _unmatchedRestrictions))
					};
			}

			private UnmatchedRestrictionsGenerator _generator;
			private HttpRequestBase _request;
			private Route.Routing.Route _route;
			private IEnumerable<RouteMatchResult> _routeMatchResults;
			private IEnumerable<IRestriction> _unmatchedRestrictions;
			private IHttpRuntime _httpRuntime;
			private IEnumerable<UrlRelativePathRestriction> _matchedRestrictions;

			[Test]
			public void Must_generate_not_acceptable_response()
			{
				ResponseResult result = _generator.GetResponse(_request, _routeMatchResults);

				Assert.That(result.CacheKey, Is.Null);
				Assert.That(result.Response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.NotAcceptable));
			}
		}

		[TestFixture]
		public class When_closest_matching_route_matched_on_url_relative_path_and_has_unmatched_accept_encoding_header_restriction
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new UnmatchedRestrictionsGenerator();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_matchedRestrictions = new[]
					{
						new UrlRelativePathRestriction("", CaseInsensitivePlainRequestValueComparer.Instance, _httpRuntime)
					};
				_unmatchedRestrictions = new IRestriction[]
					{
						new HeaderRestriction<AcceptEncodingHeader>("Accept-Encoding", (Func<string, IEnumerable<AcceptEncodingHeader>>)AcceptEncodingHeader.ParseMany, header => false)
					};
				_routeMatchResults = new[]
					{
						new RouteMatchResult(_route, MatchResult.RouteNotMatched(_matchedRestrictions, _unmatchedRestrictions))
					};
			}

			private UnmatchedRestrictionsGenerator _generator;
			private HttpRequestBase _request;
			private Route.Routing.Route _route;
			private IEnumerable<RouteMatchResult> _routeMatchResults;
			private IEnumerable<IRestriction> _unmatchedRestrictions;
			private IHttpRuntime _httpRuntime;
			private IEnumerable<UrlRelativePathRestriction> _matchedRestrictions;

			[Test]
			public void Must_generate_not_acceptable_response()
			{
				ResponseResult result = _generator.GetResponse(_request, _routeMatchResults);

				Assert.That(result.CacheKey, Is.Null);
				Assert.That(result.Response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.NotAcceptable));
			}
		}

		[TestFixture]
		public class When_closest_matching_route_matched_on_url_relative_path_and_has_unmatched_content_encoding_header_restriction
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new UnmatchedRestrictionsGenerator();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_matchedRestrictions = new[]
					{
						new UrlRelativePathRestriction("", CaseInsensitivePlainRequestValueComparer.Instance, _httpRuntime)
					};
				_unmatchedRestrictions = new IRestriction[]
					{
						new HeaderRestriction<ContentEncodingHeader>("Content-Encoding", (Func<string, IEnumerable<ContentEncodingHeader>>)ContentEncodingHeader.ParseMany, header => false)
					};
				_routeMatchResults = new[]
					{
						new RouteMatchResult(_route, MatchResult.RouteNotMatched(_matchedRestrictions, _unmatchedRestrictions))
					};
			}

			private UnmatchedRestrictionsGenerator _generator;
			private HttpRequestBase _request;
			private Route.Routing.Route _route;
			private IEnumerable<RouteMatchResult> _routeMatchResults;
			private IEnumerable<IRestriction> _unmatchedRestrictions;
			private IHttpRuntime _httpRuntime;
			private IEnumerable<UrlRelativePathRestriction> _matchedRestrictions;

			[Test]
			public void Must_generate_unsupported_media_type_response()
			{
				ResponseResult result = _generator.GetResponse(_request, _routeMatchResults);

				Assert.That(result.CacheKey, Is.Null);
				Assert.That(result.Response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.UnsupportedMediaType));
			}
		}

		[TestFixture]
		public class When_closest_matching_route_matched_on_url_relative_path_and_has_unmatched_method_restriction
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new UnmatchedRestrictionsGenerator();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_matchedRestrictions = new[]
					{
						new UrlRelativePathRestriction("", CaseInsensitivePlainRequestValueComparer.Instance, _httpRuntime)
					};
				_unmatchedRestrictions = new[]
					{
						new MethodRestriction("GET")
					};
				_routeMatchResults = new[]
					{
						new RouteMatchResult(_route, MatchResult.RouteNotMatched(_matchedRestrictions, _unmatchedRestrictions))
					};
			}

			private UnmatchedRestrictionsGenerator _generator;
			private HttpRequestBase _request;
			private Route.Routing.Route _route;
			private IEnumerable<RouteMatchResult> _routeMatchResults;
			private IEnumerable<IRestriction> _unmatchedRestrictions;
			private IHttpRuntime _httpRuntime;
			private IEnumerable<UrlRelativePathRestriction> _matchedRestrictions;

			[Test]
			public void Must_generate_method_not_allowed_response()
			{
				ResponseResult result = _generator.GetResponse(_request, _routeMatchResults);

				Assert.That(result.CacheKey, Is.Null);
				Assert.That(result.Response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
				Assert.That(result.Response.Headers.Any(arg => arg.Field == "Allow" && arg.Value == "GET"), Is.True);
			}
		}
	}
}