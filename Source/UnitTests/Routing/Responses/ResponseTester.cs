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
					new Response().Found,
					new Response().Moved,
					new Response().MovedPermanently,
					new Response().Redirect,
					new Response().RedirectKeepVerb,
					new Response().RedirectMethod,
					new Response().SeeOther,
					new Response().TemporaryRedirect);
				_responseRouteNameDelegates.AddRange(
					new Response().FoundRoute,
					new Response().MovedPermanentlyToRoute,
					new Response().MovedToRoute,
					new Response().RedirectMethodToRoute,
					new Response().RedirectToRoute,
					new Response().RedirectToRouteKeepVerb,
					new Response().SeeOtherRoute,
					new Response().TemporaryRedirectToRoute);
				_responseRouteIdDelegates.AddRange(
					new Response().FoundRoute,
					new Response().MovedPermanentlyToRoute,
					new Response().MovedToRoute,
					new Response().RedirectMethodToRoute,
					new Response().RedirectToRoute,
					new Response().RedirectToRouteKeepVerb,
					new Response().SeeOtherRoute,
					new Response().TemporaryRedirectToRoute);
				_responseRelativeUrlDelegates.AddRange(
					new Response().FoundRelativeUrl,
					new Response().MovedPermanentlyToRelativeUrl,
					new Response().MovedToRelativeUrl,
					new Response().RedirectMethodToRelativeUrl,
					new Response().RedirectToRelativeUrl,
					new Response().RedirectToRelativeUrlKeepVerb,
					new Response().SeeOtherRelativeUrl,
					new Response().TemporaryRedirectToRelativeUrl);
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
						{ new Response().Accepted(), HttpStatusCode.Accepted },
						{ new Response().Ambiguous(), HttpStatusCode.Ambiguous },
						{ new Response().BadGateway(), HttpStatusCode.BadGateway },
						{ new Response().BadRequest(), HttpStatusCode.BadRequest },
						{ new Response().Conflict(), HttpStatusCode.Conflict },
						{ new Response().Continue(), HttpStatusCode.Continue },
						{ new Response().Created(), HttpStatusCode.Created },
						{ new Response().ExpectationFailed(), HttpStatusCode.ExpectationFailed },
						{ new Response().Forbidden(), HttpStatusCode.Forbidden },
						{ new Response().Found(), HttpStatusCode.Found },
						{ new Response().GatewayTimeout(), HttpStatusCode.GatewayTimeout },
						{ new Response().Gone(), HttpStatusCode.Gone },
						{ new Response().HttpVersionNotSupported(), HttpStatusCode.HttpVersionNotSupported },
						{ new Response().InternalServerError(), HttpStatusCode.InternalServerError },
						{ new Response().LengthRequired(), HttpStatusCode.LengthRequired },
						{ new Response().MethodNotAllowed(), HttpStatusCode.MethodNotAllowed },
						{ new Response().Moved(), HttpStatusCode.Moved },
						{ new Response().MovedPermanently(), HttpStatusCode.MovedPermanently },
						{ new Response().MultipleChoices(), HttpStatusCode.MultipleChoices },
						{ new Response().NoContent(), HttpStatusCode.NoContent },
						{ new Response().NonAuthoritativeInformation(), HttpStatusCode.NonAuthoritativeInformation },
						{ new Response().NotAcceptable(), HttpStatusCode.NotAcceptable },
						{ new Response().NotFound(), HttpStatusCode.NotFound },
						{ new Response().NotImplemented(), HttpStatusCode.NotImplemented },
						{ new Response().NotModified(), HttpStatusCode.NotModified },
						{ new Response().OK(), HttpStatusCode.OK },
						{ new Response().PartialContent(), HttpStatusCode.PartialContent },
						{ new Response().PaymentRequired(), HttpStatusCode.PaymentRequired },
						{ new Response().PreconditionFailed(), HttpStatusCode.PreconditionFailed },
						{ new Response().ProxyAuthenticationRequired(), HttpStatusCode.ProxyAuthenticationRequired },
						{ new Response().Redirect(), HttpStatusCode.Redirect },
						{ new Response().RedirectKeepVerb(), HttpStatusCode.RedirectKeepVerb },
						{ new Response().RedirectMethod(), HttpStatusCode.RedirectMethod },
						{ new Response().RequestEntityTooLarge(), HttpStatusCode.RequestEntityTooLarge },
						{ new Response().RequestTimeout(), HttpStatusCode.RequestTimeout },
						{ new Response().RequestUriTooLong(), HttpStatusCode.RequestUriTooLong },
						{ new Response().RequestedRangeNotSatisfiable(), HttpStatusCode.RequestedRangeNotSatisfiable },
						{ new Response().ResetContent(), HttpStatusCode.ResetContent },
						{ new Response().SeeOther(), HttpStatusCode.SeeOther },
						{ new Response().ServiceUnavailable(), HttpStatusCode.ServiceUnavailable },
						{ new Response().SwitchingProtocols(), HttpStatusCode.SwitchingProtocols },
						{ new Response().TemporaryRedirect(), HttpStatusCode.TemporaryRedirect },
						{ new Response().Unauthorized(), HttpStatusCode.Unauthorized },
						{ new Response().UnsupportedMediaType(), HttpStatusCode.UnsupportedMediaType },
						{ new Response().Unused(), HttpStatusCode.Unused },
						{ new Response().UseProxy(), HttpStatusCode.UseProxy }
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
						{ new Response().OK().ApplicationAtom(), "application/atom+xml" },
						{ new Response().OK().ApplicationDtd(), "application/xml-dtd" },
						{ new Response().OK().ApplicationEcmaScript(), "application/ecmascript" },
						{ new Response().OK().ApplicationEdifact(), "application/EDIFACT" },
						{ new Response().OK().ApplicationEdiX12(), "application/EDI-X12" },
						{ new Response().OK().ApplicationGzip(), "application/gzip" },
						{ new Response().OK().ApplicationJavaScript(), "application/javascript" },
						{ new Response().OK().ApplicationJson(), "application/json" },
						{ new Response().OK().ApplicationOctetStream(), "application/octet-stream" },
						{ new Response().OK().ApplicationOgg(), "application/ogg" },
						{ new Response().OK().ApplicationPdf(), "application/pdf" },
						{ new Response().OK().ApplicationPostscript(), "application/postscript" },
						{ new Response().OK().ApplicationRdf(), "application/rdf+xml" },
						{ new Response().OK().ApplicationRss(), "application/rss+xml" },
						{ new Response().OK().ApplicationSoap(), "application/soap+xml" },
						{ new Response().OK().ApplicationXHtml(), "application/xhtml+xml" },
						{ new Response().OK().ApplicationXop(), "application/xop+xml" },
						{ new Response().OK().ApplicationZip(), "application/zip" },
						{ new Response().OK().AudioBasic(), "audio/basic" },
						{ new Response().OK().AudioL24(), "audio/L24" },
						{ new Response().OK().AudioMp4(), "audio/mp4" },
						{ new Response().OK().AudioMpeg(), "audio/mpeg" },
						{ new Response().OK().AudioOgg(), "audio/ogg" },
						{ new Response().OK().AudioRealAudio(), "audio/vnd.rn-realaudio" },
						{ new Response().OK().AudioVorbis(), "audio/vorbis" },
						{ new Response().OK().AudioWav(), "audio/vnd.wave" },
						{ new Response().OK().AudioWebM(), "audio/webm" },
						{ new Response().OK().ImageGif(), "image/gif" },
						{ new Response().OK().ImageJpeg(), "image/jpeg" },
						{ new Response().OK().ImagePJpeg(), "image/pjpeg" },
						{ new Response().OK().ImagePng(), "image/png" },
						{ new Response().OK().ImageSvg(), "image/svg+xml" },
						{ new Response().OK().ImageTiff(), "image/tiff" },
						{ new Response().OK().ImageIco(), "image/vnd.microsoft.icon" },
						{ new Response().OK().MessageHttp(), "message/http" },
						{ new Response().OK().MessageImdn(), "message/imdn+xml" },
						{ new Response().OK().MessagePartial(), "message/partial" },
						{ new Response().OK().MessageRfc822(), "message/rfc822" },
						{ new Response().OK().ModelExample(), "model/example" },
						{ new Response().OK().ModelIges(), "model/iges" },
						{ new Response().OK().ModelMesh(), "model/mesh" },
						{ new Response().OK().ModelVrml(), "model/vrml" },
						{ new Response().OK().ModelX3DBinary(), "model/x3d+binary" },
						{ new Response().OK().ModelX3DVrml(), "model/x3d+vrml" },
						{ new Response().OK().ModelX3DXml(), "model/x3d+xml" },
						{ new Response().OK().MultipartAlternative(), "multipart/alternative" },
						{ new Response().OK().MultipartEncrypted(), "multipart/encrypted" },
						{ new Response().OK().MultipartFormData(), "multipart/form-data" },
						{ new Response().OK().MultipartMixed(), "multipart/mixed" },
						{ new Response().OK().MultipartRelated(), "multipart/related" },
						{ new Response().OK().MultipartSigned(), "multipart/signed" },
						{ new Response().OK().TextCmd(), "text/cmd" },
						{ new Response().OK().TextCss(), "text/css" },
						{ new Response().OK().TextCsv(), "text/csv" },
						{ new Response().OK().TextHtml(), "text/html" },
						{ new Response().OK().TextPlain(), "text/plain" },
						{ new Response().OK().TextVCard(), "text/vcard" },
						{ new Response().OK().TextXml(), "text/xml" },
						{ new Response().OK().VideoFlv(), "video/x-flv" },
						{ new Response().OK().VideoMatroska(), "video/x-matroska" },
						{ new Response().OK().VideoMp4(), "video/mp4" },
						{ new Response().OK().VideoMpeg(), "video/mpeg" },
						{ new Response().OK().VideoOgg(), "video/ogg" },
						{ new Response().OK().VideoQuickTime(), "video/quicktime" },
						{ new Response().OK().VideoWebM(), "video/webm" },
						{ new Response().OK().VideoWmv(), "video/x-ms-wmv" },
						{ new Response().OK().ApplicationOpenDocumentText(), "application/vnd.oasis.opendocument.text" },
						{ new Response().OK().ApplicationOpenDocumentSpreadsheet(), "application/vnd.oasis.opendocument.spreadsheet" },
						{ new Response().OK().ApplicationOpenDocumentPresentation(), "application/vnd.oasis.opendocument.presentation" },
						{ new Response().OK().ApplicationOpenDocumentGraphics(), "application/vnd.oasis.opendocument.graphics" },
						{ new Response().OK().ApplicationExcel(), "application/vnd.ms-excel" },
						{ new Response().OK().ApplicationExcel2007(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
						{ new Response().OK().ApplicationPowerpoint(), "application/vnd.ms-powerpoint" },
						{ new Response().OK().ApplicationPowerpoint2007(), "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
						{ new Response().OK().ApplicationWord2007(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
						{ new Response().OK().ApplicationMozillaXul(), "application/vnd.mozilla.xul+xml" },
						{ new Response().OK().ApplicationGoogleEarthKml(), "application/vnd.google-earth.kml+xml" },
						{ new Response().OK().ApplicationDeb(), "application/x-deb" },
						{ new Response().OK().ApplicationDvi(), "application/x-dvi" },
						{ new Response().OK().ApplicationFontTtf(), "application/x-font-ttf" },
						{ new Response().OK().ApplicationLaTeX(), "application/x-latex" },
						{ new Response().OK().ApplicationMpegUrl(), "application/x-mpegURL" },
						{ new Response().OK().ApplicationRarCompressed(), "application/x-rar-compressed" },
						{ new Response().OK().ApplicationShockwaveFlash(), "application/x-shockwave-flash" },
						{ new Response().OK().ApplicationStuffIt(), "application/x-stuffit" },
						{ new Response().OK().ApplicationTar(), "application/x-tar" },
						{ new Response().OK().ApplicationFormEncoded(), "application/x-www-form-urlencoded" },
						{ new Response().OK().ApplicationXpInstall(), "application/x-xpinstall" },
						{ new Response().OK().AudioAac(), "audio/x-aac" },
						{ new Response().OK().AudioCaf(), "audio/x-caf" },
						{ new Response().OK().ImageXcf(), "image/x-xcf" },
						{ new Response().OK().TextGwtRpc(), "text/x-gwt-rpc" },
						{ new Response().OK().TextJQueryTmpl(), "text/x-jquery-tmpl" },
						{ new Response().OK().ApplicationPkcs12(), "application/x-pkcs12" },
						{ new Response().OK().ApplicationPkcs7Certificates(), "application/x-pkcs7-certificates" },
						{ new Response().OK().ApplicationPkcs7CertReqResp(), "application/x-pkcs7-certreqresp" },
						{ new Response().OK().ApplicationPkcs7Mime(), "application/x-pkcs7-mime" },
						{ new Response().OK().ApplicationPkcs7Signature(), "application/x-pkcs7-signature" },
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