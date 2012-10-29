using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Application
{
	public class XopResponse : ImmutableResponse
	{
		public XopResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().Content(content), configurationDelegate)
		{
		}

		public XopResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XopResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().Content(content), configurationDelegate)
		{
		}

		public XopResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XopResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().Content(content), configurationDelegate)
		{
		}

		public XopResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XopResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().Content(content), configurationDelegate)
		{
		}

		public XopResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public XopResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().Content(content.GetString()))
		{
		}

		public XopResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().ContentEncoding(encoding).Content(content.GetBytes(encoding)))
		{
		}

		public XopResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().Content(content.GetString()))
		{
		}

		public XopResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationXop().ContentEncoding(encoding).Content(content.GetBytes(encoding)))
		{
		}
	}
}