using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class TeHeaderTester
	{
		[TestFixture]
		public class When_matching_t_coding_with_header_with_effective_qvalue_greater_than_0
		{
			[Test]
			[TestCase("deflate; q=0.001", "deflate")]
			public void Must_match(string headerValue, string tCoding)
			{
				TeHeader header = TeHeader.ParseMany(headerValue).Single();

				Assert.That(header.TCodingMatches(tCoding), Is.True);
			}
		}

		[TestFixture]
		public class When_matching_t_coding_with_header_with_effective_qvalue_of_0
		{
			[Test]
			[TestCase("deflate; q=0", "deflate")]
			public void Must_not_match(string headerValue, string tCoding)
			{
				TeHeader header = TeHeader.ParseMany(headerValue).Single();

				Assert.That(header.TCodingMatches(tCoding), Is.False);
			}
		}

		[TestFixture]
		public class When_parsing_invalid_te_header
		{
			[Test]
			[TestCase(@"")]
			[TestCase(@",")]
			[TestCase(@"trailers; a; b")]
			[TestCase(@"b=1")]
			[TestCase(@"ext; q=1.1")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(TeHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_te_header
		{
			[Test]
			[TestCase(@"trailers; b=1; c=""1"", trailers, deflate; q=0.5")]
			public void Must_parse_correctly(string headerValue)
			{
				TeHeader[] headers = TeHeader.ParseMany(headerValue).ToArray();

				Assert.That(headers, Has.Length.EqualTo(3));

				Assert.That(headers[0].TCoding, Is.EqualTo("trailers"));
				Assert.That(headers[0].Parameters.ElementAt(0).Name, Is.EqualTo("b"));
				Assert.That(headers[0].Parameters.ElementAt(0).Value, Is.EqualTo("1"));
				Assert.That(headers[0].Parameters.ElementAt(1).Name, Is.EqualTo("c"));
				Assert.That(headers[0].Parameters.ElementAt(1).Value, Is.EqualTo("1"));
				Assert.That(headers[0].Qvalue, Is.Null);
				Assert.That(headers[0].EffectiveQvalue, Is.EqualTo(1m));

				Assert.That(headers[1].TCoding, Is.EqualTo("trailers"));
				Assert.That(headers[1].Parameters, Is.Empty);
				Assert.That(headers[1].Qvalue, Is.Null);
				Assert.That(headers[1].EffectiveQvalue, Is.EqualTo(1m));

				Assert.That(headers[2].TCoding, Is.EqualTo("deflate"));
				Assert.That(headers[2].Parameters, Is.Empty);
				Assert.That(headers[2].Qvalue, Is.EqualTo(0.5m));
				Assert.That(headers[2].EffectiveQvalue, Is.EqualTo(0.5m));
			}
		}
	}
}