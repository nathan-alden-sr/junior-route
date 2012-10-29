using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class ExpectHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_expect_header
		{
			[Test]
			[TestCase("")]
			[TestCase(@"100-continue; test=1")]
			[TestCase(@"100-continue; test")]
			[TestCase(@"test=""test")]
			[TestCase(@"test=test""")]
			[TestCase(@"test1; test2=2")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(ExpectHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_expect_header
		{
			[Test]
			[TestCase("100-continue", "100-continue", "")]
			[TestCase("test", "test", "")]
			[TestCase(@"test1=1; test2=2; test3; test4=""4""", "test1", "1", "test2", "2", "test3", "", "test4", "4")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				IEnumerable<ExpectHeader> headers = ExpectHeader.ParseMany(headerValue);
				var expectParams = new List<Tuple<string, string>>();

				for (int i = 1; i < parameters.Length; i += 2)
				{
					expectParams.Add(new Tuple<string, string>((string)parameters[i], (string)parameters[i + 1]));
				}

				Assert.That(headers.Select(arg => new Tuple<string, string>(arg.Name, arg.Value)), Is.EquivalentTo(expectParams));
			}
		}
	}
}