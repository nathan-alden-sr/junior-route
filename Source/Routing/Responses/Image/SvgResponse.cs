using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Junior.Route.Routing.Responses.Image
{
	public class SvgResponse : ImmutableResponse
	{
		public SvgResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().Content(content), configurationDelegate)
		{
		}

		public SvgResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SvgResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().Content(content), configurationDelegate)
		{
		}

		public SvgResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SvgResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().Content(content), configurationDelegate)
		{
		}

		public SvgResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SvgResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().Content(content), configurationDelegate)
		{
		}

		public SvgResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SvgResponse(XmlNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().Content(content.GetString()), configurationDelegate)
		{
		}

		public SvgResponse(XmlNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}

		public SvgResponse(XNode content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().Content(content.GetString()), configurationDelegate)
		{
		}

		public SvgResponse(XNode content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageSvg().ContentEncoding(encoding).Content(content.GetBytes(encoding)), configurationDelegate)
		{
		}
	}
}