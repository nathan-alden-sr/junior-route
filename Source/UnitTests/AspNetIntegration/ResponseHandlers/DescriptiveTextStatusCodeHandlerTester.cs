using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseHandlers
{
	public static class DescriptiveTextStatusCodeHandlerTester
	{
		[TestFixture]
		public class When_handling_response
		{
			[SetUp]
			public void SetUp()
			{
				_handler = new DescriptiveTextStatusCodeHandler(new StatusAndSubStatusCode(200), new StatusAndSubStatusCode(200, 1));
				_httpRequest = MockRepository.GenerateMock<HttpRequestBase>();
				_httpRequest.Stub(arg => arg.Headers).Return(new NameValueCollection());
				_httpCachePolicyBase = MockRepository.GenerateMock<HttpCachePolicyBase>();
				_httpResponse = MockRepository.GenerateMock<HttpResponseBase>();
				_httpResponse.Stub(arg => arg.Cache).Return(_httpCachePolicyBase);
				_httpResponse.Stub(arg => arg.TrySkipIisCustomErrors).PropertyBehavior();
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
			}

			private DescriptiveTextStatusCodeHandler _handler;
			private HttpRequestBase _httpRequest;
			private HttpResponseBase _httpResponse;
			private IResponse _response;
			private ICachePolicy _cachePolicy;
			private HttpCachePolicyBase _httpCachePolicyBase;
			private HttpContextBase _httpContext;

			[Test]
			[TestCase(200)]
			public void Must_handle_configured_status_codes(int statusCode)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode));

				ResponseHandlerResult result = _handler.HandleResponse(_httpContext, _response, null, null);

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseWritten));
			}

			[Test]
			[TestCase(200)]
			public void Must_handle_response_if_accept_headers_allow_text_plain(int statusCode)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode));
				_httpRequest.Headers["Accept"] = "text/plain";

				ResponseHandlerResult result = _handler.HandleResponse(_httpContext, _response, null, null);

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseWritten));
			}

			[Test]
			[TestCase(200)]
			public void Must_handle_response_if_no_accept_headers_present(int statusCode)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode));

				ResponseHandlerResult result = _handler.HandleResponse(_httpContext, _response, null, null);

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseWritten));
			}

			[Test]
			[TestCase(200)]
			public void Must_not_allow_client_caching(int statusCode)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode));
				_handler.HandleResponse(_httpContext, _response, null, null);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.NoCache));
			}

			[Test]
			[TestCase(201)]
			[TestCase(400)]
			[TestCase(500)]
			public void Must_not_handle_non_configured_status_codes(int statusCode)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode));

				ResponseHandlerResult result = _handler.HandleResponse(_httpContext, _response, null, null);

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseNotHandled));
			}

			[Test]
			[TestCase(200)]
			public void Must_not_handle_response_if_accept_headers_do_not_allow_text_html(int statusCode)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode));
				_httpRequest.Headers["Accept"] = "application/json";

				ResponseHandlerResult result = _handler.HandleResponse(_httpContext, _response, null, null);

				Assert.That(result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseNotHandled));
			}

			[Test]
			[TestCase(200, 0, "HTTP 200 (OK)")]
			[TestCase(200, 1, "HTTP 200.1 (OK)")]
			public void Must_respond_with_status_code_and_description(int statusCode, int subStatusCode, string response)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode, subStatusCode));
				_handler.HandleResponse(_httpContext, _response, null, null);

				_httpResponse.AssertWasCalled(arg => arg.BinaryWrite(Encoding.UTF8.GetBytes(response)));
			}

			[Test]
			[TestCase(200)]
			public void Must_set_try_skip_iis_custom_errors_flag(int statusCode)
			{
				_response.Stub(arg => arg.StatusCode).Return(new StatusAndSubStatusCode(statusCode));
				_handler.HandleResponse(_httpContext, _response, null, null);

				Assert.That(_httpResponse.TrySkipIisCustomErrors, Is.True);
			}
		}
	}
}