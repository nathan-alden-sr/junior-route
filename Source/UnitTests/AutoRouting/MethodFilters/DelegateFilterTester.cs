using System.Reflection.Emit;

using Junior.Route.AutoRouting.MethodFilters;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.MethodFilters
{
	public static class DelegateFilterTester
	{
		[TestFixture]
		public class When_matching_methods
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
				_filter.MatchesAsync(new DynamicMethod("test", typeof(void), null));
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