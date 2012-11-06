using Junior.Common;

namespace Junior.Route.Diagnostics.Web
{
	public class DiagnosticViewLink
	{
		private readonly string _description;
		private readonly string _heading;
		private readonly string _url;

		public DiagnosticViewLink(string heading, string url, string description)
		{
			heading.ThrowIfNull("heading");
			url.ThrowIfNull("url");
			description.ThrowIfNull("description");

			_heading = heading;
			_url = url;
			_description = description;
		}

		public string Heading
		{
			get
			{
				return _heading;
			}
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