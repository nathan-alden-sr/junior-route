using System.IO;
using System.Text;

using Junior.Route.Common;
using Junior.Route.ViewEngines.RazorEngine;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.RazorEngine
{
	public static class RazorTemplateTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_template = new RazorTemplate("name", "path", _fileSystem);
			}

			private RazorTemplate _template;
			private IFileSystem _fileSystem;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_template.Name, Is.EqualTo("name"));
				Assert.That(_template.Path, Is.EqualTo("path"));
			}
		}

		[TestFixture]
		public class When_retrieving_contents
		{
			[SetUp]
			public void SetUp()
			{
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.OpenFile("path", FileMode.Open, FileAccess.Read, FileShare.Read)).Return(new MemoryStream(Encoding.ASCII.GetBytes("contents")));
				_template = new RazorTemplate("name", "path", _fileSystem);
			}

			private RazorTemplate _template;
			private IFileSystem _fileSystem;

			[Test]
			public void Must_retrieve_file_contents()
			{
				string contents = _template.GetContents();

				Assert.That(contents, Is.EqualTo("contents"));
			}

			[Test]
			public void Must_use_provided_filesystem()
			{
				_template.GetContents();

				_fileSystem.AssertWasCalled(arg => arg.OpenFile("path", FileMode.Open, FileAccess.Read, FileShare.Read));
			}
		}
	}
}