using System;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class UrlQueryStringRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value", CaseSensitiveRegexComparer.Instance);
				_restriction2 = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value", CaseSensitiveRegexComparer.Instance);
			}

			private UrlQueryStringRestriction _restriction1;
			private UrlQueryStringRestriction _restriction2;

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
				_restriction1 = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value1", CaseSensitiveRegexComparer.Instance);
				_restriction2 = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value2", CaseSensitiveRegexComparer.Instance);
			}

			private UrlQueryStringRestriction _restriction1;
			private UrlQueryStringRestriction _restriction2;

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
				var restriction = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value", CaseSensitiveRegexComparer.Instance, optional);

				Assert.That(restriction.Field, Is.EqualTo("field"));
				Assert.That(restriction.FieldComparer, Is.SameAs(CaseInsensitivePlainComparer.Instance));
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
				_restriction = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value", CaseSensitiveRegexComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://localhost/path?field=value"));
			}

			private UrlQueryStringRestriction _restriction;
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
				_restriction = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value", CaseSensitiveRegexComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://localhost/path?field=v"));
			}

			private UrlQueryStringRestriction _restriction;
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
				_restriction = new UrlQueryStringRestriction("field", CaseInsensitivePlainComparer.Instance, "value", CaseSensitiveRegexComparer.Instance, true);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://localhost/path?field1=v"));
			}

			private UrlQueryStringRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public async void Must_match()
			{
				Assert.That(await _restriction.MatchesRequestAsync(_request), Is.True);
			}
		}
	}
}