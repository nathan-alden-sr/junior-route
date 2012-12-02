namespace Junior.Route.ViewEngines.Razor
{
	public interface ITemplate
	{
		string Contents
		{
			get;
		}

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