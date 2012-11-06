using Junior.Route.Routing.RequestValueComparers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.RequestValueComparers
{
	public static class CaseInsensitiveRegexRequestTester
	{
		[TestFixture]
		public class When_testing_if_matching_strings_match
		{
			[Test]
			[TestCase("test", "TEST")]
			[TestCase("test", "test")]
			[TestCase("test", "TEST123")]
			[TestCase("test", "test123")]
			public void Must_match(string value, string requestValue)
			{
				Assert.That(CaseInsensitiveRegexComparer.Instance.Matches(value, requestValue), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_non_matching_strings_match
		{
			[Test]
			[TestCase("^test$", "TEST1")]
			[TestCase("^test$", "test1")]
			public void Must_match(string value, string requestValue)
			{
				Assert.That(CaseInsensitiveRegexComparer.Instance.Matches(value, requestValue), Is.False);
			}
		}
	}
}