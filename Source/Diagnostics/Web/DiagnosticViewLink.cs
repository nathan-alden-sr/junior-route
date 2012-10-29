namespace Junior.Route.Diagnostics.Web
{
	public class DiagnosticViewLink
	{
		private readonly string _description;
		private readonly string _url;

		public DiagnosticViewLink(string url, string description)
		{
			_url = url;
			_description = description;
		}

		public string Url
		{
			get
			{
				return _url;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
		}
	}
}