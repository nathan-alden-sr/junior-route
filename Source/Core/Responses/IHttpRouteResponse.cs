using System.Web;

namespace NathanAlden.JuniorRouting.Core.Responses
{
	public interface IHttpRouteResponse
	{
		void WriteResponse(HttpResponseBase response);
	}
}