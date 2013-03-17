using Junior.Route.AspNetIntegration.RequestFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AspNetIntegration.RequestFilters
{
	public static class RequestFilterTester
	{
		[TestFixture]
		public class When_creating_instance_with_defer_result
		{
			[SetUp]
			public void SetUp()
			{
				_result = FilterResult.Defer();
			}

			private FilterResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(FilterResultType.Defer));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_use_juniorroute_handler_result
		{
			[SetUp]
			public void SetUp()
			{
				_result = FilterResult.UseJuniorRouteHandler();
			}

			private FilterResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(FilterResultType.UseJuniorRouteHandler));
			}
		}
	}
}