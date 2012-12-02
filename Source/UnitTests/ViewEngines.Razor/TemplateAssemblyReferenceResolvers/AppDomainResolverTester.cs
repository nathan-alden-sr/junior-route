using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.ViewEngines.Razor.TemplateAssemblyReferenceResolvers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateAssemblyReferenceResolvers
{
	public static class AppDomainResolverTester
	{
		[TestFixture]
		public class When_resolving_assemblies
		{
			[SetUp]
			public void SetUp()
			{
				_resolver = new AppDomainResolver();
			}

			private AppDomainResolver _resolver;

			[Test]
			public void Must_resolve_only_non_dynamic_assemblies()
			{
				IEnumerable<string> locations = _resolver.ResolveAssemblyLocations();
				IEnumerable<Uri> uris = locations.Select(arg => new Uri(arg));

				Assert.That(uris.All(arg => arg.IsFile), Is.True);
			}
		}
	}
}