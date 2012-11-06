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
		private readonly IRequestContext _requestContext;
		private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
			{
				DateFormatHandling = DateFormatHandling.IsoDateFormat
			};

		public FormsAuthenticationData(IRequestContext requestContext)
		{
			requestContext.ThrowIfNull("requestContext");

			_requestContext = requestContext;
		}

		public TUserData GetUserData(string cookieName = ".juniorauth")
		{
			FormsAuthenticationTicket ticket = GetTicket(cookieName);

			return ticket != null && !ticket.Expired ? JsonConvert.DeserializeObject<TUserData>(ticket.UserData ?? "{}", _serializerSettings) : null;
		}

		private FormsAuthenticationTicket GetTicket(string cookieName)
		{
			HttpCookie cookie = _requestContext.Request.Cookies[cookieName];

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