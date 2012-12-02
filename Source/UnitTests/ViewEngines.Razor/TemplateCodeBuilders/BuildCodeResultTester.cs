using System.CodeDom;

using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateCodeBuilders
{
	public static class BuildCodeResultTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_codeCompileUnit = new CodeCompileUnit();
				_result = new BuildCodeResult(_codeCompileUnit, "TypeName");
			}

			private BuildCodeResult _result;
			private CodeCompileUnit _codeCompileUnit;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.CompileUnit, Is.SameAs(_codeCompileUnit));
				Assert.That(_result.TypeFullName, Is.EqualTo("TypeName"));
			}
		}
	}
}