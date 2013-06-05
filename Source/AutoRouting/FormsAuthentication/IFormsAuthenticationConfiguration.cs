namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public interface IFormsAuthenticationConfiguration
	{
		bool Persistent
		{
			get;
		}
		bool SlidingExpiration
		{
			get;
		}
		bool RequireSsl
		{
			get;
		}
		string CookieName
		{
			get;
		}
		string CookiePath
		{
			get;
		}
		string CookieDomain
		{
			get;
		}
	}
}