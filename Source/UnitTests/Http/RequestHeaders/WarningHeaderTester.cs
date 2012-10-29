using System;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class WarningHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_warning_header
		{
			[Test]
			[TestCase(@"123 local:8080 ""test")]
			[TestCase(@"123 local: ""test""")]
			[TestCase(@"1234 local:8080 ""test""")]
			[TestCase(@"local:8080 ""test""")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(WarningHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_warning_header
		{
			[Test]
			[TestCase(@"123 host:8080 ""test"", 999 host.local. ""test"" ""Sun, 06 Nov 1994 08:49:37 GMT"", 456 test.org ""test""")]
			public void Must_parse_correctly(string headerValue)
			{
				WarningHeader[] headers = WarningHeader.ParseMany(headerValue).ToArray();

				Assert.That(headers, Has.Length.EqualTo(3));

				Assert.That(headers.ElementAt(0).WarnCode, Is.EqualTo(123));
				Assert.That(headers.ElementAt(0).WarnAgent, Is.EqualTo("host:8080"));
				Assert.That(headers.ElementAt(0).WarnText, Is.EqualTo("test"));
				Assert.That(headers.ElementAt(0).WarnDate, Is.Null);

				Assert.That(headers.ElementAt(1).WarnCode, Is.EqualTo(999));
				Assert.That(headers.ElementAt(1).WarnAgent, Is.EqualTo("host.local."));
				Assert.That(headers.ElementAt(1).WarnText, Is.EqualTo("test"));
				Assert.That(headers.ElementAt(1).WarnDate, Is.EqualTo(new DateTime(1994, 11, 06, 8, 49, 37, DateTimeKind.Utc)));

				Assert.That(headers.ElementAt(2).WarnCode, Is.EqualTo(456));
				Assert.That(headers.ElementAt(2).WarnAgent, Is.EqualTo("test.org"));
				Assert.That(headers.ElementAt(2).WarnText, Is.EqualTo("test"));
				Assert.That(headers.ElementAt(2).WarnDate, Is.Null);
			}
		}
	}
}