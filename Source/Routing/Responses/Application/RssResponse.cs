using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Application
{
	public class RssResponse : ImmutableResponse
	{
		public RssResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().Content(content), configurationDelegate)
		{
		}

		public RssResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RssResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().Content(content), configurationDelegate)
		{
		}

		public RssResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RssResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().Content(content), configurationDelegate)
		{
		}

		public RssResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RssResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().Content(content), configurationDelegate)
		{
		}

		public RssResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RssResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().Content(content.GetString()), configurationDelegate)
		{
		}

		public RssResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public RssResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().Content(content.GetString()), configurationDelegate)
		{
		}

		public RssResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationRss().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}
	}
}