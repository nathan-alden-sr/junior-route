using System.Configuration;

namespace Junior.Route.Routing.AntiCsrf.TokenGenerators
{
	public class CryptoAntiCsrfTokenGeneratorConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("lengthInBytes", DefaultValue = 256, IsRequired = true)]
		public int LengthInBytes
		{
			get
			{
				return (int)this["lengthInBytes"];
			}
		}
	}
}