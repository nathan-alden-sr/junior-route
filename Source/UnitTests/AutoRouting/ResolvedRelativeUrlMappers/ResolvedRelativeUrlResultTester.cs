using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ResolvedRelativeUrlMappers
{
	public static class ResolvedRelativeUrlResultTester
	{
		[TestFixture]
		public class When_creating_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped("relative");
			}

			private ResolvedRelativeUrlResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResolvedRelativeUrl, Is.EqualTo("relative"));
				Assert.That(_result.ResultType, Is.EqualTo(ResolvedRelativeUrlResultType.ResolvedRelativeUrlMapped));
			}
		}

		[TestFixture]
		public class When_creating_not_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = ResolvedRelativeUrlResult.ResolvedRelativeUrlNotMapped();
			}

			private ResolvedRelativeUrlResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResolvedRelativeUrl, Is.Null);
				Assert.That(_result.ResultType, Is.EqualTo(ResolvedRelativeUrlResultType.ResolvedRelativeUrlNotMapped));
			}
		}
	}
}