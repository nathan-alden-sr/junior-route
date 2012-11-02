using System;
using System.Web;

using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class RefererUrlHostTypeRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new RefererUrlHostTypeRestriction(UriHostNameType.Basic);
				_restriction2 = new RefererUrlHostTypeRestriction(UriHostNameType.Basic);
			}

			private RefererUrlHostTypeRestriction _restriction1;
			private RefererUrlHostTypeRestriction _restriction2;

			[Test]
			public void Must_be_equal()
			{
				Assert.That(_restriction1.Equals(_restriction2), Is.True);
			}
		}

		[TestFixture]
		public class When_comparing_inequal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new RefererUrlHostTypeRestriction(UriHostNameType.Basic);
				_restriction2 = new RefererUrlHostTypeRestriction(UriHostNameType.Dns);
			}

			private RefererUrlHostTypeRestriction _restriction1;
			private RefererUrlHostTypeRestriction _restriction2;

			[Test]
			public void Must_not_be_equal()
			{
				Assert.That(_restriction1.Equals(_restriction2), Is.False);
			}
		}

		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new RefererUrlHostTypeRestriction(UriHostNameType.IPv4);
			}

			private RefererUrlHostTypeRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.Type, Is.EqualTo(UriHostNameType.IPv4));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new RefererUrlHostTypeRestriction(UriHostNameType.IPv4);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://127.0.0.1"));
			}

			private RefererUrlHostTypeRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_non_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new RefererUrlHostTypeRestriction(UriHostNameType.IPv6);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://127.0.0.1"));
			}

			private RefererUrlHostTypeRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.False);
			}
		}
	}
}