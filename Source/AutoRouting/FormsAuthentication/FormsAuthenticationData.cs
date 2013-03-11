using System;
using System.Web;
using System.Web.Security;

using Junior.Common;

using Newtonsoft.Json;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public class FormsAuthenticationData<TUserData> : IFormsAuthenticationData<TUserData>
		where TUserData : class
	{
		private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat };

		public TUserData GetUserData(HttpRequestBase request, string cookieName = ".juniorauth")
		{
			request.ThrowIfNull("request");

			FormsAuthenticationTicket ticket = GetTicket(request, cookieName);

			return ticket != null && !ticket.Expired ? JsonConvert.DeserializeObject<TUserData>(ticket.UserData ?? "{}", _serializerSettings) : null;
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

				return !ticket.Expired ? ticket : null;
			}
			catch (ArgumentException)
			{
				return null;
			}
		}
	}
}