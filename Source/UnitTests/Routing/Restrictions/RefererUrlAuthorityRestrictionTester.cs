using System;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class RefererUrlAuthorityRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new RefererUrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new RefererUrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlAuthorityRestriction _restriction1;
			private RefererUrlAuthorityRestriction _restriction2;

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
				_restriction1 = new RefererUrlAuthorityRestriction("authority1", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new RefererUrlAuthorityRestriction("authority2", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlAuthorityRestriction _restriction1;
			private RefererUrlAuthorityRestriction _restriction2;

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
				_restriction = new RefererUrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlAuthorityRestriction _restriction;

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
				_restriction = new RefererUrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://authority/path"));
			}

			private RefererUrlAuthorityRestriction _restriction;
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
				_restriction = new RefererUrlAuthorityRestriction("authority", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://localhost/path2"));
			}

			private RefererUrlAuthorityRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.False);
			}
		}
	}
}