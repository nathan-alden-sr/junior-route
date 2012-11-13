using Junior.Common;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateRepositories;

using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Junior.Route.ViewEngines.RazorEngine
{
	public static class RazorEngine
	{
		public static void ConfigureResolver(IRazorTemplateRepository templateRepository)
		{
			templateRepository.ThrowIfNull("templateRepository");

			var configuration = new TemplateServiceConfiguration
				{
					Resolver = new DelegateTemplateResolver(templateName => templateRepository.GetByName(templateName).GetContents())
				};

			Razor.SetTemplateService(new TemplateService(configuration));
		}
	}
}