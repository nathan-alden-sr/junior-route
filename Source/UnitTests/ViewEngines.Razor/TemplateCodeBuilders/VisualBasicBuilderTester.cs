using System.CodeDom;
using System.Linq;

using Junior.Route.ViewEngines.Razor;
using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateCodeBuilders
{
	public static class VisualBasicBuilderTester
	{
		[TestFixture]
		public class When_building_code
		{
			[SetUp]
			public void SetUp()
			{
				_builder = new VisualBasicBuilder();
			}

			private VisualBasicBuilder _builder;

			[Test]
			public void Must_build_correct_type_name()
			{
				BuildCodeResult result = _builder.BuildCode<ITemplate>("Template", "ClassName", null);

				Assert.That(result.TypeFullName, Is.EqualTo("Junior.Route.ViewEngines.Razor.TemplateCodeBuilders.DynamicTemplates.ClassName"));
			}

			[Test]
			public void Must_call_supplied_configuration_delegate()
			{
				bool delegateExecuted = false;

				_builder.BuildCode<ITemplate>("Template", "ClassName", configurationDelegate => delegateExecuted = true);

				Assert.That(delegateExecuted, Is.True);
			}

			[Test]
			public void Must_compile_correct_namespaces()
			{
				BuildCodeResult result = _builder.BuildCode<ITemplate>("Template", "ClassName", null);

				Assert.That(result.CompileUnit.Namespaces.Cast<CodeNamespace>().Select(arg => arg.Name), Is.EquivalentTo(new[] { "Junior.Route.ViewEngines.Razor.TemplateCodeBuilders.DynamicTemplates" }));
			}
		}
	}
}