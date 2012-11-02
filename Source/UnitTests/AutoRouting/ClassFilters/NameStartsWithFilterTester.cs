using System;
using System.Web;
using System.Web.Caching;

using Junior.Route.AutoRouting.ClassFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ClassFilters
{
	public static class NameStartsWithFilterTester
	{
		[TestFixture]
		public class When_matching_types
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new NameStartsWithFilter("Http");
			}

			private NameStartsWithFilter _filter;

			[Test]
			[TestCase(typeof(HttpRequestBase))]
			[TestCase(typeof(HttpResponseBase))]
			public void Must_match_types_with_names_ending_with_namespace(Type type)
			{
				Assert.That(_filter.Matches(type), Is.True);
			}

			[Test]
			[TestCase(typeof(Cache))]
			[TestCase(typeof(string))]
			public void Must_not_match_types_with_names_not_ending_with_namespace(Type type)
			{
				Assert.That(_filter.Matches(type), Is.False);
			}
		}
	}
}