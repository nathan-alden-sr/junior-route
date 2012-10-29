using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class IfMatchHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_if_match_header
		{
			[Test]
			[TestCase("")]
			[TestCase(",")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(IfMatchHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_if_match_header
		{
			[Test]
			[TestCase("*", "*", false)]
			[TestCase(@"""tag1"", ""tag2"", W/""tag3""", "tag1", false, "tag2", false, "tag3", true)]
			[TestCase("\"tag1\",\r\n \"tag2\",\r\n W/\"tag3\"", "tag1", false, "tag2", false, "tag3", true)]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				var entityTags = new List<EntityTag>();

				for (int i = 1; i < parameters.Length; i += 2)
				{
					entityTags.Add(new EntityTag((string)parameters[i], (bool)parameters[i + 1]));
				}

				IEnumerable<IfMatchHeader> headers = IfMatchHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => arg.EntityTag), Is.EquivalentTo(entityTags));
			}
		}
	}
}