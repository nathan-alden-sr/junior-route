using System.Collections.Generic;

namespace Junior.Route.ViewEngines.Razor.TemplateRepositories
{
	public interface ITemplateRepository
	{
		TTemplate Get<TTemplate>(string relativePath, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate;

		TTemplate Get<TTemplate>(string relativePath)
			where TTemplate : ITemplate;

		TTemplate Get<TTemplate, TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>;

		TTemplate Get<TTemplate, TModel>(string relativePath, TModel model)
			where TTemplate : ITemplate<TModel>;

		Template Get(string relativePath, IEnumerable<string> namespaceImports);
		Template Get(string relativePath);
		Template<TModel> Get<TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports);
		Template<TModel> Get<TModel>(string relativePath, TModel model);

		string Run<TTemplate>(string relativePath, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate;

		string Run<TTemplate>(string relativePath)
			where TTemplate : ITemplate;

		string Run<TTemplate, TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>;

		string Run<TTemplate, TModel>(string relativePath, TModel model)
			where TTemplate : ITemplate<TModel>;

		string Run(string relativePath, IEnumerable<string> namespaceImports);
		string Run(string relativePath);
		string Run<TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports);
		string Run<TModel>(string relativePath, TModel model);
	}
}