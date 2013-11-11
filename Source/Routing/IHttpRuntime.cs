using System.Web.Caching;

namespace Junior.Route.Routing
{
	public interface IHttpRuntime
	{
		string AppDomainAppPath
		{
			get;
		}

		string AppDomainAppVirtualPath
		{
			get;
		}

		Cache Cache
		{
			get;
		}
	}
}