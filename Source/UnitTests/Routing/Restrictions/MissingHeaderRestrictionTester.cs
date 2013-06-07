using System.Collections.Specialized;
using System.Web;

using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Restrictions
{
	public static class MissingHeaderRestrictionTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_restriction1 = new MissingHeaderRestriction("field");
				_restriction2 = new MissingHeaderRestriction("field");
			}

			private MissingHeaderRestriction _restriction1;
			private MissingHeaderRestriction _restriction2;

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
				_restriction1 = new MissingHeaderRestriction("field1");
				_restriction2 = new MissingHeaderRestriction("field2");
			}

			private MissingHeaderRestriction _restriction1;
			private MissingHeaderRestriction _restriction2;

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
				_restriction = new MissingHeaderRestriction("field");
			}

			private MissingHeaderRestriction _restriction;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_restriction.Field, Is.EqualTo("field"));
			}
		}

		[TestFixture]
		public class When_testing_if_matching_restriction_matches_request
		{
			[SetUp]
			public void SetUp()
			{
				_restriction = new MissingHeaderRestriction("field");
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Headers).Return(new NameValueCollection());
			}

			private MissingHeaderRestriction _restriction;
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
				_restriction = new MissingHeaderRestriction("field");
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Headers).Return(new NameValueCollection { { "field", "value" } });
			}

			private MissingHeaderRestriction _restriction;
			private HttpRequestBase _request;

			[Test]
			public async void Must_not_match()
			{
				Assert.That(await _restriction.MatchesRequestAsync(_request), Is.False);
			}
		}
	}
}