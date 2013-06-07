using System.Linq;
using System.Net;
using System.Web;

using Junior.Route.AutoRouting.AuthenticationProviders;
using Junior.Route.AutoRouting.FormsAuthentication;
using Junior.Route.Common;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AuthenticationProviders
{
	public static class FormsAuthenticationProviderTester
	{
		[TestFixture]
		public class When_creating_instance_with_no_redirect_on_failed_authentication
		{
			[SetUp]
			public void SetUp()
			{
				_helper = MockRepository.GenerateMock<IFormsAuthenticationHelper>();
				_provider = FormsAuthenticationProvider.CreateWithNoRedirectOnFailedAuthentication(_helper);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_response = _provider.GetFailedAuthenticationResponseAsync(_request).Result;
			}

			private FormsAuthenticationProvider _provider;
			private IResponse _response;
			private HttpRequestBase _request;
			private IFormsAuthenticationHelper _helper;

			[Test]
			public void Must_get_unauthorized_failed_authentication_response()
			{
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_relative_url_redirection_on_failed_authentication_and_no_return_url_and_get_method
		{
			[SetUp]
			public void SetUp()
			{
				_urlResolver = MockRepository.GenerateMock<IUrlResolver>();
				_urlResolver.Stub(arg => arg.Absolute("relative")).Return("/absolute");
				_helper = MockRepository.GenerateMock<IFormsAuthenticationHelper>();
				_provider = FormsAuthenticationProvider.CreateWithRelativeUrlRedirectOnFailedAuthentication(_helper, _urlResolver, "relative", null);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("GET");
				_response = _provider.GetFailedAuthenticationResponseAsync(_request).Result;
			}

			private FormsAuthenticationProvider _provider;
			private IResponse _response;
			private HttpRequestBase _request;
			private IUrlResolver _urlResolver;
			private IFormsAuthenticationHelper _helper;

			[Test]
			public void Must_get_found_response_with_location_header()
			{
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Found));
				Assert.That(_response.Headers.Any(arg => arg.Field == "Location" && arg.Value == "/absolute"), Is.True);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_relative_url_redirection_on_failed_authentication_and_no_return_url_and_post_method
		{
			[SetUp]
			public void SetUp()
			{
				_urlResolver = MockRepository.GenerateMock<IUrlResolver>();
				_urlResolver.Stub(arg => arg.Absolute("relative")).Return("/absolute");
				_helper = MockRepository.GenerateMock<IFormsAuthenticationHelper>();
				_provider = FormsAuthenticationProvider.CreateWithRelativeUrlRedirectOnFailedAuthentication(_helper, _urlResolver, "relative", null);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_response = _provider.GetFailedAuthenticationResponseAsync(_request).Result;
			}

			private FormsAuthenticationProvider _provider;
			private IResponse _response;
			private HttpRequestBase _request;
			private IUrlResolver _urlResolver;
			private IFormsAuthenticationHelper _helper;

			[Test]
			public void Must_get_see_other_response_with_location_header()
			{
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.SeeOther));
				Assert.That(_response.Headers.Any(arg => arg.Field == "Location" && arg.Value == "/absolute"), Is.True);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_relative_url_redirection_on_failed_authentication_and_return_url
		{
			[SetUp]
			public void SetUp()
			{
				_urlResolver = MockRepository.GenerateMock<IUrlResolver>();
				_urlResolver.Stub(arg => arg.Absolute("relative")).Return("/absolute");
				_helper = MockRepository.GenerateMock<IFormsAuthenticationHelper>();
				_provider = FormsAuthenticationProvider.CreateWithRelativeUrlRedirectOnFailedAuthentication(_helper, _urlResolver, "relative");
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.RawUrl).Return("/return");
				_response = _provider.GetFailedAuthenticationResponseAsync(_request).Result;
			}

			private FormsAuthenticationProvider _provider;
			private IResponse _response;
			private HttpRequestBase _request;
			private IUrlResolver _urlResolver;
			private IFormsAuthenticationHelper _helper;

			[Test]
			public void Must_get_found_response_with_location_header()
			{
				Assert.That(_response.Headers.Any(arg => arg.Field == "Location" && arg.Value == "/absolute?ReturnURL=%2freturn"), Is.True);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_route_redirection_on_failed_authentication_and_no_return_url_and_get_method
		{
			[SetUp]
			public void SetUp()
			{
				_urlResolver = MockRepository.GenerateMock<IUrlResolver>();
				_urlResolver.Stub(arg => arg.Route("route")).Return("/route");
				_helper = MockRepository.GenerateMock<IFormsAuthenticationHelper>();
				_provider = FormsAuthenticationProvider.CreateWithRouteRedirectOnFailedAuthentication(_helper, _urlResolver, "route", null);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("GET");
				_response = _provider.GetFailedAuthenticationResponseAsync(_request).Result;
			}

			private FormsAuthenticationProvider _provider;
			private IResponse _response;
			private HttpRequestBase _request;
			private IUrlResolver _urlResolver;
			private IFormsAuthenticationHelper _helper;

			[Test]
			public void Must_get_found_response_with_location_header()
			{
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Found));
				Assert.That(_response.Headers.Any(arg => arg.Field == "Location" && arg.Value == "/route"), Is.True);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_route_redirection_on_failed_authentication_and_no_return_url_and_post_method
		{
			[SetUp]
			public void SetUp()
			{
				_urlResolver = MockRepository.GenerateMock<IUrlResolver>();
				_urlResolver.Stub(arg => arg.Route("route")).Return("/route");
				_helper = MockRepository.GenerateMock<IFormsAuthenticationHelper>();
				_provider = FormsAuthenticationProvider.CreateWithRouteRedirectOnFailedAuthentication(_helper, _urlResolver, "route", null);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.HttpMethod).Return("POST");
				_response = _provider.GetFailedAuthenticationResponseAsync(_request).Result;
			}

			private FormsAuthenticationProvider _provider;
			private IResponse _response;
			private HttpRequestBase _request;
			private IUrlResolver _urlResolver;
			private IFormsAuthenticationHelper _helper;

			[Test]
			public void Must_get_found_response_with_location_header()
			{
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.SeeOther));
				Assert.That(_response.Headers.Any(arg => arg.Field == "Location" && arg.Value == "/route"), Is.True);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_route_redirection_on_failed_authentication_and_return_url
		{
			[SetUp]
			public void SetUp()
			{
				_urlResolver = MockRepository.GenerateMock<IUrlResolver>();
				_urlResolver.Stub(arg => arg.Route("route")).Return("/route");
				_helper = MockRepository.GenerateMock<IFormsAuthenticationHelper>();
				_provider = FormsAuthenticationProvider.CreateWithRouteRedirectOnFailedAuthentication(_helper, _urlResolver, "route");
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.RawUrl).Return("/return");
				_response = _provider.GetFailedAuthenticationResponseAsync(_request).Result;
			}

			private FormsAuthenticationProvider _provider;
			private IResponse _response;
			private HttpRequestBase _request;
			private IUrlResolver _urlResolver;
			private IFormsAuthenticationHelper _helper;

			[Test]
			public void Must_get_found_response_with_location_header()
			{
				Assert.That(_response.Headers.Any(arg => arg.Field == "Location" && arg.Value == "/route?ReturnURL=%2freturn"), Is.True);
			}
		}
	}
}