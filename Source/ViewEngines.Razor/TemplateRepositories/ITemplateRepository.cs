using System.Collections.Generic;

namespace Junior.Route.ViewEngines.Razor.TemplateRepositories
{
	public interface ITemplateRepository
	{
		TTemplate Get<TTemplate>(string key, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate;

		TTemplate Get<TTemplate>(string key)
			where TTemplate : ITemplate;

		TTemplate Get<TTemplate, TModel>(string key, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>;

		TTemplate Get<TTemplate, TModel>(string key, TModel model)
			where TTemplate : ITemplate<TModel>;

		Template Get(string key, IEnumerable<string> namespaceImports);
		Template Get(string key);
		Template<TModel> Get<TModel>(string key, TModel model, IEnumerable<string> namespaceImports);
		Template<TModel> Get<TModel>(string key, TModel model);

		string Run<TTemplate>(string key, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate;

		string Run<TTemplate>(string key)
			where TTemplate : ITemplate;

		string Run<TTemplate, TModel>(string key, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>;

		string Run<TTemplate, TModel>(string key, TModel model)
			where TTemplate : ITemplate<TModel>;

		string Run(string key, IEnumerable<string> namespaceImports);
		string Run(string key);
		string Run<TModel>(string key, TModel model, IEnumerable<string> namespaceImports);
		string Run<TModel>(string key, TModel model);
	}
}