using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class AcceptEncodingHeaderTester
	{
		[TestFixture]
		public class When_matching_content_coding_with_header_with_effective_qvalue_greater_than_0
		{
			[Test]
			[TestCase("*; q=0.001", "compress")]
			[TestCase("compress; q=0.001", "compress")]
			public void Must_match(string headerValue, string contentCoding)
			{
				AcceptEncodingHeader header = AcceptEncodingHeader.ParseMany(headerValue).Single();

				Assert.That(header.ContentCodingMatches(contentCoding), Is.True);
			}
		}

		[TestFixture]
		public class When_matching_content_coding_with_header_with_effective_qvalue_of_0
		{
			[Test]
			[TestCase("*; q=0", "compress")]
			[TestCase("compress; q=0", "compress")]
			public void Must_not_match(string headerValue, string contentCoding)
			{
				AcceptEncodingHeader header = AcceptEncodingHeader.ParseMany(headerValue).Single();

				Assert.That(header.ContentCodingMatches(contentCoding), Is.False);
			}
		}

		[TestFixture]
		public class When_parsing_invalid_accept_encoding_header
		{
			[Test]
			[TestCase("compress; q=-1")]
			[TestCase("gzip; q=1.001")]
			[TestCase("identity; q2=1")]
			[TestCase("*; q2=1")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(AcceptEncodingHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_accept_encoding_header
		{
			[Test]
			[TestCase("compress; q=1., gzip; q=0, identity; q=0.567, *")]
			public void Must_parse_correctly(string headerValue)
			{
				AcceptEncodingHeader[] headers = AcceptEncodingHeader.ParseMany(headerValue).ToArray();

				Assert.That(headers, Has.Length.EqualTo(4));

				Assert.That(headers[0].ContentCoding, Is.EqualTo("compress"));
				Assert.That(headers[0].Qvalue, Is.EqualTo(1m));
				Assert.That(headers[0].EffectiveQvalue, Is.EqualTo(1m));

				Assert.That(headers[1].ContentCoding, Is.EqualTo("gzip"));
				Assert.That(headers[1].Qvalue, Is.EqualTo(0m));
				Assert.That(headers[1].EffectiveQvalue, Is.EqualTo(0m));

				Assert.That(headers[2].ContentCoding, Is.EqualTo("identity"));
				Assert.That(headers[2].Qvalue, Is.EqualTo(0.567m));
				Assert.That(headers[2].EffectiveQvalue, Is.EqualTo(0.567m));

				Assert.That(headers[3].ContentCoding, Is.EqualTo("*"));
				Assert.That(headers[3].Qvalue, Is.Null);
				Assert.That(headers[3].EffectiveQvalue, Is.EqualTo(1m));
			}
		}
	}
}