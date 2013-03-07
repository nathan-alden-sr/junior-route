using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;

using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateCodeBuilders
{
	public static class CSharpCodeGeneratorTester
	{
		[TestFixture]
		public class When_throw_exception_parameter_is_false_and_razor_error_occurs
		{
			[SetUp]
			public void SetUp()
			{
				_host = MockRepository.GenerateMock<RazorEngineHost>();
				_generator = new CSharpCodeGenerator("ClassName", "RootNamespace", @"Test.cshtml", _host, false);
			}

			private RazorEngineHost _host;
			private CSharpCodeGenerator _generator;

			[Test]
			public void Must_not_throw_exception()
			{
				Assert.That(() => _generator.VisitError(new RazorError("", 0, 0, 0)), Throws.Nothing);
			}
		}

		[TestFixture]
		public class When_throw_exception_parameter_is_true_and_razor_error_occurs
		{
			[SetUp]
			public void SetUp()
			{
				_host = MockRepository.GenerateMock<RazorEngineHost>();
				_generator = new CSharpCodeGenerator("ClassName", "RootNamespace", @"Test.cshtml", _host);
			}

			private RazorEngineHost _host;
			private CSharpCodeGenerator _generator;

			[Test]
			public void Must_throw_exception()
			{
				Assert.That(() => _generator.VisitError(new RazorError("", 0, 0, 0)), Throws.InstanceOf<TemplateParsingException>());
			}
		}
	}
}