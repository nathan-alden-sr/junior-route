using System.Collections.Generic;

namespace Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories
{
	public interface ITemplateRepository
	{
		string Execute<TTemplate>(string relativePath, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate;

		string Execute<TTemplate>(string relativePath)
			where TTemplate : ITemplate;

		string Execute<TTemplate, TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>;

		string Execute<TTemplate, TModel>(string relativePath, TModel model)
			where TTemplate : ITemplate<TModel>;

		string Execute(string relativePath, IEnumerable<string> namespaceImports);
		string Execute(string relativePath);
		string Execute<TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports);
		string Execute<TModel>(string relativePath, TModel model);
	}
}