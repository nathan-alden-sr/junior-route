using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class MapResultTester
	{
		[TestFixture]
		public class When_creating_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = MapResult.ValueMapped(0);
			}

			private MapResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(_result.Value, Is.EqualTo(0));
			}
		}

		[TestFixture]
		public class When_creating_not_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = MapResult.ValueNotMapped();
			}

			private MapResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueNotMapped));
				Assert.That(_result.Value, Is.Null);
			}
		}
	}
}