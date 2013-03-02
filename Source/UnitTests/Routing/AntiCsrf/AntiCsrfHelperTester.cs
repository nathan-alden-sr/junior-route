using System.Web;

using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.TokenGenerators;
using Junior.Route.Routing.AntiCsrf.Validators;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AntiCsrf
{
	public static class AntiCsrfHelperTester
	{
		[TestFixture]
		public class When_generating_cookie
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_tokenGenerator = MockRepository.GenerateMock<IAntiCsrfTokenGenerator>();
				_tokenValidator = MockRepository.GenerateMock<IAntiCsrfTokenValidator>();
				_helper = new AntiCsrfHelper(_configuration, _tokenGenerator, _tokenValidator);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfTokenGenerator _tokenGenerator;
			private IAntiCsrfTokenValidator _tokenValidator;
			private AntiCsrfHelper _helper;

			[Test]
			public void Must_generate_httponly_cookie()
			{
				HttpCookie cookie = _helper.GenerateCookie("token");

				Assert.That(cookie.HttpOnly, Is.True);
			}

			[Test]
			public void Must_use_configured_cookie_name()
			{
				HttpCookie cookie = _helper.GenerateCookie("token");

				Assert.That(cookie.Name, Is.EqualTo("name"));
			}

			[Test]
			public void Must_use_token_for_value()
			{
				HttpCookie cookie = _helper.GenerateCookie("token");

				Assert.That(cookie.Value, Is.EqualTo("token"));
			}
		}

		[TestFixture]
		public class When_generating_data
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.CookieName).Return(".name");
				_configuration.Stub(arg => arg.FormFieldName).Return("__name");
				_tokenGenerator = MockRepository.GenerateMock<IAntiCsrfTokenGenerator>();
				_tokenGenerator.Stub(arg => arg.Generate()).Return("token");
				_tokenValidator = MockRepository.GenerateMock<IAntiCsrfTokenValidator>();
				_helper = new AntiCsrfHelper(_configuration, _tokenGenerator, _tokenValidator);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfTokenGenerator _tokenGenerator;
			private IAntiCsrfTokenValidator _tokenValidator;
			private AntiCsrfHelper _helper;

			[Test]
			public void Must_generate_correct_data()
			{
				AntiCsrfData data = _helper.GenerateData();

				Assert.That(data.Cookie.Name, Is.EqualTo(".name"));
				Assert.That(data.Cookie.Value, Is.EqualTo("token"));
				Assert.That(data.Cookie.HttpOnly, Is.True);
				Assert.That(data.HiddenInputFieldHtml, Is.EqualTo(@"<input type=""hidden"" name=""__name"" value=""token""/>"));
				Assert.That(data.Token, Is.EqualTo("token"));
			}
		}

		[TestFixture]
		public class When_generating_hidden_input_field_with_known_token_and_disabled_configuration
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(false);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_tokenGenerator = MockRepository.GenerateMock<IAntiCsrfTokenGenerator>();
				_tokenValidator = MockRepository.GenerateMock<IAntiCsrfTokenValidator>();
				_helper = new AntiCsrfHelper(_configuration, _tokenGenerator, _tokenValidator);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfTokenGenerator _tokenGenerator;
			private IAntiCsrfTokenValidator _tokenValidator;
			private AntiCsrfHelper _helper;

			[Test]
			public void Must_not_generate_html()
			{
				string html = _helper.GenerateHiddenInputFieldHtml("token");

				Assert.That(html, Has.Length.EqualTo(0));
			}
		}

		[TestFixture]
		public class When_generating_hidden_input_field_with_known_token_and_enabled_configuration
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_tokenGenerator = MockRepository.GenerateMock<IAntiCsrfTokenGenerator>();
				_tokenValidator = MockRepository.GenerateMock<IAntiCsrfTokenValidator>();
				_helper = new AntiCsrfHelper(_configuration, _tokenGenerator, _tokenValidator);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfTokenGenerator _tokenGenerator;
			private IAntiCsrfTokenValidator _tokenValidator;
			private AntiCsrfHelper _helper;

			[Test]
			public void Must_generate_correct_html()
			{
				string html = _helper.GenerateHiddenInputFieldHtml("token");

				Assert.That(html, Is.EqualTo(@"<input type=""hidden"" name=""name"" value=""token""/>"));
			}
		}

		[TestFixture]
		public class When_generating_token
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_tokenGenerator = MockRepository.GenerateMock<IAntiCsrfTokenGenerator>();
				_tokenValidator = MockRepository.GenerateMock<IAntiCsrfTokenValidator>();
				_helper = new AntiCsrfHelper(_configuration, _tokenGenerator, _tokenValidator);
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfTokenGenerator _tokenGenerator;
			private IAntiCsrfTokenValidator _tokenValidator;
			private AntiCsrfHelper _helper;

			[Test]
			public void Must_use_token_generator()
			{
				_helper.GenerateToken();
				_tokenGenerator.AssertWasCalled(arg => arg.Generate());
			}
		}
	}
}