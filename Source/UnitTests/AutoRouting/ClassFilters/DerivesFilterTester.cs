using System;

using Junior.Route.AutoRouting.ClassFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ClassFilters
{
	public static class DerivesFilterTester
	{
		[TestFixture]
		public class When_matching_types
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new DerivesFilter(typeof(BaseEndpoint));
			}

			private DerivesFilter _filter;

			public abstract class BaseEndpoint
			{
			}

			public class Endpoint : BaseEndpoint
			{
			}

			[Test]
			[TestCase(typeof(Endpoint))]
			public void Must_match_types_that_derive_base_type(Type type)
			{
				Assert.That(_filter.Matches(type), Is.True);
			}

			[Test]
			[TestCase(typeof(BaseEndpoint))]
			[TestCase(typeof(int))]
			public void Must_not_match_types_that_do_not_derive_base_type(Type type)
			{
				Assert.That(_filter.Matches(type), Is.False);
			}
		}

		[TestFixture]
		public class When_matching_types_using_generic_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new DerivesFilter<BaseEndpoint>();
			}

			private DerivesFilter<BaseEndpoint> _filter;

			public abstract class BaseEndpoint
			{
			}

			public class Endpoint : BaseEndpoint
			{
			}

			[Test]
			[TestCase(typeof(Endpoint))]
			public void Must_match_types_that_derive_base_type(Type type)
			{
				Assert.That(_filter.Matches(type), Is.True);
			}

			[Test]
			[TestCase(typeof(BaseEndpoint))]
			[TestCase(typeof(int))]
			public void Must_not_match_types_that_do_not_derive_base_type(Type type)
			{
				Assert.That(_filter.Matches(type), Is.False);
			}
		}
	}
}