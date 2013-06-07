using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
				_httpContext = MockRepository.GenerateMock<HttpContextBase>();
				_httpContext.Stub(arg => arg.Request).Return(_httpRequest);
				_httpContext.Stub(arg => arg.Response).Return(_httpResponse);
				_cachePolicy = MockRepository.GenerateMock<ICachePolicy>();
				_cachePolicy.Stub(arg => arg.Clone()).Return(_cachePolicy);
				_cachePolicy.Stub(arg => arg.ClientCacheExpirationUtcTimestamp).Return(DateTime.UtcNow);
				_response = MockRepository.GenerateMock<IResponse>();
				_response.Stub(arg => arg.CachePolicy).Return(_cachePolicy);
				_response.Stub(arg => arg.Cookies).Return(Enumerable.Empty<Cookie>());
				_response.Stub(arg => arg.Headers).Return(Enumerable.Empty<Header>());
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(HttpStatusCode.OK));
				_response.Stub(arg => arg.GetContentAsync()).Return(new byte[0].AsCompletedTask());
				_cache = MockRepository.GenerateMock<ICache>();
				_cache.Stub(arg => arg.GetAsync(Arg<string>.Is.Anything)).Return(Task<CacheItem>.Factory.Empty());
				_cache.Stub(arg => arg.AddAsync(Arg<string>.Is.Anything, Arg<CacheResponse>.Is.Anything, Arg<DateTime>.Is.Anything)).Return(Task.Factory.Empty());
			}

			private ISystemClock _systemClock;
			private CacheableResponseHandler _handler;
			private HttpRequestBase _httpRequest;
			private HttpResponseBase _httpResponse;
			private IResponse _response;
			private ICachePolicy _cachePolicy;
			private ICache _cache;
			private HttpContextBase _httpContext;

			[Test]
			public async void Must_cache_if_cache_policy_allows_server_caching()
			{
				_cachePolicy.Stub(arg => arg.AllowsServerCaching).Return(true);
				_cachePolicy.Stub(arg => arg.ServerCacheExpirationUtcTimestamp).Return(DateTime.UtcNow);
				_response.CachePolicy.Stub(arg => arg.HasPolicy).Return(true);

				await _handler.HandleResponse(_httpContext, _response, _cache, "key");

				_cache.AssertWasCalled(arg => arg.AddAsync(Arg<string>.Is.Equal("key"), Arg<CacheResponse>.Is.Anything, Arg<DateTime>.Is.Anything));
			}

			[Test]
			public async void Must_not_handle_response_when_response_has_cache_policy_but_no_cache_or_cache_key_was_provided()
			{
				_response.CachePolicy.Stub(arg => arg.HasPolicy).Return(true);

				ResponseHandlerResult result = await _handler.HandleResponse(_httpContext, _response, null, null);

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseNotHandled));
			}

			[Test]
			public async void Must_not_handle_response_when_response_has_no_cache_policy_but_cache_and_cache_key_was_provided()
			{
				_response.CachePolicy.Stub(arg => arg.HasPolicy).Return(false);

				ResponseHandlerResult result = await _handler.HandleResponse(_httpContext, _response, _cache, "key");

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseNotHandled));
			}
		}
	}
}