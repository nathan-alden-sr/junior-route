using System.Linq;
using System.Net;
using System.Web;

using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

using Cookie = Junior.Route.Routing.Responses.Cookie;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseHandlers
{
	public static class NonCacheableResponseHandlerTester
	{
		[TestFixture]
		public class When_handling_response
		{
			[SetUp]
			public void SetUp()
			{
				_handler = new NonCacheableResponseHandler();
				_httpRequest = MockRepository.GenerateMock<HttpRequestBase>();
				_httpCachePolicyBase = MockRepository.GenerateMock<HttpCachePolicyBase>();
				_httpResponse = MockRepository.GenerateMock<HttpResponseBase>();
				_httpResponse.Stub(arg => arg.BinaryWrite(Arg<byte[]>.Is.Anything)).WhenCalled(arg => _responseWritten = true);
				_httpResponse.Stub(arg => arg.Cache).Return(_httpCachePolicyBase);
				_httpContext = MockRepository.GenerateMock<HttpContextBase>();
				_httpContext.Stub(arg => arg.Request).Return(_httpRequest);
				_httpContext.Stub(arg => arg.Response).Return(_httpResponse);
				_cachePolicy = MockRepository.GenerateMock<ICachePolicy>();
				_cachePolicy.Stub(arg => arg.Clone()).Return(_cachePolicy);
				_response = MockRepository.GenerateMock<IResponse>();
				_response.Stub(arg => arg.CachePolicy).Return(_cachePolicy);
				_response.Stub(arg => arg.Cookies).Return(Enumerable.Empty<Cookie>());
				_response.Stub(arg => arg.GetContent()).Return(new byte[0]);
				_response.Stub(arg => arg.Headers).Return(Enumerable.Empty<Header>());
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(HttpStatusCode.OK));
				_cache = MockRepository.GenerateMock<ICache>();
				_result = _handler.HandleResponse(_httpContext, _response, _cache, "key");
			}

			private NonCacheableResponseHandler _handler;
			private HttpRequestBase _httpRequest;
			private HttpResponseBase _httpResponse;
			private IResponse _response;
			private ICache _cache;
			private ResponseHandlerResult _result;
			private bool _responseWritten;
			private ICachePolicy _cachePolicy;
			private HttpCachePolicyBase _httpCachePolicyBase;
			private HttpContextBase _httpContext;

			[Test]
			public void Must_result_in_response_written()
			{
				Assert.That(_result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseWritten));
			}

			[Test]
			public void Must_write_response_to_httpresponse()
			{
				Assert.That(_responseWritten, Is.True);
			}
		}
	}
}