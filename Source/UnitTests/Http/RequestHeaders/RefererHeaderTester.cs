using System;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class RefererHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_referer_header
		{
			[Test]
			[TestCase("http:// bad")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(RefererHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_referer_header
		{
			[Test]
			[TestCase("http://local")]
			[TestCase("http://test.org")]
			[TestCase("//test.org")]
			[TestCase("test")]
			public void Must_parse_correctly(string headerValue)
			{
				RefererHeader header = RefererHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.Url, Is.EqualTo(new Uri(headerValue, UriKind.RelativeOrAbsolute)));
			}
		}
	}
}