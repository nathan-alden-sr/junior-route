using System;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class RefererUrlSchemeRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new RefererUrlSchemeRestriction("scheme", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new RefererUrlSchemeRestriction("scheme", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlSchemeRestriction _restriction1;
			private RefererUrlSchemeRestriction _restriction2;

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
				_restriction1 = new RefererUrlSchemeRestriction("scheme1", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new RefererUrlSchemeRestriction("scheme2", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlSchemeRestriction _restriction1;
			private RefererUrlSchemeRestriction _restriction2;

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
				_restriction = new RefererUrlSchemeRestriction("scheme", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlSchemeRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.Scheme, Is.EqualTo("scheme"));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new RefererUrlSchemeRestriction("http", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://scheme/path"));
			}

			private RefererUrlSchemeRestriction _restriction;
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
				_restriction = new RefererUrlSchemeRestriction("https", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://localhost/path2"));
			}

			private RefererUrlSchemeRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.False);
			}
		}
	}
}