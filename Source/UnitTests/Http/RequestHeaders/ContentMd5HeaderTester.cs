using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class ContentMd5HeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_content_md5_header
		{
			[Test]
			[TestCase("")]
			[TestCase("A")]
			[TestCase("dGVzdA==")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(ContentMd5Header.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_content_md5_header
		{
			[Test]
			[TestCase("MTIzNDU2Nzg5MDEyMzQ1Ng==", "1234567890123456")]
			public void Must_parse_correctly(string headerValue, string hash)
			{
				ContentMd5Header header = ContentMd5Header.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.Md5Digest, Is.EquivalentTo(hash.Select(arg => (byte)arg)));
			}
		}
	}
}