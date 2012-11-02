using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Message
{
	public class ImdnResponse : ImmutableResponse
	{
		public ImdnResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ImdnResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().Content(content.GetString()), configurationDelegate)
		{
		}

		public ImdnResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public ImdnResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().Content(content.GetString()), configurationDelegate)
		{
		}

		public ImdnResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageImdn().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}
	}
}