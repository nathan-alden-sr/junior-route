using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class ViewHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_via_header
		{
			[Test]
			[TestCase("received")]
			[TestCase("received invalid host")]
			[TestCase("received invalid host (Invalid Comment")]
			[TestCase("received host:")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(ViaHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_via_header
		{
			[Test]
			[TestCase("1.0 local", "1.0", "local")]
			[TestCase("1.0 local, 1.1 host.remote, 2.0 test.org.:8080", "1.0", "local", "1.1", "host.remote", "2.0", "test.org.:8080")]
			[TestCase("1.0 local,\r\n 1.1 host.remote,\r\n 2.0 test.org.:8080", "1.0", "local", "1.1", "host.remote", "2.0", "test.org.:8080")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				var receivedFields = new List<Tuple<string, string>>();

				for (int i = 1; i < parameters.Length; i += 2)
				{
					receivedFields.Add(new Tuple<string, string>((string)parameters[i], (string)parameters[i + 1]));
				}

				IEnumerable<ViaHeader> headers = ViaHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => new Tuple<string, string>(arg.ReceivedProtocol, arg.ReceivedBy)), Is.EquivalentTo(receivedFields));
			}
		}
	}
}