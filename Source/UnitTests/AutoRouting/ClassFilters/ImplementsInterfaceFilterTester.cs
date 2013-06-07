using System;

using Junior.Route.AutoRouting.ClassFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ClassFilters
{
	public static class ImplementsInterfaceFilterTester
	{
		[TestFixture]
		public class When_matching_types
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new ImplementsInterfaceFilter(typeof(IEndpoint));
			}

			private ImplementsInterfaceFilter _filter;

			public interface IEndpoint
			{
			}

			public class Endpoint : IEndpoint
			{
			}

			[Test]
			[TestCase(typeof(Endpoint))]
			public async void Must_match_types_that_implement_interface(Type type)
			{
				Assert.That(await _filter.MatchesAsync(type), Is.True);
			}

			[Test]
			[TestCase(typeof(string))]
			public async void Must_not_match_types_that_do_not_match_interface(Type type)
			{
				Assert.That(await _filter.MatchesAsync(type), Is.False);
			}
		}

		[TestFixture]
		public class When_matching_types_using_generic_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new ImplementsInterfaceFilter<IEndpoint>();
			}

			private ImplementsInterfaceFilter<IEndpoint> _filter;

			public interface IEndpoint
			{
			}

			public class Endpoint : IEndpoint
			{
			}

			[Test]
			[TestCase(typeof(Endpoint))]
			public async void Must_match_types_that_implement_interface(Type type)
			{
				Assert.That(await _filter.MatchesAsync(type), Is.True);
			}

			[Test]
			[TestCase(typeof(string))]
			public async void Must_not_match_types_that_do_not_match_interface(Type type)
			{
				Assert.That(await _filter.MatchesAsync(type), Is.False);
			}
		}
	}
}