using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.ViewEngines.Razor;
using Junior.Route.ViewEngines.Razor.CodeDomProviderFactories;
using Junior.Route.ViewEngines.Razor.CompiledTemplateFactories;
using Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers;
using Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories;
using Junior.Route.ViewEngines.Razor.TemplateClassNameBuilders;
using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;
using Junior.Route.ViewEngines.Razor.TemplateCompilers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.Razor.Routing.TemplateRepositories
{
	public static class FileSystemRepositoryTester
	{
		[TestFixture]
		public class When_getting_template_with_anonymous_model
		{
			[SetUp]
			public void SetUp()
			{
				var model = new { Foo = "Bar" };

				_pathResolver = MockRepository.GenerateMock<ITemplatePathResolver>();
				_pathResolver.Stub(arg => arg.Absolute("Template")).Return(@"C:\Template.cshtml");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.OpenFile(@"C:\Template.cshtml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).Return(new MemoryStream(Encoding.ASCII.GetBytes("Hello, world.")));
				_compiler = MockRepository.GenerateMock<ITemplateCompiler>();
				_classNameBuilder = MockRepository.GenerateMock<ITemplateClassNameBuilder>();
				_classNameBuilder.Stub(arg => arg.BuildFromRandomGuid()).Return("ClassName");
				_codeBuilder = MockRepository.GenerateMock<ITemplateCodeBuilder>();
				_codeDomProvider = MockRepository.GenerateMock<CodeDomProvider>();
				_codeDomProviderFactory = MockRepository.GenerateMock<ICodeDomProviderFactory>();
				_codeDomProviderFactory.Stub(arg => arg.CreateFromFileExtension(Arg<string>.Is.Anything)).Return(_codeDomProvider);
				_compiledTemplateFactory = MockRepository.GenerateMock<ICompiledTemplateFactory>();
				_fileSystemRepositoryConfiguration = MockRepository.GenerateMock<IFileSystemRepositoryConfiguration>();
				_repository = new FileSystemRepository(_pathResolver, _fileSystem, _compiler, _classNameBuilder, _codeBuilder, _codeDomProviderFactory, _compiledTemplateFactory, _fileSystemRepositoryConfiguration);
				Stub(model);
				Get(model);
			}

			private void Stub<T>(T model)
			{
				var template = MockRepository.GenerateMock<ITemplate<T>>();

				_compiler
					.Stub(arg => arg.Compile<ITemplate<T>>(
						Arg<string>.Is.Anything,
						Arg<string>.Is.Anything,
						Arg<ITemplateCodeBuilder>.Is.Anything,
						Arg<CodeDomProvider>.Is.Anything,
						Arg<Action<CodeTypeDeclaration>>.Is.NotNull,
						Arg<IEnumerable<string>>.Is.Anything))
					.Return(typeof(ITemplate<T>));
				_compiledTemplateFactory.Stub(arg => arg.CreateFromType<T>(Arg<Type>.Is.Anything)).Return(template);
			}

			private void Get<T>(T model)
			{
				_repository.Get<ITemplate<T>, T>("Template", model, _namespaces);
			}

			private readonly IEnumerable<string> _namespaces = "System.Text".ToEnumerable();
			private ITemplatePathResolver _pathResolver;
			private IFileSystem _fileSystem;
			private ITemplateCompiler _compiler;
			private ITemplateClassNameBuilder _classNameBuilder;
			private ITemplateCodeBuilder _codeBuilder;
			private ICodeDomProviderFactory _codeDomProviderFactory;
			private ICompiledTemplateFactory _compiledTemplateFactory;
			private FileSystemRepository _repository;
			private CodeDomProvider _codeDomProvider;
			private IFileSystemRepositoryConfiguration _fileSystemRepositoryConfiguration;

			private void Assert<T>(T model)
			{
				_compiler.AssertWasCalled(
					arg => arg.Compile<ITemplate<T>>(
						Arg<string>.Is.Anything,
						Arg<string>.Is.Anything,
						Arg<ITemplateCodeBuilder>.Is.Anything,
						Arg<CodeDomProvider>.Is.Anything,
						Arg<Action<CodeTypeDeclaration>>.Is.NotNull,
						Arg<IEnumerable<string>>.Is.Anything));
			}

			[Test]
			public void Must_pass_delegate_to_provided_compiler()
			{
				Assert(new { Foo = "Bar" });
			}
		}

		[TestFixture]
		public class When_getting_template_with_non_anonymous_model
		{
			[SetUp]
			public void SetUp()
			{
				_pathResolver = MockRepository.GenerateMock<ITemplatePathResolver>();
				_pathResolver.Stub(arg => arg.Absolute("Template")).Return(@"C:\Template.cshtml");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.OpenFile(@"C:\Template.cshtml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).Return(new MemoryStream(Encoding.ASCII.GetBytes("Hello, world.")));
				_compiler = MockRepository.GenerateMock<ITemplateCompiler>();
				_compiler
					.Stub(arg => arg.Compile<ITemplate<string>>(
						Arg<string>.Is.Anything,
						Arg<string>.Is.Anything,
						Arg<ITemplateCodeBuilder>.Is.Anything,
						Arg<CodeDomProvider>.Is.Anything,
						Arg<Action<CodeTypeDeclaration>>.Is.Anything,
						Arg<IEnumerable<string>>.Is.Anything))
					.Return(typeof(ITemplate<string>));
				_classNameBuilder = MockRepository.GenerateMock<ITemplateClassNameBuilder>();
				_classNameBuilder.Stub(arg => arg.BuildFromRandomGuid()).Return("ClassName");
				_codeBuilder = MockRepository.GenerateMock<ITemplateCodeBuilder>();
				_codeDomProvider = MockRepository.GenerateMock<CodeDomProvider>();
				_codeDomProviderFactory = MockRepository.GenerateMock<ICodeDomProviderFactory>();
				_codeDomProviderFactory.Stub(arg => arg.CreateFromFileExtension(Arg<string>.Is.Anything)).Return(_codeDomProvider);
				_template = MockRepository.GenerateMock<ITemplate<string>>();
				_compiledTemplateFactory = MockRepository.GenerateMock<ICompiledTemplateFactory>();
				_compiledTemplateFactory.Stub(arg => arg.CreateFromType<string>(Arg<Type>.Is.Anything)).Return(_template);
				_fileSystemRepositoryConfiguration = MockRepository.GenerateMock<IFileSystemRepositoryConfiguration>();
				_repository = new FileSystemRepository(_pathResolver, _fileSystem, _compiler, _classNameBuilder, _codeBuilder, _codeDomProviderFactory, _compiledTemplateFactory, _fileSystemRepositoryConfiguration);
				_repository.Get<ITemplate<string>, string>("Template", "Model", _namespaces);
			}

			private readonly IEnumerable<string> _namespaces = "System.Text".ToEnumerable();
			private ITemplatePathResolver _pathResolver;
			private IFileSystem _fileSystem;
			private ITemplateCompiler _compiler;
			private ITemplateClassNameBuilder _classNameBuilder;
			private ITemplateCodeBuilder _codeBuilder;
			private ICodeDomProviderFactory _codeDomProviderFactory;
			private ICompiledTemplateFactory _compiledTemplateFactory;
			private FileSystemRepository _repository;
			private ITemplate<string> _template;
			private CodeDomProvider _codeDomProvider;
			private IFileSystemRepositoryConfiguration _fileSystemRepositoryConfiguration;

			[Test]
			public void Must_read_file_contents()
			{
				_fileSystem.AssertWasCalled(arg => arg.OpenFile(@"C:\Template.cshtml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			}

			[Test]
			public void Must_resolve_absolute_path()
			{
				_pathResolver.AssertWasCalled(arg => arg.Absolute("Template"));
			}

			[Test]
			public void Must_use_absolute_path_extension()
			{
				_codeDomProviderFactory.AssertWasCalled(arg => arg.CreateFromFileExtension(".cshtml"));
			}

			[Test]
			public void Must_use_cached_type_on_subsequent_calls()
			{
				_repository.Get<ITemplate<string>, string>("Template", "Model", _namespaces);
				_compiler.AssertWasCalled(
					arg => arg.Compile<ITemplate<string>>(
						Arg<string>.Is.Anything,
						Arg<string>.Is.Anything,
						Arg<ITemplateCodeBuilder>.Is.Anything,
						Arg<CodeDomProvider>.Is.Anything,
						Arg<Action<CodeTypeDeclaration>>.Is.Anything,
						Arg<IEnumerable<string>>.Is.Anything),
					options => options.Repeat.Once());
			}

			[Test]
			public void Must_use_provided_classnamebuilder()
			{
				_classNameBuilder.AssertWasCalled(arg => arg.BuildFromRandomGuid());
			}

			[Test]
			public void Must_use_provided_codedomproviderfactory()
			{
				_codeDomProviderFactory.AssertWasCalled(arg => arg.CreateFromFileExtension(Arg<string>.Is.Anything));
			}

			[Test]
			public void Must_use_provided_compiledtemplatefactory()
			{
				_compiledTemplateFactory.AssertWasCalled(arg => arg.CreateFromType<string>(Arg<Type>.Is.Anything));
			}

			[Test]
			public void Must_use_provided_compiler()
			{
				_compiler.AssertWasCalled(arg => arg.Compile<ITemplate<string>>("Hello, world.", "ClassName", _codeBuilder, _codeDomProvider, null, _namespaces));
			}
		}

		[TestFixture]
		public class When_getting_template_without_model
		{
			[SetUp]
			public void SetUp()
			{
				_pathResolver = MockRepository.GenerateMock<ITemplatePathResolver>();
				_pathResolver.Stub(arg => arg.Absolute("Template")).Return(@"C:\Template.cshtml");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.OpenFile(@"C:\Template.cshtml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).Return(new MemoryStream(Encoding.ASCII.GetBytes("Hello, world.")));
				_compiler = MockRepository.GenerateMock<ITemplateCompiler>();
				_classNameBuilder = MockRepository.GenerateMock<ITemplateClassNameBuilder>();
				_classNameBuilder.Stub(arg => arg.BuildFromRandomGuid()).Return("ClassName");
				_codeBuilder = MockRepository.GenerateMock<ITemplateCodeBuilder>();
				_codeDomProvider = MockRepository.GenerateMock<CodeDomProvider>();
				_codeDomProviderFactory = MockRepository.GenerateMock<ICodeDomProviderFactory>();
				_codeDomProviderFactory.Stub(arg => arg.CreateFromFileExtension(Arg<string>.Is.Anything)).Return(_codeDomProvider);
				_template = MockRepository.GenerateMock<ITemplate>();
				_compiledTemplateFactory = MockRepository.GenerateMock<ICompiledTemplateFactory>();
				_compiledTemplateFactory.Stub(arg => arg.CreateFromType(Arg<Type>.Is.Anything)).Return(_template);
				_fileSystemRepositoryConfiguration = MockRepository.GenerateMock<IFileSystemRepositoryConfiguration>();
				_repository = new FileSystemRepository(_pathResolver, _fileSystem, _compiler, _classNameBuilder, _codeBuilder, _codeDomProviderFactory, _compiledTemplateFactory, _fileSystemRepositoryConfiguration);
				_repository.Get<ITemplate>("Template", _namespaces);
			}

			private readonly IEnumerable<string> _namespaces = "System.Text".ToEnumerable();
			private ITemplatePathResolver _pathResolver;
			private IFileSystem _fileSystem;
			private ITemplateCompiler _compiler;
			private ITemplateClassNameBuilder _classNameBuilder;
			private ITemplateCodeBuilder _codeBuilder;
			private ICodeDomProviderFactory _codeDomProviderFactory;
			private ICompiledTemplateFactory _compiledTemplateFactory;
			private FileSystemRepository _repository;
			private ITemplate _template;
			private CodeDomProvider _codeDomProvider;
			private IFileSystemRepositoryConfiguration _fileSystemRepositoryConfiguration;

			[Test]
			public void Must_read_file_contents()
			{
				_fileSystem.AssertWasCalled(arg => arg.OpenFile(@"C:\Template.cshtml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			}

			[Test]
			public void Must_resolve_absolute_path()
			{
				_pathResolver.AssertWasCalled(arg => arg.Absolute("Template"));
			}

			[Test]
			public void Must_use_absolute_path_extension()
			{
				_codeDomProviderFactory.AssertWasCalled(arg => arg.CreateFromFileExtension(".cshtml"));
			}

			[Test]
			public void Must_use_cached_type_on_subsequent_calls()
			{
				_repository.Get<ITemplate>("Template", _namespaces);
				_compiler.AssertWasCalled(
					arg => arg.Compile<ITemplate>(
						Arg<string>.Is.Anything,
						Arg<string>.Is.Anything,
						Arg<ITemplateCodeBuilder>.Is.Anything,
						Arg<CodeDomProvider>.Is.Anything,
						Arg<Action<CodeTypeDeclaration>>.Is.Anything,
						Arg<IEnumerable<string>>.Is.Anything),
					options => options.Repeat.Once());
			}

			[Test]
			public void Must_use_provided_classnamebuilder()
			{
				_classNameBuilder.AssertWasCalled(arg => arg.BuildFromRandomGuid());
			}

			[Test]
			public void Must_use_provided_codedomproviderfactory()
			{
				_codeDomProviderFactory.AssertWasCalled(arg => arg.CreateFromFileExtension(Arg<string>.Is.Anything));
			}

			[Test]
			public void Must_use_provided_compiledtemplatefactory()
			{
				_compiledTemplateFactory.AssertWasCalled(arg => arg.CreateFromType(Arg<Type>.Is.Anything));
			}

			[Test]
			public void Must_use_provided_compiler()
			{
				_compiler.AssertWasCalled(arg => arg.Compile<ITemplate>("Hello, world.", "ClassName", _codeBuilder, _codeDomProvider, null, _namespaces));
			}
		}
	}
}