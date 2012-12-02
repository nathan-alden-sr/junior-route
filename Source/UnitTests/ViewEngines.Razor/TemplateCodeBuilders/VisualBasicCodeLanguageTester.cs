using System.Web.Razor;
using System.Web.Razor.Generator;

using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateCodeBuilders
{
	public static class VisualBasicCodeLanguageTester
	{
		[TestFixture]
		public class When_creating_code_generator
		{
			[Test]
			public void Must_create_visualbasiccodegenerator()
			{
				var language = new VisualBasicCodeLanguage();
				var host = MockRepository.GenerateMock<RazorEngineHost>();
				RazorCodeGenerator generator = language.CreateCodeGenerator("ClassName", "RootNamespace", "Template.vbhtml", host);

				Assert.That(generator, Is.InstanceOf<VisualBasicCodeGenerator>());
			}

			[Test]
			[TestCase(true)]
			[TestCase(false)]
			public void Must_honor_exception_parameter(bool throwExceptionOnParserError)
			{
				var language = new VisualBasicCodeLanguage(throwExceptionOnParserError);
				var host = MockRepository.GenerateMock<RazorEngineHost>();
				RazorCodeGenerator generator = language.CreateCodeGenerator("ClassName", "RootNamespace", "Template.vbhtml", host);

				Assert.That(((VisualBasicCodeGenerator)generator).ThrowExceptionOnParserError, Is.EqualTo(throwExceptionOnParserError));
			}

			[Test]
			public void Must_set_properties()
			{
				var language = new VisualBasicCodeLanguage();

				Assert.That(language.ThrowExceptionOnParserError, Is.True);
			}
		}
	}
}