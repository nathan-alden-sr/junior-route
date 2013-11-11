using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Text
{
	public class XmlResponse : ImmutableResponse
	{
		public XmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().Content(content), configurationDelegate)
		{
		}

		public XmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().Content(content), configurationDelegate)
		{
		}

		public XmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().Content(content), configurationDelegate)
		{
		}

		public XmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().Content(content), configurationDelegate)
		{
		}

		public XmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XmlResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().Content(content.GetString()), configurationDelegate)
		{
		}

		public XmlResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public XmlResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().Content(content.GetString()), configurationDelegate)
		{
		}

		public XmlResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public XmlResponse(Action<Response> configurationDelegate = null)
			: base(new Response().TextXml(), configurationDelegate)
		{
		}

		public XmlResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextXml().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}