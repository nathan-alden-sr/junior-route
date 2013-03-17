using Junior.Route.AspNetIntegration.ErrorHandlers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AspNetIntegration.ErrorHandlers
{
	public static class HandleResultTester
	{
		[TestFixture]
		public class When_creating_instance_with_error_handled_result
		{
			[SetUp]
			public void SetUp()
			{
				_result = HandleResult.ErrorHandled();
			}

			private HandleResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(HandleResultType.Handled));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_error_not_handled_result
		{
			[SetUp]
			public void SetUp()
			{
				_result = HandleResult.ErrorNotHandled();
			}

			private HandleResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(HandleResultType.NotHandled));
			}
		}
	}
}