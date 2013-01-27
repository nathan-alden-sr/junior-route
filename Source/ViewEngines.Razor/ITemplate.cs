using Junior.Route.ViewEngines.Razor.TemplateRepositories;

namespace Junior.Route.ViewEngines.Razor
{
	public interface ITemplate
	{
		ITemplateRepository TemplateRepository
		{
			get;
			set;
		}

		string Run();
		string Run(TemplateRunContext context);
		void Execute();
		void Write(object value);
		void WriteLiteral(string value);
	}

	public interface ITemplate<TModel> : ITemplate
	{
		TModel Model
		{
			get;
			set;
		}
	}
}