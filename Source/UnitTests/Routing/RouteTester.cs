using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Responses.Text;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing
{
	public static class RouteTester
	{
		[TestFixture]
		public class When_adding_duplicate_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance), new UrlPortRestriction(80));
				_route.AddRestrictions(new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance), new UrlPortRestriction(80));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_omit_duplicates()
			{
				IRestriction[] restrictions = _route.GetRestrictions().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_adding_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions((IEnumerable<IRestriction>)new IRestriction[] { new MethodRestriction("GET"), new UrlPortRestriction(0) });
				_route.AddRestrictions(new MethodRestriction("POST"), new UrlPortRestriction(1));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				IRestriction[] restrictions = _route.GetRestrictions().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_clearing_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(
					new HeaderRestriction<DateHeader>("Date", headerValue => DateHeader.Parse(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new MethodRestriction("GET"));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_clear_all_restrictions()
			{
				_route.ClearRestrictions();

				Assert.That(_route.GetRestrictions(), Is.Empty);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_id_and_scheme_and_resolved_relative_url
		{
			[SetUp]
			public void SetUp()
			{
				_name = "name";
				_id = Guid.Parse("2fa8d2d3-94ca-4c43-8dd0-50717c165c1f");
				_scheme = Scheme.NotSpecified;
				_resolvedRelativeUrl = "resolved";
				_route = new Route.Routing.Route(_name, _id, _scheme, _resolvedRelativeUrl);
			}

			private string _name;
			private Route.Routing.Route _route;
			private string _resolvedRelativeUrl;
			private Guid _id;
			private Scheme _scheme;

			[Test]
			public void Must_set_id()
			{
				Assert.That(_route.Id, Is.EqualTo(_id));
			}

			[Test]
			public void Must_set_name()
			{
				Assert.That(_route.Name, Is.EqualTo(_name));
			}

			[Test]
			public void Must_set_resolved_relative_url()
			{
				Assert.That(_route.ResolvedRelativeUrl, Is.EqualTo(_resolvedRelativeUrl));
			}

			[Test]
			public void Must_set_scheme()
			{
				Assert.That(_route.Scheme, Is.EqualTo(_scheme));
			}
		}

		[TestFixture]
		public class When_getting_generic_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(
					new HeaderRestriction<DateHeader>("Date", headerValue => DateHeader.Parse(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new MethodRestriction("GET"));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_get_correct_restrictions()
			{
				object[] restrictions = _route.GetGenericRestrictions(typeof(HeaderRestriction<>)).Cast<object>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(3));
			}
		}

		[TestFixture]
		public class When_getting_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(
					new HeaderRestriction<DateHeader>("Date", headerValue => DateHeader.Parse(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new MethodRestriction("GET"));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_get_correct_restrictions()
			{
				IRestriction[] restrictions = _route.GetRestrictions().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(3));
			}
		}

		[TestFixture]
		public class When_getting_restrictions_by_generic_type
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(
					new HeaderRestriction<DateHeader>("Date", headerValue => DateHeader.Parse(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new MethodRestriction("GET"));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_get_correct_restrictions()
			{
				HeaderRestriction<AllowHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<AllowHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_getting_restrictions_by_type
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(
					new HeaderRestriction<DateHeader>("Date", headerValue => DateHeader.Parse(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new MethodRestriction("GET"));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_get_correct_restrictions()
			{
				IRestriction[] restrictions = _route.GetRestrictions(typeof(HeaderRestriction<AllowHeader>)).Cast<IRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_getting_restrictions_types
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(
					new HeaderRestriction<DateHeader>("Date", headerValue => DateHeader.Parse(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true),
					new MethodRestriction("GET"));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_get_correct_restriction_types()
			{
				IEnumerable<Type> types = _route.GetRestrictionTypes();

				Assert.That(types, Is.EquivalentTo(new[] { typeof(HeaderRestriction<DateHeader>), typeof(HeaderRestriction<AllowHeader>), typeof(MethodRestriction) }));
			}
		}

		[TestFixture]
		public class When_responding_with_nocontentresponse
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_route.RespondWithNoContent();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public void Must_indicate_null_return_type()
			{
				Assert.That(_route.ResponseType, Is.Null);
			}

			[Test]
			public async void Must_return_nocontentresponse()
			{
				IResponse processedResponse = await _route.ProcessResponseAsync(_context);

				Assert.That(processedResponse.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.NoContent));
			}
		}

		[TestFixture]
		public class When_responding_with_nocontentresponse_using_delegate
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_route.RespondWithNoContent(context => _delegateCalled = true);
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;
			private bool _delegateCalled;

			[Test]
			public async void Must_call_delegate()
			{
				await _route.ProcessResponseAsync(_context);

				Assert.That(_delegateCalled, Is.True);
			}

			[Test]
			public void Must_indicate_null_return_type()
			{
				Assert.That(_route.ResponseType, Is.Null);
			}

			[Test]
			public async void Must_return_nocontentresponse()
			{
				IResponse processedResponse = await _route.ProcessResponseAsync(_context);

				Assert.That(processedResponse.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.NoContent));
			}
		}

		[TestFixture]
		public class When_responding_with_response
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => new Response().NoContent());

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(Response)));
			}

			[Test]
			public async void Must_return_response()
			{
				IResponse response = new Response().NoContent();

				_route.RespondWith(response);

				IResponse processedResponse = await _route.ProcessResponseAsync(_context);

				Assert.That(processedResponse, Is.EqualTo(response));
			}
		}

		[TestFixture]
		public class When_responding_with_response_and_specific_return_type
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => (IResponse)new Response().NoContent(), typeof(CssResponse));

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(CssResponse)));
			}

			[Test]
			public async void Must_return_response()
			{
				IResponse response = new Response().NoContent();

				_route.RespondWith(response, typeof(CssResponse));

				IResponse processedResponse = await _route.ProcessResponseAsync(_context);

				Assert.That(processedResponse, Is.EqualTo(response));
			}
		}

		[TestFixture]
		public class When_responding_with_response_delegate
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public async void Must_execute_delegate()
			{
				bool executed = false;

				_route.RespondWith(
					request =>
					{
						executed = true;
						return (IResponse)new Response().NoContent();
					});

				await _route.ProcessResponseAsync(_context);

				Assert.That(executed, Is.True);
			}

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => new Response().NoContent());

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(Response)));
			}
		}

		[TestFixture]
		public class When_responding_with_response_delegate_and_specific_return_type
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public async void Must_execute_delegate()
			{
				bool executed = false;

				_route.RespondWith(
					request =>
					{
						executed = true;
						return (IResponse)new Response().NoContent();
					},
					typeof(CssResponse));

				await _route.ProcessResponseAsync(_context);

				Assert.That(executed, Is.True);
			}

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => (IResponse)new Response().NoContent(), typeof(CssResponse));

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(CssResponse)));
			}
		}

		[TestFixture]
		public class When_responding_with_task
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => new Response().NoContent().AsCompletedTask());

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(Response)));
			}

			[Test]
			public async void Must_return_response()
			{
				IResponse response = new Response().NoContent();

				_route.RespondWith(response.AsCompletedTask());

				IResponse processedResponse = await _route.ProcessResponseAsync(_context);

				Assert.That(processedResponse, Is.EqualTo(response));
			}
		}

		[TestFixture]
		public class When_responding_with_task_and_specific_return_type
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => new CssResponse("").AsCompletedTask(), typeof(CssResponse));

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(CssResponse)));
			}

			[Test]
			public async void Must_return_response()
			{
				IResponse response = new Response().NoContent();

				_route.RespondWith(response.AsCompletedTask(), typeof(CssResponse));

				IResponse processedResponse = await _route.ProcessResponseAsync(_context);

				Assert.That(processedResponse, Is.EqualTo(response));
			}
		}

		[TestFixture]
		public class When_responding_with_task_delegate
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public async void Must_execute_delegate()
			{
				bool executed = false;

				_route.RespondWith(
					request =>
					{
						executed = true;
						return new Response().NoContent().AsCompletedTask();
					});

				await _route.ProcessResponseAsync(_context);

				Assert.That(executed, Is.True);
			}

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => new Response().NoContent().AsCompletedTask());

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(Response)));
			}
		}

		[TestFixture]
		public class When_responding_with_task_delegate_and_specific_return_type
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private Route.Routing.Route _route;
			private HttpContextBase _context;

			[Test]
			public async void Must_execute_delegate()
			{
				bool executed = false;

				_route.RespondWith(
					request =>
					{
						executed = true;
						return new CssResponse("").AsCompletedTask();
					},
					typeof(CssResponse));

				await _route.ProcessResponseAsync(_context);

				Assert.That(executed, Is.True);
			}

			[Test]
			public void Must_indicate_correct_return_type()
			{
				_route.RespondWith(request => new CssResponse("").AsCompletedTask(), typeof(CssResponse));

				Assert.That(_route.ResponseType, Is.EqualTo(typeof(CssResponse)));
			}
		}

		[TestFixture]
		public class When_restricting_by_accept_charset_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByAcceptCharsetHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<AcceptCharsetHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<AcceptCharsetHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_accept_encoding_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByAcceptEncodingHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<AcceptEncodingHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<AcceptEncodingHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_accept_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByAcceptHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<AcceptHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<AcceptHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_accept_language_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByAcceptLanguageHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<AcceptLanguageHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<AcceptLanguageHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_allow_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByAllowHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<AllowHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<AllowHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_basic_authorization_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByBasicAuthorizationHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<BasicAuthorizationHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<BasicAuthorizationHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_basic_proxy_authorization_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByBasicProxyAuthorizationHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<BasicProxyAuthorizationHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<BasicProxyAuthorizationHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_cache_control_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByCacheControlHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<CacheControlHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<CacheControlHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_connection_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByConnectionHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<ConnectionHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<ConnectionHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_content_encoding_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByContentEncodingHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<ContentEncodingHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<ContentEncodingHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_content_language_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByContentLanguageHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<ContentLanguageHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<ContentLanguageHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_content_length_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByContentLengthHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<ContentLengthHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<ContentLengthHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_content_md5_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByContentMd5Header(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<ContentMd5Header>[] restrictions = _route.GetRestrictions<HeaderRestriction<ContentMd5Header>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_cookie
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByCookie("name1", "value1");
				_route.RestrictByCookie("name2", CaseSensitivePlainComparer.Instance, "value2", CaseSensitiveRegexComparer.Instance);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				CookieRestriction[] restrictions = _route.GetRestrictions<CookieRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_restricting_by_date_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByDateHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<DateHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<DateHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_digest_authorization_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByDigestAuthorizationHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<DigestAuthorizationHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<DigestAuthorizationHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_digest_proxy_authorization_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByDigestProxyAuthorizationHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<DigestProxyAuthorizationHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<DigestProxyAuthorizationHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_expect_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByExpectHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<ExpectHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<ExpectHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_from_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByFromHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<FromHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<FromHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByHeader("field1", "value1", CaseSensitiveRegexComparer.Instance);
				_route.RestrictByHeader("field2", "value2");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction[] restrictions = _route.GetRestrictions<HeaderRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_restricting_by_header_with_delegates
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByHeaders("field1", headerValue => new[] { 0, 1 }, i => true);
				_route.RestrictByHeader("field2", headerValue => 0, i => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<int>[] restrictions = _route.GetRestrictions<HeaderRestriction<int>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_restricting_by_host_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByHostHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<HostHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<HostHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_if_match_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByIfMatchHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<IfMatchHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<IfMatchHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_if_modified_since_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByIfModifiedSinceHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<IfModifiedSinceHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<IfModifiedSinceHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_if_none_match_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByIfNoneMatchHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<IfNoneMatchHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<IfNoneMatchHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_if_range_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByIfRangeHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<IfRangeHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<IfRangeHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_if_unmodified_since_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByIfUnmodifiedSinceHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<IfUnmodifiedSinceHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<IfUnmodifiedSinceHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_max_forwards_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByMaxForwardsHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<MaxForwardsHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<MaxForwardsHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_methods
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByMethods((IEnumerable<string>)new[] { "CONNECT" });
				_route.RestrictByMethods((IEnumerable<HttpMethod>)new[] { HttpMethod.Delete, HttpMethod.Get });
				_route.RestrictByMethods("HEAD", "POST");
				_route.RestrictByMethods(HttpMethod.Put, HttpMethod.Trace);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				MethodRestriction[] restrictions = _route.GetRestrictions<MethodRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(7));
			}
		}

		[TestFixture]
		public class When_restricting_by_missing_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByMissingHeader("header");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				MissingHeaderRestriction[] restrictions = _route.GetRestrictions<MissingHeaderRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_pragma_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByPragmaHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<PragmaHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<PragmaHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_range_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRangeHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<RangeHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<RangeHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<RefererHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<RefererHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrl(uri => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlRestriction[] restrictions = _route.GetRestrictions<RefererUrlRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_absolute_path
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlAbsolutePaths((IEnumerable<string>)new[] { "path1", "path2" });
				_route.RestrictByRefererUrlAbsolutePaths("path3", "path4");
				_route.RestrictByRefererUrlAbsolutePaths(new[] { "path5", "path6" }, CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlAbsolutePath("path7", CaseInsensitivePlainComparer.Instance);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlAbsolutePathRestriction[] restrictions = _route.GetRestrictions<RefererUrlAbsolutePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(7));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_authorities
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlAuthorities((IEnumerable<string>)new[] { "authority1", "authority2" });
				_route.RestrictByRefererUrlAuthorities("authority1", "authority2");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlAuthorityRestriction[] restrictions = _route.GetRestrictions<RefererUrlAuthorityRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_fragment
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlFragment("fragment1", CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlFragments((IEnumerable<string>)new[] { "fragment2", "fragment3" });
				_route.RestrictByRefererUrlFragments(new[] { "fragment4", "fragment5" }, CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlFragments("fragment6", "fragment7");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlFragmentRestriction[] restrictions = _route.GetRestrictions<RefererUrlFragmentRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(7));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_host_types
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlHostTypes(new[] { UriHostNameType.Basic, UriHostNameType.Dns });
				_route.RestrictByRefererUrlHostTypes(UriHostNameType.IPv4, UriHostNameType.IPv6);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlHostTypeRestriction[] restrictions = _route.GetRestrictions<RefererUrlHostTypeRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_hosts
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlHosts(new[] { "host1", "host2" });
				_route.RestrictByRefererUrlHosts("host3", "host4");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlHostRestriction[] restrictions = _route.GetRestrictions<RefererUrlHostRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_paths_and_queries
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlPathAndQuery("path1", CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlPathsAndQueries((IEnumerable<string>)new[] { "path2", "path3" });
				_route.RestrictByRefererUrlPathsAndQueries(new[] { "path4", "path5" }, CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlPathsAndQueries("path6", "path7");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlPathAndQueryRestriction[] restrictions = _route.GetRestrictions<RefererUrlPathAndQueryRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(7));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_ports
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlPorts((IEnumerable<ushort>)new[] { (ushort)0, (ushort)1 });
				_route.RestrictByRefererUrlPorts(2, 3);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlPortRestriction[] restrictions = _route.GetRestrictions<RefererUrlPortRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_queries
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlQuery("query1", CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlQueries((IEnumerable<string>)new[] { "query2", "query3" });
				_route.RestrictByRefererUrlQueries(new[] { "query4", "query5" }, CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlQueries("query6", "query7");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlQueryRestriction[] restrictions = _route.GetRestrictions<RefererUrlQueryRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(7));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_query_string
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlQueryString("field1", CaseInsensitivePlainComparer.Instance, "value1", CaseInsensitivePlainComparer.Instance);
				_route.RestrictByRefererUrlQueryString("field2", "value2");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlQueryStringRestriction[] restrictions = _route.GetRestrictions<RefererUrlQueryStringRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_restricting_by_referer_url_schemes
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByRefererUrlSchemes((IEnumerable<string>)new[] { "scheme1", "scheme2" });
				_route.RestrictByRefererUrlSchemes("scheme3", "scheme4");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				RefererUrlSchemeRestriction[] restrictions = _route.GetRestrictions<RefererUrlSchemeRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_relative_paths
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_route.RestrictByUrlRelativePath("path1", CaseInsensitivePlainComparer.Instance, _httpRuntime);
				_route.RestrictByUrlRelativePaths(new[] { "path2", "path3" }, _httpRuntime);
			}

			private Route.Routing.Route _route;
			private IHttpRuntime _httpRuntime;

			[Test]
			public void Must_add_restrictions()
			{
				UrlRelativePathRestriction[] restrictions = _route.GetRestrictions<UrlRelativePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(3));
			}
		}

		[TestFixture]
		public class When_restricting_by_te_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByTeHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<TeHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<TeHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_trailer_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByTrailerHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<TrailerHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<TrailerHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_transfer_encoding_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByTransferEncodingHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<TransferEncodingHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<TransferEncodingHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_upgrade_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUpgradeHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<UpgradeHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<UpgradeHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_url
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrl(uri => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlRestriction[] restrictions = _route.GetRestrictions<UrlRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_authorities
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlAuthorities((IEnumerable<string>)new[] { "authority1", "authority2" });
				_route.RestrictByUrlAuthorities("authority1", "authority2");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlAuthorityRestriction[] restrictions = _route.GetRestrictions<UrlAuthorityRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_fragment
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlFragment("fragment1", CaseInsensitivePlainComparer.Instance);
				_route.RestrictByUrlFragments((IEnumerable<string>)new[] { "fragment2", "fragment3" });
				_route.RestrictByUrlFragments(new[] { "fragment4", "fragment5" }, CaseInsensitivePlainComparer.Instance);
				_route.RestrictByUrlFragments("fragment6", "fragment7");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlFragmentRestriction[] restrictions = _route.GetRestrictions<UrlFragmentRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(7));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_host_types
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlHostTypes(new[] { UriHostNameType.Basic, UriHostNameType.Dns });
				_route.RestrictByUrlHostTypes(UriHostNameType.IPv4, UriHostNameType.IPv6);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlHostTypeRestriction[] restrictions = _route.GetRestrictions<UrlHostTypeRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_hosts
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlHosts(new[] { "host1", "host2" });
				_route.RestrictByUrlHosts("host3", "host4");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlHostRestriction[] restrictions = _route.GetRestrictions<UrlHostRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_ports
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlPorts((IEnumerable<ushort>)new[] { (ushort)0, (ushort)1 });
				_route.RestrictByUrlPorts(2, 3);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlPortRestriction[] restrictions = _route.GetRestrictions<UrlPortRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_queries
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlQuery("query1", CaseInsensitivePlainComparer.Instance);
				_route.RestrictByUrlQueries((IEnumerable<string>)new[] { "query2", "query3" });
				_route.RestrictByUrlQueries(new[] { "query4", "query5" }, CaseInsensitivePlainComparer.Instance);
				_route.RestrictByUrlQueries("query6", "query7");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlQueryRestriction[] restrictions = _route.GetRestrictions<UrlQueryRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(7));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_query_string
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlQueryString("field1", CaseInsensitivePlainComparer.Instance, "value1", CaseInsensitivePlainComparer.Instance);
				_route.RestrictByUrlQueryString("field2", "value2");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlQueryStringRestriction[] restrictions = _route.GetRestrictions<UrlQueryStringRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(2));
			}
		}

		[TestFixture]
		public class When_restricting_by_url_schemes
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUrlSchemes((IEnumerable<string>)new[] { "scheme1", "scheme2" });
				_route.RestrictByUrlSchemes("scheme3", "scheme4");
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				UrlSchemeRestriction[] restrictions = _route.GetRestrictions<UrlSchemeRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(4));
			}
		}

		[TestFixture]
		public class When_restricting_by_user_agent_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByUserAgentHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<UserAgentHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<UserAgentHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_vary_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByVaryHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<VaryHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<VaryHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_via_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByViaHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<ViaHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<ViaHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_restricting_by_warning_header
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.RestrictByWarningHeader(header => true);
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_add_restrictions()
			{
				HeaderRestriction<WarningHeader>[] restrictions = _route.GetRestrictions<HeaderRestriction<WarningHeader>>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_testing_if_route_has_generic_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(
					new HeaderRestriction<DateHeader>("Date", headerValue => DateHeader.Parse(headerValue), header => true),
					new HeaderRestriction<AllowHeader>("Allow", headerValue => AllowHeader.ParseMany(headerValue), header => true));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_return_correct_result()
			{
				Assert.That(_route.HasGenericRestrictions(typeof(HeaderRestriction<>)), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_route_has_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(new MethodRestriction("GET"), new UrlPortRestriction(0), new MethodRestriction("POST"), new UrlPortRestriction(1));
			}

			private Route.Routing.Route _route;

			[Test]
			public void Must_return_correct_result()
			{
				Assert.That(_route.HasRestrictions<MethodRestriction>(), Is.True);
				Assert.That(_route.HasRestrictions<UrlPortRestriction>(), Is.True);
				Assert.That(_route.HasRestrictions<UrlHostRestriction>(), Is.False);
				Assert.That(_route.HasRestrictions(typeof(MethodRestriction)), Is.True);
				Assert.That(_route.HasRestrictions(typeof(UrlPortRestriction)), Is.True);
				Assert.That(_route.HasRestrictions(typeof(UrlHostRestriction)), Is.False);
			}
		}

		[TestFixture]
		public class When_testing_if_route_matches_restrictions_with_all_matching_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance), new UrlPortRestriction(80));
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://host:80"));
				_matchResult = _route.MatchesRequestAsync(_request).Result;
			}

			private Route.Routing.Route _route;
			private HttpRequestBase _request;
			private MatchResult _matchResult;

			[Test]
			public void Must_result_in_a_match()
			{
				Assert.That(_matchResult.ResultType, Is.EqualTo(MatchResultType.RouteMatched));
			}
		}

		[TestFixture]
		public class When_testing_if_route_matches_restrictions_with_no_matching_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance), new UrlPortRestriction(80));
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://host1:81"));
				_matchResult = _route.MatchesRequestAsync(_request).Result;
			}

			private Route.Routing.Route _route;
			private HttpRequestBase _request;
			private MatchResult _matchResult;

			[Test]
			public void Must_result_in_a_match()
			{
				Assert.That(_matchResult.ResultType, Is.EqualTo(MatchResultType.RouteNotMatched));
			}
		}

		[TestFixture]
		public class When_testing_if_route_matches_restrictions_with_some_matching_restrictions
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route("name", Guid.NewGuid(), Scheme.NotSpecified, "route");
				_route.AddRestrictions(new UrlHostRestriction("host", CaseInsensitivePlainComparer.Instance), new UrlPortRestriction(80));
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Url).Return(new Uri("http://host:81"));
				_matchResult = _route.MatchesRequestAsync(_request).Result;
			}

			private Route.Routing.Route _route;
			private HttpRequestBase _request;
			private MatchResult _matchResult;

			[Test]
			public void Must_result_in_a_match()
			{
				Assert.That(_matchResult.ResultType, Is.EqualTo(MatchResultType.RouteNotMatched));
			}
		}
	}
}