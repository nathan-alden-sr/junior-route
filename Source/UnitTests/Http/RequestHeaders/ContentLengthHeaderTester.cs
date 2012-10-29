using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class ContentLengthHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_content_length_header
		{
			[Test]
			[TestCase("")]
			[TestCase("-1")]
			[TestCase("a")]
			[TestCase("1,000")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(ContentLengthHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_content_length_header
		{
			[Test]
			[TestCase("0", 0ul)]
			[TestCase("18446744073709551615", 18446744073709551615ul)]
			public void Must_parse_correctly(string headerValue, ulong expectedContentLength)
			{
				ContentLengthHeader header = ContentLengthHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.ContentLength, Is.EqualTo(expectedContentLength));
			}
		}
	}
}