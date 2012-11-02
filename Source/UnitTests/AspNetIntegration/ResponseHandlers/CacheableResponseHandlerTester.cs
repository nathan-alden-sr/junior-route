using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

using Cookie = Junior.Route.Routing.Responses.Cookie;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseHandlers
{
	public static class CacheableResponseHandlerTester
	{
		[TestFixture]
		public class When_handling_response
		{
			[SetUp]
			public void SetUp()
			{
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_handler = new CacheableResponseHandler(_systemClock);
				_httpRequest = MockRepository.GenerateMock<HttpRequestBase>();
				_httpRequest.Stub(arg => arg.Headers).Return(new NameValueCollection());
				_httpResponse = MockRepository.GenerateMock<HttpResponseBase>();
				_httpResponse.Stub(arg => arg.Headers).Return(new NameValueCollection());
				_httpResponse.Stub(arg => arg.OutputStream).Return(new MemoryStream());
				_cachePolicy = MockRepository.GenerateMock<ICachePolicy>();
				_cachePolicy.Stub(arg => arg.Clone()).Return(_cachePolicy);
				_cachePolicy.Stub(arg => arg.Expires).Return(DateTime.UtcNow);
				_response = MockRepository.GenerateMock<IResponse>();
				_response.Stub(arg => arg.CachePolicy).Return(_cachePolicy);
				_response.Stub(arg => arg.Cookies).Return(Enumerable.Empty<Cookie>());
				_response.Stub(arg => arg.Headers).Return(Enumerable.Empty<Header>());
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(HttpStatusCode.OK));
				_response.Stub(arg => arg.GetContent()).Return(new byte[0]);
				_cache = MockRepository.GenerateMock<ICache>();
			}

			private ISystemClock _systemClock;
			private CacheableResponseHandler _handler;
			private HttpRequestBase _httpRequest;
			private HttpResponseBase _httpResponse;
			private IResponse _response;
			private ICachePolicy _cachePolicy;
			private ICache _cache;

			[Test]
			public void Must_cache_if_cache_policy_allows_server_caching()
			{
				_cachePolicy.Stub(arg => arg.AllowsServerCaching).Return(true);
				_response.CachePolicy.Stub(arg => arg.HasPolicy).Return(true);

				_handler.HandleResponse(_httpRequest, _httpResponse, _response, _cache, "key");

				_cache.AssertWasCalled(arg => arg.Add(Arg<string>.Is.Equal("key"), Arg<CacheResponse>.Is.Anything, Arg<DateTime>.Is.Anything));
			}

			[Test]
			public void Must_not_handle_response_when_response_has_cache_policy_but_no_cache_or_cache_key_was_provided()
			{
				_response.CachePolicy.Stub(arg => arg.HasPolicy).Return(true);

				ResponseHandlerResult result = _handler.HandleResponse(_httpRequest, _httpResponse, _response, null, null);

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseNotHandled));
			}

			[Test]
			public void Must_not_handle_response_when_response_has_no_cache_policy_but_cache_and_cache_key_was_provided()
			{
				_response.CachePolicy.Stub(arg => arg.HasPolicy).Return(false);

				ResponseHandlerResult result = _handler.HandleResponse(_httpRequest, _httpResponse, _response, _cache, "key");

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseNotHandled));
			}
		}
	}
}