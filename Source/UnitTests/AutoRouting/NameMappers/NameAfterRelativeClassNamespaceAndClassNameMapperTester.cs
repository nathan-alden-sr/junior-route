using System.Web;

using Junior.Route.AutoRouting.NameMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.NameMappers
{
	public static class NameAfterRelativeClassNamespaceAndClassNameMapperTester
	{
		[TestFixture]
		public class When_naming_routes
		{
			[Test]
			[TestCase(true, " ", "web http request base")]
			[TestCase(false, " ", "Web Http Request Base")]
			[TestCase(false, "_", "Web_Http_Request_Base")]
			public async void Must_name_correctly(bool makeLowercase, string wordSeparator, string expectedName)
			{
				var mapper = new NameAfterRelativeClassNamespaceAndClassNameMapper("System", makeLowercase, wordSeparator);
				NameResult result = await mapper.MapAsync(typeof(HttpRequestBase), typeof(HttpRequestBase).GetMethod("Abort"));

				Assert.That(result.Name, Is.EqualTo(expectedName));
				Assert.That(result.ResultType, Is.EqualTo(NameResultType.NameMapped));
			}
		}
	}
}