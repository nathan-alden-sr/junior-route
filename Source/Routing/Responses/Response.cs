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
		private readonly HashSet<Cookie> _cookies = new HashSet<Cookie>();
		private readonly HashSet<Header> _headers = new HashSet<Header>();
		private readonly int _statusCode;
		private readonly int _subStatusCode;
		private Func<byte[]> _binaryContent;
		private CachePolicy _cachePolicy;
		private string _charset;
		private Encoding _contentEncoding = _defaultContentEncoding;
		private string _contentType;
		private Encoding _headerEncoding = _defaultHeaderEncoding;
		private Func<string> _stringContent;

		public Response(HttpStatusCode statusCode, int subStatusCode = 0)
		{
			_statusCode = (int)statusCode;
			_subStatusCode = subStatusCode;
			_contentEncoding = _defaultContentEncoding;
		}

		public Response(int statusCode, int subStatusCode = 0)
		{
			_statusCode = statusCode;
			_subStatusCode = subStatusCode;
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return String.Format(
					"{0}{1} {2}{3}{4}",
					_statusCode,
					_subStatusCode > 0 ? "." + _subStatusCode : "",
					HttpWorkerRequest.GetStatusDescription(_statusCode),
					_contentType.IfNotNull(arg => ", " + arg) ?? "",
					_contentEncoding.IfNotNull(arg => ", " + arg.EncodingName) ?? "");
			}
		}

		int IResponse.StatusCode
		{
			get
			{
				return _statusCode;
			}
		}

		HttpStatusCode? IResponse.ParsedStatusCode
		{
			get
			{
				return Enum<HttpStatusCode>.IsDefined(_statusCode) ? (HttpStatusCode?)_statusCode : null;
			}
		}

		int IResponse.SubStatusCode
		{
			get
			{
				return _subStatusCode;
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

		public Response Charset()
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

		#region Caching

		public CachePolicy CacheInPublicClientCacheAndServerCache(DateTime expires)
		{
			_cachePolicy = new CachePolicy()
				.PublicClientCaching(expires)
				.ServerCaching();

			return _cachePolicy;
		}

		public CachePolicy CacheInPublicClientCacheAndServerCache(TimeSpan maxAge)
		{
			_cachePolicy = new CachePolicy()
				.PublicClientCaching(maxAge)
				.ServerCaching();

			return _cachePolicy;
		}

		public CachePolicy CacheInPublicClientCacheOnly(DateTime expires)
		{
			_cachePolicy = new CachePolicy()
				.PublicClientCaching(expires);

			return _cachePolicy;
		}

		public CachePolicy CacheInPublicClientCacheOnly(TimeSpan maxAge)
		{
			_cachePolicy = new CachePolicy()
				.PublicClientCaching(maxAge);

			return _cachePolicy;
		}

		public CachePolicy CacheInPrivateClientCacheAndServerCache(DateTime expires)
		{
			_cachePolicy = new CachePolicy()
				.PrivateClientCaching(expires)
				.ServerCaching();

			return _cachePolicy;
		}

		public CachePolicy CacheInPrivateClientCacheAndServerCache(TimeSpan maxAge)
		{
			_cachePolicy = new CachePolicy()
				.PrivateClientCaching(maxAge)
				.ServerCaching();

			return _cachePolicy;
		}

		public CachePolicy CacheInPrivateClientCacheOnly(DateTime expires)
		{
			_cachePolicy = new CachePolicy()
				.PrivateClientCaching(expires);

			return _cachePolicy;
		}

		public CachePolicy CacheInPrivateClientCacheOnly(TimeSpan maxAge)
		{
			_cachePolicy = new CachePolicy()
				.PrivateClientCaching(maxAge);

			return _cachePolicy;
		}

		public CachePolicy CacheInServerCacheOnly()
		{
			_cachePolicy = new CachePolicy()
				.NoClientCaching()
				.ServerCaching()
				.AllowResponseInBrowserHistory(false);

			return _cachePolicy;
		}

		public CachePolicy NoCaching()
		{
			_cachePolicy = new CachePolicy()
				.NoClientCaching()
				.NoServerCaching()
				.AllowResponseInBrowserHistory(false);

			return _cachePolicy;
		}

		public Response SetCachePolicy(CachePolicy policy)
		{
			policy.ThrowIfNull("policy");

			_cachePolicy = policy;

			return this;
		}

		public Response RemoveCachePolicy()
		{
			_cachePolicy = null;

			return this;
		}

		#endregion

		#region HTTP status codes

		public static Response Continue()
		{
			return new Response(HttpStatusCode.Continue);
		}

		public static Response SwitchingProtocols()
		{
			return new Response(HttpStatusCode.SwitchingProtocols);
		}

		public static Response OK()
		{
			return new Response(HttpStatusCode.OK);
		}

		public static Response Created()
		{
			return new Response(HttpStatusCode.Created);
		}

		public static Response Accepted()
		{
			return new Response(HttpStatusCode.Accepted);
		}

		public static Response NonAuthoritativeInformation()
		{
			return new Response(HttpStatusCode.NonAuthoritativeInformation);
		}

		public static Response NoContent()
		{
			return new Response(HttpStatusCode.NoContent);
		}

		public static Response ResetContent()
		{
			return new Response(HttpStatusCode.ResetContent);
		}

		public static Response PartialContent()
		{
			return new Response(HttpStatusCode.PartialContent);
		}

		public static Response Ambiguous()
		{
			return new Response(HttpStatusCode.Ambiguous);
		}

		public static Response MultipleChoices()
		{
			return new Response(HttpStatusCode.MultipleChoices);
		}

		public static Response Moved()
		{
			return new Response(HttpStatusCode.Moved);
		}

		public static Response MovedPermanently()
		{
			return new Response(HttpStatusCode.MovedPermanently);
		}

		public static Response Found()
		{
			return new Response(HttpStatusCode.Found);
		}

		public static Response Redirect()
		{
			return new Response(HttpStatusCode.Redirect);
		}

		public static Response RedirectMethod()
		{
			return new Response(HttpStatusCode.RedirectMethod);
		}

		public static Response SeeOther()
		{
			return new Response(HttpStatusCode.SeeOther);
		}

		public static Response NotModified()
		{
			return new Response(HttpStatusCode.NotModified);
		}

		public static Response UseProxy()
		{
			return new Response(HttpStatusCode.UseProxy);
		}

		public static Response Unused()
		{
			return new Response(HttpStatusCode.Unused);
		}

		public static Response RedirectKeepVerb()
		{
			return new Response(HttpStatusCode.RedirectKeepVerb);
		}

		public static Response TemporaryRedirect()
		{
			return new Response(HttpStatusCode.TemporaryRedirect);
		}

		public static Response BadRequest()
		{
			return new Response(HttpStatusCode.BadRequest);
		}

		public static Response Unauthorized()
		{
			return new Response(HttpStatusCode.Unauthorized);
		}

		public static Response PaymentRequired()
		{
			return new Response(HttpStatusCode.PaymentRequired);
		}

		public static Response Forbidden()
		{
			return new Response(HttpStatusCode.Forbidden);
		}

		public static Response NotFound()
		{
			return new Response(HttpStatusCode.NotFound);
		}

		public static Response MethodNotAllowed()
		{
			return new Response(HttpStatusCode.MethodNotAllowed);
		}

		public static Response NotAcceptable()
		{
			return new Response(HttpStatusCode.NotAcceptable);
		}

		public static Response ProxyAuthenticationRequired()
		{
			return new Response(HttpStatusCode.ProxyAuthenticationRequired);
		}

		public static Response RequestTimeout()
		{
			return new Response(HttpStatusCode.RequestTimeout);
		}

		public static Response Conflict()
		{
			return new Response(HttpStatusCode.Conflict);
		}

		public static Response Gone()
		{
			return new Response(HttpStatusCode.Gone);
		}

		public static Response LengthRequired()
		{
			return new Response(HttpStatusCode.LengthRequired);
		}

		public static Response PreconditionFailed()
		{
			return new Response(HttpStatusCode.PreconditionFailed);
		}

		public static Response RequestEntityTooLarge()
		{
			return new Response(HttpStatusCode.RequestEntityTooLarge);
		}

		public static Response RequestUriTooLong()
		{
			return new Response(HttpStatusCode.RequestUriTooLong);
		}

		public static Response UnsupportedMediaType()
		{
			return new Response(HttpStatusCode.UnsupportedMediaType);
		}

		public static Response RequestedRangeNotSatisfiable()
		{
			return new Response(HttpStatusCode.RequestedRangeNotSatisfiable);
		}

		public static Response ExpectationFailed()
		{
			return new Response(HttpStatusCode.ExpectationFailed);
		}

		public static Response InternalServerError()
		{
			return new Response(HttpStatusCode.InternalServerError);
		}

		public static Response NotImplemented()
		{
			return new Response(HttpStatusCode.NotImplemented);
		}

		public static Response BadGateway()
		{
			return new Response(HttpStatusCode.BadGateway);
		}

		public static Response ServiceUnavailable()
		{
			return new Response(HttpStatusCode.ServiceUnavailable);
		}

		public static Response GatewayTimeout()
		{
			return new Response(HttpStatusCode.GatewayTimeout);
		}

		public static Response HttpVersionNotSupported()
		{
			return new Response(HttpStatusCode.HttpVersionNotSupported);
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