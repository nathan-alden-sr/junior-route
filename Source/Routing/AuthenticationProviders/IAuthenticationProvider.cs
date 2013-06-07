using System.Threading.Tasks;
using System.Web;

using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.AuthenticationProviders
{
	public interface IAuthenticationProvider
	{
		Task<AuthenticationResult> AuthenticateAsync(HttpRequestBase request, HttpResponseBase response, Route route);
		Task<IResponse> GetFailedAuthenticationResponseAsync(HttpRequestBase request);
	}
}