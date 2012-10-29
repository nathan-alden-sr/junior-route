using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class AcceptHeaderTester
	{
		[TestFixture]
		public class When_matching_media_type_with_header_with_effective_qvalue_greater_than_0
		{
			[Test]
			[TestCase("*/*; q=0.001", "text/plain")]
			[TestCase("text/*; q=0.001", "text/plain")]
			[TestCase("text/plain; q=0.001", "text/plain")]
			public void Must_match(string headerValue, string mediaType)
			{
				AcceptHeader header = AcceptHeader.ParseMany(headerValue).Single();

				Assert.That(header.MediaTypeMatches(mediaType), Is.True);
			}
		}

		[TestFixture]
		public class When_matching_media_type_with_header_with_effective_qvalue_of_0
		{
			[Test]
			[TestCase("*/*; q=0", "text/plain")]
			[TestCase("text/*; q=0", "text/plain")]
			[TestCase("text/plain; q=0", "text/plain")]
			public void Must_not_match(string headerValue, string mediaType)
			{
				AcceptHeader header = AcceptHeader.ParseMany(headerValue).Single();

				Assert.That(header.MediaTypeMatches(mediaType), Is.False);
			}
		}

		[TestFixture]
		public class When_parsing_invalid_accept_header
		{
			[Test]
			[TestCase("text/")]
			[TestCase("/")]
			[TestCase("/*")]
			[TestCase("text/plain; q=-1; extension1=value1")]
			[TestCase("text/plain; q=1.001; extension1=value1")]
			[TestCase(@"text/plain; q=1.001; extension1=""value1")]
			[TestCase(@"text/plain; q=1.001; extension1=value1""")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(AcceptHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_accept_header
		{
			[Test]
			[TestCase("text/html, text/plain; q=0.9; extension1=value1, application/json; param1=value1; param2=value2; q=1.0; extension1=value1; extension2=value2, text/*, */*; q=1.")]
			public void Must_parse_correctly(string headerValue)
			{
				AcceptHeader[] headers = AcceptHeader.ParseMany(headerValue).ToArray();

				Assert.That(headers, Has.Length.EqualTo(5));

				Assert.That(headers[0].Type, Is.EqualTo("text"));
				Assert.That(headers[0].Subtype, Is.EqualTo("html"));
				Assert.That(headers[0].Qvalue, Is.Null);
				Assert.That(headers[0].EffectiveQvalue, Is.EqualTo(1m));
				Assert.That(headers[0].Extensions, Is.Empty);

				Assert.That(headers[1].Type, Is.EqualTo("text"));
				Assert.That(headers[1].Subtype, Is.EqualTo("plain"));
				Assert.That(headers[1].Qvalue, Is.EqualTo(0.9m));
				Assert.That(headers[1].EffectiveQvalue, Is.EqualTo(0.9m));
				Assert.That(headers[1].Extensions.Count(), Is.EqualTo(1));
				Assert.That(headers[1].Extensions.ElementAt(0).Name, Is.EqualTo("extension1"));
				Assert.That(headers[1].Extensions.ElementAt(0).Value, Is.EqualTo("value1"));

				Assert.That(headers[2].Type, Is.EqualTo("application"));
				Assert.That(headers[2].Subtype, Is.EqualTo("json"));
				Assert.That(headers[2].Parameters.Count(), Is.EqualTo(2));
				Assert.That(headers[2].Parameters.ElementAt(0).Name, Is.EqualTo("param1"));
				Assert.That(headers[2].Parameters.ElementAt(0).Value, Is.EqualTo("value1"));
				Assert.That(headers[2].Parameters.ElementAt(1).Name, Is.EqualTo("param2"));
				Assert.That(headers[2].Parameters.ElementAt(1).Value, Is.EqualTo("value2"));
				Assert.That(headers[2].Qvalue, Is.EqualTo(1m));
				Assert.That(headers[2].EffectiveQvalue, Is.EqualTo(1m));
				Assert.That(headers[2].Extensions.Count(), Is.EqualTo(2));
				Assert.That(headers[2].Extensions.ElementAt(0).Name, Is.EqualTo("extension1"));
				Assert.That(headers[2].Extensions.ElementAt(0).Value, Is.EqualTo("value1"));
				Assert.That(headers[2].Extensions.ElementAt(1).Name, Is.EqualTo("extension2"));
				Assert.That(headers[2].Extensions.ElementAt(1).Value, Is.EqualTo("value2"));

				Assert.That(headers[3].Type, Is.EqualTo("text"));
				Assert.That(headers[3].Subtype, Is.EqualTo("*"));
				Assert.That(headers[3].Qvalue, Is.Null);
				Assert.That(headers[3].EffectiveQvalue, Is.EqualTo(1m));
				Assert.That(headers[3].Extensions, Is.Empty);

				Assert.That(headers[4].Type, Is.EqualTo("*"));
				Assert.That(headers[4].Subtype, Is.EqualTo("*"));
				Assert.That(headers[4].Qvalue, Is.EqualTo(1m));
				Assert.That(headers[4].EffectiveQvalue, Is.EqualTo(1m));
				Assert.That(headers[4].Extensions, Is.Empty);
			}
		}
	}
}