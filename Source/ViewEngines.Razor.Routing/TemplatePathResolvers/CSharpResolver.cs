using Junior.Route.Routing;

namespace Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers
{
	public class CSharpResolver : TemplatePathResolver
	{
		public CSharpResolver(IHttpRuntime httpRuntime)
			: base(".cshtml", httpRuntime)
		{
		}
	}
}