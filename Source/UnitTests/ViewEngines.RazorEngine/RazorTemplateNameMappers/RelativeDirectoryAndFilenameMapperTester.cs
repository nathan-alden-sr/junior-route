using Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.RazorEngine.RazorTemplateNameMappers
{
	public static class RelativeDirectoryAndFilenameMapperTester
	{
		[TestFixture]
		public class When_retrieving_template_name
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new RelativeDirectoryAndFilenameMapper();
			}

			private RelativeDirectoryAndFilenameMapper _mapper;

			[Test]
			[TestCase(@"relative\view.cshtml", @"relative\view.cshtml")]
			[TestCase(@"view.cshtml", "view.cshtml")]
			[TestCase(@"relative\view.", @"relative\view.")]
			[TestCase(@"view", "view")]
			public void Must_retrieve_directory_and_filename_portion_without_extension(string relativePath, string expectedName)
			{
				Assert.That(_mapper.Map(relativePath), Is.EqualTo(expectedName));
			}
		}
	}
}