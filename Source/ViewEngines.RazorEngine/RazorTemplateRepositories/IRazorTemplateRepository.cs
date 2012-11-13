namespace Junior.Route.ViewEngines.RazorEngine.RazorTemplateRepositories
{
	public interface IRazorTemplateRepository
	{
		IRazorTemplate GetByName(string name);
	}
}