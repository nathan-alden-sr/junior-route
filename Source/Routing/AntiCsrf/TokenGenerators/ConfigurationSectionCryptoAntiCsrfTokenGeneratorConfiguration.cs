using System.Configuration;

namespace Junior.Route.Routing.AntiCsrf.TokenGenerators
{
	public class ConfigurationSectionCryptoAntiCsrfTokenGeneratorConfiguration : ICryptoAntiCsrfTokenGeneratorConfiguration
	{
		private readonly CryptoAntiCsrfTokenGeneratorConfigurationSection _configurationSection = (CryptoAntiCsrfTokenGeneratorConfigurationSection)ConfigurationManager.GetSection("cryptoAntiCsrfTokenGenerator") ?? new CryptoAntiCsrfTokenGeneratorConfigurationSection();

		public int LengthInBytes
		{
			get
			{
				return _configurationSection.LengthInBytes;
			}
		}
	}
}