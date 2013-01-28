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
				.Where(arg => !arg.IsDynamic && !String.IsNullOrEmpty(arg.Location))
				.Select(arg => arg.Location)
				.ToArray();
		}
	}
}