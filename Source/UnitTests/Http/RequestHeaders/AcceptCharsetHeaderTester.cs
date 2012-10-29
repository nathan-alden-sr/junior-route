using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class AcceptCharsetHeaderTester
	{
		[TestFixture]
		public class When_matching_charset_with_header_with_effective_qvalue_greater_than_0
		{
			[Test]
			[TestCase("*; q=0.001", "iso-8859-5")]
			[TestCase("iso-8859-5; q=0.001", "iso-8859-5")]
			public void Must_match(string headerValue, string charset)
			{
				AcceptCharsetHeader header = AcceptCharsetHeader.ParseMany(headerValue).Single();

				Assert.That(header.CharsetMatches(charset), Is.True);
			}
		}

		[TestFixture]
		public class When_matching_media_type_with_header_with_effective_qvalue_of_0
		{
			[Test]
			[TestCase("*; q=0", "iso-8859-5")]
			[TestCase("iso-8859-5; q=0", "iso-8859-5")]
			public void Must_not_match(string headerValue, string charset)
			{
				AcceptCharsetHeader header = AcceptCharsetHeader.ParseMany(headerValue).Single();

				Assert.That(header.CharsetMatches(charset), Is.False);
			}
		}

		[TestFixture]
		public class When_parsing_invalid_accept_charset_header
		{
			[Test]
			[TestCase("utf-8; q=-1")]
			[TestCase("utf-16; q=1.001")]
			[TestCase("utf-16; q2=1")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(AcceptCharsetHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_accept_charset_header
		{
			[Test]
			[TestCase("utf-8; q=1., utf-16")]
			public void Must_parse_correctly(string headerValue)
			{
				AcceptCharsetHeader[] headers = AcceptCharsetHeader.ParseMany(headerValue).ToArray();

				Assert.That(headers, Has.Length.EqualTo(2));

				Assert.That(headers[0].Charset, Is.EqualTo("utf-8"));
				Assert.That(headers[0].Qvalue, Is.EqualTo(1m));
				Assert.That(headers[0].EffectiveQvalue, Is.EqualTo(1m));

				Assert.That(headers[1].Charset, Is.EqualTo("utf-16"));
				Assert.That(headers[1].Qvalue, Is.Null);
				Assert.That(headers[1].EffectiveQvalue, Is.EqualTo(1m));
			}
		}
	}
}