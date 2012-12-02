namespace Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers
{
	public interface ITemplatePathResolver
	{
		string Absolute(string relativePath);
	}
}