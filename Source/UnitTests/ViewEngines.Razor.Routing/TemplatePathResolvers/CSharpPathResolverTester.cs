using Junior.Route.Routing;
using Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.Razor.Routing.TemplatePathResolvers
{
	public static class CSharpPathResolverTester
	{
		[TestFixture]
		public class When_resolving_template_path_with_extension
		{
			[SetUp]
			public void SetUp()
			{
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppPath).Return(@"C:\Test");
				_resolver = new CSharpResolver(_httpRuntime);
			}

			private IHttpRuntime _httpRuntime;
			private CSharpResolver _resolver;

			[Test]
			public void Must_resolve_with_provided_extension()
			{
				string path = _resolver.Absolute(@"Templates\Foo.custom");

				Assert.That(path, Is.EqualTo(@"C:\Test\Templates\Foo.custom"));
			}
		}

		[TestFixture]
		public class When_resolving_template_path_without_extension
		{
			[SetUp]
			public void SetUp()
			{
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_httpRuntime.Stub(arg => arg.AppDomainAppPath).Return(@"C:\Test");
				_resolver = new CSharpResolver(_httpRuntime);
			}

			private IHttpRuntime _httpRuntime;
			private CSharpResolver _resolver;

			[Test]
			public void Must_resolve_with_default_extension()
			{
				string path = _resolver.Absolute(@"Templates\Foo");

				Assert.That(path, Is.EqualTo(@"C:\Test\Templates\Foo.cshtml"));
			}
		}
	}
}