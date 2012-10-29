using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class PngResponse : ImmutableResponse
	{
		public PngResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePng().Content(content), configurationDelegate)
		{
		}

		public PngResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePng().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PngResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePng().Content(content), configurationDelegate)
		{
		}

		public PngResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePng().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}