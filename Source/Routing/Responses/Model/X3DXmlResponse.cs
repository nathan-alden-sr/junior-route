using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Model
{
	public class X3DXmlResponse : ImmutableResponse
	{
		public X3DXmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DXmlResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().Content(content.GetString()), configurationDelegate)
		{
		}

		public X3DXmlResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public X3DXmlResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().Content(content.GetString()), configurationDelegate)
		{
		}

		public X3DXmlResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DXml().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}
	}
}