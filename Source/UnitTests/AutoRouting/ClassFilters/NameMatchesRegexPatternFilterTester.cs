using System;
using System.Web;

using Junior.Route.AutoRouting.ClassFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ClassFilters
{
	public static class NameMatchesRegexPatternFilterTester
	{
		[TestFixture]
		public class When_matching_types
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new NameMatchesRegexPatternFilter("(?:Req.*Base)");
			}

			private NameMatchesRegexPatternFilter _filter;

			[Test]
			[TestCase(typeof(HttpRequestBase))]
			public async void Must_match_types_with_names_matching_pattern(Type type)
			{
				Assert.That(await _filter.MatchesAsync(type), Is.True);
			}

			[Test]
			[TestCase(typeof(HttpResponseBase))]
			public async void Must_not_match_types_with_names_not_matching_pattern(Type type)
			{
				Assert.That(await _filter.MatchesAsync(type), Is.False);
			}
		}
	}
}