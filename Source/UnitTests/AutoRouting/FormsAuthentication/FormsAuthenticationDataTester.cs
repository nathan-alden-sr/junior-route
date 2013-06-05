using System;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.FormsAuthentication;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.FormsAuthentication
{
	public static class FormsAuthenticationDataTester
	{
		[TestFixture]
		public class When_getting_user_data_from_expired_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				var helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				Cookie cookie = helper.GenerateTicket(DateTime.Now.AddYears(-1), @"{ ""P"": ""V"" }");

				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { cookie.GetHttpCookie() });
				_authenticationData = new FormsAuthenticationData<dynamic>(_configuration);
			}

			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;
			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;

			[Test]
			public void Must_not_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData(_request);

				Assert.That((object)userData, Is.Null);
			}
		}

		[TestFixture]
		public class When_getting_user_data_from_invalid_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie(".juniorauth", "invalid") });
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_authenticationData = new FormsAuthenticationData<dynamic>(_configuration);
			}

			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;
			private IFormsAuthenticationConfiguration _configuration;

			[Test]
			public void Must_not_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData(_request);

				Assert.That((object)userData, Is.Null);
			}
		}

		[TestFixture]
		public class When_getting_user_data_from_missing_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_authenticationData = new FormsAuthenticationData<dynamic>(_configuration);
			}

			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;
			private IFormsAuthenticationConfiguration _configuration;

			[Test]
			public void Must_not_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData(_request);

				Assert.That((object)userData, Is.Null);
			}
		}

		[TestFixture]
		public class When_getting_user_data_from_valid_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_configuration = MockRepository.GenerateMock<IFormsAuthenticationConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.CookiePath).Return("/");
				var helper = new FormsAuthenticationHelper(_configuration, _systemClock);
				Cookie cookie = helper.GenerateTicket(DateTime.Now.AddYears(1), @"{ ""P"": ""V"" }");

				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { cookie.GetHttpCookie() });
				_authenticationData = new FormsAuthenticationData<dynamic>(_configuration);
			}

			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;
			private ISystemClock _systemClock;
			private IFormsAuthenticationConfiguration _configuration;

			[Test]
			public void Must_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData(_request);

				Assert.That((object)userData, Is.Not.Null);
				Assert.That((string)userData.P, Is.EqualTo("V"));
			}
		}
	}
}