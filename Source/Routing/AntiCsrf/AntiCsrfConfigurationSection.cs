using System.Configuration;

namespace Junior.Route.Routing.AntiCsrf
{
	public class AntiCsrfConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
		public bool Enabled
		{
			get
			{
				return (bool)this["enabled"];
			}
		}

		[ConfigurationProperty("cookieName", DefaultValue = ".junioranticsrftoken", IsRequired = false)]
		public string CookieName
		{
			get
			{
				return (string)this["cookieName"];
			}
		}

		[ConfigurationProperty("formFieldName", DefaultValue = "__JuniorAntiCsrfToken", IsRequired = false)]
		public string FormFieldName
		{
			get
			{
				return (string)this["formFieldName"];
			}
		}

		[ConfigurationProperty("validateHttpPost", DefaultValue = true, IsRequired = false)]
		public bool ValidateHttpPost
		{
			get
			{
				return (bool)this["validateHttpPost"];
			}
		}

		[ConfigurationProperty("validateHttpPut", DefaultValue = true, IsRequired = false)]
		public bool ValidateHttpPut
		{
			get
			{
				return (bool)this["validateHttpPut"];
			}
		}

		[ConfigurationProperty("validateHttpDelete", DefaultValue = true, IsRequired = false)]
		public bool ValidateHttpDelete
		{
			get
			{
				return (bool)this["validateHttpDelete"];
			}
		}
	}
}