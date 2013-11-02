using Junior.Route.Routing.RequestValueComparers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.RequestValueComparers
{
	public static class OptionalCaseInsensitivePlainRequestTester
	{
		[TestFixture]
		public class When_testing_if_null_value_matches_request_value
		{
			[Test]
			[TestCase(null, null)]
			[TestCase(null, "value")]
			public void Must_match(string value, string requestValue)
			{
				Assert.That(OptionalCaseInsensitivePlainComparer.Instance.Matches(value, requestValue), Is.True);
			}
		}
	}
}