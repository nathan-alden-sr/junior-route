using System;
using System.Security.Cryptography;

using Junior.Common;

namespace Junior.Route.Routing.AntiCsrf.TokenGenerators
{
	public class CryptoAntiCsrfTokenGenerator : IAntiCsrfTokenGenerator
	{
		private static readonly RNGCryptoServiceProvider _cryptoServiceProvider = new RNGCryptoServiceProvider();
		private readonly ICryptoAntiCsrfTokenGeneratorConfiguration _configuration;

		public CryptoAntiCsrfTokenGenerator(ICryptoAntiCsrfTokenGeneratorConfiguration configuration)
		{
			configuration.ThrowIfNull("configuration");

			_configuration = configuration;
		}

		public string Generate()
		{
			int lengthInBytes = _configuration.LengthInBytes;

			if (lengthInBytes < 1)
			{
				throw new ApplicationException("LengthInBytes must be at least 1.");
			}

			var token = new byte[lengthInBytes];

			_cryptoServiceProvider.GetBytes(token);

			return Convert.ToBase64String(token);
		}
	}
}