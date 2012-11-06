using System;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class RefererUrlAbsolutePathRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new RefererUrlAbsolutePathRestriction("path", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new RefererUrlAbsolutePathRestriction("path", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlAbsolutePathRestriction _restriction1;
			private RefererUrlAbsolutePathRestriction _restriction2;

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
				_restriction1 = new RefererUrlAbsolutePathRestriction("path", CaseInsensitivePlainComparer.Instance);
				_restriction2 = new RefererUrlAbsolutePathRestriction("path", CaseSensitivePlainComparer.Instance);
			}

			private RefererUrlAbsolutePathRestriction _restriction1;
			private RefererUrlAbsolutePathRestriction _restriction2;

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
				_restriction = new RefererUrlAbsolutePathRestriction("path", CaseInsensitivePlainComparer.Instance);
			}

			private RefererUrlAbsolutePathRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.AbsolutePath, Is.EqualTo("path"));
				Assert.That(_restriction.Comparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new RefererUrlAbsolutePathRestriction("/path", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://localhost/path"));
			}

			private RefererUrlAbsolutePathRestriction _restriction;
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
				_restriction = new RefererUrlAbsolutePathRestriction("path1", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://localhost/path2"));
			}

			private RefererUrlAbsolutePathRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.False);
			}
		}
	}
}