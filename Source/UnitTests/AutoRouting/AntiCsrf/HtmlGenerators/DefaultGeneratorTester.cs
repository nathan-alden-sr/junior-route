using System;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.AntiCsrf.HtmlGenerators;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.CookieManagers;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.AntiCsrf.HtmlGenerators
{
	public static class DefaultGeneratorTester
	{
		[TestFixture]
		public class When_generating_hidden_input_html_and_configuration_is_disabled
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(false);
				_cookieManager = MockRepository.GenerateMock<IAntiCsrfCookieManager>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_generator = new DefaultGenerator(_configuration, _cookieManager, _nonceRepository, _guidFactory, _systemClock);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfCookieManager _cookieManager;
			private IAntiCsrfNonceRepository _nonceRepository;
			private IGuidFactory _guidFactory;
			private ISystemClock _systemClock;
			private DefaultGenerator _generator;
			private HttpResponseBase _response;

			[Test]
			public async void Must_generate_empty_string()
			{
				Assert.That(await _generator.GenerateHiddenInputHtmlAsync(_response), Is.EqualTo(""));
			}
		}

		[TestFixture]
		public class When_generating_hidden_input_html_and_session_id_is_missing
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_cookieManager = MockRepository.GenerateMock<IAntiCsrfCookieManager>();
				_cookieManager.Stub(arg => arg.GetSessionIdAsync(Arg<HttpResponseBase>.Is.Anything)).Return(Task<Guid?>.Factory.Empty());
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_generator = new DefaultGenerator(_configuration, _cookieManager, _nonceRepository, _guidFactory, _systemClock);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfCookieManager _cookieManager;
			private IAntiCsrfNonceRepository _nonceRepository;
			private IGuidFactory _guidFactory;
			private ISystemClock _systemClock;
			private DefaultGenerator _generator;
			private HttpResponseBase _response;

			[Test]
			public async void Must_generate_empty_string()
			{
				Assert.That(await _generator.GenerateHiddenInputHtmlAsync(_response), Is.EqualTo(""));
			}
		}

		[TestFixture]
		public class When_generating_hidden_input_html_and_session_id_is_present
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.NonceDuration).Return(TimeSpan.FromMinutes(1));
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_cookieManager = MockRepository.GenerateMock<IAntiCsrfCookieManager>();
				_sessionId = Guid.Parse("3ff1a1d6-1604-462e-b347-1314e962ac29");
				_cookieManager.Stub(arg => arg.GetSessionIdAsync(Arg<HttpResponseBase>.Is.Anything)).Return(((Guid?)_sessionId).AsCompletedTask());
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_nonceRepository.Stub(arg => arg.AddAsync(Arg<Guid>.Is.Anything, Arg<Guid>.Is.Anything, Arg<DateTime>.Is.Anything, Arg<DateTime>.Is.Anything)).Return(Task.Factory.Empty());
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_nonce = Guid.Parse("4dc041ab-7259-466a-b9a7-846dd9595f4e");
				_guidFactory.Stub(arg => arg.Random()).Return(_nonce);
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_currentUtcTimestamp = new DateTime(2013, 1, 2);
				_systemClock.Stub(arg => arg.UtcDateTime).Return(_currentUtcTimestamp);
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_generator = new DefaultGenerator(_configuration, _cookieManager, _nonceRepository, _guidFactory, _systemClock);
				_hiddenInputHtml = _generator.GenerateHiddenInputHtmlAsync(_response).Result;
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfCookieManager _cookieManager;
			private IAntiCsrfNonceRepository _nonceRepository;
			private IGuidFactory _guidFactory;
			private ISystemClock _systemClock;
			private DefaultGenerator _generator;
			private Guid _sessionId;
			private Guid _nonce;
			private DateTime _currentUtcTimestamp;
			private string _hiddenInputHtml;
			private HttpResponseBase _response;

			[Test]
			public void Must_add_new_nonce_to_repository()
			{
				_nonceRepository.AssertWasCalled(arg => arg.AddAsync(_sessionId, _nonce, _currentUtcTimestamp, _currentUtcTimestamp.AddMinutes(1)));
			}

			[Test]
			public void Must_generate_new_nonce_using_guidfactory()
			{
				_guidFactory.AssertWasCalled(arg => arg.Random());
			}

			[Test]
			public void Must_return_correct_html()
			{
				Assert.That(_hiddenInputHtml, Is.EqualTo(String.Format(@"<input type=""hidden"" name=""name"" value=""{0}""/>", _nonce.ToString("N"))));
			}
		}
	}
}