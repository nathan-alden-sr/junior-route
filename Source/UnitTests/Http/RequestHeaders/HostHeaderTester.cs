using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class HostHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_host_header
		{
			[Test]
			[TestCase("")]
			[TestCase(":")]
			[TestCase(".")]
			[TestCase("0.0.0.0:")]
			[TestCase("0.0.0.0:65536")]
			[TestCase("0.0.0.0:66000")]
			[TestCase("255.255.255.256:80")]
			[TestCase("255.255.255.256")]
			[TestCase("300.0.0.0")]
			[TestCase("259.0.0.0")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(HostHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_host_header
		{
			[Test]
			[TestCase("127.0.0.1", "127.0.0.1", null)]
			[TestCase("127.0.0.1:80", "127.0.0.1", (ushort)80)]
			[TestCase("127.0.0.1:0", "127.0.0.1", (ushort)0)]
			[TestCase("127.0.0.1:65535", "127.0.0.1", (ushort)65535)]
			[TestCase("a-999-bbbb.local", "a-999-bbbb.local", null)]
			[TestCase("9-b-abc.com:80", "9-b-abc.com", (ushort)80)]
			[TestCase("a.b.c.com.:0", "a.b.c.com.", (ushort)0)]
			[TestCase("test.local.:65535", "test.local.", (ushort)65535)]
			[TestCase("local", "local", null)]
			public void Must_parse_correctly(string headerValue, string expectedHost, ushort? expectedPort)
			{
				HostHeader header = HostHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.Host, Is.EqualTo(expectedHost));
				Assert.That(header.Port, Is.EqualTo(expectedPort));
			}
		}
	}
}