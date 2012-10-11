using System.Reflection;
using System.Web;

using Spark;

namespace NathanAlden.JuniorRouting.Diagnostics.Responses
{
	public abstract class View : AbstractSparkView
	{
		private readonly string _version;

		protected View()
		{
			_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public abstract string Title
		{
			get;
		}

		public string AssemblyVersion
		{
			get
			{
				return _version;
			}
		}

		public virtual string RootUrl
		{
			get
			{
				string rootUrl = HttpRuntime.AppDomainAppVirtualPath;

				return rootUrl.EndsWith("/") ? rootUrl : rootUrl + "/";
			}
		}
	}
}