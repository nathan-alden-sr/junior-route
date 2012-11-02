namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public interface IFormsAuthenticationData<out TUserData>
		where TUserData : class
	{
		TUserData GetUserData(string cookieName = ".juniorauth");
	}
}