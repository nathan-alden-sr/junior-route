using System;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class UrlAuthorityRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new UrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new UrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
			}

			private UrlAuthorityRestriction _restriction1;
			private UrlAuthorityRestriction _restriction2;

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
				_restriction1 = new UrlAuthorityRestriction("authority1", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new UrlAuthorityRestriction("authority2", CaseInsensitivePlainComparer.Instance);
			}

			private UrlAuthorityRestriction _restriction1;
			private UrlAuthorityRestriction _restriction2;

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
				_restriction = new UrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
			}

			private UrlAuthorityRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.Authority, Is.EqualTo("authority"));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new UrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://authority/path"));
			}

			private UrlAuthorityRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public async void Must_match()
			{
				Assert.That(await _restriction.MatchesRequestAsync(_request), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_non_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new UrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://localhost/path2"));
			}

			private UrlAuthorityRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public async void Must_not_match()
			{
				Assert.That(await _restriction.MatchesRequestAsync(_request), Is.False);
			}
		}
	}
}