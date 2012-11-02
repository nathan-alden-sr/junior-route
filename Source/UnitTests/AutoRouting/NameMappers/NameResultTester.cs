using Junior.Route.AutoRouting.NameMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.NameMappers
{
	public static class NameResultTester
	{
		[TestFixture]
		public class When_creating_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = NameResult.NameMapped("name");
			}

			private NameResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.Name, Is.EqualTo("name"));
				Assert.That(_result.ResultType, Is.EqualTo(NameResultType.NameMapped));
			}
		}

		[TestFixture]
		public class When_creating_not_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = NameResult.NameNotMapped();
			}

			private NameResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.Name, Is.Null);
				Assert.That(_result.ResultType, Is.EqualTo(NameResultType.NameNotMapped));
			}
		}
	}
}