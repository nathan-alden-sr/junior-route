using System.Web;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public interface IFormsAuthenticationData<out TUserData>
		where TUserData : class
	{
		TUserData GetUserData(HttpRequestBase request, string cookieName = ".juniorauth");
	}
}