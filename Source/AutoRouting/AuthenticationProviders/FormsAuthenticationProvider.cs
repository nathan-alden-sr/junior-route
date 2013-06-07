using System;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.FormsAuthentication;
using Junior.Route.Common;
using Junior.Route.Routing.AuthenticationProviders;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AutoRouting.AuthenticationProviders
{
	public class FormsAuthenticationProvider : IAuthenticationProvider
	{
		private readonly Func<string> _failedAuthenticationRedirectAbsoluteUrlDelegate;
		private readonly IFormsAuthenticationHelper _helper;
		private readonly string _returnUrlQueryStringField;

		private FormsAuthenticationProvider(IFormsAuthenticationHelper helper, Func<string> failedAuthenticationRedirectAbsoluteUrlDelegate = null, string returnUrlQueryStringField = null)
		{
			_helper = helper;
			_failedAuthenticationRedirectAbsoluteUrlDelegate = failedAuthenticationRedirectAbsoluteUrlDelegate;
			_returnUrlQueryStringField = returnUrlQueryStringField;
		}

		public Task<AuthenticationResult> AuthenticateAsync(HttpRequestBase request, HttpResponseBase response, Routing.Route route)
		{
			request.ThrowIfNull("request");
			response.ThrowIfNull("response");
			route.ThrowIfNull("route");

			if (!_helper.IsTicketValid(request))
			{
				return AuthenticationResult.AuthenticationFailed.AsCompletedTask();
			}

			Cookie cookie = _helper.RenewTicket(request);

			response.Cookies.Remove(cookie.Name);
			response.Cookies.Add(cookie.GetHttpCookie());

			return AuthenticationResult.AuthenticationSucceeded.AsCompletedTask();
		}

		public Task<IResponse> GetFailedAuthenticationResponseAsync(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			if (_failedAuthenticationRedirectAbsoluteUrlDelegate == null)
			{
				return new Response().Unauthorized().AsCompletedTask<IResponse>();
			}

			string absoluteUrl = _failedAuthenticationRedirectAbsoluteUrlDelegate();
			Uri url;

			if (!Uri.TryCreate(absoluteUrl, UriKind.RelativeOrAbsolute, out url))
			{
				throw new InvalidOperationException("Invalid failed authentication redirect URL.");
			}

			if (_returnUrlQueryStringField != null)
			{
				string returnUrlQueryString = String.Format("{0}={1}", _returnUrlQueryStringField, HttpUtility.UrlEncode(request.RawUrl));

				absoluteUrl += (absoluteUrl.IndexOf('?') > -1 ? "&" : "?") + returnUrlQueryString;
			}

			return (String.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) ? new Response().Found(absoluteUrl) : new Response().SeeOther(absoluteUrl)).AsCompletedTask<IResponse>();
		}

		public static FormsAuthenticationProvider CreateWithNoRedirectOnFailedAuthentication(IFormsAuthenticationHelper helper)
		{
			helper.ThrowIfNull("helper");

			return new FormsAuthenticationProvider(helper);
		}

		public static FormsAuthenticationProvider CreateWithRouteRedirectOnFailedAuthentication(IFormsAuthenticationHelper helper, IUrlResolver urlResolver, string routeName, string returnUrlQueryStringField = "ReturnURL")
		{
			helper.ThrowIfNull("helper");
			urlResolver.ThrowIfNull("urlResolver");
			routeName.ThrowIfNull("routeName");
			if (returnUrlQueryStringField == "")
			{
				throw new ArgumentException("Return URL query string field cannot be an empty string.", "returnUrlQueryStringField");
			}

			return new FormsAuthenticationProvider(helper, () => urlResolver.Route(routeName), returnUrlQueryStringField);
		}

		public static FormsAuthenticationProvider CreateWithRouteRedirectOnFailedAuthentication(IFormsAuthenticationHelper helper, IUrlResolver urlResolver, Guid routeId, string returnUrlQueryStringField = "ReturnURL")
		{
			helper.ThrowIfNull("helper");
			urlResolver.ThrowIfNull("urlResolver");
			if (returnUrlQueryStringField == "")
			{
				throw new ArgumentException("Return URL query string field cannot be an empty string.", "returnUrlQueryStringField");
			}

			return new FormsAuthenticationProvider(helper, () => urlResolver.Route(routeId), returnUrlQueryStringField);
		}

		public static FormsAuthenticationProvider CreateWithRelativeUrlRedirectOnFailedAuthentication(IFormsAuthenticationHelper helper, IUrlResolver urlResolver, string relativeUrl, string returnUrlQueryStringField = "ReturnURL")
		{
			helper.ThrowIfNull("helper");
			urlResolver.ThrowIfNull("urlResolver");
			relativeUrl.ThrowIfNull("relativeUrl");
			if (returnUrlQueryStringField == "")
			{
				throw new ArgumentException("Return URL query string field cannot be an empty string.", "returnUrlQueryStringField");
			}

			return new FormsAuthenticationProvider(helper, () => urlResolver.Absolute(relativeUrl), returnUrlQueryStringField);
		}
	}
}