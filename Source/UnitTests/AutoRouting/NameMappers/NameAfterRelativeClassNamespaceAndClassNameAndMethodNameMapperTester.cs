using Junior.Route.AutoRouting.NameMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.NameMappers
{
	public static class NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapperTester
	{
		public class Endpoint
		{
			public void Get()
			{
			}

			public void GetAsync()
			{
			}
		}

		[TestFixture]
		public class When_naming_routes
		{
			[Test]
			[TestCase(true, " ", "Get", "name mappers endpoint get")]
			[TestCase(false, " ", "Get", "Name Mappers Endpoint Get")]
			[TestCase(false, "_", "Get", "Name_Mappers_Endpoint_Get")]
			[TestCase(true, " ", "GetAsync", "name mappers endpoint get")]
			[TestCase(false, " ", "GetAsync", "Name Mappers Endpoint Get")]
			[TestCase(false, "_", "GetAsync", "Name_Mappers_Endpoint_Get")]
			public async void Must_name_correctly(bool makeLowercase, string wordSeparator, string methodName, string expectedName)
			{
				var mapper = new NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapper("Junior.Route.UnitTests.AutoRouting", makeLowercase, wordSeparator);
				NameResult result = await mapper.MapAsync(typeof(Endpoint), typeof(Endpoint).GetMethod("Get"));

				Assert.That(result.Name, Is.EqualTo(expectedName));
				Assert.That(result.ResultType, Is.EqualTo(NameResultType.NameMapped));
			}
		}
	}
}