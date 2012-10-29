using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class AcceptLanguageHeaderTester
	{
		[TestFixture]
		public class When_matching_language_with_header_with_effective_qvalue_greater_than_0
		{
			[Test]
			[TestCase("*; q=0.001", "en")]
			[TestCase("en; q=0.001", "en")]
			[TestCase("en-gb; q=0.001", "en")]
			[TestCase("en-gb; q=0.001", "en-gb")]
			public void Must_match(string headerValue, string languageRange)
			{
				AcceptLanguageHeader header = AcceptLanguageHeader.ParseMany(headerValue).Single();

				Assert.That(header.LanguageRangeMatches(languageRange), Is.True);
			}
		}

		[TestFixture]
		public class When_matching_media_type_with_header_with_effective_qvalue_of_0
		{
			[Test]
			[TestCase("*; q=0", "en")]
			[TestCase("en; q=0", "en")]
			[TestCase("en-gb; q=0", "en")]
			[TestCase("en-gb; q=0", "en-gb")]
			public void Must_not_match(string headerValue, string languageRange)
			{
				AcceptLanguageHeader header = AcceptLanguageHeader.ParseMany(headerValue).Single();

				Assert.That(header.LanguageRangeMatches(languageRange), Is.False);
			}
		}

		[TestFixture]
		public class When_parsing_invalid_accept_language_header
		{
			[Test]
			[TestCase("de; q=-1")]
			[TestCase("en; q=1.001")]
			[TestCase("en-gb; q2=1")]
			[TestCase("en-; q=1")]
			[TestCase("-gb; q=1")]
			[TestCase("-; q=1")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(AcceptLanguageHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_accept_language_header
		{
			[Test]
			[TestCase("de; q=1., en; q=0.567, en-gb, fr; q=0.000")]
			public void Must_parse_correctly(string headerValue)
			{
				AcceptLanguageHeader[] headers = AcceptLanguageHeader.ParseMany(headerValue).ToArray();

				Assert.That(headers, Has.Length.EqualTo(4));

				Assert.That(headers[0].Language, Is.EqualTo("de"));
				Assert.That(headers[0].LanguagePrefix, Is.Null);
				Assert.That(headers[0].LanguageRange, Is.EqualTo("de"));
				Assert.That(headers[0].Qvalue, Is.EqualTo(1m));
				Assert.That(headers[0].EffectiveQvalue, Is.EqualTo(1m));

				Assert.That(headers[1].Language, Is.EqualTo("en"));
				Assert.That(headers[1].LanguagePrefix, Is.Null);
				Assert.That(headers[1].LanguageRange, Is.EqualTo("en"));
				Assert.That(headers[1].Qvalue, Is.EqualTo(0.567m));
				Assert.That(headers[1].EffectiveQvalue, Is.EqualTo(0.567m));

				Assert.That(headers[2].Language, Is.EqualTo("gb"));
				Assert.That(headers[2].LanguagePrefix, Is.EqualTo("en"));
				Assert.That(headers[2].LanguageRange, Is.EqualTo("en-gb"));
				Assert.That(headers[2].Qvalue, Is.Null);
				Assert.That(headers[2].EffectiveQvalue, Is.EqualTo(1m));

				Assert.That(headers[3].Language, Is.EqualTo("fr"));
				Assert.That(headers[3].LanguagePrefix, Is.Null);
				Assert.That(headers[3].LanguageRange, Is.EqualTo("fr"));
				Assert.That(headers[3].Qvalue, Is.EqualTo(0m));
				Assert.That(headers[3].EffectiveQvalue, Is.EqualTo(0m));
			}
		}
	}
}