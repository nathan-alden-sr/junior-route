using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

using Junior.Common;

using Newtonsoft.Json;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public class FormsAuthenticationData<TUserData> : IFormsAuthenticationData<TUserData>
		where TUserData : class
	{
		private readonly IFormsAuthenticationConfiguration _configuration;

		public FormsAuthenticationData(IFormsAuthenticationConfiguration configuration)
		{
			configuration.ThrowIfNull("configuration");

			_configuration = configuration;
		}

		public TUserData GetUserData(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			FormsAuthenticationTicket ticket = GetTicket(request, _configuration.CookieName);

			return ticket != null && !ticket.Expired ? JsonConvert.DeserializeObject<TUserData>(ticket.UserData ?? "{}") : null;
		}

		private static FormsAuthenticationTicket GetTicket(HttpRequestBase request, string cookieName)
		{
			HttpCookie cookie = request.Cookies[cookieName];

			if (cookie == null)
			{
				return null;
			}

			try
			{
				FormsAuthenticationTicket ticket = System.Web.Security.FormsAuthentication.Decrypt(cookie.Value);

				return ticket != null && !ticket.Expired ? ticket : null;
			}
			catch (ArgumentException)
			{
				return null;
			}
			catch (HttpException)
			{
				return null;
			}
			catch (CryptographicException)
			{
				return null;
			}
		}
	}
}