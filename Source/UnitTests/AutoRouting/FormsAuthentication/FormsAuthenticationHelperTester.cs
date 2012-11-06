using System;

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
		public class When_generating_ticket
		{
			[SetUp]
			public void SetUp()
			{
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_systemClock.Stub(arg => arg.LocalDateTime).Return(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Local));
				_helper = new FormsAuthenticationHelper(_systemClock);
				_cookie = _helper.GenerateTicket(_systemClock.LocalDateTime.AddYears(1), @"{ ""P"": ""V"" }", persistent:true);
			}

			private ISystemClock _systemClock;
			private FormsAuthenticationHelper _helper;
			private Cookie _cookie;

			[Test]
			public void Must_set_cookie_properties()
			{
				Assert.That(_cookie.Domain, Is.Null);
				Assert.That(_cookie.Expires, Is.EqualTo(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Local)));
				Assert.That(_cookie.HttpOnly, Is.True);
				Assert.That(_cookie.Name, Is.EqualTo(".juniorauth"));
				Assert.That(_cookie.Path, Is.EqualTo("/"));
				Assert.That(_cookie.Secure, Is.False);
				Assert.That(_cookie.Shareable, Is.False);
				Assert.That(_cookie.Value, Is.Not.Null.Or.Empty);
			}
		}
	}
}