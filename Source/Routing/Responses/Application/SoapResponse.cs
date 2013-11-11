using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Application
{
	public class SoapResponse : ImmutableResponse
	{
		public SoapResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().Content(content), configurationDelegate)
		{
		}

		public SoapResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SoapResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().Content(content), configurationDelegate)
		{
		}

		public SoapResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SoapResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().Content(content), configurationDelegate)
		{
		}

		public SoapResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SoapResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().Content(content), configurationDelegate)
		{
		}

		public SoapResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SoapResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().Content(content.GetString()), configurationDelegate)
		{
		}

		public SoapResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public SoapResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().Content(content.GetString()), configurationDelegate)
		{
		}

		public SoapResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public SoapResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap(), configurationDelegate)
		{
		}

		public SoapResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationSoap().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}