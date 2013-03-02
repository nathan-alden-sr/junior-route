using System.Collections.Specialized;
using System.Web;

using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.Validators;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.Validators
{
	public static class DefaultAntiCsrfTokenValidatorTester
	{
		[TestFixture]
		public class When_validating_with_anti_csrf_disabled
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(false);
				_validator = new DefaultAntiCsrfTokenValidator(_configuration);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private DefaultAntiCsrfTokenValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public void Must_return_validation_disabled()
			{
				Assert.That(_validator.Validate(_request), Is.EqualTo(ValidationResult.ValidationDisabled));
			}
		}

		[TestFixture]
		public class When_validating_with_cookie_missing
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_validator = new DefaultAntiCsrfTokenValidator(_configuration);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "value2" } });
			}

			private IAntiCsrfConfiguration _configuration;
			private DefaultAntiCsrfTokenValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public void Must_return_cookie_missing()
			{
				Assert.That(_validator.Validate(_request), Is.EqualTo(ValidationResult.CookieMissing));
			}
		}

		[TestFixture]
		public class When_validating_with_cookie_that_does_not_match_token
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_validator = new DefaultAntiCsrfTokenValidator(_configuration);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "value1") });
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "value2" } });
			}

			private IAntiCsrfConfiguration _configuration;
			private DefaultAntiCsrfTokenValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public void Must_not_match()
			{
				Assert.That(_validator.Validate(_request), Is.EqualTo(ValidationResult.TokensDoNotMatch));
			}
		}

		[TestFixture]
		public class When_validating_with_cookie_that_matches_token
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_validator = new DefaultAntiCsrfTokenValidator(_configuration);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "value") });
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "value" } });
			}

			private IAntiCsrfConfiguration _configuration;
			private DefaultAntiCsrfTokenValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public void Must_match()
			{
				Assert.That(_validator.Validate(_request), Is.EqualTo(ValidationResult.TokensMatch));
			}
		}

		[TestFixture]
		public class When_validating_with_form_field_missing
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_validator = new DefaultAntiCsrfTokenValidator(_configuration);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "value1") });
				_request.Stub(arg => arg.Form).Return(new NameValueCollection());
			}

			private IAntiCsrfConfiguration _configuration;
			private DefaultAntiCsrfTokenValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public void Must_return_form_field_missing()
			{
				Assert.That(_validator.Validate(_request), Is.EqualTo(ValidationResult.FormFieldMissing));
			}
		}

		[TestFixture]
		public class When_validating_with_unsupported_httpmethod
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_validator = new DefaultAntiCsrfTokenValidator(_configuration);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("CONNECT");
			}

			private IAntiCsrfConfiguration _configuration;
			private DefaultAntiCsrfTokenValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public void Must_result_in_validation_skipped()
			{
				Assert.That(_validator.Validate(_request), Is.EqualTo(ValidationResult.ValidationSkipped));
			}
		}
	}
}