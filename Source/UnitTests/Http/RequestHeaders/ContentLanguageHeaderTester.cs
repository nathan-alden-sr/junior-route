using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class ContentLanguageHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_content_language_header
		{
			[Test]
			[TestCase("")]
			[TestCase(",")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(ContentLanguageHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_content_language_header
		{
			[Test]
			[TestCase("en, da, de", "en", "da", "de")]
			[TestCase("en,\r\n da,\r\n de", "en", "da", "de")]
			[TestCase("en-gb, en-gb-a, en-gb-b, x-pig-latin", "en-gb", "en-gb-a", "en-gb-b", "x-pig-latin")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<string> languageTags = parameters.Where((arg, i) => i > 0).Cast<string>();
				IEnumerable<ContentLanguageHeader> headers = ContentLanguageHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.LanguageTag), Is.EquivalentTo(languageTags));
			}
		}
	}
}