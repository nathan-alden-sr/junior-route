using System;
using System.Web;

using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class RefererUrlRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				Func<Uri, bool> matchDelegate = uri => true;

				_restriction1 = new RefererUrlRestriction(matchDelegate);
				_restriction2 = new RefererUrlRestriction(matchDelegate);
			}

			private RefererUrlRestriction _restriction1;
			private RefererUrlRestriction _restriction2;

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
				_restriction1 = new RefererUrlRestriction(uri => true);
				_restriction2 = new RefererUrlRestriction(uri => true);
			}

			private RefererUrlRestriction _restriction1;
			private RefererUrlRestriction _restriction2;

			[Test]
			public void Must_not_be_equal()
			{
				Assert.That(_restriction1.Equals(_restriction2), Is.False);
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new RefererUrlRestriction(uri => true);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private RefererUrlRestriction _restriction;
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
				_restriction = new RefererUrlRestriction(uri => false);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private RefererUrlRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public async void Must_not_match()
			{
				Assert.That(await _restriction.MatchesRequestAsync(_request), Is.False);
			}
		}
	}
}