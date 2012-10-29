using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class FromHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_from_header
		{
			[Test]
			[TestCase("aa.com")]
			[TestCase("a@hp .com")]
			[TestCase("@hp.com")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(FromHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_from_header
		{
			[Test]
			[TestCase("test@domain.local")]
			[TestCase("Name <test@domain.local>")]
			[TestCase("person@domain.org")]
			public void Must_parse_correctly(string headerValue)
			{
				FromHeader header = FromHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.Mailbox, Is.EqualTo(headerValue));
			}
		}
	}
}