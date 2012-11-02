using System.Web;

using Junior.Route.AutoRouting.NameMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.NameMappers
{
	public static class NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapperTester
	{
		[TestFixture]
		public class When_naming_routes
		{
			[Test]
			[TestCase(true, " ", "web http request base abort")]
			[TestCase(false, " ", "Web Http Request Base Abort")]
			[TestCase(false, "_", "Web_Http_Request_Base_Abort")]
			public void Must_name_correctly(bool makeLowercase, string wordSeparator, string expectedName)
			{
				var mapper = new NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapper("System", makeLowercase, wordSeparator);
				NameResult result = mapper.Map(typeof(HttpRequestBase), typeof(HttpRequestBase).GetMethod("Abort"));

				Assert.That(result.Name, Is.EqualTo(expectedName));
				Assert.That(result.ResultType, Is.EqualTo(NameResultType.NameMapped));
			}
		}
	}
}