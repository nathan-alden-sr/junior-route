using System;
using System.Web;
using System.Web.Caching;

using Junior.Route.AutoRouting.ClassFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ClassFilters
{
	public static class HasNamespaceAncestorFilterTester
	{
		[TestFixture]
		public class When_matching_types
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new HasNamespaceAncestorFilter("System.Web");
			}

			private HasNamespaceAncestorFilter _filter;

			[Test]
			[TestCase(typeof(HttpRequestBase))]
			[TestCase(typeof(Cache))]
			public void Must_match_types_that_have_ancestor_namespace(Type type)
			{
				Assert.That(_filter.Matches(type), Is.True);
			}

			[Test]
			[TestCase(typeof(int))]
			public void Must_not_match_types_that_do_not_have_ancestor_namespace(Type type)
			{
				Assert.That(_filter.Matches(type), Is.False);
			}
		}
	}
}