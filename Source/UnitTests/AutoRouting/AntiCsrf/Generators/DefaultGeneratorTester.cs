using System;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.AutoRouting.AntiCsrf.Generators;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.Generators;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.AntiCsrf.Generators
{
	public static class DefaultGeneratorTester
	{
		[TestFixture]
		public class When_generating_nonce_and_configuration_is_disabled
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(false);
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_generator = new DefaultGenerator(_configuration, _nonceRepository, _guidFactory, _systemClock);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private IGuidFactory _guidFactory;
			private ISystemClock _systemClock;
			private DefaultGenerator _generator;

			[Test]
			public async void Must_not_generate_nonce()
			{
				Assert.That(await _generator.Generate(), Is.Null);
			}
		}

		[TestFixture]
		public class When_generating_nonce_and_session_id_is_missing
		{
			[SetUp]
			public async void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.NonceDuration).Return(TimeSpan.FromMinutes(1));
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_nonceRepository.Stub(arg => arg.AddAsync(Arg<Guid>.Is.Anything, Arg<Guid>.Is.Anything, Arg<DateTime>.Is.Anything, Arg<DateTime>.Is.Anything)).Return(Task.Factory.Empty());
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_currentUtcTimestamp = new DateTime(2013, 1, 2);
				_systemClock.Stub(arg => arg.UtcDateTime).Return(_currentUtcTimestamp);
				_generator = new DefaultGenerator(_configuration, _nonceRepository, _guidFactory, _systemClock);
				_sessionId = Guid.Parse("d9e78712-7b7f-4acd-8042-f065d8f23303");
				_nonceId = Guid.Parse("1706c6a1-6b3e-4095-8e9b-ade9d4a51a83");
				_guidFactory.Stub(arg => arg.Random()).Return(_sessionId).Repeat.Once();
				_guidFactory.Stub(arg => arg.Random()).Return(_nonceId).Repeat.Once();
				_nonce = await _generator.Generate();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private IGuidFactory _guidFactory;
			private ISystemClock _systemClock;
			private DefaultGenerator _generator;
			private AntiCsrfNonce _nonce;
			private Guid _sessionId;
			private Guid _nonceId;
			private DateTime _currentUtcTimestamp;

			[Test]
			public void Must_add_new_session_and_nonce_to_repository()
			{
				_nonceRepository.AssertWasCalled(arg => arg.AddAsync(Arg<Guid>.Is.Equal(_sessionId), Arg<Guid>.Is.Equal(_nonceId), Arg<DateTime>.Is.Anything, Arg<DateTime>.Is.Anything));
			}

			[Test]
			public void Must_generate_new_session_and_nonce_using_guidfactory()
			{
				_guidFactory.AssertWasCalled(arg => arg.Random(), options => options.Repeat.Twice());
			}

			[Test]
			public void Must_return_correct_session_and_nonce()
			{
				Assert.That(_nonce.SessionId, Is.EqualTo(_sessionId));
				Assert.That(_nonce.Nonce, Is.EqualTo(_nonceId));
			}
		}

		[TestFixture]
		public class When_generating_nonce_and_session_id_is_present
		{
			[SetUp]
			public async void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.NonceDuration).Return(TimeSpan.FromMinutes(1));
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_nonceRepository.Stub(arg => arg.AddAsync(Arg<Guid>.Is.Anything, Arg<Guid>.Is.Anything, Arg<DateTime>.Is.Anything, Arg<DateTime>.Is.Anything)).Return(Task.Factory.Empty());
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_currentUtcTimestamp = new DateTime(2013, 1, 2);
				_systemClock.Stub(arg => arg.UtcDateTime).Return(_currentUtcTimestamp);
				_generator = new DefaultGenerator(_configuration, _nonceRepository, _guidFactory, _systemClock);
				_sessionId = Guid.Parse("d9e78712-7b7f-4acd-8042-f065d8f23303");
				_nonceId = Guid.Parse("1706c6a1-6b3e-4095-8e9b-ade9d4a51a83");
				_guidFactory.Stub(arg => arg.Random()).Return(_nonceId).Repeat.Once();
				_nonce = await _generator.Generate(_sessionId);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private IGuidFactory _guidFactory;
			private ISystemClock _systemClock;
			private DefaultGenerator _generator;
			private AntiCsrfNonce _nonce;
			private Guid _sessionId;
			private Guid _nonceId;
			private DateTime _currentUtcTimestamp;

			[Test]
			public void Must_add_nonce_with_existing_session_to_repository()
			{
				_nonceRepository.AssertWasCalled(arg => arg.AddAsync(Arg<Guid>.Is.Equal(_sessionId), Arg<Guid>.Is.Equal(_nonceId), Arg<DateTime>.Is.Anything, Arg<DateTime>.Is.Anything));
			}

			[Test]
			public void Must_generate_new_nonce_using_guidfactory()
			{
				_guidFactory.AssertWasCalled(arg => arg.Random(), options => options.Repeat.Once());
			}

			[Test]
			public void Must_return_correct_session_and_nonce()
			{
				Assert.That(_nonce.SessionId, Is.EqualTo(_sessionId));
				Assert.That(_nonce.Nonce, Is.EqualTo(_nonceId));
			}
		}
	}
}