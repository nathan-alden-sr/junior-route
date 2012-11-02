using Junior.Route.AutoRouting.ClassFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ClassFilters
{
	public static class DelegateFilterTester
	{
		[TestFixture]
		public class When_matching_types
		{
			[SetUp]
			public void SetUp()
			{
				_filter = new DelegateFilter(
					type =>
						{
							_executed = true;
							return true;
						});
				_filter.Matches(typeof(object));
			}

			private DelegateFilter _filter;
			private bool _executed;

			[Test]
			public void Must_call_delegate()
			{
				Assert.That(_executed, Is.True);
			}
		}
	}
}