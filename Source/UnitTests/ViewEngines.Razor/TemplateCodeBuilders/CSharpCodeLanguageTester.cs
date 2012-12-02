using System.Web.Razor;
using System.Web.Razor.Generator;

using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateCodeBuilders
{
	public static class CSharpCodeLanguageTester
	{
		[TestFixture]
		public class When_creating_code_generator
		{
			[Test]
			public void Must_create_csharpcodegenerator()
			{
				var language = new CSharpCodeLanguage();
				var host = MockRepository.GenerateMock<RazorEngineHost>();
				RazorCodeGenerator generator = language.CreateCodeGenerator("ClassName", "RootNamespace", "Template.cshtml", host);

				Assert.That(generator, Is.InstanceOf<CSharpCodeGenerator>());
			}

			[Test]
			[TestCase(true)]
			[TestCase(false)]
			public void Must_honor_exception_parameter(bool throwExceptionOnParserError)
			{
				var language = new CSharpCodeLanguage(throwExceptionOnParserError);
				var host = MockRepository.GenerateMock<RazorEngineHost>();
				RazorCodeGenerator generator = language.CreateCodeGenerator("ClassName", "RootNamespace", "Template.cshtml", host);

				Assert.That(((CSharpCodeGenerator)generator).ThrowExceptionOnParserError, Is.EqualTo(throwExceptionOnParserError));
			}

			[Test]
			public void Must_set_properties()
			{
				var language = new CSharpCodeLanguage();

				Assert.That(language.ThrowExceptionOnParserError, Is.True);
			}
		}
	}
}