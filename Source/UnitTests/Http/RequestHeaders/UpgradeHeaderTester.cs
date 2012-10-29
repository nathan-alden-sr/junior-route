using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class UpgradeHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_upgrade_header
		{
			[Test]
			[TestCase("")]
			[TestCase(",")]
			[TestCase("/")]
			[TestCase("/2.1")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(UpgradeHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_upgrade_header
		{
			[Test]
			[TestCase("HTTP", "HTTP", null)]
			[TestCase("HTTP/1.1, HTTP/2.0, Another, Product/3.5.6.9", "HTTP", "1.1", "HTTP", "2.0", "Another", null, "Product", "3.5.6.9")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				var products = new List<Tuple<string, string>>();

				for (int i = 1; i < parameters.Length; i += 2)
				{
					products.Add(new Tuple<string, string>((string)parameters[i], (string)parameters[i + 1]));
				}

				IEnumerable<UpgradeHeader> headers = UpgradeHeader.ParseMany(headerValue);

				Assert.That((headers.Select(arg => new Tuple<string, string>(arg.Product, arg.ProductVersion))), Is.EquivalentTo(products));
			}
		}
	}
}