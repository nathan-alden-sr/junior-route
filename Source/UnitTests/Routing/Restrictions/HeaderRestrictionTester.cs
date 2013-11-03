using System.Collections.Specialized;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class HeaderRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new HeaderRestriction("field1", "value1", CaseSensitiveRegexComparer.Instance);
				_restriction2 = new HeaderRestriction("field1", "value1", CaseSensitiveRegexComparer.Instance);
			}

			private HeaderRestriction _restriction1;
			private HeaderRestriction _restriction2;

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
				_restriction1 = new HeaderRestriction("field1", "value1", CaseSensitiveRegexComparer.Instance);
				_restriction2 = new HeaderRestriction("field2", "value2", CaseSensitiveRegexComparer.Instance);
			}

			private HeaderRestriction _restriction1;
			private HeaderRestriction _restriction2;

			[Test]
			public void Must_not_be_equal()
			{
				Assert.That(_restriction1.Equals(_restriction2), Is.False);
			}
		}

		[TestFixture]
		public class When_creating_instance
		{
			[Test]
			[TestCase(true)]
			[TestCase(false)]
			public void Must_set_properties(bool optional)
			{
				var restriction = new HeaderRestriction("field", "value", CaseSensitiveRegexComparer.Instance, optional);

				Assert.That(restriction.Field, Is.EqualTo("field"));
				Assert.That(restriction.Value, Is.EqualTo("value"));
				Assert.That(restriction.ValueComparer, Is.SameAs(CaseSensitiveRegexComparer.Instance));
				Assert.That(restriction.Optional, Is.EqualTo(optional));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new HeaderRestriction("field", "value", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Headers).Return(new NameValueCollection { { "field", "value" } });
			}

			private HeaderRestriction _restriction;
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
				_restriction = new HeaderRestriction("field", "value", CaseInsensitivePlainComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Headers).Return(new NameValueCollection { { "field", "value1" } });
			}

			private HeaderRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public async void Must_not_match()
			{
				Assert.That(await _restriction.MatchesRequestAsync(_request), Is.False);
			}
		}

		[TestFixture]
		public class When_testing_if_optional_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new HeaderRestriction("field", "value", CaseInsensitivePlainComparer.Instance, true);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Headers).Return(new NameValueCollection { { "field1", "value1" } });
			}

			private HeaderRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public async void Must_match()
			{
				Assert.That(await _restriction.MatchesRequestAsync(_request), Is.True);
			}
		}
	}
}