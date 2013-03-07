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

		[ConfigurationProperty("cookieName", DefaultValue = ".juniorroutesession", IsRequired = false)]
		public string CookieName
		{
			get
			{
				return (string)this["cookieName"];
			}
		}

		[ConfigurationProperty("formFieldName", DefaultValue = "__JuniorRouteNonce", IsRequired = false)]
		public string FormFieldName
		{
			get
			{
				return (string)this["formFieldName"];
			}
		}

		[ConfigurationProperty("nonceDurationInMinutes", DefaultValue = 60, IsRequired = false)]
		public int NonceDurationInMinutes
		{
			get
			{
				return (int)this["nonceDurationInMinutes"];
			}
		}

		[ConfigurationProperty("memoryCacheName", DefaultValue = "JuniorRoute", IsRequired = false)]
		public string MemoryCacheName
		{
			get
			{
				return (string)this["memoryCacheName"];
			}
		}
	}
}