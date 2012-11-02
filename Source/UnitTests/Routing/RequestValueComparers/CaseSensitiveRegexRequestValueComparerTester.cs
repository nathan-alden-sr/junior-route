using Junior.Route.Routing.RequestValueComparers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.RequestValueComparers
{
	public static class CaseSensitiveRegexRequestValueComparerTester
	{
		[TestFixture]
		public class When_testing_if_matching_strings_match
		{
			[Test]
			[TestCase("test", "test")]
			[TestCase("test", "test123")]
			public void Must_match(string value, string requestValue)
			{
				Assert.That(CaseSensitiveRegexRequestValueComparer.Instance.Matches(value, requestValue), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_non_matching_strings_match
		{
			[Test]
			[TestCase("^test$", "test1")]
			public void Must_match(string value, string requestValue)
			{
				Assert.That(CaseSensitiveRegexRequestValueComparer.Instance.Matches(value, requestValue), Is.False);
			}
		}
	}
}