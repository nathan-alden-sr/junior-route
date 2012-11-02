using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Caching
{
	public static class CacheResponseTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				Response response = Response
					.OK()
					.ApplicationJson()
					.Charset("utf-8")
					.Content("content")
					.ContentEncoding(Encoding.ASCII)
					.Cookie(new HttpCookie("name", "value"))
					.Header("field", "value")
					.HeaderEncoding(Encoding.UTF8);

				response.CachePolicy.ETag("etag");

				_cacheResponse = new CacheResponse(response);
			}

			private CacheResponse _cacheResponse;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_cacheResponse.CachePolicy.ETag, Is.EqualTo("etag"));
				Assert.That(_cacheResponse.Charset, Is.EqualTo("utf-8"));
				Assert.That(_cacheResponse.Content, Is.EqualTo(Encoding.ASCII.GetBytes("content")));
				Assert.That(_cacheResponse.ContentEncoding, Is.SameAs(Encoding.ASCII));
				Assert.That(_cacheResponse.ContentType, Is.EqualTo("application/json"));
				Assert.That(_cacheResponse.Cookies.Count(), Is.EqualTo(1));
				Assert.That(_cacheResponse.Cookies.ElementAt(0).Name, Is.EqualTo("name"));
				Assert.That(_cacheResponse.Cookies.ElementAt(0).Value, Is.EqualTo("value"));
				Assert.That(_cacheResponse.Headers.ElementAt(0).Field, Is.EqualTo("field"));
				Assert.That(_cacheResponse.Headers.ElementAt(0).Value, Is.EqualTo("value"));
				Assert.That(_cacheResponse.HeaderEncoding, Is.SameAs(Encoding.UTF8));
			}
		}

		[TestFixture]
		public class When_writing_response
		{
			[SetUp]
			public void SetUp()
			{
				_httpCachePolicyBase = MockRepository.GenerateMock<HttpCachePolicyBase>();
				_memoryStream = new MemoryStream();

				_httpResponseBase = MockRepository.GenerateMock<HttpResponseBase>();
				_httpResponseBase.Stub(arg => arg.Cache).Return(_httpCachePolicyBase);
				_httpResponseBase.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
				_httpResponseBase.Stub(arg => arg.Headers).Return(new NameValueCollection());
				_httpResponseBase.Stub(arg => arg.OutputStream).Return(_memoryStream);

				Response response = Response
					.OK()
					.ApplicationJson()
					.Charset("utf-8")
					.Content("content")
					.ContentEncoding(Encoding.ASCII)
					.Cookie(new HttpCookie("name", "value"))
					.Header("field", "value")
					.HeaderEncoding(Encoding.UTF8);

				response.CachePolicy.NoClientCaching();

				_cacheResponse = new CacheResponse(response);
				_cacheResponse.WriteResponse(_httpResponseBase);
			}

			private HttpResponseBase _httpResponseBase;
			private CacheResponse _cacheResponse;
			private HttpCachePolicyBase _httpCachePolicyBase;
			private MemoryStream _memoryStream;

			[Test]
			public void Must_set_response_properties_and_apply_cache_policy()
			{
				_httpResponseBase.AssertWasCalled(arg => arg.StatusCode = 200);
				_httpResponseBase.AssertWasCalled(arg => arg.SubStatusCode = 0);
				_httpResponseBase.AssertWasCalled(arg => arg.ContentType = "application/json");
				_httpResponseBase.AssertWasCalled(arg => arg.Charset = "utf-8");
				_httpResponseBase.AssertWasCalled(arg => arg.ContentEncoding = Encoding.ASCII);
				_httpResponseBase.AssertWasCalled(arg => arg.HeaderEncoding = Encoding.UTF8);
				Assert.That(_httpResponseBase.Cookies, Has.Count.EqualTo(1));
				Assert.That(_httpResponseBase.Headers, Has.Count.EqualTo(1));
				Assert.That(_memoryStream.ToArray(), Is.EquivalentTo(Encoding.ASCII.GetBytes("content")));
				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.NoCache));
			}
		}
	}
}