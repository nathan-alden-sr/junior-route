using System;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting;
using Junior.Route.AutoRouting.FormsAuthentication;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.FormsAuthentication
{
	public static class FormsAuthenticationRequestContextTester
	{
		[TestFixture]
		public class When_getting_user_data_from_expired_ticket
		{
			[SetUp]
			public void SetUp()
			{
				var systemClock = MockRepository.GenerateMock<ISystemClock>();
				var helper = new FormsAuthenticationHelper(systemClock);
				Cookie cookie = helper.GenerateTicket(DateTime.Now.AddYears(-1), @"{ ""P"": ""V"" }");

				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { cookie.GetHttpCookie() });
				_requestContext = MockRepository.GenerateMock<IRequestContext>();
				_requestContext.Stub(arg => arg.Request).Return(_request);
				_authenticationData = new FormsAuthenticationData<dynamic>(_requestContext);
			}

			private IRequestContext _requestContext;
			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;

			[Test]
			public void Must_not_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData();

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
				_requestContext = MockRepository.GenerateMock<IRequestContext>();
				_requestContext.Stub(arg => arg.Request).Return(_request);
				_authenticationData = new FormsAuthenticationData<dynamic>(_requestContext);
			}

			private IRequestContext _requestContext;
			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;

			[Test]
			public void Must_not_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData();

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
				_requestContext = MockRepository.GenerateMock<IRequestContext>();
				_requestContext.Stub(arg => arg.Request).Return(_request);
				_authenticationData = new FormsAuthenticationData<dynamic>(_requestContext);
			}

			private IRequestContext _requestContext;
			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;

			[Test]
			public void Must_not_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData();

				Assert.That((object)userData, Is.Null);
			}
		}

		[TestFixture]
		public class When_getting_user_data_from_valid_ticket
		{
			[SetUp]
			public void SetUp()
			{
				var systemClock = MockRepository.GenerateMock<ISystemClock>();
				var helper = new FormsAuthenticationHelper(systemClock);
				Cookie cookie = helper.GenerateTicket(DateTime.Now.AddYears(1), @"{ ""P"": ""V"" }");

				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { cookie.GetHttpCookie() });
				_requestContext = MockRepository.GenerateMock<IRequestContext>();
				_requestContext.Stub(arg => arg.Request).Return(_request);
				_authenticationData = new FormsAuthenticationData<dynamic>(_requestContext);
			}

			private IRequestContext _requestContext;
			private HttpRequestBase _request;
			private FormsAuthenticationData<dynamic> _authenticationData;

			[Test]
			public void Must_get_user_data()
			{
				dynamic userData = _authenticationData.GetUserData();

				Assert.That((object)userData, Is.Not.Null);
				Assert.That((string)userData.P, Is.EqualTo("V"));
			}
		}
	}
}