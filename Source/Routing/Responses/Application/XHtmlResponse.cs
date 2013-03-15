using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Application
{
	public class XHtmlResponse : ImmutableResponse
	{
		public XHtmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XHtmlResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().Content(content.GetString()), configurationDelegate)
		{
		}

		public XHtmlResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public XHtmlResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().Content(content.GetString()), configurationDelegate)
		{
		}

		public XHtmlResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationXHtml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}
	}
}