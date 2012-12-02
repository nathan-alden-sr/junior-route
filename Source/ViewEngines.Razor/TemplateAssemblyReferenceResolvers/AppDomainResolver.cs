using System;
using System.Collections.Generic;
using System.Linq;

namespace Junior.Route.ViewEngines.Razor.TemplateAssemblyReferenceResolvers
{
	public class AppDomainResolver : ITemplateAssemblyReferenceResolver
	{
		public IEnumerable<string> ResolveAssemblyLocations()
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.Where(arg => !arg.IsDynamic)
				.Select(arg => arg.Location)
				.ToArray();
		}
	}
}