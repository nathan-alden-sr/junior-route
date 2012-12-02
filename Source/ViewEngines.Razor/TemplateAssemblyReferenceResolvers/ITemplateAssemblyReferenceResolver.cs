using System.Collections.Generic;

namespace Junior.Route.ViewEngines.Razor.TemplateAssemblyReferenceResolvers
{
	public interface ITemplateAssemblyReferenceResolver
	{
		IEnumerable<string> ResolveAssemblyLocations();
	}
}