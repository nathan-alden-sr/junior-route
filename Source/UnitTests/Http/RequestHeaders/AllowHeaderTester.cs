using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class AllowHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_allow_header
		{
			[Test]
			[TestCase(",")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(AllowHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_allow_header
		{
			[Test]
			[TestCase("")]
			[TestCase("GET,HEAD,POST,PUT,DELETE,TRACE,CONNECT,Custom", "GET", "HEAD", "POST", "PUT", "DELETE", "TRACE", "CONNECT", "Custom")]
			[TestCase("GET,\r\n HEAD,\r\n POST,\r\n PUT,\r\n DELETE,\r\n TRACE,\r\n CONNECT,\r\n Custom", "GET", "HEAD", "POST", "PUT", "DELETE", "TRACE", "CONNECT", "Custom")]
			[TestCase("GET", "GET")]
			[TestCase("HEAD", "HEAD")]
			[TestCase("POST", "POST")]
			[TestCase("PUT", "PUT")]
			[TestCase("DELETE", "DELETE")]
			[TestCase("TRACE", "TRACE")]
			[TestCase("CONNECT", "CONNECT")]
			[TestCase("Custom", "Custom")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<string> methods = parameters.Where((arg, i) => i > 0).Cast<string>();
				IEnumerable<AllowHeader> headers = AllowHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.Method), Is.EquivalentTo(methods));
			}
		}
	}
}