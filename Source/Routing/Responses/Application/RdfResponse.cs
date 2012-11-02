using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Application
{
	public class RdfResponse : ImmutableResponse
	{
		public RdfResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().Content(content), configurationDelegate)
		{
		}

		public RdfResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RdfResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().Content(content), configurationDelegate)
		{
		}

		public RdfResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RdfResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().Content(content), configurationDelegate)
		{
		}

		public RdfResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RdfResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().Content(content), configurationDelegate)
		{
		}

		public RdfResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RdfResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().Content(content.GetString()), configurationDelegate)
		{
		}

		public RdfResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public RdfResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().Content(content.GetString()), configurationDelegate)
		{
		}

		public RdfResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRdf().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}
	}
}