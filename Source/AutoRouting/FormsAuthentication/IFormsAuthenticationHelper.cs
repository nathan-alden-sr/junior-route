using System;

using Junior.Route.Routing.Responses;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public interface IFormsAuthenticationHelper
	{
		Cookie GenerateCookie(string jsonUserData, DateTime expiration, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/");
		Cookie GenerateCookie(object jsonUserData, DateTime expiration, bool persistent = false, string cookieName = ".juniorauth", string cookiePath = "/");
		void RemoveCookie();
	}
}