using System.Linq;
using System.Net;
using System.Web;

using Junior.Route.AspNetIntegration.ResponseGenerators;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseGenerators
{
	public static class NotFoundGeneratorTester
	{
		[TestFixture]
		public class When_generating_response
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new NotFoundGenerator();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private NotFoundGenerator _generator;
			private HttpRequestBase _request;

			[Test]
			public void Must_generate_not_found_response()
			{
				ResponseResult result = _generator.GetResponse(_request, Enumerable.Empty<RouteMatchResult>());

				Assert.That(result.CacheKey, Is.Null);
				Assert.That(result.Response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.NotFound));
				Assert.That(result.ResultType, Is.EqualTo(ResponseResultType.ResponseGenerated));
			}
		}
	}
}