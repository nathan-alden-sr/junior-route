using System.Configuration;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public class FormsAuthenticationConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("persistent", DefaultValue = false, IsRequired = false)]
		public bool Persistent
		{
			get
			{
				return (bool)this["persistent"];
			}
		}

		[ConfigurationProperty("slidingExpiration", DefaultValue = true, IsRequired = false)]
		public bool SlidingExpiration
		{
			get
			{
				return (bool)this["slidingExpiration"];
			}
		}

		[ConfigurationProperty("requireSsl", DefaultValue = false, IsRequired = false)]
		public bool RequireSsl
		{
			get
			{
				return (bool)this["requireSsl"];
			}
		}

		[ConfigurationProperty("cookieName", DefaultValue = ".juniorauth", IsRequired = false)]
		public string CookieName
		{
			get
			{
				return (string)this["cookieName"];
			}
		}

		[ConfigurationProperty("cookiePath", DefaultValue = "/", IsRequired = false)]
		public string CookiePath
		{
			get
			{
				return (string)this["cookiePath"];
			}
		}

		[ConfigurationProperty("cookieDomain", DefaultValue = "", IsRequired = false)]
		public string CookieDomain
		{
			get
			{
				return (string)this["cookieDomain"];
			}
		}
	}
}