using System;
using System.Web;
using System.Web.Caching;

using Junior.Route.AutoRouting.ClassFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ClassFilters
{
	public static class InNamespaceFilterTester
	{
		[TestFixture]
		public class When_matching_types
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new InNamespaceFilter("System.Web");
			}

			private InNamespaceFilter _filter;

			[Test]
			[TestCase(typeof(HttpRequestBase))]
			public void Must_match_types_in_namespace(Type type)
			{
				Assert.That(_filter.Matches(type), Is.True);
			}

			[Test]
			[TestCase(typeof(Cache))]
			[TestCase(typeof(string))]
			public void Must_not_match_types_not_in_namespace(Type type)
			{
				Assert.That(_filter.Matches(type), Is.False);
			}
		}
	}
}