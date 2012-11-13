using System;
using System.IO;

using Junior.Route.Common;
using Junior.Route.ViewEngines.RazorEngine;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateRepositories;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.RazorEngine.RazorTemplateRespositories
{
	public static class FileSystemRepositoryTester
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
			}

			private IRazorTemplateNameMapper _nameMapper;
			private IFileSystem _fileSystem;

			[Test]
			public void Must_retrieve_directory_files_using_supplied_parameters()
			{
				_fileSystem.Stub(arg => arg.DirectoryExists("relative")).Return(true);
				_fileSystem.Stub(arg => arg.GetDirectoryFiles(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<SearchOption>.Is.Anything)).Return(new string[0]);

				// ReSharper disable ObjectCreationAsStatement
				new FileSystemRepository("relative", "*.*", SearchOption.TopDirectoryOnly, _nameMapper, _fileSystem);
				// ReSharper restore ObjectCreationAsStatement

				_fileSystem.AssertWasCalled(arg => arg.GetDirectoryFiles("relative", "*.*", SearchOption.TopDirectoryOnly));
			}

			[Test]
			public void Must_throw_exception_if_directory_does_not_exist()
			{
				_fileSystem.Stub(arg => arg.DirectoryExists("relative")).Return(false);

				Assert.Throws<ApplicationException>(() => new FileSystemRepository("relative", "*.*", SearchOption.TopDirectoryOnly, _nameMapper, _fileSystem));
			}
		}

		[TestFixture]
		public class When_getting_template_by_name
		{
			[SetUp]
			public void SetUp()
			{
				_nameMapper = MockRepository.GenerateMock<IRazorTemplateNameMapper>();
				_nameMapper.Stub(arg => arg.Map("name.cshtml")).Return("name");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("relative")).Return("relative");
				_fileSystem.Stub(arg => arg.DirectoryExists("relative")).Return(true);
				_fileSystem.Stub(arg => arg.GetDirectoryFiles(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<SearchOption>.Is.Anything)).Return(new[] { @"relative\name.cshtml" });
				_repository = new FileSystemRepository("relative", "*.*", SearchOption.AllDirectories, _nameMapper, _fileSystem);
			}

			private IRazorTemplateNameMapper _nameMapper;
			private IFileSystem _fileSystem;
			private FileSystemRepository _repository;

			[Test]
			public void Must_retrieve_template_using_directory_file_and_name_mapper()
			{
				IRazorTemplate template = _repository.GetByName("name");

				Assert.That(template.Name, Is.EqualTo("name"));
				Assert.That(template.Path, Is.EqualTo(@"relative\name.cshtml"));
			}
		}
	}
}