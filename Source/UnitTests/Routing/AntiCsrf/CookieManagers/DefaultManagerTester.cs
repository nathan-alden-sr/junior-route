using System;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.CookieManagers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.CookieManagers
{
	public static class DefaultManagerTester
	{
		[TestFixture]
		public class When_configuring_cookie_and_request_cookie_does_not_exist
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();

				var requestCookies = new HttpCookieCollection();
				var responseCookies = new HttpCookieCollection();

				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(requestCookies);
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_response.Stub(arg => arg.Cookies).Return(responseCookies);
				_manager = new DefaultManager(_configuration, _guidFactory);
			}

			private DefaultManager _manager;
			private IAntiCsrfConfiguration _configuration;
			private IGuidFactory _guidFactory;
			private HttpRequestBase _request;
			private HttpResponseBase _response;

			[Test]
			public void Must_add_cookie_to_response()
			{
				_manager.ConfigureCookieAsync(_request, _response);

				Assert.That(_response.Cookies.AllKeys.Contains("name"));
			}

			[Test]
			public void Must_set_cookie_value_to_new_session_id_using_guidfactory()
			{
				Guid sessionId = Guid.Parse("7c5ec674-f3cb-442a-a72e-877bdb66f777");

				_guidFactory.Stub(arg => arg.Random()).Return(sessionId);
				_manager.ConfigureCookieAsync(_request, _response);

				Assert.That(_response.Cookies["name"].Value, Is.EqualTo(sessionId.ToString("N")));
			}

			[Test]
			public void Must_set_http_only_flag()
			{
				_manager.ConfigureCookieAsync(_request, _response);

				Assert.That(_response.Cookies["name"].HttpOnly, Is.True);
			}
		}

		[TestFixture]
		public class When_configuring_cookie_and_request_cookie_exists
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", _sessionId.ToString("N")) });
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_response.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
				_cookieManager = new DefaultManager(_configuration, _guidFactory);
				_cookieManager.ConfigureCookieAsync(_request, _response);
				_sessionId = Guid.Parse("6a9ef3cd-eb3f-49b6-90ba-0dfda4cb183d");
			}

			private DefaultManager _cookieManager;
			private IAntiCsrfConfiguration _configuration;
			private IGuidFactory _guidFactory;
			private HttpRequestBase _request;
			private HttpResponseBase _response;
			private Guid _sessionId;

			[Test]
			public void Must_add_request_cookie_to_response()
			{
				Assert.That(_response.Cookies.AllKeys.Contains("name"));
			}

			[Test]
			public void Must_set_cookie_value_to_request_cookie_value()
			{
				Assert.That(_response.Cookies["name"].Value, Is.EqualTo(_sessionId.ToString("N")));
				_guidFactory.AssertWasNotCalled(arg => arg.Random());
			}

			[Test]
			public void Must_set_http_only_flag()
			{
				Assert.That(_response.Cookies["name"].HttpOnly, Is.True);
			}
		}

		[TestFixture]
		public class When_getting_session_id_and_response_cookie_does_not_exist
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_response.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());

				_cookieManager = new DefaultManager(_configuration, _guidFactory);
			}

			private IAntiCsrfConfiguration _configuration;
			private IGuidFactory _guidFactory;
			private DefaultManager _cookieManager;
			private HttpResponseBase _response;

			[Test]
			public void Must_return_null()
			{
				Assert.That(_cookieManager.GetSessionIdAsync(_response), Is.Null);
			}
		}

		[TestFixture]
		public class When_getting_session_id_and_response_cookie_exists
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_sessionId = Guid.Parse("98b92ab2-3905-4e25-aefe-cff7dc5df9d3");
				_response.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", _sessionId.ToString("N")) { HttpOnly = true } });
				_cookieManager = new DefaultManager(_configuration, _guidFactory);
			}

			private IAntiCsrfConfiguration _configuration;
			private IGuidFactory _guidFactory;
			private DefaultManager _cookieManager;
			private HttpResponseBase _response;
			private Guid _sessionId;

			[Test]
			public async void Must_return_session_id_from_response_cookie()
			{
				Assert.That(await _cookieManager.GetSessionIdAsync(_response), Is.EqualTo(_sessionId));
			}
		}
	}
}