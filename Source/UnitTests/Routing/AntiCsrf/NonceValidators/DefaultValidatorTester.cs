using System;
using System.Collections.Specialized;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;
using Junior.Route.Routing.AntiCsrf.NonceValidators;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.NonceValidators
{
	public static class DefaultValidatorTester
	{
		[TestFixture]
		public class When_delete_is_disabled_for_validation
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_skip_validation()
			{
				_request.Stub(arg => arg.HttpMethod).Return("DELETE");
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpDelete).Return(false);

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.ValidationSkipped));
			}
		}

		[TestFixture]
		public class When_delete_is_enabled_for_validation
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_not_skip_validation()
			{
				_request.Stub(arg => arg.HttpMethod).Return("DELETE");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection());
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpDelete).Return(true);

				Assert.That(await _validator.ValidateAsync(_request), Is.Not.EqualTo(ValidationResult.ValidationSkipped));
			}
		}

		[TestFixture]
		public class When_post_is_disabled_for_validation
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_skip_validation()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(false);

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.ValidationSkipped));
			}
		}

		[TestFixture]
		public class When_post_is_enabled_for_validation
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_not_skip_validation()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection());
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);

				Assert.That(await _validator.ValidateAsync(_request), Is.Not.EqualTo(ValidationResult.ValidationSkipped));
			}
		}

		[TestFixture]
		public class When_put_is_disabled_for_validation
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_skip_validation()
			{
				_request.Stub(arg => arg.HttpMethod).Return("PUT");
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPut).Return(false);

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.ValidationSkipped));
			}
		}

		[TestFixture]
		public class When_put_is_enabled_for_validation
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_not_skip_validation()
			{
				_request.Stub(arg => arg.HttpMethod).Return("PUT");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection());
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPut).Return(true);

				Assert.That(await _validator.ValidateAsync(_request), Is.Not.EqualTo(ValidationResult.ValidationSkipped));
			}
		}

		[TestFixture]
		public class When_validating_and_configuration_is_disabled
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_return_validation_disabled()
			{
				_configuration.Stub(arg => arg.Enabled).Return(false);

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.ValidationDisabled));
			}
		}

		[TestFixture]
		public class When_validating_and_cookie_is_missing
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_return_cookie_missing()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "value" } });
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection());
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.CookieName).Return("name");

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.CookieMissing));
			}
		}

		[TestFixture]
		public class When_validating_and_cookie_is_not_a_guid
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_return_nonce_invalid()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "ae758932-238f-4eac-a426-9b6fbc20326c" } });
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "value") });
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.CookieName).Return("name");

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.CookieInvalid));
			}
		}

		[TestFixture]
		public class When_validating_and_form_field_is_missing
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_return_form_field_missing()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection());
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.FormFieldMissing));
			}
		}

		[TestFixture]
		public class When_validating_and_form_field_is_not_a_guid
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_return_form_field_invalid()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "value" } });
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name") });
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.CookieName).Return("name");

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.FormFieldInvalid));
			}
		}

		[TestFixture]
		public class When_validating_and_nonce_is_in_repository
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_return_nonce_valid()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "ae758932-238f-4eac-a426-9b6fbc20326c" } });
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "ae758932-238f-4eac-a426-9b6fbc20326c") });
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_nonceRepository.Stub(arg => arg.ExistsAsync(Arg<Guid>.Is.Anything, Arg<Guid>.Is.Anything, Arg<DateTime>.Is.Anything)).Return(true.AsCompletedTask());

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.NonceValid));
			}
		}

		[TestFixture]
		public class When_validating_and_nonce_is_not_in_repository
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<IAntiCsrfConfiguration>();
				_nonceRepository = MockRepository.GenerateMock<IAntiCsrfNonceRepository>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_validator = new DefaultValidator(_configuration, _nonceRepository, _systemClock);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private IAntiCsrfConfiguration _configuration;
			private IAntiCsrfNonceRepository _nonceRepository;
			private ISystemClock _systemClock;
			private DefaultValidator _validator;
			private HttpRequestBase _request;

			[Test]
			public async void Must_return_nonce_invalid()
			{
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "name", "ae758932-238f-4eac-a426-9b6fbc20326c" } });
				_request.Stub(arg => arg.Cookies).Return(new HttpCookieCollection { new HttpCookie("name", "ae758932-238f-4eac-a426-9b6fbc20326c") });
				_configuration.Stub(arg => arg.Enabled).Return(true);
				_configuration.Stub(arg => arg.ValidateHttpPost).Return(true);
				_configuration.Stub(arg => arg.FormFieldName).Return("name");
				_configuration.Stub(arg => arg.CookieName).Return("name");
				_nonceRepository.Stub(arg => arg.ExistsAsync(Arg<Guid>.Is.Anything, Arg<Guid>.Is.Anything, Arg<DateTime>.Is.Anything)).Return(false.AsCompletedTask());

				Assert.That(await _validator.ValidateAsync(_request), Is.EqualTo(ValidationResult.NonceInvalid));
			}
		}
	}
}