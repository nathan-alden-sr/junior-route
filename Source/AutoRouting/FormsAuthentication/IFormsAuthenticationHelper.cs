using System;
using System.Web;

using Junior.Route.Routing.Responses;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public interface IFormsAuthenticationHelper
	{
		Cookie GenerateTicket(DateTime expiration, string jsonUserData = null);
		Cookie GenerateTicket(DateTime expiration, object jsonUserData);
		bool IsTicketValid(HttpRequestBase request);
		Cookie RenewTicket(HttpRequestBase request);
		void RemoveTicket(HttpResponseBase response);
	}
}