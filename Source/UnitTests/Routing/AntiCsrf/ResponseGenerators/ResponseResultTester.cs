using Junior.Route.Routing.AntiCsrf.ResponseGenerators;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.ResponseGenerators
{
	public static class ResponseResultTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_response = Response.OK();
				_responseResult = ResponseResult.ResponseGenerated(_response);
			}

			private ResponseResult _responseResult;
			private Response _response;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_responseResult.Response, Is.SameAs(_response));
				Assert.That(_responseResult.ResultType, Is.EqualTo(ResponseResultType.ResponseGenerated));
			}
		}
	}
}