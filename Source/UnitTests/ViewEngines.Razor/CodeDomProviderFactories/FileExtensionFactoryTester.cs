using System;

using Junior.Route.ViewEngines.Razor.CodeDomProviderFactories;

using Microsoft.CSharp;
using Microsoft.VisualBasic;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor.CodeDomProviderFactories
{
	public static class FileExtensionFactoryTester
	{
		[TestFixture]
		public class When_creating_codedomprovider_from_cshtml_extension
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new FileExtensionFactory();
			}

			private FileExtensionFactory _factory;

			[Test]
			[TestCase(".cshtml")]
			[TestCase(".CSHTML")]
			public void Must_create_csharpcodeprovider(string extension)
			{
				Assert.That(_factory.CreateFromFileExtension(extension), Is.TypeOf<CSharpCodeProvider>());
			}
		}

		[TestFixture]
		public class When_creating_codedomprovider_from_unknown_extension
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new FileExtensionFactory();
			}

			private FileExtensionFactory _factory;

			[Test]
			[TestCase(".")]
			[TestCase("")]
			[TestCase(".cs")]
			[TestCase(".vb")]
			public void Must_throw_exception(string extension)
			{
				Assert.Throws<ArgumentException>(() => _factory.CreateFromFileExtension(extension));
			}
		}

		[TestFixture]
		public class When_creating_codedomprovider_from_vbhtml_extension
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new FileExtensionFactory();
			}

			private FileExtensionFactory _factory;

			[Test]
			[TestCase(".vbhtml")]
			[TestCase(".VBHTML")]
			public void Must_create_csharpcodeprovider(string extension)
			{
				Assert.That(_factory.CreateFromFileExtension(extension), Is.TypeOf<VBCodeProvider>());
			}
		}
	}
}