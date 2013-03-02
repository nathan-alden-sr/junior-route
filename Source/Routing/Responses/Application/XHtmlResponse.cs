using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf;

namespace Junior.Route.Routing.Responses.Application
{
	public class XHtmlResponse : ImmutableResponse
	{
		public XHtmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(Func<byte[]> content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(Func<byte[]> content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(Func<string> content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(Func<string> content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(byte[] content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(byte[] content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(string content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(string content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content.GetString()), configurationDelegate)
		{
		}

		public XHtmlResponse(XmlNode content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content.GetString()), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public XHtmlResponse(XmlNode content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content.GetString()), configurationDelegate)
		{
		}

		public XHtmlResponse(XNode content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().Content(content.GetString()), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public XHtmlResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public XHtmlResponse(XNode content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXHtml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		private static Response AntiCsrf(Response response, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate)
		{
			antiCsrfData.ThrowIfNull("antiCsrfData");

			response.Cookie(antiCsrfData.Cookie);

			if (configurationDelegate != null)
			{
				configurationDelegate(response);
			}

			return response;
		}
	}
}