using System;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.FormsAuthentication;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.FormsAuthentication
{
	public static class FormsAuthenticationHelperTester
	{
		[TestFixture]
		public class When_determining_if_invalid_ticket_is_invalid
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "invalid") });
			}

			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;
			private FormsAuthenticationHelper _helper;
			private HttpRequestBase _request;

			[Test]
			public void Must_be_invalid()
			{
				Assert.That(_helper.IsTicketValid(_request), Is.False);
			}
		}

		[TestFixture]
		public class When_determining_if_non_existent_ticket_is_invalid
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
			}

			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;
			private FormsAuthenticationHelper _helper;
			private HttpRequestBase _request;

			[Test]
			public void Must_be_invalid()
			{
				Assert.That(_helper.IsTicketValid(_request), Is.False);
			}
		}

		[TestFixture]
		public class When_determining_if_valid_ticket_is_valid
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.CookiePath).Return("/path");
				_systemClock = MockRepository.GenerateMock<ISystemClock>();

				DateTime now = DateTime.Now;

				_systemClock.Stub(arg => arg.LocalDateTime).Return(now);
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { _helper.GenerateTicket(now.AddDays(1)).GetHttpCookie() });
			}

			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;
			private FormsAuthenticationHelper _helper;
			private HttpRequestBase _request;

			[Test]
			public void Must_be_valid()
			{
				Assert.That(_helper.IsTicketValid(_request), Is.True);
			}
		}

		[TestFixture]
		public class When_generating_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_configuration.Stub(arg => arg.CookieDomain).Return("domain.com");
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.CookiePath).Return("/path");
				_configuration.Stub(arg => arg.Persistent).Return(true);
				_configuration.Stub(arg => arg.RequireSsl).Return(true);
				_configuration.Stub(arg => arg.SlidingExpiration).Return(true);
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_systemClock.Stub(arg => arg.LocalDateTime).Return(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Local));
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_cookie = _helper.GenerateTicket(_systemClock.LocalDateTime.AddYears(1), @"{ ""P"": ""V"" }");
			}

			private ISystemClock _systemClock;
			private FormsAuthenticationHelper _helper;
			private Cookie _cookie;
			private IFormsAuthenticationConfiguration _configuration;

			[Test]
			public void Must_set_cookie_properties()
			{
				Assert.That(_cookie.Domain, Is.EqualTo("domain.com"));
				Assert.That(_cookie.Expires, Is.EqualTo(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Local)));
				Assert.That(_cookie.HttpOnly, Is.True);
				Assert.That(_cookie.Name, Is.EqualTo("name"));
				Assert.That(_cookie.Path, Is.EqualTo("/path"));
				Assert.That(_cookie.Secure, Is.True);
				Assert.That(_cookie.Shareable, Is.False);
				Assert.That(_cookie.Value, Is.Not.Null.Or.Empty);
			}
		}

		[TestFixture]
		public class When_removing_non_existent_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_response.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
			}

			private ISystemClock _systemClock;
			private FormsAuthenticationHelper _helper;
			private IFormsAuthenticationConfiguration _configuration;
			private HttpResponseBase _response;

			[Test]
			public void Must_not_throw_exception()
			{
				Assert.DoesNotThrow(() => _helper.RemoveTicket(_response));
			}
		}

		[TestFixture]
		public class When_removing_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_configuration.Stub(arg => arg.CookieDomain).Return("domain.com");
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.CookiePath).Return("/path");
				_configuration.Stub(arg => arg.Persistent).Return(true);
				_configuration.Stub(arg => arg.RequireSsl).Return(true);
				_configuration.Stub(arg => arg.SlidingExpiration).Return(true);
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_systemClock.Stub(arg => arg.LocalDateTime).Return(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Local));
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_cookie = _helper.GenerateTicket(_systemClock.LocalDateTime.AddYears(1), @"{ ""P"": ""V"" }");
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_response.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { _cookie.GetHttpCookie() });
			}

			private ISystemClock _systemClock;
			private FormsAuthenticationHelper _helper;
			private Cookie _cookie;
			private IFormsAuthenticationConfiguration _configuration;
			private HttpResponseBase _response;

			[Test]
			public void Must_replace_cookie_with_expired_cookie()
			{
				_helper.RemoveTicket(_response);

				Assert.That(_response.Cookies.Count, Is.EqualTo(1));
				Assert.That(_response.Cookies[0].Expires, Is.EqualTo(new DateTime(2000, 1, 1)));
			}
		}

		[TestFixture]
		public class When_renewing_invalid_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "invalid") });
			}

			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;
			private FormsAuthenticationHelper _helper;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_return_a_cookie()
			{
				Assert.That(_helper.RenewTicket(_request), Is.Null);
			}

			[Test]
			public void Must_not_throw_exception()
			{
				Assert.DoesNotThrow(() => Assert.That(_helper.RenewTicket(_request), Is.Null));
			}
		}

		[TestFixture]
		public class When_renewing_non_existent_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
			}

			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;
			private FormsAuthenticationHelper _helper;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_return_a_cookie()
			{
				Assert.DoesNotThrow(() => Assert.That(_helper.RenewTicket(_request), Is.Null));
			}
		}

		[TestFixture]
		public class When_renewing_valid_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.CookiePath).Return("/path");
				_systemClock = MockRepository.GenerateMock<ISystemClock>();

				DateTime now = DateTime.Now;

				_systemClock.Stub(arg => arg.LocalDateTime).Return(now);
				_helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { _helper.GenerateTicket(now.AddDays(1)).GetHttpCookie() });
			}

			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;
			private FormsAuthenticationHelper _helper;
			private HttpRequestBase _request;

			[Test]
			public void Must_return_a_cookie()
			{
				Assert.That(_helper.RenewTicket(_request), Is.Not.Null);
			}
		}
	}
}