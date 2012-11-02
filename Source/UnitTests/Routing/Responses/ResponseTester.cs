using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using Junior.Route.Common;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

using Rhino.Mocks;

using Cookie = Junior.Route.Routing.Responses.Cookie;

namespace Junior.Route.UnitTests.Routing.Responses
{
	public static class ResponseTester
	{
		[TestFixture]
		public class When_creating_instance_with_httpstatuscode
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.Created, 1);
			}

			private IResponse _response;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Created));
				Assert.That(_response.StatusCode.SubStatusCode, Is.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_integer_status_codes
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(201, 1);
			}

			private IResponse _response;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_response.StatusCode.StatusCode, Is.EqualTo(201));
				Assert.That(_response.StatusCode.SubStatusCode, Is.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_redirect
		{
			[SetUp]
			public void SetUp()
			{
				_responseLocationDelegates.AddRange(
					Response.Found,
					Response.Moved,
					Response.MovedPermanently,
					Response.Redirect,
					Response.RedirectKeepVerb,
					Response.RedirectMethod,
					Response.SeeOther,
					Response.TemporaryRedirect);
				_responseRouteNameDelegates.AddRange(
					Response.FoundRoute,
					Response.MovedPermanentlyToRoute,
					Response.MovedToRoute,
					Response.RedirectMethodToRoute,
					Response.RedirectToRoute,
					Response.RedirectToRouteKeepVerb,
					Response.SeeOtherRoute,
					Response.TemporaryRedirectToRoute);
				_responseRouteIdDelegates.AddRange(
					Response.FoundRoute,
					Response.MovedPermanentlyToRoute,
					Response.MovedToRoute,
					Response.RedirectMethodToRoute,
					Response.RedirectToRoute,
					Response.RedirectToRouteKeepVerb,
					Response.SeeOtherRoute,
					Response.TemporaryRedirectToRoute);
				_responseRelativeUrlDelegates.AddRange(
					Response.FoundRelativeUrl,
					Response.MovedPermanentlyToRelativeUrl,
					Response.MovedToRelativeUrl,
					Response.RedirectMethodToRelativeUrl,
					Response.RedirectToRelativeUrl,
					Response.RedirectToRelativeUrlKeepVerb,
					Response.SeeOtherRelativeUrl,
					Response.TemporaryRedirectToRelativeUrl);
				_urlResolver = MockRepository.GenerateMock<IUrlResolver>();
				_urlResolver.Stub(arg => arg.Absolute(Arg<string>.Is.Anything)).WhenCalled(arg => arg.ReturnValue = "http://" + arg.Arguments.First()).Return(null);
				_urlResolver.Stub(arg => arg.Route(Arg<string>.Is.Anything)).WhenCalled(arg => arg.ReturnValue = "http://" + arg.Arguments.First()).Return(null);
				_urlResolver.Stub(arg => arg.Route(Arg<Guid>.Is.Anything)).WhenCalled(arg => arg.ReturnValue = "http://" + arg.Arguments.First()).Return(null);
			}

			private readonly HashSet<Func<string, Response>> _responseLocationDelegates = new HashSet<Func<string, Response>>();
			private readonly HashSet<Func<IUrlResolver, string, Response>> _responseRouteNameDelegates = new HashSet<Func<IUrlResolver, string, Response>>();
			private readonly HashSet<Func<IUrlResolver, Guid, Response>> _responseRouteIdDelegates = new HashSet<Func<IUrlResolver, Guid, Response>>();
			private readonly HashSet<Func<IUrlResolver, string, Response>> _responseRelativeUrlDelegates = new HashSet<Func<IUrlResolver, string, Response>>();
			private IUrlResolver _urlResolver;

			[Test]
			public void Must_set_location_header()
			{
				foreach (Func<string, Response> responseLocationDelegate in _responseLocationDelegates)
				{
					IResponse response = responseLocationDelegate("test");
					Header header = response.Headers.First();

					Assert.That(header.Field, Is.EqualTo("Location"));
					Assert.That(header.Value, Is.EqualTo("test"));
				}

				foreach (Func<IUrlResolver, string, Response> responseRouteNameDelegate in _responseRouteNameDelegates)
				{
					IResponse response = responseRouteNameDelegate(_urlResolver, "test");
					Header header = response.Headers.First();

					Assert.That(header.Field, Is.EqualTo("Location"));
					Assert.That(header.Value, Is.EqualTo("http://test"));
				}

				foreach (Func<IUrlResolver, Guid, Response> responseRouteIdDelegate in _responseRouteIdDelegates)
				{
					Guid routeId = Guid.NewGuid();
					IResponse response = responseRouteIdDelegate(_urlResolver, routeId);
					Header header = response.Headers.First();

					Assert.That(header.Field, Is.EqualTo("Location"));
					Assert.That(header.Value, Is.EqualTo("http://" + routeId.ToString()));
				}

				foreach (Func<IUrlResolver, string, Response> responseRelativeUrlDelegate in _responseRelativeUrlDelegates)
				{
					IResponse response = responseRelativeUrlDelegate(_urlResolver, "test");
					Header header = response.Headers.First();

					Assert.That(header.Field, Is.EqualTo("Location"));
					Assert.That(header.Value, Is.EqualTo("http://test"));
				}
			}
		}

		[TestFixture]
		public class When_creating_instance_with_status_code_factory
		{
			[SetUp]
			public void SetUp()
			{
				_statusCodesByResponse = new Dictionary<IResponse, HttpStatusCode>
					{
						{ Response.Accepted(), HttpStatusCode.Accepted },
						{ Response.Ambiguous(), HttpStatusCode.Ambiguous },
						{ Response.BadGateway(), HttpStatusCode.BadGateway },
						{ Response.BadRequest(), HttpStatusCode.BadRequest },
						{ Response.Conflict(), HttpStatusCode.Conflict },
						{ Response.Continue(), HttpStatusCode.Continue },
						{ Response.Created(), HttpStatusCode.Created },
						{ Response.ExpectationFailed(), HttpStatusCode.ExpectationFailed },
						{ Response.Forbidden(), HttpStatusCode.Forbidden },
						{ Response.Found(), HttpStatusCode.Found },
						{ Response.GatewayTimeout(), HttpStatusCode.GatewayTimeout },
						{ Response.Gone(), HttpStatusCode.Gone },
						{ Response.HttpVersionNotSupported(), HttpStatusCode.HttpVersionNotSupported },
						{ Response.InternalServerError(), HttpStatusCode.InternalServerError },
						{ Response.LengthRequired(), HttpStatusCode.LengthRequired },
						{ Response.MethodNotAllowed(), HttpStatusCode.MethodNotAllowed },
						{ Response.Moved(), HttpStatusCode.Moved },
						{ Response.MovedPermanently(), HttpStatusCode.MovedPermanently },
						{ Response.MultipleChoices(), HttpStatusCode.MultipleChoices },
						{ Response.NoContent(), HttpStatusCode.NoContent },
						{ Response.NonAuthoritativeInformation(), HttpStatusCode.NonAuthoritativeInformation },
						{ Response.NotAcceptable(), HttpStatusCode.NotAcceptable },
						{ Response.NotFound(), HttpStatusCode.NotFound },
						{ Response.NotImplemented(), HttpStatusCode.NotImplemented },
						{ Response.NotModified(), HttpStatusCode.NotModified },
						{ Response.OK(), HttpStatusCode.OK },
						{ Response.PartialContent(), HttpStatusCode.PartialContent },
						{ Response.PaymentRequired(), HttpStatusCode.PaymentRequired },
						{ Response.PreconditionFailed(), HttpStatusCode.PreconditionFailed },
						{ Response.ProxyAuthenticationRequired(), HttpStatusCode.ProxyAuthenticationRequired },
						{ Response.Redirect(), HttpStatusCode.Redirect },
						{ Response.RedirectKeepVerb(), HttpStatusCode.RedirectKeepVerb },
						{ Response.RedirectMethod(), HttpStatusCode.RedirectMethod },
						{ Response.RequestEntityTooLarge(), HttpStatusCode.RequestEntityTooLarge },
						{ Response.RequestTimeout(), HttpStatusCode.RequestTimeout },
						{ Response.RequestUriTooLong(), HttpStatusCode.RequestUriTooLong },
						{ Response.RequestedRangeNotSatisfiable(), HttpStatusCode.RequestedRangeNotSatisfiable },
						{ Response.ResetContent(), HttpStatusCode.ResetContent },
						{ Response.SeeOther(), HttpStatusCode.SeeOther },
						{ Response.ServiceUnavailable(), HttpStatusCode.ServiceUnavailable },
						{ Response.SwitchingProtocols(), HttpStatusCode.SwitchingProtocols },
						{ Response.TemporaryRedirect(), HttpStatusCode.TemporaryRedirect },
						{ Response.Unauthorized(), HttpStatusCode.Unauthorized },
						{ Response.UnsupportedMediaType(), HttpStatusCode.UnsupportedMediaType },
						{ Response.Unused(), HttpStatusCode.Unused },
						{ Response.UseProxy(), HttpStatusCode.UseProxy }
					};
			}

			private Dictionary<IResponse, HttpStatusCode> _statusCodesByResponse;

			[Test]
			public void Must_set_content_type()
			{
				foreach (KeyValuePair<IResponse, HttpStatusCode> pair in _statusCodesByResponse)
				{
					Assert.That(pair.Key.StatusCode.ParsedStatusCode, Is.EqualTo(pair.Value));
				}
			}
		}

		[TestFixture]
		public class When_creating_instance_with_statusandsubstatuscodes
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(new StatusAndSubStatusCode(HttpStatusCode.Created, 1));
			}

			private IResponse _response;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_response.StatusCode.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
				Assert.That(_response.StatusCode.SubStatusCode, Is.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_setting_charset
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.Charset("utf-8");
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).Charset, Is.EqualTo("utf-8"));
			}
		}

		[TestFixture]
		public class When_setting_content_encoding
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.ContentEncoding(Encoding.ASCII);
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).ContentEncoding, Is.SameAs(Encoding.ASCII));
			}
		}

		[TestFixture]
		public class When_setting_content_type
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.ContentType("text/html");
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).ContentType, Is.EqualTo("text/html"));
			}
		}

		[TestFixture]
		public class When_setting_content_with_byte_array
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.Content(new byte[] { 0, 1, 2, 3 });
			}

			private Response _response;

			[Test]
			public void Must_return_content()
			{
				Assert.That(_response.GetContent(), Is.EquivalentTo(new byte[] { 0, 1, 2, 3 }));
			}
		}

		[TestFixture]
		public class When_setting_content_with_byte_array_delegate
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.Content(() => new byte[] { 0, 1, 2, 3 });
			}

			private Response _response;

			[Test]
			public void Must_return_content()
			{
				Assert.That(_response.GetContent(), Is.EquivalentTo(new byte[] { 0, 1, 2, 3 }));
			}
		}

		[TestFixture]
		public class When_setting_content_with_string
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.Content("content");
			}

			private Response _response;

			[Test]
			public void Must_return_content()
			{
				Assert.That(_response.GetContent(), Is.EqualTo(Encoding.ASCII.GetBytes("content")));
			}
		}

		[TestFixture]
		public class When_setting_content_with_string_delegate
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.Content(() => "content");
			}

			private Response _response;

			[Test]
			public void Must_return_content()
			{
				Assert.That(_response.GetContent(), Is.EqualTo(Encoding.ASCII.GetBytes("content")));
			}
		}

		[TestFixture]
		public class When_setting_cookies
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.Cookie(new Cookie(new HttpCookie("name1")));
				_response.Cookie(new HttpCookie("name2"));
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Cookie[] cookies = ((IResponse)_response).Cookies.ToArray();

				Assert.That(cookies, Has.Length.EqualTo(2));

				for (int i = 0; i < cookies.Length; i++)
				{
					Assert.That(cookies[i].Name, Is.EqualTo("name" + (i + 1)));
				}
			}
		}

		[TestFixture]
		public class When_setting_default_content_encoding
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.DefaultContentEncoding();
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).ContentEncoding, Is.SameAs(Encoding.UTF8));
			}
		}

		[TestFixture]
		public class When_setting_default_header_encoding
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.DefaultHeaderEncoding();
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).HeaderEncoding, Is.SameAs(Encoding.UTF8));
			}
		}

		[TestFixture]
		public class When_setting_header_encoding
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.HeaderEncoding(Encoding.ASCII);
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).HeaderEncoding, Is.SameAs(Encoding.ASCII));
			}
		}

		[TestFixture]
		public class When_setting_headers
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.Header("field1", "value1");
				_response.Headers((IEnumerable<Header>)new[] { new Header("field2", "value2"), new Header("field3", "value3") });
				_response.Headers(new Header("field4", "value4"), new Header("field5", "value5"));
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Header[] headers = ((IResponse)_response).Headers.ToArray();

				Assert.That(headers, Has.Length.EqualTo(5));

				for (int i = 0; i < headers.Length; i++)
				{
					Assert.That(headers[i].Field, Is.EqualTo("field" + (i + 1)));
					Assert.That(headers[i].Value, Is.EqualTo("value" + (i + 1)));
				}
			}
		}

		[TestFixture]
		public class When_setting_no_charset
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.NoCharset();
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).Charset, Is.Null);
			}
		}

		[TestFixture]
		public class When_setting_no_content_type
		{
			[SetUp]
			public void SetUp()
			{
				_response = new Response(HttpStatusCode.OK);
				_response.NoContentType();
			}

			private Response _response;

			[Test]
			public void Must_set_property()
			{
				Assert.That(((IResponse)_response).ContentType, Is.Null);
			}
		}

		[TestFixture]
		public class When_setting_predefined_content_type
		{
			[SetUp]
			public void SetUp()
			{
				contentTypesByResponse = new Dictionary<IResponse, string>
					{
						{ Response.OK().ApplicationAtom(), "application/atom+xml" },
						{ Response.OK().ApplicationDtd(), "application/xml-dtd" },
						{ Response.OK().ApplicationEcmaScript(), "application/ecmascript" },
						{ Response.OK().ApplicationEdifact(), "application/EDIFACT" },
						{ Response.OK().ApplicationEdiX12(), "application/EDI-X12" },
						{ Response.OK().ApplicationGzip(), "application/gzip" },
						{ Response.OK().ApplicationJavaScript(), "application/javascript" },
						{ Response.OK().ApplicationJson(), "application/json" },
						{ Response.OK().ApplicationOctetStream(), "application/octet-stream" },
						{ Response.OK().ApplicationOgg(), "application/ogg" },
						{ Response.OK().ApplicationPdf(), "application/pdf" },
						{ Response.OK().ApplicationPostscript(), "application/postscript" },
						{ Response.OK().ApplicationRdf(), "application/rdf+xml" },
						{ Response.OK().ApplicationRss(), "application/rss+xml" },
						{ Response.OK().ApplicationSoap(), "application/soap+xml" },
						{ Response.OK().ApplicationXHtml(), "application/xhtml+xml" },
						{ Response.OK().ApplicationXop(), "application/xop+xml" },
						{ Response.OK().ApplicationZip(), "application/zip" },
						{ Response.OK().AudioBasic(), "audio/basic" },
						{ Response.OK().AudioL24(), "audio/L24" },
						{ Response.OK().AudioMp4(), "audio/mp4" },
						{ Response.OK().AudioMpeg(), "audio/mpeg" },
						{ Response.OK().AudioOgg(), "audio/ogg" },
						{ Response.OK().AudioRealAudio(), "audio/vnd.rn-realaudio" },
						{ Response.OK().AudioVorbis(), "audio/vorbis" },
						{ Response.OK().AudioWav(), "audio/vnd.wave" },
						{ Response.OK().AudioWebM(), "audio/webm" },
						{ Response.OK().ImageGif(), "image/gif" },
						{ Response.OK().ImageJpeg(), "image/jpeg" },
						{ Response.OK().ImagePJpeg(), "image/pjpeg" },
						{ Response.OK().ImagePng(), "image/png" },
						{ Response.OK().ImageSvg(), "image/svg+xml" },
						{ Response.OK().ImageTiff(), "image/tiff" },
						{ Response.OK().ImageIco(), "image/vnd.microsoft.icon" },
						{ Response.OK().MessageHttp(), "message/http" },
						{ Response.OK().MessageImdn(), "message/imdn+xml" },
						{ Response.OK().MessagePartial(), "message/partial" },
						{ Response.OK().MessageRfc822(), "message/rfc822" },
						{ Response.OK().ModelExample(), "model/example" },
						{ Response.OK().ModelIges(), "model/iges" },
						{ Response.OK().ModelMesh(), "model/mesh" },
						{ Response.OK().ModelVrml(), "model/vrml" },
						{ Response.OK().ModelX3DBinary(), "model/x3d+binary" },
						{ Response.OK().ModelX3DVrml(), "model/x3d+vrml" },
						{ Response.OK().ModelX3DXml(), "model/x3d+xml" },
						{ Response.OK().MultipartAlternative(), "multipart/alternative" },
						{ Response.OK().MultipartEncrypted(), "multipart/encrypted" },
						{ Response.OK().MultipartFormData(), "multipart/form-data" },
						{ Response.OK().MultipartMixed(), "multipart/mixed" },
						{ Response.OK().MultipartRelated(), "multipart/related" },
						{ Response.OK().MultipartSigned(), "multipart/signed" },
						{ Response.OK().TextCmd(), "text/cmd" },
						{ Response.OK().TextCss(), "text/css" },
						{ Response.OK().TextCsv(), "text/csv" },
						{ Response.OK().TextHtml(), "text/html" },
						{ Response.OK().TextPlain(), "text/plain" },
						{ Response.OK().TextVCard(), "text/vcard" },
						{ Response.OK().TextXml(), "text/xml" },
						{ Response.OK().VideoFlv(), "video/x-flv" },
						{ Response.OK().VideoMatroska(), "video/x-matroska" },
						{ Response.OK().VideoMp4(), "video/mp4" },
						{ Response.OK().VideoMpeg(), "video/mpeg" },
						{ Response.OK().VideoOgg(), "video/ogg" },
						{ Response.OK().VideoQuickTime(), "video/quicktime" },
						{ Response.OK().VideoWebM(), "video/webm" },
						{ Response.OK().VideoWmv(), "video/x-ms-wmv" },
						{ Response.OK().ApplicationOpenDocumentText(), "application/vnd.oasis.opendocument.text" },
						{ Response.OK().ApplicationOpenDocumentSpreadsheet(), "application/vnd.oasis.opendocument.spreadsheet" },
						{ Response.OK().ApplicationOpenDocumentPresentation(), "application/vnd.oasis.opendocument.presentation" },
						{ Response.OK().ApplicationOpenDocumentGraphics(), "application/vnd.oasis.opendocument.graphics" },
						{ Response.OK().ApplicationExcel(), "application/vnd.ms-excel" },
						{ Response.OK().ApplicationExcel2007(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
						{ Response.OK().ApplicationPowerpoint(), "application/vnd.ms-powerpoint" },
						{ Response.OK().ApplicationPowerpoint2007(), "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
						{ Response.OK().ApplicationWord2007(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
						{ Response.OK().ApplicationMozillaXul(), "application/vnd.mozilla.xul+xml" },
						{ Response.OK().ApplicationGoogleEarthKml(), "application/vnd.google-earth.kml+xml" },
						{ Response.OK().ApplicationDeb(), "application/x-deb" },
						{ Response.OK().ApplicationDvi(), "application/x-dvi" },
						{ Response.OK().ApplicationFontTtf(), "application/x-font-ttf" },
						{ Response.OK().ApplicationLaTeX(), "application/x-latex" },
						{ Response.OK().ApplicationMpegUrl(), "application/x-mpegURL" },
						{ Response.OK().ApplicationRarCompressed(), "application/x-rar-compressed" },
						{ Response.OK().ApplicationShockwaveFlash(), "application/x-shockwave-flash" },
						{ Response.OK().ApplicationStuffIt(), "application/x-stuffit" },
						{ Response.OK().ApplicationTar(), "application/x-tar" },
						{ Response.OK().ApplicationFormEncoded(), "application/x-www-form-urlencoded" },
						{ Response.OK().ApplicationXpInstall(), "application/x-xpinstall" },
						{ Response.OK().AudioAac(), "audio/x-aac" },
						{ Response.OK().AudioCaf(), "audio/x-caf" },
						{ Response.OK().ImageXcf(), "image/x-xcf" },
						{ Response.OK().TextGwtRpc(), "text/x-gwt-rpc" },
						{ Response.OK().TextJQueryTmpl(), "text/x-jquery-tmpl" },
						{ Response.OK().ApplicationPkcs12(), "application/x-pkcs12" },
						{ Response.OK().ApplicationPkcs7Certificates(), "application/x-pkcs7-certificates" },
						{ Response.OK().ApplicationPkcs7CertReqResp(), "application/x-pkcs7-certreqresp" },
						{ Response.OK().ApplicationPkcs7Mime(), "application/x-pkcs7-mime" },
						{ Response.OK().ApplicationPkcs7Signature(), "application/x-pkcs7-signature" },
					};
			}

			private Dictionary<IResponse, string> contentTypesByResponse;

			[Test]
			public void Must_set_content_type()
			{
				foreach (KeyValuePair<IResponse, string> pair in contentTypesByResponse)
				{
					Assert.That(pair.Key.ContentType, Is.EqualTo(pair.Value));
				}
			}
		}
	}
}