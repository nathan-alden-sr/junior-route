using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class CookieRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new CookieRestriction("name1", CaseSensitivePlainRequestValueComparer.Instance, "value1", CaseSensitiveRegexRequestValueComparer.Instance);
				_restriction2 = new CookieRestriction("name1", CaseSensitivePlainRequestValueComparer.Instance, "value1", CaseSensitiveRegexRequestValueComparer.Instance);
			}

			private CookieRestriction _restriction1;
			private CookieRestriction _restriction2;

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
				_restriction1 = new CookieRestriction("name1", CaseSensitivePlainRequestValueComparer.Instance, "value1", CaseSensitiveRegexRequestValueComparer.Instance);
				_restriction2 = new CookieRestriction("name2", CaseSensitivePlainRequestValueComparer.Instance, "value2", CaseSensitiveRegexRequestValueComparer.Instance);
			}

			private CookieRestriction _restriction1;
			private CookieRestriction _restriction2;

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
				_restriction = new CookieRestriction("name", CaseSensitivePlainRequestValueComparer.Instance, "value", CaseSensitiveRegexRequestValueComparer.Instance);
			}

			private CookieRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.Name, Is.EqualTo("name"));
				Assert.That(_restriction.NameComparer, Is.SameAs(CaseSensitivePlainRequestValueComparer.Instance));
				Assert.That(_restriction.Value, Is.EqualTo("value"));
				Assert.That(_restriction.ValueComparer, Is.SameAs(CaseSensitiveRegexRequestValueComparer.Instance));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new CookieRestriction("name", CaseSensitivePlainRequestValueComparer.Instance, "value", CaseInsensitivePlainRequestValueComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "value") });
			}

			private CookieRestriction _restriction;
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
				_restriction = new CookieRestriction("name", CaseSensitivePlainRequestValueComparer.Instance, "value", CaseInsensitivePlainRequestValueComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "value1") });
			}

			private CookieRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.False);
			}
		}
	}
}