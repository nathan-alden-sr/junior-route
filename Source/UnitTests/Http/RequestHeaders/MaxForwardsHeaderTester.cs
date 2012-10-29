using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class MaxForwardsHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_max_forwards_header
		{
			[Test]
			[TestCase("")]
			[TestCase("-")]
			[TestCase("a")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(MaxForwardsHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_max_forwards_header
		{
			[Test]
			[TestCase("0", 0)]
			[TestCase("10000", 10000)]
			[TestCase("2147483647", 2147483647)]
			public void Must_parse_correctly(string headerValue, int expectedMaxForwards)
			{
				MaxForwardsHeader header = MaxForwardsHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.MaxForwards, Is.EqualTo(expectedMaxForwards));
			}
		}
	}
}