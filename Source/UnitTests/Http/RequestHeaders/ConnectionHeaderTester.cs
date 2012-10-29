using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class ConnectionHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_connection_header
		{
			[Test]
			[TestCase("")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(ConnectionHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_connection_header
		{
			[Test]
			[TestCase("open", "open")]
			[TestCase("open, close, token", "open", "close", "token")]
			[TestCase("open,\r\n close,\r\n token", "open", "close", "token")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<string> connectionTokens = parameters.Where((arg, i) => i > 0).Cast<string>();
				IEnumerable<ConnectionHeader> headers = ConnectionHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.ConnectionToken), Is.EquivalentTo(connectionTokens));
			}
		}
	}
}