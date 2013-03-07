using System.Web;

namespace Junior.Route.AutoRouting
{
	public interface IResponseContext
	{
		HttpResponseBase Response
		{
			get;
		}
	}
}