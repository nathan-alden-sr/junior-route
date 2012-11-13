using System.IO;

using Junior.Route.Common;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateRepositories;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.RazorEngine.RazorTemplateRespositories
{
	public static class VisualBasicFileSystemRepositoryTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_nameMapper = MockRepository.GenerateMock<IRazorTemplateNameMapper>();
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("relative")).Return("relative");
				_fileSystem.Stub(arg => arg.DirectoryExists("relative")).Return(true);
				_fileSystem.Stub(arg => arg.GetDirectoryFiles(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<SearchOption>.Is.Anything)).Return(new string[0]);
				// ReSharper disable ObjectCreationAsStatement
				new VisualBasicFileSystemRepository("relative", SearchOption.TopDirectoryOnly, _nameMapper, _fileSystem);
				// ReSharper restore ObjectCreationAsStatement
			}

			private IRazorTemplateNameMapper _nameMapper;
			private IFileSystem _fileSystem;

			[Test]
			public void Must_search_for_cshtml_files()
			{
				_fileSystem.AssertWasCalled(arg => arg.GetDirectoryFiles("relative", "*.vbhtml", SearchOption.TopDirectoryOnly));
			}
		}
	}
}