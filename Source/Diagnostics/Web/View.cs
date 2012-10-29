using System.Reflection;

using Junior.Route.Common;

using Spark;

namespace Junior.Route.Diagnostics.Web
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

		public IUrlResolver UrlResolver
		{
			get;
			set;
		}

		public string AssemblyVersion
		{
			get
			{
				return _version;
			}
		}
	}
}