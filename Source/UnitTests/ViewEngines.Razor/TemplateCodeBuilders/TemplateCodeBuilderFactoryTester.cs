using System;

using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateCodeBuilders
{
	public static class TemplateCodeBuilderFactoryTester
	{
		[TestFixture]
		public class When_creating_instance_from_cshtml_extension
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new TemplateCodeBuilderFactory();
			}

			private TemplateCodeBuilderFactory _factory;

			[Test]
			[TestCase(".cshtml")]
			[TestCase(".CSHTML")]
			public void Must_create_csharpbuilder(string extension)
			{
				Assert.That(_factory.CreateFromFileExtension(extension), Is.InstanceOf<CSharpBuilder>());
			}
		}

		[TestFixture]
		public class When_creating_instance_from_unknown_extension
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new TemplateCodeBuilderFactory();
			}

			private TemplateCodeBuilderFactory _factory;

			[Test]
			[TestCase("")]
			[TestCase(".")]
			[TestCase(".cs")]
			[TestCase(".vb")]
			public void Must_throw_exception(string extension)
			{
				Assert.That(() => _factory.CreateFromFileExtension(extension), Throws.ArgumentException);
			}
		}

		[TestFixture]
		public class When_creating_instance_from_vbhtml_extension
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new TemplateCodeBuilderFactory();
			}

			private TemplateCodeBuilderFactory _factory;

			[Test]
			[TestCase(".vbhtml")]
			[TestCase(".VBHTML")]
			public void Must_create_visualbasicbuilder(string extension)
			{
				Assert.That(_factory.CreateFromFileExtension(extension), Is.InstanceOf<VisualBasicBuilder>());
			}
		}
	}
}