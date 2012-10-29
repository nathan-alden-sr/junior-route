using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class VaryHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_vary_header
		{
			[Test]
			[TestCase("")]
			[TestCase(",")]
			[TestCase("/")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(VaryHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_vary_header
		{
			[Test]
			[TestCase("*", "*")]
			[TestCase(@"tag1, tag2, tag3", "tag1", "tag2", "tag3")]
			[TestCase("tag1,\r\n tag2,\r\n tag3", "tag1", "tag2", "tag3")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<string> fieldNames = parameters.Where((arg, i) => i > 0).Cast<string>();
				IEnumerable<VaryHeader> headers = VaryHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.FieldName), Is.EquivalentTo(fieldNames));
			}
		}
	}
}