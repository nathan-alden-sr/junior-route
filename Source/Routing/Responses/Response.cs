using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing.Caching;

namespace Junior.Route.Routing.Responses
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class Response : IResponse
	{
		private static readonly Encoding _defaultContentEncoding = Encoding.UTF8;
		private static readonly Encoding _defaultHeaderEncoding = Encoding.UTF8;
		private readonly CachePolicy _cachePolicy = new CachePolicy();
		private readonly HashSet<Cookie> _cookies = new HashSet<Cookie>();
		private readonly HashSet<Header> _headers = new HashSet<Header>();
		private Func<byte[]> _binaryContent;
		private string _charset = "utf-8";
		private Encoding _contentEncoding = _defaultContentEncoding;
		private string _contentType;
		private Encoding _headerEncoding = _defaultHeaderEncoding;
		private bool _skipIisCustomErrors = true;
		private StatusAndSubStatusCode _statusCode;
		private Func<string> _stringContent;

		public Response()
			: this(new StatusAndSubStatusCode(HttpStatusCode.OK))
		{
		}

		public Response(StatusAndSubStatusCode statusCode)
		{
			_statusCode = statusCode;
		}

		public Response(int statusCode, int subStatusCode = 0)
			: this(new StatusAndSubStatusCode(statusCode, subStatusCode))
		{
		}

		public Response(HttpStatusCode statusCode, int subStatusCode = 0)
			: this(new StatusAndSubStatusCode(statusCode, subStatusCode))
		{
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				var parameters = new List<string>();

				if (_statusCode.StatusDescription.Length > 0)
				{
					parameters.Add(_statusCode.StatusDescription);
				}
				if (_contentType != null)
				{
					parameters.Add(_contentType);
				}
				if (_contentEncoding != null)
				{
					parameters.Add(_contentEncoding.EncodingName);
				}

				return String.Format("{0}{1} {2}", _statusCode.StatusCode, _statusCode.SubStatusCode == 0 ? "" : "." + _statusCode.SubStatusCode, String.Join(", ", parameters));
			}
		}

		public CachePolicy CachePolicy
		{
			get
			{
				return _cachePolicy;
			}
		}

		StatusAndSubStatusCode IResponse.StatusCode
		{
			get
			{
				return _statusCode;
			}
		}

		string IResponse.ContentType
		{
			get
			{
				return _contentType;
			}
		}

		string IResponse.Charset
		{
			get
			{
				return _charset;
			}
		}

		Encoding IResponse.ContentEncoding
		{
			get
			{
				return _contentEncoding;
			}
		}

		IEnumerable<Header> IResponse.Headers
		{
			get
			{
				return _headers;
			}
		}

		Encoding IResponse.HeaderEncoding
		{
			get
			{
				return _headerEncoding;
			}
		}

		IEnumerable<Cookie> IResponse.Cookies
		{
			get
			{
				return _cookies;
			}
		}

		ICachePolicy IResponse.CachePolicy
		{
			get
			{
				return _cachePolicy;
			}
		}

		bool IResponse.SkipIisCustomErrors
		{
			get
			{
				return _skipIisCustomErrors;
			}
		}

		public byte[] GetContent()
		{
			if (_binaryContent == null && _stringContent == null)
			{
				return new byte[0];
			}

			return _binaryContent != null ? _binaryContent() : _contentEncoding.GetBytes(_stringContent());
		}

		public Response ContentType(string contentType)
		{
			_contentType = contentType;

			return this;
		}

		public Response NoContentType()
		{
			_contentType = null;

			return this;
		}

		public Response Charset(string charset)
		{
			_charset = charset;

			return this;
		}

		public Response NoCharset()
		{
			_charset = null;

			return this;
		}

		public Response ContentEncoding(Encoding encoding)
		{
			_contentEncoding = encoding;

			return this;
		}

		public Response DefaultContentEncoding()
		{
			_contentEncoding = _defaultContentEncoding;

			return this;
		}

		public Response Header(Header header)
		{
			header.ThrowIfNull("header");

			_headers.Add(header);

			return this;
		}

		public Response Header(string field, string value)
		{
			_headers.Add(new Header(field, value));

			return this;
		}

		public Response Headers(IEnumerable<Header> headers)
		{
			headers.ThrowIfNull("headers");

			_headers.AddRange(headers);

			return this;
		}

		public Response Headers(params Header[] headers)
		{
			return Headers((IEnumerable<Header>)headers);
		}

		public Response HeaderEncoding(Encoding encoding)
		{
			_headerEncoding = encoding;

			return this;
		}

		public Response DefaultHeaderEncoding()
		{
			_headerEncoding = _defaultHeaderEncoding;

			return this;
		}

		public Response Cookie(Cookie cookie)
		{
			cookie.ThrowIfNull("cookie");

			_cookies.Add(cookie);

			return this;
		}

		public Response Cookie(HttpCookie cookie, bool shareable = false)
		{
			cookie.ThrowIfNull("cookie");

			_cookies.Add(new Cookie(cookie, shareable));

			return this;
		}

		public Response Cookie(string name, string value, bool shareable = false)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			_cookies.Add(new Cookie(new HttpCookie(name, value), shareable));

			return this;
		}

		public Response Content(Func<string> content)
		{
			content.ThrowIfNull("content");

			_binaryContent = null;
			_stringContent = content;

			return this;
		}

		public Response Content(string content)
		{
			content.ThrowIfNull("content");

			_binaryContent = null;
			_stringContent = () => content;

			return this;
		}

		public Response Content(Func<byte[]> content)
		{
			content.ThrowIfNull("content");

			_binaryContent = content;
			_stringContent = null;

			return this;
		}

		public Response Content(byte[] content)
		{
			content.ThrowIfNull("content");

			_binaryContent = () => content;
			_stringContent = null;

			return this;
		}

		#region HTTP status codes

		public Response StatusCode(StatusAndSubStatusCode statusCode)
		{
			statusCode.ThrowIfNull("statusCode");

			_statusCode = statusCode;

			return this;
		}

		public Response StatusCode(int statusCode, int subStatusCode = 0)
		{
			_statusCode = new StatusAndSubStatusCode(statusCode, subStatusCode);

			return this;
		}

		public Response StatusCode(HttpStatusCode statusCode, int subStatusCode = 0)
		{
			_statusCode = new StatusAndSubStatusCode(statusCode, subStatusCode);

			return this;
		}

		public Response SkipIisCustomErrors()
		{
			_skipIisCustomErrors = true;

			return this;
		}

		public Response DoNotSkipIisCustomErrors()
		{
			_skipIisCustomErrors = false;

			return this;
		}

		public Response Continue()
		{
			return StatusCode(HttpStatusCode.Continue);
		}

		public Response SwitchingProtocols()
		{
			return StatusCode(HttpStatusCode.SwitchingProtocols);
		}

		public Response OK()
		{
			return StatusCode(HttpStatusCode.OK);
		}

		public Response Created()
		{
			return StatusCode(HttpStatusCode.Created);
		}

		public Response Accepted()
		{
			return StatusCode(HttpStatusCode.Accepted);
		}

		public Response NonAuthoritativeInformation()
		{
			return StatusCode(HttpStatusCode.NonAuthoritativeInformation);
		}

		public Response NoContent()
		{
			return StatusCode(HttpStatusCode.NoContent);
		}

		public Response ResetContent()
		{
			return StatusCode(HttpStatusCode.ResetContent);
		}

		public Response PartialContent()
		{
			return StatusCode(HttpStatusCode.PartialContent);
		}

		public Response Ambiguous()
		{
			return StatusCode(HttpStatusCode.Ambiguous);
		}

		public Response MultipleChoices()
		{
			return StatusCode(HttpStatusCode.MultipleChoices);
		}

		public Response Moved()
		{
			return StatusCode(HttpStatusCode.Moved);
		}

		public Response Moved(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.Moved).Header("Location", location);
		}

		public Response MovedToRoute(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Moved(urlResolver.Route(routeName));
		}

		public Response MovedToRoute(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Moved(urlResolver.Route(routeId));
		}

		public Response MovedToRelativeUrl(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Moved(urlResolver.Absolute(relativeUrl));
		}

		public Response MovedPermanently()
		{
			return StatusCode(HttpStatusCode.MovedPermanently);
		}

		public Response MovedPermanently(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.MovedPermanently).Header("Location", location);
		}

		public Response MovedPermanentlyToRoute(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return MovedPermanently(urlResolver.Route(routeName));
		}

		public Response MovedPermanentlyToRoute(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return MovedPermanently(urlResolver.Route(routeId));
		}

		public Response MovedPermanentlyToRelativeUrl(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return MovedPermanently(urlResolver.Absolute(relativeUrl));
		}

		public Response Found()
		{
			return StatusCode(HttpStatusCode.Found);
		}

		public Response Found(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.Found).Header("Location", location);
		}

		public Response FoundRoute(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Found(urlResolver.Route(routeName));
		}

		public Response FoundRoute(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Found(urlResolver.Route(routeId));
		}

		public Response FoundRelativeUrl(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Found(urlResolver.Absolute(relativeUrl));
		}

		public Response Redirect()
		{
			return StatusCode(HttpStatusCode.Redirect);
		}

		public Response Redirect(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.Redirect).Header("Location", location);
		}

		public Response RedirectToRoute(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Redirect(urlResolver.Route(routeName));
		}

		public Response RedirectToRoute(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Redirect(urlResolver.Route(routeId));
		}

		public Response RedirectToRelativeUrl(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return Redirect(urlResolver.Absolute(relativeUrl));
		}

		public Response RedirectMethod()
		{
			return StatusCode(HttpStatusCode.RedirectMethod);
		}

		public Response RedirectMethod(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.RedirectMethod).Header("Location", location);
		}

		public Response RedirectMethodToRoute(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return RedirectMethod(urlResolver.Route(routeName));
		}

		public Response RedirectMethodToRoute(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return RedirectMethod(urlResolver.Route(routeId));
		}

		public Response RedirectMethodToRelativeUrl(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return RedirectMethod(urlResolver.Absolute(relativeUrl));
		}

		public Response SeeOther()
		{
			return StatusCode(HttpStatusCode.SeeOther);
		}

		public Response SeeOther(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.SeeOther).Header("Location", location);
		}

		public Response SeeOtherRoute(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return SeeOther(urlResolver.Route(routeName));
		}

		public Response SeeOtherRoute(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return SeeOther(urlResolver.Route(routeId));
		}

		public Response SeeOtherRelativeUrl(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return SeeOther(urlResolver.Absolute(relativeUrl));
		}

		public Response NotModified()
		{
			return StatusCode(HttpStatusCode.NotModified);
		}

		public Response UseProxy()
		{
			return StatusCode(HttpStatusCode.UseProxy);
		}

		public Response Unused()
		{
			return StatusCode(HttpStatusCode.Unused);
		}

		public Response RedirectKeepVerb()
		{
			return StatusCode(HttpStatusCode.RedirectKeepVerb);
		}

		public Response RedirectKeepVerb(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.RedirectKeepVerb).Header("Location", location);
		}

		public Response RedirectToRouteKeepVerb(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return RedirectKeepVerb(urlResolver.Route(routeName));
		}

		public Response RedirectToRouteKeepVerb(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return RedirectKeepVerb(urlResolver.Route(routeId));
		}

		public Response RedirectToRelativeUrlKeepVerb(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return RedirectKeepVerb(urlResolver.Absolute(relativeUrl));
		}

		public Response TemporaryRedirect()
		{
			return StatusCode(HttpStatusCode.TemporaryRedirect);
		}

		public Response TemporaryRedirect(string location)
		{
			location.ThrowIfNull("location");

			return StatusCode(HttpStatusCode.TemporaryRedirect).Header("Location", location);
		}

		public Response TemporaryRedirectToRoute(IUrlResolver urlResolver, string routeName)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return TemporaryRedirect(urlResolver.Route(routeName));
		}

		public Response TemporaryRedirectToRoute(IUrlResolver urlResolver, Guid routeId)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return TemporaryRedirect(urlResolver.Route(routeId));
		}

		public Response TemporaryRedirectToRelativeUrl(IUrlResolver urlResolver, string relativeUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");

			return TemporaryRedirect(urlResolver.Absolute(relativeUrl));
		}

		public Response BadRequest(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.BadRequest);
		}

		public Response Unauthorized(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.Unauthorized);
		}

		public Response PaymentRequired(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.PaymentRequired);
		}

		public Response Forbidden(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.Forbidden);
		}

		public Response NotFound(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.NotFound);
		}

		public Response MethodNotAllowed(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.MethodNotAllowed);
		}

		public Response NotAcceptable(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.NotAcceptable);
		}

		public Response ProxyAuthenticationRequired(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.ProxyAuthenticationRequired);
		}

		public Response RequestTimeout(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.RequestTimeout);
		}

		public Response Conflict(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.Conflict);
		}

		public Response Gone(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.Gone);
		}

		public Response LengthRequired(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.LengthRequired);
		}

		public Response PreconditionFailed(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.PreconditionFailed);
		}

		public Response RequestEntityTooLarge(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.RequestEntityTooLarge);
		}

		public Response RequestUriTooLong(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.RequestUriTooLong);
		}

		public Response UnsupportedMediaType(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.UnsupportedMediaType);
		}

		public Response RequestedRangeNotSatisfiable(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.RequestedRangeNotSatisfiable);
		}

		public Response ExpectationFailed(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.ExpectationFailed);
		}

		public Response InternalServerError(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.InternalServerError);
		}

		public Response NotImplemented(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.NotImplemented);
		}

		public Response BadGateway(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.BadGateway);
		}

		public Response ServiceUnavailable(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.ServiceUnavailable);
		}

		public Response GatewayTimeout(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.GatewayTimeout);
		}

		public Response HttpVersionNotSupported(bool skipIisCustomErrors = true)
		{
			_skipIisCustomErrors = skipIisCustomErrors;

			return StatusCode(HttpStatusCode.HttpVersionNotSupported);
		}

		#endregion

		#region IANA content types

		public Response ApplicationAtom()
		{
			_contentType = "application/atom+xml";

			return this;
		}

		public Response ApplicationDtd()
		{
			_contentType = "application/xml-dtd";

			return this;
		}

		public Response ApplicationEcmaScript()
		{
			_contentType = "application/ecmascript";

			return this;
		}

		public Response ApplicationEdifact()
		{
			_contentType = "application/EDIFACT";

			return this;
		}

		public Response ApplicationEdiX12()
		{
			_contentType = "application/EDI-X12";

			return this;
		}

		public Response ApplicationGzip()
		{
			_contentType = "application/gzip";

			return this;
		}

		public Response ApplicationJavaScript()
		{
			_contentType = "application/javascript";

			return this;
		}

		public Response ApplicationJson()
		{
			_contentType = "application/json";

			return this;
		}

		public Response ApplicationOctetStream()
		{
			_contentType = "application/octet-stream";

			return this;
		}

		public Response ApplicationOgg()
		{
			_contentType = "application/ogg";

			return this;
		}

		public Response ApplicationPdf()
		{
			_contentType = "application/pdf";

			return this;
		}

		public Response ApplicationPostscript()
		{
			_contentType = "application/postscript";

			return this;
		}

		public Response ApplicationRdf()
		{
			_contentType = "application/rdf+xml";

			return this;
		}

		public Response ApplicationRss()
		{
			_contentType = "application/rss+xml";

			return this;
		}

		public Response ApplicationSoap()
		{
			_contentType = "application/soap+xml";

			return this;
		}

		public Response ApplicationXHtml()
		{
			_contentType = "application/xhtml+xml";

			return this;
		}

		public Response ApplicationXop()
		{
			_contentType = "application/xop+xml";

			return this;
		}

		public Response ApplicationZip()
		{
			_contentType = "application/zip";

			return this;
		}

		public Response AudioBasic()
		{
			_contentType = "audio/basic";

			return this;
		}

		public Response AudioL24()
		{
			_contentType = "audio/L24";

			return this;
		}

		public Response AudioMp4()
		{
			_contentType = "audio/mp4";

			return this;
		}

		public Response AudioMpeg()
		{
			_contentType = "audio/mpeg";

			return this;
		}

		public Response AudioOgg()
		{
			_contentType = "audio/ogg";

			return this;
		}

		public Response AudioRealAudio()
		{
			_contentType = "audio/vnd.rn-realaudio";

			return this;
		}

		public Response AudioVorbis()
		{
			_contentType = "audio/vorbis";

			return this;
		}

		public Response AudioWav()
		{
			_contentType = "audio/vnd.wave";

			return this;
		}

		public Response AudioWebM()
		{
			_contentType = "audio/webm";

			return this;
		}

		public Response ImageGif()
		{
			_contentType = "image/gif";

			return this;
		}

		public Response ImageJpeg()
		{
			_contentType = "image/jpeg";

			return this;
		}

		public Response ImagePJpeg()
		{
			_contentType = "image/pjpeg";

			return this;
		}

		public Response ImagePng()
		{
			_contentType = "image/png";

			return this;
		}

		public Response ImageSvg()
		{
			_contentType = "image/svg+xml";

			return this;
		}

		public Response ImageTiff()
		{
			_contentType = "image/tiff";

			return this;
		}

		public Response ImageIco()
		{
			_contentType = "image/vnd.microsoft.icon";

			return this;
		}

		public Response MessageHttp()
		{
			_contentType = "message/http";

			return this;
		}

		public Response MessageImdn()
		{
			_contentType = "message/imdn+xml";

			return this;
		}

		public Response MessagePartial()
		{
			_contentType = "message/partial";

			return this;
		}

		public Response MessageRfc822()
		{
			_contentType = "message/rfc822";

			return this;
		}

		public Response ModelExample()
		{
			_contentType = "model/example";

			return this;
		}

		public Response ModelIges()
		{
			_contentType = "model/iges";

			return this;
		}

		public Response ModelMesh()
		{
			_contentType = "model/mesh";

			return this;
		}

		public Response ModelVrml()
		{
			_contentType = "model/vrml";

			return this;
		}

		public Response ModelX3DBinary()
		{
			_contentType = "model/x3d+binary";

			return this;
		}

		public Response ModelX3DVrml()
		{
			_contentType = "model/x3d+vrml";

			return this;
		}

		public Response ModelX3DXml()
		{
			_contentType = "model/x3d+xml";

			return this;
		}

		public Response MultipartAlternative()
		{
			_contentType = "multipart/alternative";

			return this;
		}

		public Response MultipartEncrypted()
		{
			_contentType = "multipart/encrypted";

			return this;
		}

		public Response MultipartFormData()
		{
			_contentType = "multipart/form-data";

			return this;
		}

		public Response MultipartMixed()
		{
			_contentType = "multipart/mixed";

			return this;
		}

		public Response MultipartRelated()
		{
			_contentType = "multipart/related";

			return this;
		}

		public Response MultipartSigned()
		{
			_contentType = "multipart/signed";

			return this;
		}

		public Response TextCmd()
		{
			_contentType = "text/cmd";

			return this;
		}

		public Response TextCss()
		{
			_contentType = "text/css";

			return this;
		}

		public Response TextCsv()
		{
			_contentType = "text/csv";

			return this;
		}

		public Response TextHtml()
		{
			_contentType = "text/html";

			return this;
		}

		public Response TextPlain()
		{
			_contentType = "text/plain";

			return this;
		}

		public Response TextVCard()
		{
			_contentType = "text/vcard";

			return this;
		}

		public Response TextXml()
		{
			_contentType = "text/xml";

			return this;
		}

		public Response VideoFlv()
		{
			_contentType = "video/x-flv";

			return this;
		}

		public Response VideoMatroska()
		{
			_contentType = "video/x-matroska";

			return this;
		}

		public Response VideoMp4()
		{
			_contentType = "video/mp4";

			return this;
		}

		public Response VideoMpeg()
		{
			_contentType = "video/mpeg";

			return this;
		}

		public Response VideoOgg()
		{
			_contentType = "video/ogg";

			return this;
		}

		public Response VideoQuickTime()
		{
			_contentType = "video/quicktime";

			return this;
		}

		public Response VideoWebM()
		{
			_contentType = "video/webm";

			return this;
		}

		public Response VideoWmv()
		{
			_contentType = "video/x-ms-wmv";

			return this;
		}

		#endregion

		#region Non-standard content types

		public Response ApplicationOpenDocumentText()
		{
			_contentType = "application/vnd.oasis.opendocument.text";

			return this;
		}

		public Response ApplicationOpenDocumentSpreadsheet()
		{
			_contentType = "application/vnd.oasis.opendocument.spreadsheet";

			return this;
		}

		public Response ApplicationOpenDocumentPresentation()
		{
			_contentType = "application/vnd.oasis.opendocument.presentation";

			return this;
		}

		public Response ApplicationOpenDocumentGraphics()
		{
			_contentType = "application/vnd.oasis.opendocument.graphics";

			return this;
		}

		public Response ApplicationExcel()
		{
			_contentType = "application/vnd.ms-excel";

			return this;
		}

		public Response ApplicationExcel2007()
		{
			_contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

			return this;
		}

		public Response ApplicationPowerpoint()
		{
			_contentType = "application/vnd.ms-powerpoint";

			return this;
		}

		public Response ApplicationPowerpoint2007()
		{
			_contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

			return this;
		}

		public Response ApplicationWord2007()
		{
			_contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

			return this;
		}

		public Response ApplicationMozillaXul()
		{
			_contentType = "application/vnd.mozilla.xul+xml";

			return this;
		}

		public Response ApplicationGoogleEarthKml()
		{
			_contentType = "application/vnd.google-earth.kml+xml";

			return this;
		}

		public Response ApplicationDeb()
		{
			_contentType = "application/x-deb";

			return this;
		}

		public Response ApplicationDvi()
		{
			_contentType = "application/x-dvi";

			return this;
		}

		public Response ApplicationFontTtf()
		{
			_contentType = "application/x-font-ttf";

			return this;
		}

		public Response ApplicationLaTeX()
		{
			_contentType = "application/x-latex";

			return this;
		}

		public Response ApplicationMpegUrl()
		{
			_contentType = "application/x-mpegURL";

			return this;
		}

		public Response ApplicationRarCompressed()
		{
			_contentType = "application/x-rar-compressed";

			return this;
		}

		public Response ApplicationShockwaveFlash()
		{
			_contentType = "application/x-shockwave-flash";

			return this;
		}

		public Response ApplicationStuffIt()
		{
			_contentType = "application/x-stuffit";

			return this;
		}

		public Response ApplicationTar()
		{
			_contentType = "application/x-tar";

			return this;
		}

		public Response ApplicationFormEncoded()
		{
			_contentType = "application/x-www-form-urlencoded";

			return this;
		}

		public Response ApplicationXpInstall()
		{
			_contentType = "application/x-xpinstall";

			return this;
		}

		public Response AudioAac()
		{
			_contentType = "audio/x-aac";

			return this;
		}

		public Response AudioCaf()
		{
			_contentType = "audio/x-caf";

			return this;
		}

		public Response ImageXcf()
		{
			_contentType = "image/x-xcf";

			return this;
		}

		public Response TextGwtRpc()
		{
			_contentType = "text/x-gwt-rpc";

			return this;
		}

		public Response TextJQueryTmpl()
		{
			_contentType = "text/x-jquery-tmpl";

			return this;
		}

		public Response ApplicationPkcs12()
		{
			_contentType = "application/x-pkcs12";

			return this;
		}

		public Response ApplicationPkcs7Certificates()
		{
			_contentType = "application/x-pkcs7-certificates";

			return this;
		}

		public Response ApplicationPkcs7CertReqResp()
		{
			_contentType = "application/x-pkcs7-certreqresp";

			return this;
		}

		public Response ApplicationPkcs7Mime()
		{
			_contentType = "application/x-pkcs7-mime";

			return this;
		}

		public Response ApplicationPkcs7Signature()
		{
			_contentType = "application/x-pkcs7-signature";

			return this;
		}

		#endregion
	}
}