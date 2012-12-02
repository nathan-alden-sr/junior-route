using Junior.Route.Routing;

namespace Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers
{
	public class VisualBasicResolver : TemplatePathResolver
	{
		public VisualBasicResolver(IHttpRuntime httpRuntime)
			: base(".vbhtml", httpRuntime)
		{
		}
	}
}