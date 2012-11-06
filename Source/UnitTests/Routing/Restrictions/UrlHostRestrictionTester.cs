using System;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class UrlHostRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance);
			}

			private UrlHostRestriction _restriction1;
			private UrlHostRestriction _restriction2;

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
				_restriction1 = new UrlHostRestriction("host1", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new UrlHostRestriction("host2", CaseInsensitivePlainComparer.Instance);
			}

			private UrlHostRestriction _restriction1;
			private UrlHostRestriction _restriction2;

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
				_restriction = new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance);
			}

			private UrlHostRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.Host, Is.EqualTo("host"));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://host"));
			}

			private UrlHostRestriction _restriction;
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
				_restriction = new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://localhost"));
			}

			private UrlHostRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.False);
			}
		}
	}
}