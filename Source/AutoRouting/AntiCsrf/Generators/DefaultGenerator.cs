using System;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.Generators;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

namespace Junior.Route.AutoRouting.AntiCsrf.Generators
{
	public class DefaultGenerator : IAntiCsrfGenerator
	{
		private readonly IAntiCsrfConfiguration _configuration;
		private readonly IGuidFactory _guidFactory;
		private readonly IAntiCsrfNonceRepository _nonceRepository;
		private readonly ISystemClock _systemClock;

		public DefaultGenerator(IAntiCsrfConfiguration configuration, IAntiCsrfNonceRepository nonceRepository, IGuidFactory guidFactory, ISystemClock systemClock)
		{
			configuration.ThrowIfNull("configuration");
			nonceRepository.ThrowIfNull("nonceRepository");
			guidFactory.ThrowIfNull("guidFactory");
			systemClock.ThrowIfNull("systemClock");

			_configuration = configuration;
			_nonceRepository = nonceRepository;
			_guidFactory = guidFactory;
			_systemClock = systemClock;
		}

		public async Task<AntiCsrfNonce> Generate(Guid? sessionId = null)
		{
			if (!_configuration.Enabled)
			{
				return null;
			}

			sessionId = sessionId ?? _guidFactory.Random();

			Guid nonce = _guidFactory.Random();
			DateTime currentTimestamp = _systemClock.UtcDateTime;

			await _nonceRepository.AddAsync(sessionId.Value, nonce, currentTimestamp, currentTimestamp + _configuration.NonceDuration);

			return new AntiCsrfNonce(sessionId.Value, nonce);
		}
	}
}