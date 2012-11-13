namespace Junior.Route.ViewEngines.RazorEngine
{
	public interface IRazorTemplate
	{
		string Name
		{
			get;
		}
		string Path
		{
			get;
		}

		string GetContents();
	}
}