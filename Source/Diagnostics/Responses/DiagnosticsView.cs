using Junior.Common;

namespace NathanAlden.JuniorRouting.Diagnostics.Responses
{
	public abstract class DiagnosticsView : View
	{
		public override string Title
		{
			get
			{
				return "Diagnostics - JuniorRouting";
			}
		}

		public override string RootUrl
		{
			get
			{
				return base.RootUrl + BaseUrl;
			}
		}

		private string BaseUrl
		{
			get;
			set;
		}

		public void Populate(string baseUrl)
		{
			baseUrl.ThrowIfNull("baseUrl");

			BaseUrl = baseUrl;
		}
	}
}