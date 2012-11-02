using System.Web;

using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ResolvedRelativeUrlMappers
{
	public static class ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapperTester
	{
		[TestFixture]
		public class When_determining_resolved_relative_url
		{
			[Test]
			[TestCase(true, "_", "web/http_request_base")]
			[TestCase(false, "_", "Web/Http_Request_Base")]
			[TestCase(false, "-", "Web/Http-Request-Base")]
			public void Must_generate_correct_resolved_relative_url(bool makeLowercase, string wordSeparator, string expectedName)
			{
				var mapper = new ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper("System", makeLowercase, wordSeparator);
				ResolvedRelativeUrlResult result = mapper.Map(typeof(HttpRequestBase), typeof(HttpRequestBase).GetMethod("Abort"));

				Assert.That(result.ResolvedRelativeUrl, Is.EqualTo(expectedName));
				Assert.That(result.ResultType, Is.EqualTo(ResolvedRelativeUrlResultType.ResolvedRelativeUrlMapped));
			}
		}
	}
}