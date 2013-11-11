using System;
using System.Web;
using System.Web.Security;

using Junior.Common;
using Junior.Route.Routing.Responses;

using Newtonsoft.Json;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public class FormsAuthenticationHelper : IFormsAuthenticationHelper
	{
		private readonly IFormsAuthenticationConfiguration _configuration;
		private readonly ISystemClock _systemClock;

		public FormsAuthenticationHelper(IFormsAuthenticationConfiguration configuration, ISystemClock systemClock)
		{
			configuration.ThrowIfNull("configuration");
			systemClock.ThrowIfNull("systemClock");

			_configuration = configuration;
			_systemClock = systemClock;
		}

		public Cookie GenerateTicket(DateTime expiration, string jsonUserData = null)
		{
			var ticket = new FormsAuthenticationTicket(1, _configuration.CookieName, _systemClock.LocalDateTime, expiration, _configuration.Persistent, jsonUserData ?? "{}", _configuration.CookiePath);
			string encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);
			var cookie = new HttpCookie(_configuration.CookieName, encryptedTicket)
			{
				HttpOnly = true,
				Path = _configuration.CookiePath,
				Secure = _configuration.RequireSsl,
				Shareable = false
			};

			if (_configuration.CookieDomain != null)
			{
				cookie.Domain = _configuration.CookieDomain;
			}
			if (ticket.IsPersistent)
			{
				cookie.Expires = ticket.Expiration;
			}

			return new Cookie(cookie);
		}

		public Cookie GenerateTicket(DateTime expiration, object jsonUserData)
		{
			string serializedJsonUserData = jsonUserData.IfNotNull(arg => JsonConvert.SerializeObject(jsonUserData));

			return GenerateTicket(expiration, serializedJsonUserData);
		}

		public bool IsTicketValid(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			HttpCookie cookie = GetRequestCookie(request);

			if (cookie == null)
			{
				return false;
			}

			FormsAuthenticationTicket ticket;

			try
			{
				ticket = System.Web.Security.FormsAuthentication.Decrypt(cookie.Value);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (HttpException)
			{
				return false;
			}

			return ticket != null && !ticket.Expired;
		}

		public Cookie RenewTicket(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			HttpCookie cookie = GetRequestCookie(request);

			if (cookie == null)
			{
				return null;
			}

			FormsAuthenticationTicket ticket;

			try
			{
				ticket = System.Web.Security.FormsAuthentication.Decrypt(cookie.Value);
			}
			catch (ArgumentException)
			{
				return null;
			}
			catch (HttpException)
			{
				return null;
			}

			if (ticket == null || ticket.Expired)
			{
				return null;
			}
			if (!_configuration.SlidingExpiration)
			{
				return new Cookie(cookie);
			}

			ticket = System.Web.Security.FormsAuthentication.RenewTicketIfOld(ticket);

			return ticket != null ? GenerateTicket(ticket.Expiration, ticket.UserData) : null;
		}

		public void RemoveTicket(HttpResponseBase response)
		{
			response.ThrowIfNull("response");

			var cookie = new HttpCookie(_configuration.CookieName, "")
			{
				Expires = new DateTime(2000, 01, 01),
				HttpOnly = true,
				Path = _configuration.CookiePath,
				Secure = _configuration.RequireSsl,
				Shareable = false
			};

			if (_configuration.CookieDomain != null)
			{
				cookie.Domain = _configuration.CookieDomain;
			}

			response.Cookies.Remove(_configuration.CookieName);
			response.Cookies.Add(cookie);
		}

		private HttpCookie GetRequestCookie(HttpRequestBase request)
		{
			return request.Cookies[_configuration.CookieName];
		}
	}
}