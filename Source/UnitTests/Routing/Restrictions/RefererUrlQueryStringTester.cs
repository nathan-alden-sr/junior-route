using System;
using System.Web;

using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class RefererUrlQueryStringRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new RefererUrlQueryStringRestriction("field", CaseInsensitivePlainRequestValueComparer.Instance, "value", CaseSensitiveRegexRequestValueComparer.Instance);
				_restriction2 = new RefererUrlQueryStringRestriction("field", CaseInsensitivePlainRequestValueComparer.Instance, "value", CaseSensitiveRegexRequestValueComparer.Instance);
			}

			private RefererUrlQueryStringRestriction _restriction1;
			private RefererUrlQueryStringRestriction _restriction2;

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
				_restriction1 = new RefererUrlQueryStringRestriction("field", CaseInsensitivePlainRequestValueComparer.Instance, "value1", CaseSensitiveRegexRequestValueComparer.Instance);
				_restriction2 = new RefererUrlQueryStringRestriction("field", CaseInsensitivePlainRequestValueComparer.Instance, "value2", CaseSensitiveRegexRequestValueComparer.Instance);
			}

			private RefererUrlQueryStringRestriction _restriction1;
			private RefererUrlQueryStringRestriction _restriction2;

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
				_restriction = new RefererUrlQueryStringRestriction("field", CaseInsensitivePlainRequestValueComparer.Instance, "value", CaseSensitiveRegexRequestValueComparer.Instance);
			}

			private RefererUrlQueryStringRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.Field, Is.EqualTo("field"));
				Assert.That(_restriction.FieldComparer, Is.SameAs(CaseInsensitivePlainRequestValueComparer.Instance));
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
				_restriction = new RefererUrlQueryStringRestriction("field", CaseInsensitivePlainRequestValueComparer.Instance, "value", CaseSensitiveRegexRequestValueComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://localhost/path?field=value"));
			}

			private RefererUrlQueryStringRestriction _restriction;
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
				_restriction = new RefererUrlQueryStringRestriction("field", CaseInsensitivePlainRequestValueComparer.Instance, "value", CaseSensitiveRegexRequestValueComparer.Instance);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.UrlReferrer).Return(new Uri("http://localhost/path?field=v"));
			}

			private RefererUrlQueryStringRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_restriction.MatchesRequest(_request), Is.False);
			}
		}
	}
}