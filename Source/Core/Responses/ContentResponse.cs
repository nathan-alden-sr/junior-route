using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Responses
{
	[DebuggerDisplay("{_statusCode}, {_contentType}")]
	public class ContentResponse : IHttpRouteResponse
	{
		private readonly int _statusCode;
		private readonly int _subStatusCode;
		private Func<byte[]> _binaryContentDelegate;
		private Func<string> _stringContentDelegate;

		public ContentResponse(HttpStatusCode statusCode, int subStatusCode = 0)
		{
			_statusCode = (int)statusCode;
			_subStatusCode = subStatusCode;
			ResponseEncoding = System.Text.Encoding.UTF8;
		}

		public ContentResponse(int statusCode, int subStatusCode = 0)
		{
			_statusCode = statusCode;
			_subStatusCode = subStatusCode;
			ResponseEncoding = System.Text.Encoding.UTF8;
		}

		public int ResponseStatusCode
		{
			get
			{
				return _statusCode;
			}
		}

		public HttpStatusCode? ParsedResponseStatusCode
		{
			get
			{
				return Enum<HttpStatusCode>.IsDefined(_statusCode) ? (HttpStatusCode?)_statusCode : null;
			}
		}

		public int ResponseSubStatusCode
		{
			get
			{
				return _subStatusCode;
			}
		}

		public string ResponseContentType
		{
			get;
			private set;
		}

		public Encoding ResponseEncoding
		{
			get;
			private set;
		}

		public virtual void WriteResponse(HttpResponseBase response)
		{
			response.ThrowIfNull("response");

			response.StatusCode = _statusCode;
			response.ContentType = ResponseContentType;
			response.ContentEncoding = ResponseEncoding;

			if (_binaryContentDelegate == null && _stringContentDelegate == null)
			{
				return;
			}

			if (_binaryContentDelegate != null)
			{
				byte[] content = _binaryContentDelegate();

				response.OutputStream.Write(content, 0, content.Length);
			}
			else
			{
				var writer = new StreamWriter(response.OutputStream, ResponseEncoding);

				writer.Write(_stringContentDelegate());
				writer.Flush();
			}
		}

		public ContentResponse ContentType(string contentType)
		{
			contentType.ThrowIfNull("contentType");

			ResponseContentType = contentType;

			return this;
		}

		public ContentResponse Content(Func<string> content)
		{
			content.ThrowIfNull("content");

			_binaryContentDelegate = null;
			_stringContentDelegate = content;

			return this;
		}

		public ContentResponse Content(string content)
		{
			content.ThrowIfNull("content");

			_binaryContentDelegate = null;
			_stringContentDelegate = () => content;

			return this;
		}

		public ContentResponse Content(Func<byte[]> content)
		{
			content.ThrowIfNull("content");

			_binaryContentDelegate = content;
			_stringContentDelegate = null;

			return this;
		}

		public ContentResponse Content(byte[] content)
		{
			content.ThrowIfNull("content");

			_binaryContentDelegate = () => content;
			_stringContentDelegate = null;

			return this;
		}

		public ContentResponse Encoding(Encoding encoding)
		{
			encoding.ThrowIfNull("encoding");

			ResponseEncoding = encoding;

			return this;
		}

		public static ContentResponse StatusCodes(HttpStatusCode statusCode, int subStatusCode = 0)
		{
			return new ContentResponse(statusCode, subStatusCode);
		}

		public static ContentResponse StatusCodes(int statusCode, int subStatusCode = 0)
		{
			return new ContentResponse(statusCode, subStatusCode);
		}

		#region HTTP status codes

		public static ContentResponse Continue()
		{
			return new ContentResponse(HttpStatusCode.Continue);
		}

		public static ContentResponse SwitchingProtocols()
		{
			return new ContentResponse(HttpStatusCode.SwitchingProtocols);
		}

		public static ContentResponse OK()
		{
			return new ContentResponse(HttpStatusCode.OK);
		}

		public static ContentResponse Created()
		{
			return new ContentResponse(HttpStatusCode.Created);
		}

		public static ContentResponse Accepted()
		{
			return new ContentResponse(HttpStatusCode.Accepted);
		}

		public static ContentResponse NonAuthoritativeInformation()
		{
			return new ContentResponse(HttpStatusCode.NonAuthoritativeInformation);
		}

		public static ContentResponse NoContent()
		{
			return new ContentResponse(HttpStatusCode.NoContent);
		}

		public static ContentResponse ResetContent()
		{
			return new ContentResponse(HttpStatusCode.ResetContent);
		}

		public static ContentResponse PartialContent()
		{
			return new ContentResponse(HttpStatusCode.PartialContent);
		}

		public static ContentResponse Ambiguous()
		{
			return new ContentResponse(HttpStatusCode.Ambiguous);
		}

		public static ContentResponse MultipleChoices()
		{
			return new ContentResponse(HttpStatusCode.MultipleChoices);
		}

		public static ContentResponse Moved()
		{
			return new ContentResponse(HttpStatusCode.Moved);
		}

		public static ContentResponse MovedPermanently()
		{
			return new ContentResponse(HttpStatusCode.MovedPermanently);
		}

		public static ContentResponse Found()
		{
			return new ContentResponse(HttpStatusCode.Found);
		}

		public static ContentResponse Redirect()
		{
			return new ContentResponse(HttpStatusCode.Redirect);
		}

		public static ContentResponse RedirectMethod()
		{
			return new ContentResponse(HttpStatusCode.RedirectMethod);
		}

		public static ContentResponse SeeOther()
		{
			return new ContentResponse(HttpStatusCode.SeeOther);
		}

		public static ContentResponse NotModified()
		{
			return new ContentResponse(HttpStatusCode.NotModified);
		}

		public static ContentResponse UseProxy()
		{
			return new ContentResponse(HttpStatusCode.UseProxy);
		}

		public static ContentResponse Unused()
		{
			return new ContentResponse(HttpStatusCode.Unused);
		}

		public static ContentResponse RedirectKeepVerb()
		{
			return new ContentResponse(HttpStatusCode.RedirectKeepVerb);
		}

		public static ContentResponse TemporaryRedirect()
		{
			return new ContentResponse(HttpStatusCode.TemporaryRedirect);
		}

		public static ContentResponse BadRequest()
		{
			return new ContentResponse(HttpStatusCode.BadRequest);
		}

		public static ContentResponse Unauthorized()
		{
			return new ContentResponse(HttpStatusCode.Unauthorized);
		}

		public static ContentResponse PaymentRequired()
		{
			return new ContentResponse(HttpStatusCode.PaymentRequired);
		}

		public static ContentResponse Forbidden()
		{
			return new ContentResponse(HttpStatusCode.Forbidden);
		}

		public static ContentResponse NotFound()
		{
			return new ContentResponse(HttpStatusCode.NotFound);
		}

		public static ContentResponse MethodNotAllowed()
		{
			return new ContentResponse(HttpStatusCode.MethodNotAllowed);
		}

		public static ContentResponse NotAcceptable()
		{
			return new ContentResponse(HttpStatusCode.NotAcceptable);
		}

		public static ContentResponse ProxyAuthenticationRequired()
		{
			return new ContentResponse(HttpStatusCode.ProxyAuthenticationRequired);
		}

		public static ContentResponse RequestTimeout()
		{
			return new ContentResponse(HttpStatusCode.RequestTimeout);
		}

		public static ContentResponse Conflict()
		{
			return new ContentResponse(HttpStatusCode.Conflict);
		}

		public static ContentResponse Gone()
		{
			return new ContentResponse(HttpStatusCode.Gone);
		}

		public static ContentResponse LengthRequired()
		{
			return new ContentResponse(HttpStatusCode.LengthRequired);
		}

		public static ContentResponse PreconditionFailed()
		{
			return new ContentResponse(HttpStatusCode.PreconditionFailed);
		}

		public static ContentResponse RequestEntityTooLarge()
		{
			return new ContentResponse(HttpStatusCode.RequestEntityTooLarge);
		}

		public static ContentResponse RequestUriTooLong()
		{
			return new ContentResponse(HttpStatusCode.RequestUriTooLong);
		}

		public static ContentResponse UnsupportedMediaType()
		{
			return new ContentResponse(HttpStatusCode.UnsupportedMediaType);
		}

		public static ContentResponse RequestedRangeNotSatisfiable()
		{
			return new ContentResponse(HttpStatusCode.RequestedRangeNotSatisfiable);
		}

		public static ContentResponse ExpectationFailed()
		{
			return new ContentResponse(HttpStatusCode.ExpectationFailed);
		}

		public static ContentResponse InternalServerError()
		{
			return new ContentResponse(HttpStatusCode.InternalServerError);
		}

		public static ContentResponse NotImplemented()
		{
			return new ContentResponse(HttpStatusCode.NotImplemented);
		}

		public static ContentResponse BadGateway()
		{
			return new ContentResponse(HttpStatusCode.BadGateway);
		}

		public static ContentResponse ServiceUnavailable()
		{
			return new ContentResponse(HttpStatusCode.ServiceUnavailable);
		}

		public static ContentResponse GatewayTimeout()
		{
			return new ContentResponse(HttpStatusCode.GatewayTimeout);
		}

		public static ContentResponse HttpVersionNotSupported()
		{
			return new ContentResponse(HttpStatusCode.HttpVersionNotSupported);
		}

		#endregion

		#region IANA Content Types

		public ContentResponse ApplicationAtom()
		{
			ResponseContentType = "application/atom+xml";

			return this;
		}

		public ContentResponse ApplicationEcmaScript()
		{
			ResponseContentType = "application/ecmascript";

			return this;
		}

		public ContentResponse ApplicationEdiX12()
		{
			ResponseContentType = "application/EDI-X12";

			return this;
		}

		public ContentResponse ApplicationEdifact()
		{
			ResponseContentType = "application/EDIFACT";

			return this;
		}

		public ContentResponse ApplicationJson()
		{
			ResponseContentType = "application/json";

			return this;
		}

		public ContentResponse ApplicationJavaScript()
		{
			ResponseContentType = "application/javascript";

			return this;
		}

		public ContentResponse ApplicationOctetStream()
		{
			ResponseContentType = "application/octet-stream";

			return this;
		}

		public ContentResponse ApplicationOgg()
		{
			ResponseContentType = "application/ogg";

			return this;
		}

		public ContentResponse ApplicationPdf()
		{
			ResponseContentType = "application/pdf";

			return this;
		}

		public ContentResponse ApplicationPostscript()
		{
			ResponseContentType = "application/postscript";

			return this;
		}

		public ContentResponse ApplicationRdf()
		{
			ResponseContentType = "application/rdf+xml";

			return this;
		}

		public ContentResponse ApplicationRss()
		{
			ResponseContentType = "application/rss+xml";

			return this;
		}

		public ContentResponse ApplicationSoap()
		{
			ResponseContentType = "application/soap+xml";

			return this;
		}

		public ContentResponse ApplicationXHtml()
		{
			ResponseContentType = "application/xhtml+xml";

			return this;
		}

		public ContentResponse ApplicationDtd()
		{
			ResponseContentType = "application/xml-dtd";

			return this;
		}

		public ContentResponse ApplicationXop()
		{
			ResponseContentType = "application/xop+xml";

			return this;
		}

		public ContentResponse ApplicationZip()
		{
			ResponseContentType = "application/zip";

			return this;
		}

		public ContentResponse ApplicationGzip()
		{
			ResponseContentType = "application/gzip";

			return this;
		}

		public ContentResponse AudioBasic()
		{
			ResponseContentType = "audio/basic";

			return this;
		}

		public ContentResponse AudioL24()
		{
			ResponseContentType = "audio/L24";

			return this;
		}

		public ContentResponse AudioMp4()
		{
			ResponseContentType = "audio/mp4";

			return this;
		}

		public ContentResponse AudioMpeg()
		{
			ResponseContentType = "audio/mpeg";

			return this;
		}

		public ContentResponse AudioOgg()
		{
			ResponseContentType = "audio/ogg";

			return this;
		}

		public ContentResponse AudioVorbis()
		{
			ResponseContentType = "audio/vorbis";

			return this;
		}

		public ContentResponse AudioRealAudio()
		{
			ResponseContentType = "audio/vnd.rn-realaudio";

			return this;
		}

		public ContentResponse AudioWav()
		{
			ResponseContentType = "audio/vnd.wave";

			return this;
		}

		public ContentResponse AudioWebM()
		{
			ResponseContentType = "audio/webm";

			return this;
		}

		public ContentResponse ImageGif()
		{
			ResponseContentType = "image/gif";

			return this;
		}

		public ContentResponse ImageJpeg()
		{
			ResponseContentType = "image/jpeg";

			return this;
		}

		public ContentResponse ImagePJpeg()
		{
			ResponseContentType = "image/pjpeg";

			return this;
		}

		public ContentResponse ImagePng()
		{
			ResponseContentType = "image/png";

			return this;
		}

		public ContentResponse ImageSvg()
		{
			ResponseContentType = "image/svg+xml";

			return this;
		}

		public ContentResponse ImageTiff()
		{
			ResponseContentType = "image/tiff";

			return this;
		}

		public ContentResponse ImageIco()
		{
			ResponseContentType = "image/vnd.microsoft.icon";

			return this;
		}

		public ContentResponse MessageHttp()
		{
			ResponseContentType = "message/http";

			return this;
		}

		public ContentResponse MessageImdn()
		{
			ResponseContentType = "message/imdn+xml";

			return this;
		}

		public ContentResponse MessagePartial()
		{
			ResponseContentType = "message/partial";

			return this;
		}

		public ContentResponse MessageRfc822()
		{
			ResponseContentType = "message/rfc822";

			return this;
		}

		public ContentResponse ModelExample()
		{
			ResponseContentType = "model/example";

			return this;
		}

		public ContentResponse ModelIges()
		{
			ResponseContentType = "model/iges";

			return this;
		}

		public ContentResponse ModelMesh()
		{
			ResponseContentType = "model/mesh";

			return this;
		}

		public ContentResponse ModelVrml()
		{
			ResponseContentType = "model/vrml";

			return this;
		}

		public ContentResponse ModelX3DBinary()
		{
			ResponseContentType = "model/x3d+binary";

			return this;
		}

		public ContentResponse ModelX3DVrml()
		{
			ResponseContentType = "model/x3d+vrml";

			return this;
		}

		public ContentResponse ModelX3DXml()
		{
			ResponseContentType = "model/x3d+xml";

			return this;
		}

		public ContentResponse MultipartMixed()
		{
			ResponseContentType = "multipart/mixed";

			return this;
		}

		public ContentResponse MultipartAlternative()
		{
			ResponseContentType = "multipart/alternative";

			return this;
		}

		public ContentResponse MultipartRelated()
		{
			ResponseContentType = "multipart/related";

			return this;
		}

		public ContentResponse MultipartFormData()
		{
			ResponseContentType = "multipart/form-data";

			return this;
		}

		public ContentResponse MultipartSigned()
		{
			ResponseContentType = "multipart/signed";

			return this;
		}

		public ContentResponse MultipartEncrypted()
		{
			ResponseContentType = "multipart/encrypted";

			return this;
		}

		public ContentResponse TextCmd()
		{
			ResponseContentType = "text/cmd";

			return this;
		}

		public ContentResponse TextCss()
		{
			ResponseContentType = "text/css";

			return this;
		}

		public ContentResponse TextCsv()
		{
			ResponseContentType = "text/csv";

			return this;
		}

		public ContentResponse TextHtml()
		{
			ResponseContentType = "text/html";

			return this;
		}

		public ContentResponse TextPlain()
		{
			ResponseContentType = "text/plain";

			return this;
		}

		public ContentResponse TextVCard()
		{
			ResponseContentType = "text/vcard";

			return this;
		}

		public ContentResponse TextXml()
		{
			ResponseContentType = "text/xml";

			return this;
		}

		public ContentResponse VideoMpeg()
		{
			ResponseContentType = "video/mpeg";

			return this;
		}

		public ContentResponse VideoMp4()
		{
			ResponseContentType = "video/mp4";

			return this;
		}

		public ContentResponse VideoOgg()
		{
			ResponseContentType = "video/ogg";

			return this;
		}

		public ContentResponse VideoQuickTime()
		{
			ResponseContentType = "video/quicktime";

			return this;
		}

		public ContentResponse VideoWebM()
		{
			ResponseContentType = "video/webm";

			return this;
		}

		public ContentResponse VideoMatroska()
		{
			ResponseContentType = "video/x-matroska";

			return this;
		}

		public ContentResponse VideoWmv()
		{
			ResponseContentType = "video/x-ms-wmv";

			return this;
		}

		public ContentResponse VideoFlv()
		{
			ResponseContentType = "video/x-flv";

			return this;
		}

		#endregion

		#region Non-standard Content Types

		public ContentResponse ApplicationOpenDocumentText()
		{
			ResponseContentType = "application/vnd.oasis.opendocument.text";

			return this;
		}

		public ContentResponse ApplicationOpenDocumentSpreadsheet()
		{
			ResponseContentType = "application/vnd.oasis.opendocument.spreadsheet";

			return this;
		}

		public ContentResponse ApplicationOpenDocumentPresentation()
		{
			ResponseContentType = "application/vnd.oasis.opendocument.presentation";

			return this;
		}

		public ContentResponse ApplicationOpenDocumentGraphics()
		{
			ResponseContentType = "application/vnd.oasis.opendocument.graphics";

			return this;
		}

		public ContentResponse ApplicationExcel()
		{
			ResponseContentType = "application/vnd.ms-excel";

			return this;
		}

		public ContentResponse ApplicationExcel2007()
		{
			ResponseContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

			return this;
		}

		public ContentResponse ApplicationPowerpoint()
		{
			ResponseContentType = "application/vnd.ms-powerpoint";

			return this;
		}

		public ContentResponse ApplicationPowerpoint2007()
		{
			ResponseContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

			return this;
		}

		public ContentResponse ApplicationWord2007()
		{
			ResponseContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

			return this;
		}

		public ContentResponse ApplicationMozillaXul()
		{
			ResponseContentType = "application/vnd.mozilla.xul+xml";

			return this;
		}

		public ContentResponse ApplicationGoogleEarthKml()
		{
			ResponseContentType = "application/vnd.google-earth.kml+xml";

			return this;
		}

		public ContentResponse ApplicationDeb()
		{
			ResponseContentType = "application/x-deb";

			return this;
		}

		public ContentResponse ApplicationDvi()
		{
			ResponseContentType = "application/x-dvi";

			return this;
		}

		public ContentResponse ApplicationFontTtf()
		{
			ResponseContentType = "application/x-font-ttf";

			return this;
		}

		public ContentResponse ApplicationLaTeX()
		{
			ResponseContentType = "application/x-latex";

			return this;
		}

		public ContentResponse ApplicationMpegUrl()
		{
			ResponseContentType = "application/x-mpegURL";

			return this;
		}

		public ContentResponse ApplicationRarCompressed()
		{
			ResponseContentType = "application/x-rar-compressed";

			return this;
		}

		public ContentResponse ApplicationShockwaveFlash()
		{
			ResponseContentType = "application/x-shockwave-flash";

			return this;
		}

		public ContentResponse ApplicationStuffIt()
		{
			ResponseContentType = "application/x-stuffit";

			return this;
		}

		public ContentResponse ApplicationTar()
		{
			ResponseContentType = "application/x-tar";

			return this;
		}

		public ContentResponse ApplicationFormEncoded()
		{
			ResponseContentType = "application/x-www-form-urlencoded";

			return this;
		}

		public ContentResponse ApplicationXpInstall()
		{
			ResponseContentType = "application/x-xpinstall";

			return this;
		}

		public ContentResponse AudioAac()
		{
			ResponseContentType = "audio/x-aac";

			return this;
		}

		public ContentResponse AudioCaf()
		{
			ResponseContentType = "audio/x-caf";

			return this;
		}

		public ContentResponse ImageXcf()
		{
			ResponseContentType = "image/x-xcf";

			return this;
		}

		public ContentResponse TextGwtRpc()
		{
			ResponseContentType = "text/x-gwt-rpc";

			return this;
		}

		public ContentResponse TextJQueryTmpl()
		{
			ResponseContentType = "text/x-jquery-tmpl";

			return this;
		}

		public ContentResponse ApplicationPkcs12()
		{
			ResponseContentType = "application/x-pkcs12";

			return this;
		}

		public ContentResponse ApplicationPkcs7Certificates()
		{
			ResponseContentType = "application/x-pkcs7-certificates";

			return this;
		}

		public ContentResponse ApplicationPkcs7CertReqResp()
		{
			ResponseContentType = "application/x-pkcs7-certreqresp";

			return this;
		}

		public ContentResponse ApplicationPkcs7Mime()
		{
			ResponseContentType = "application/x-pkcs7-mime";

			return this;
		}

		public ContentResponse ApplicationPkcs7Signature()
		{
			ResponseContentType = "application/x-pkcs7-signature";

			return this;
		}

		#endregion
	}
}