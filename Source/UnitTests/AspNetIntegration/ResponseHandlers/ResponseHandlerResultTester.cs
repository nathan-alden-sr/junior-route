using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseHandlers
{
	public static class ResponseHandlerResultTester
	{
		[TestFixture]
		public class When_creating_not_handled_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = ResponseHandlerResult.ResponseNotHandled();
			}

			private ResponseHandlerResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseNotHandled));
				Assert.That(_result.SuggestedResponse, Is.Null);
			}
		}

		[TestFixture]
		public class When_creating_suggested_instance
		{
			[SetUp]
			public void SetUp()
			{
				_response = MockRepository.GenerateMock<IResponse>();
				_result = ResponseHandlerResult.ResponseSuggested(_response);
			}

			private ResponseHandlerResult _result;
			private IResponse _response;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseSuggested));
				Assert.That(_result.SuggestedResponse, Is.SameAs(_response));
			}
		}

		[TestFixture]
		public class When_creating_written_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = ResponseHandlerResult.ResponseWritten();
			}

			private ResponseHandlerResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.ResultType, Is.EqualTo(ResponseHandlerResultType.ResponseWritten));
				Assert.That(_result.SuggestedResponse, Is.Null);
			}
		}
	}
}