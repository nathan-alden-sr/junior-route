using System.Diagnostics;
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
			try
			{
				Assembly coreAssembly = typeof(Routing.Route).Assembly;
				FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(coreAssembly.Location);

				_version = fileVersionInfo.FileVersion;
			}
			catch
			{
				_version = "(unknown)";
			}
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