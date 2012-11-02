using System.Web;

namespace Junior.Route.AutoRouting
{
	public interface IRequestContext
	{
		HttpRequestBase Request
		{
			get;
		}
	}
}