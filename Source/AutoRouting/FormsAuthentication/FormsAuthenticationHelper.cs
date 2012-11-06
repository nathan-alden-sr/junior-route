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

		public void RemoveTicket()
		{
			System.Web.Security.FormsAuthentication.SignOut();
		}

		public Cookie GenerateTicket(DateTime expiration, string jsonUserData = null, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/")
		{
			var ticket = new FormsAuthenticationTicket(1, cookieName, _systemClock.LocalDateTime, expiration, persistent, jsonUserData ?? "{}", cookiePath);
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

		public Cookie GenerateTicket(DateTime expiration, object jsonUserData, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/")
		{
			string serializedJsonUserData = jsonUserData.IfNotNull(arg => JsonConvert.SerializeObject(jsonUserData, _serializerSettings));

			return GenerateTicket(expiration, serializedJsonUserData, persistent, cookieName, cookiePath);
		}
	}
}