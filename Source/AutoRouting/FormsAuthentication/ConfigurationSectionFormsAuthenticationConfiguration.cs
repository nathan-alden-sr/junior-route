using System.Configuration;

namespace Junior.Route.AutoRouting.FormsAuthentication
{
	public class ConfigurationSectionFormsAuthenticationConfiguration : IFormsAuthenticationConfiguration
	{
		private readonly FormsAuthenticationConfigurationSection _configurationSection = (FormsAuthenticationConfigurationSection)ConfigurationManager.GetSection("formsAuthentication") ?? new FormsAuthenticationConfigurationSection();

		public bool Persistent
		{
			get
			{
				return _configurationSection.Persistent;
			}
		}

		public bool SlidingExpiration
		{
			get
			{
				return _configurationSection.SlidingExpiration;
			}
		}

		public bool RequireSsl
		{
			get
			{
				return _configurationSection.RequireSsl;
			}
		}

		public string CookieName
		{
			get
			{
				return _configurationSection.CookieName;
			}
		}

		public string CookiePath
		{
			get
			{
				return _configurationSection.CookiePath;
			}
		}

		public string CookieDomain
		{
			get
			{
				return _configurationSection.CookieDomain;
			}
		}
	}
}