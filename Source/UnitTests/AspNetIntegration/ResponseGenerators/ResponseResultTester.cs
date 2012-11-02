using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseGenerators
{
	public static class ResponseResultTester
	{
		[TestFixture]
		public class When_creating_response_generated_instance
		{
			[SetUp]
			public void SetUp()
			{
				_response = MockRepository.GenerateMock<IResponse>();
				_result = ResponseResult.ResponseGenerated(_response, "key");
			}

			private IResponse _response;
			private ResponseResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.CacheKey, Is.EqualTo("key"));
				Assert.That(_result.Response, Is.SameAs(_response));
				Assert.That(_result.ResultType, Is.EqualTo(ResponseResultType.ResponseGenerated));
			}
		}

		[TestFixture]
		public class When_creating_response_not_generated_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = ResponseResult.ResponseNotGenerated();
			}

			private ResponseResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.CacheKey, Is.Null);
				Assert.That(_result.Response, Is.Null);
				Assert.That(_result.ResultType, Is.EqualTo(ResponseResultType.ResponseNotGenerated));
			}
		}
	}
}