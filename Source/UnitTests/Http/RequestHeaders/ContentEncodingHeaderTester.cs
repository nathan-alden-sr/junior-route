using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class ContentEncodingHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_content_encoding_header
		{
			[Test]
			[TestCase("")]
			[TestCase(",")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(ContentEncodingHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_content_encoding_header
		{
			[Test]
			[TestCase("gzip, compress, identity", "gzip", "compress", "identity")]
			[TestCase("gzip,\r\n compress,\r\n identity", "gzip", "compress", "identity")]
			[TestCase("gzip", "gzip")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<string> contentCodings = parameters.Where((arg, i) => i > 0).Cast<string>();
				IEnumerable<ContentEncodingHeader> headers = ContentEncodingHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.ContentCoding), Is.EquivalentTo(contentCodings));
			}
		}
	}
}