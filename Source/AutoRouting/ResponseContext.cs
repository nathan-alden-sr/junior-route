using System.Web;

namespace Junior.Route.AutoRouting
{
	public class ResponseContext : IResponseContext
	{
		public HttpResponseBase Response
		{
			get
			{
				return new HttpResponseWrapper(HttpContext.Current.Response);
			}
		}
	}
}