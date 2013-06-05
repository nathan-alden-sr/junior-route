using System.Web;

using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.AuthenticationProviders
{
	public interface IAuthenticationProvider
	{
		AuthenticationResult Authenticate(HttpRequestBase request, HttpResponseBase response, Route route);
		IResponse GetFailedAuthenticationResponse(HttpRequestBase request);
	}
}