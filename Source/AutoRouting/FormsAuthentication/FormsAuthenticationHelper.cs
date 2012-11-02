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
		private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
			{
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				Formatting = Formatting.None
			};
		private readonly ISystemClock _systemClock;

		public FormsAuthenticationHelper(ISystemClock systemClock)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		public void RemoveCookie()
		{
			System.Web.Security.FormsAuthentication.SignOut();
		}

		public Cookie GenerateCookie(string jsonUserData, DateTime expiration, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/")
		{
			jsonUserData.ThrowIfNull("jsonUserData");

			var ticket = new FormsAuthenticationTicket(1, cookieName, _systemClock.LocalDateTime, expiration, persistent, jsonUserData, cookiePath);
			string encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);
			var cookie = new HttpCookie(cookieName, encryptedTicket)
				{
					HttpOnly = true
				};

			if (ticket.IsPersistent)
			{
				cookie.Expires = ticket.Expiration;
			}

			return new Cookie(cookie);
		}

		public Cookie GenerateCookie(object jsonUserData, DateTime expiration, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/")
		{
			jsonUserData.ThrowIfNull("jsonUserData");

			string serializedJsonUserData = JsonConvert.SerializeObject(jsonUserData, _serializerSettings);

			return GenerateCookie(serializedJsonUserData, expiration, persistent, cookieName, cookiePath);
		}
	}
}