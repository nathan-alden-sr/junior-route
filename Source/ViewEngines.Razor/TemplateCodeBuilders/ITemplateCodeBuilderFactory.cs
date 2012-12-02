namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public interface ITemplateCodeBuilderFactory
	{
		ITemplateCodeBuilder CreateFromFileExtension(string extension);
	}
}