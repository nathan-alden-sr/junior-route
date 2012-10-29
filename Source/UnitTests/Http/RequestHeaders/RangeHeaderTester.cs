using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class RangeHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_range_header
		{
			[Test]
			[TestCase("")]
			[TestCase("-")]
			[TestCase("a-b")]
			[TestCase("1-b")]
			[TestCase("a-1")]
			[TestCase("bytes=2000-1999")]
			[TestCase("1000-2000")]
			[TestCase("bytes=a-1")]
			[TestCase("bytes=")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(RangeHeader.ParseMany(headerValue), Is.Empty);
			}
		}

		[TestFixture]
		public class When_parsing_valid_range_header
		{
			[Test]
			[TestCase("bytes=1000-2000", 1000ul, 2000ul)]
			[TestCase("bytes=-2000", null, 2000ul)]
			[TestCase("bytes=1000-", 1000ul, null)]
			[TestCase("bytes=1-2,5-6", 1ul, 2ul, 5ul, 6ul)]
			[TestCase("bytes=1-2,\r\n 5-6", 1ul, 2ul, 5ul, 6ul)]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				var byteRanges = new List<Tuple<ulong?, ulong?>>();

				for (int i = 1; i < parameters.Length; i += 2)
				{
					byteRanges.Add(new Tuple<ulong?, ulong?>((ulong?)parameters[i], (ulong?)parameters[i + 1]));
				}

				IEnumerable<RangeHeader> headers = RangeHeader.ParseMany(headerValue);

				Assert.That(headers.Select(arg => new Tuple<ulong?, ulong?>(arg.FirstBytePos, arg.LastBytePos)), Is.EquivalentTo(byteRanges));
			}
		}
	}
}