using System.Web;
using System.Web.Caching;

using Junior.Route.Routing;

namespace Junior.Route.AspNetIntegration
{
	public class HttpRuntimeWrapper : IHttpRuntime
	{
		public string AppDomainAppPath
		{
			get
			{
				return HttpRuntime.AppDomainAppPath;
			}
		}

		public string AppDomainAppVirtualPath
		{
			get
			{
				return HttpRuntime.AppDomainAppVirtualPath;
			}
		}

		public Cache Cache
		{
			get
			{
				return HttpRuntime.Cache;
			}
		}
	}
}