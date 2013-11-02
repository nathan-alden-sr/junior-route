using System;

using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.NonceRepositories
{
	public static class MemoryCacheRepositoryTester
	{
		[TestFixture]
		public class When_adding_nonce_to_cache
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.MemoryCacheName).Return("cache");
				_repository = new MemoryCacheRepository(_configuration);
			}

			private IAntiCsrfConfiguration _configuration;
			private MemoryCacheRepository _repository;

			[Test]
			[Ignore]
			public void Must_require_utc_created_timestamp()
			{
				Assert.That(async () => await _repository.AddAsync(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.UtcNow), Throws.ArgumentException);
			}

			[Test]
			[Ignore]
			public void Must_require_utc_expires_timestamp()
			{
				Assert.That(async () => await _repository.AddAsync(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.Now), Throws.ArgumentException);
			}
		}

		[TestFixture]
		public class When_checking_if_nonce_exists_and_nonce_is_expired
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.MemoryCacheName).Return("cache");
				_repository = new MemoryCacheRepository(_configuration);
				_sessionId = Guid.Parse("710f7634-b013-4fb8-abf5-97c98e4993c3");
				_nonce = Guid.Parse("0dd344d0-7115-4c0e-864a-afaecb339138");
				_repository.AddAsync(_sessionId, _nonce, DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1)), DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1)));
			}

			private IAntiCsrfConfiguration _configuration;
			private MemoryCacheRepository _repository;
			private Guid _sessionId;
			private Guid _nonce;

			[Test]
			public async void Must_not_exist()
			{
				Assert.That(await _repository.ExistsAsync(_sessionId, _nonce, DateTime.UtcNow), Is.False);
			}
		}

		[TestFixture]
		public class When_checking_if_nonce_exists_and_nonce_is_not_expired
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.MemoryCacheName).Return("cache");
				_repository = new MemoryCacheRepository(_configuration);
				_sessionId = Guid.Parse("710f7634-b013-4fb8-abf5-97c98e4993c3");
				_nonce = Guid.Parse("0dd344d0-7115-4c0e-864a-afaecb339138");
				_repository.AddAsync(_sessionId, _nonce, DateTime.UtcNow, DateTime.MaxValue.ToUniversalTime());
			}

			private IAntiCsrfConfiguration _configuration;
			private MemoryCacheRepository _repository;
			private Guid _sessionId;
			private Guid _nonce;

			[Test]
			public async void Must_exist()
			{
				Assert.That(await _repository.ExistsAsync(_sessionId, _nonce, DateTime.MinValue.ToUniversalTime()), Is.True);
			}
		}
	}
}