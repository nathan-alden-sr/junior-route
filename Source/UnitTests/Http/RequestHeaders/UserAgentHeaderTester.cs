using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class UserAgentHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_user_agent_header
		{
			[Test]
			[TestCase("")]
			[TestCase("/")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(UserAgentHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_user_agent_header
		{
			[Test]
			[TestCase("HTTP (Comment)", "HTTP")]
			[TestCase("(Comment) (Comment)")]
			[TestCase("HTTP/1.0 HTTP/1.1 HTTP/2.0 (Comment) HTTP/0.0", "HTTP/1.0", "HTTP/1.1", "HTTP/2.0", "HTTP/0.0")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<string> products = parameters.Where((arg, i) => i > 0).Cast<string>();
				IEnumerable<UserAgentHeader> headers = UserAgentHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.Product), Is.EquivalentTo(products));
			}
		}
	}
}