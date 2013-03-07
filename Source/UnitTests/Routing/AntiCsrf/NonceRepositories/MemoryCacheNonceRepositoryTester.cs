using System;

using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.NonceRepositories
{
	public static class MemoryCacheNonceRepositoryTester
	{
		[TestFixture]
		public class When_adding_nonce_to_cache
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.MemoryCacheName).Return("cache");
				_nonceRepository = new MemoryCacheNonceRepository(_configuration);
			}

			private IAntiCsrfConfiguration _configuration;
			private MemoryCacheNonceRepository _nonceRepository;

			[Test]
			[Ignore]
			[ExpectedException(typeof(ArgumentException))]
#warning Update to use async Assert.That(..., Throws.InstanceOf<>) when NUnit 2.6.3 becomes available
			public async void Must_require_utc_created_timestamp()
			{
				await _nonceRepository.Add(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.UtcNow);
			}

			[Test]
			[Ignore]
			[ExpectedException(typeof(ArgumentException))]
#warning Update to use async Assert.That(..., Throws.InstanceOf<>) when NUnit 2.6.3 becomes available
			public async void Must_require_utc_expires_timestamp()
			{
				await _nonceRepository.Add(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.Now);
			}
		}

		[TestFixture]
		public class When_checking_if_nonce_exists_and_nonce_is_expired
		{
			[SetUp]
			public async void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.MemoryCacheName).Return("cache");
				_nonceRepository = new MemoryCacheNonceRepository(_configuration);
				_sessionId = Guid.Parse("710f7634-b013-4fb8-abf5-97c98e4993c3");
				_nonce = Guid.Parse("0dd344d0-7115-4c0e-864a-afaecb339138");
				await _nonceRepository.Add(_sessionId, _nonce, DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1)), DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1)));
			}

			private IAntiCsrfConfiguration _configuration;
			private MemoryCacheNonceRepository _nonceRepository;
			private Guid _sessionId;
			private Guid _nonce;

			[Test]
			public async void Must_not_exist()
			{
				Assert.That(await _nonceRepository.Exists(_sessionId, _nonce, DateTime.UtcNow), Is.False);
			}
		}

		[TestFixture]
		public class When_checking_if_nonce_exists_and_nonce_is_not_expired
		{
			[SetUp]
			public async void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.MemoryCacheName).Return("cache");
				_nonceRepository = new MemoryCacheNonceRepository(_configuration);
				_sessionId = Guid.Parse("710f7634-b013-4fb8-abf5-97c98e4993c3");
				_nonce = Guid.Parse("0dd344d0-7115-4c0e-864a-afaecb339138");
				await _nonceRepository.Add(_sessionId, _nonce, DateTime.UtcNow, DateTime.MaxValue.ToUniversalTime());
			}

			private IAntiCsrfConfiguration _configuration;
			private MemoryCacheNonceRepository _nonceRepository;
			private Guid _sessionId;
			private Guid _nonce;

			[Test]
			public async void Must_exist()
			{
				Assert.That(await _nonceRepository.Exists(_sessionId, _nonce, DateTime.MinValue.ToUniversalTime()), Is.True);
			}
		}
	}
}