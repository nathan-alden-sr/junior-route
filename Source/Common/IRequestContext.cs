using System.Web;

namespace Junior.Route.Common
{
	public interface IRequestContext
	{
		HttpRequestBase Request
		{
			get;
		}
	}
}