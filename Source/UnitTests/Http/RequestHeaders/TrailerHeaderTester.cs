using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class TrailerHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_trailer_header
		{
			[Test]
			[TestCase("")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(TrailerHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_trailer_header
		{
			[Test]
			[TestCase("field", "field")]
			[TestCase("field1, field2, field3", "field1", "field2", "field3")]
			[TestCase("field1,\r\n field2,\r\n field3", "field1", "field2", "field3")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<string> fieldNames = parameters.Where((arg, i) => i > 0).Cast<string>();
				IEnumerable<TrailerHeader> headers = TrailerHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.FieldName), Is.EquivalentTo(fieldNames));
			}
		}
	}
}