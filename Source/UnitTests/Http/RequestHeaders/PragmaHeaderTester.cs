using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class PragmaHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_pragma_header
		{
			[Test]
			[TestCase("")]
			[TestCase(",")]
			[TestCase(@"x=""a")]
			[TestCase(@"x=a""")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(PragmaHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_pragma_header
		{
			[Test]
			[TestCase("no-cache", "no-cache", "")]
			[TestCase("no-cache, no-cache", "no-cache", "", "no-cache", "")]
			[TestCase(@"no-cache, param1=1, param2=""2""", "no-cache", "", "param1", "1", "param2", "2")]
			[TestCase(@"param1=1, param2=""2""", "param1", "1", "param2", "2")]
			[TestCase(@"param1, param2", "param1", "", "param2", "")]
			[TestCase("no-cache, param1=1,\r\n param2=\"2\"", "no-cache", "", "param1", "1", "param2", "2")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				var directives = new List<Tuple<string, string>>();

				for (int i = 1; i < parameters.Length; i += 2)
				{
					directives.Add(new Tuple<string, string>((string)parameters[i], (string)parameters[i + 1]));
				}

				IEnumerable<PragmaHeader> headers = PragmaHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => new Tuple<string, string>(arg.Name, arg.Value)), Is.EquivalentTo(directives));
			}
		}
	}
}