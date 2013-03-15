using System;
using System.Web;
using System.Web.Security;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.AuthenticationProviders
{
	public class FormsAuthenticationProvider : IAuthenticationProvider
	{
		private readonly bool _appendReturnUrl;
		private readonly string _cookieName;
		private readonly Func<string> _failedAuthenticationRedirectAbsoluteUrlDelegate;
		private readonly string _returnUrlQueryStringField;

		private FormsAuthenticationProvider(Func<string> failedAuthenticationRedirectAbsoluteUrlDelegate, bool appendReturnUrl, string returnUrlQueryStringField, string cookieName = ".juniorauth")
		{
			_failedAuthenticationRedirectAbsoluteUrlDelegate = failedAuthenticationRedirectAbsoluteUrlDelegate;
			_appendReturnUrl = appendReturnUrl;
			_returnUrlQueryStringField = returnUrlQueryStringField;
			_cookieName = cookieName;
		}

		public AuthenticationResult Authenticate(HttpRequestBase request, Route route)
		{
			request.ThrowIfNull("request");
			route.ThrowIfNull("route");

			HttpCookie cookie = request.Cookies[_cookieName];

			if (cookie == null)
			{
				return AuthenticationResult.AuthenticationFailed;
			}

			try
			{
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

				return ticket.Expired ? AuthenticationResult.AuthenticationFailed : AuthenticationResult.AuthenticationSucceeded;
			}
			catch (ArgumentException)
			{
				return AuthenticationResult.AuthenticationFailed;
			}
		}

		public IResponse GetFailedAuthenticationResponse(HttpRequestBase request)
		{
			if (_failedAuthenticationRedirectAbsoluteUrlDelegate == null)
			{
				return new Response().Unauthorized();
			}

			string absoluteUrl = _failedAuthenticationRedirectAbsoluteUrlDelegate();
			Uri url;

			if (!Uri.TryCreate(absoluteUrl, UriKind.RelativeOrAbsolute, out url))
			{
				throw new InvalidOperationException("Invalid failed authentication redirect URL.");
			}

			if (_appendReturnUrl)
			{
				string returnUrlQueryString = String.Format("{0}={1}", _returnUrlQueryStringField, HttpUtility.UrlEncode(request.RawUrl));

				absoluteUrl += (absoluteUrl.IndexOf('?') > -1 ? "&" : "?") + returnUrlQueryString;
			}

			return String.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) ? new Response().Found(absoluteUrl) : new Response().SeeOther(absoluteUrl);
		}

		public static FormsAuthenticationProvider CreateWithNoRedirectOnFailedAuthentication()
		{
			return new FormsAuthenticationProvider(null, false, null);
		}

		public static FormsAuthenticationProvider CreateWithRouteRedirectOnFailedAuthentication(IUrlResolver urlResolver, string routeName, bool appendReturnUrl = false, string returnUrlQueryStringField = "ReturnURL", string cookieName = ".juniorauth")
		{
			urlResolver.ThrowIfNull("urlResolver");
			routeName.ThrowIfNull("routeName");
			cookieName.ThrowIfNull("cookieName");

			return new FormsAuthenticationProvider(() => urlResolver.Route(routeName), appendReturnUrl, returnUrlQueryStringField, cookieName);
		}

		public static FormsAuthenticationProvider CreateWithRouteRedirectOnFailedAuthentication(IUrlResolver urlResolver, Guid routeId, bool appendReturnUrl = false, string returnUrlQueryStringField = "ReturnURL", string cookieName = ".juniorauth")
		{
			urlResolver.ThrowIfNull("urlResolver");
			cookieName.ThrowIfNull("cookieName");

			return new FormsAuthenticationProvider(() => urlResolver.Route(routeId), appendReturnUrl, returnUrlQueryStringField, cookieName);
		}

		public static FormsAuthenticationProvider CreateWithRelativeUrlRedirectOnFailedAuthentication(IUrlResolver urlResolver, string relativeUrl, bool appendReturnUrl = false, string returnUrlQueryStringField = "ReturnURL", string cookieName = ".juniorauth")
		{
			urlResolver.ThrowIfNull("urlResolver");
			relativeUrl.ThrowIfNull("relativeUrl");
			cookieName.ThrowIfNull("cookieName");

			return new FormsAuthenticationProvider(() => urlResolver.Absolute(relativeUrl), appendReturnUrl, returnUrlQueryStringField, cookieName);
		}
	}
}