using System;

using Junior.Route.Routing.Responses;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public interface IFormsAuthenticationHelper
	{
		Cookie GenerateTicket(DateTime expiration, string jsonUserData = null, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/");
		Cookie GenerateTicket(DateTime expiration, object jsonUserData = null, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/");
		void RemoveTicket();
	}
}