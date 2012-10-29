using System.Web;

namespace Junior.Route.Common
{
	public class RequestContext : IRequestContext
	{
		public HttpRequestBase Request
		{
			get
			{
				return new HttpRequestWrapper(HttpContext.Current.Request);
			}
		}
	}
}