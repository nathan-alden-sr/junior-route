using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class IfRangeHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_if_range_header
		{
			[Test]
			[TestCase("")]
			[TestCase("zzz, 06 Nov 1994 08:49:37 GMT")]
			[TestCase("Sun, 00 Nov 1994 08:49:37 GMT")]
			[TestCase("Sun, 06 zzz 1994 08:49:37 GMT")]
			[TestCase("Sun, 06 Nov zzzz 08:49:37 GMT")]
			[TestCase("Sun, 06 Nov 1994 24:49:37 GMT")]
			[TestCase("Sun, 06 Nov 1994 08:60:37 GMT")]
			[TestCase("Sun, 06 Nov 1994 08:49:60 GMT")]
			[TestCase("Sun, 06 Nov 1994 08:49:37")]
			[TestCase("zzzzzz, 06-Nov-94 08:49:37 GMT")]
			[TestCase("Sunday, 00-Nov-94 08:49:37 GMT")]
			[TestCase("Sunday, 06-zzz-94 08:49:37 GMT")]
			[TestCase("Sunday, 06-Nov-zz 08:49:37 GMT")]
			[TestCase("Sunday, 06-Nov-94 24:49:37 GMT")]
			[TestCase("Sunday, 06-Nov-94 08:60:37 GMT")]
			[TestCase("Sunday, 06-Nov-94 08:49:60 GMT")]
			[TestCase("Sunday, 06-Nov-94 08:49:37")]
			[TestCase("zzz Nov  6 08:49:37 1994")]
			[TestCase("Sun zzz  6 08:49:37 1994")]
			[TestCase("Sun Nov 31 08:49:37 1994")]
			[TestCase("Sun Nov  6 24:49:37 1994")]
			[TestCase("Sun Nov  6 08:60:37 1994")]
			[TestCase("Sun Nov  6 08:49:60 1994")]
			[TestCase("Sun Nov  6 08:49:37 zzzz")]
			[TestCase(@"W/""")]
			[TestCase(@"W/""""")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(IfModifiedSinceHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_if_range_with_entity_tag_header
		{
			[Test]
			[TestCase(@"W/""test""", "test", true)]
			[TestCase(@"""test""", "test", false)]
			public void Must_parse_correctly(string headerValue, string expectedValue, bool expectedWeak)
			{
				IfRangeHeader header = IfRangeHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.EntityTag, Is.Not.Null);
				Assert.That(header.EntityTag.Value, Is.EqualTo(expectedValue));
				Assert.That(header.EntityTag.Weak, Is.EqualTo(expectedWeak));
			}
		}

		[TestFixture]
		public class When_parsing_valid_if_range_with_http_date_header
		{
			[Test]
			[TestCase("Sun, 06 Nov 1994 08:49:37 GMT", 1994, 11, 6, 8, 49, 37)]
			[TestCase("Sunday, 06-Nov-94 08:49:37 GMT", 1994, 11, 6, 8, 49, 37)]
			[TestCase("Sun Nov  6 08:49:37 1994", 1994, 11, 6, 8, 49, 37)]
			public void Must_parse_correctly(string headerValue, int year, int month, int day, int hour, int minute, int second)
			{
				IfRangeHeader header = IfRangeHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.HttpDate, Is.Not.Null);
				Assert.That(header.HttpDate.Value.Year, Is.EqualTo(year));
				Assert.That(header.HttpDate.Value.Month, Is.EqualTo(month));
				Assert.That(header.HttpDate.Value.Day, Is.EqualTo(day));
				Assert.That(header.HttpDate.Value.Hour, Is.EqualTo(hour));
				Assert.That(header.HttpDate.Value.Minute, Is.EqualTo(minute));
				Assert.That(header.HttpDate.Value.Second, Is.EqualTo(second));
			}
		}
	}
}