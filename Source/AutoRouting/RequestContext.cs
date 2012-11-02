using System.Web;

namespace Junior.Route.AutoRouting
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