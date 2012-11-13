using Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.RazorEngine.RazorTemplateNameMappers
{
	public static class FilenameWithoutExtensionMapperTester
	{
		[TestFixture]
		public class When_retrieving_template_name
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FilenameWithoutExtensionMapper();
			}

			private FilenameWithoutExtensionMapper _mapper;

			[Test]
			[TestCase(@"relative\view.cshtml", "view")]
			[TestCase(@"view.cshtml", "view")]
			[TestCase(@"relative\view.", "view")]
			[TestCase(@"view", "view")]
			public void Must_only_retrieve_filename_portion_without_extension(string relativePath, string expectedName)
			{
				Assert.That(_mapper.Map(relativePath), Is.EqualTo(expectedName));
			}
		}
	}
}